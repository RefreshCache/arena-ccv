using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MapPoint;
using Arena.Core;
using Arena.Utility;

namespace ImportMapPointAreas
{
    public partial class frmMain : Form
    {
        private int OrganizationId = Int32.Parse(ConfigurationManager.AppSettings["organization"]);
        private AreaCollection mappointAreas = null;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            status.Text = string.Empty;

            this.Show();
            System.Windows.Forms.Application.DoEvents();

            string mapPointFile = string.Empty;

            openFileDialog1.Filter = "Mappoint Files (*.ptm)|*.ptm";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                mappointAreas = new AreaCollection();

                MapPoint.Application mApp = new MapPoint.Application();
                mApp.Visible = false;
                mApp.UserControl = false;

                MapPoint.Map map = mApp.OpenMap(openFileDialog1.FileName, false);

                int i = 0;
                foreach (MapPoint.Shape shape in map.Shapes)
                {
                    //try
                    //{
                        if (shape.Type == GeoShapeType.geoFreeform)
                        {
                            i++;

                            status.Text = "Importing Shape " + i.ToString();
                            System.Windows.Forms.Application.DoEvents();

                            AreaCoordinateCollection coordinates = new AreaCoordinateCollection();

                            System.Object[] objects = (System.Object[])shape.Vertices;

                            foreach (System.Object obj in objects)
                            {
                                if (obj is MapPoint.Location)
                                {
                                    MapPoint.Location location = (MapPoint.Location)obj;

                                    AreaCoordinate coordinate = new AreaCoordinate();
                                    coordinate.Latitude = location.Latitude;
                                    coordinate.Longitude = location.Longitude;
                                    coordinates.Add(coordinate);

                                    System.Windows.Forms.Application.DoEvents();
                                }
                            }

                            Area area = new Area(coordinates.MinLatitude(), coordinates.MinLongitude());
                            if (area == null || area.AreaID == -1)
                            {
                                area.AreaID = int.MinValue;
                                area.OrganizationID = OrganizationId;
                                area.Name = "Area: " + i.ToString();
                                area.MapHeight = 400;
                                area.MapWidth = 600;
                            }

                            area.Coordinates = coordinates;

                            for (int j = 0; j < area.Coordinates.Count; j++)
                            {
                                AreaCoordinate aCoordinate = area.Coordinates[j];
                                aCoordinate.AreaID = area.AreaID;
                                aCoordinate.Order = j;
                            }

                            if (area.Blob != null)
                                area.Blob.DateModified = DateTime.Now;

                            mappointAreas.Add(area);
                        }
                    //}
                    //catch (System.Exception ex)
                    //{
                    //    MessageBox.Show("Area " + i.ToString() + ": " + ex.Message, "Error Reading Area");
                    //}
                }

                LoadExistingAreas();
                LoadMappointAreas(0);
            }
            else
            {
                btnLink.Enabled = false;
                btnCreateNew.Enabled = false;
                btnRemove.Enabled = false;
                btnImport.Enabled = false;
            }

            status.Text = string.Empty;
            Cursor.Current = Cursors.Default;
        }

        private void LoadMappointAreas(int selectedIndex)
        {
            lbAreas.Items.Clear();
            foreach (Area area in mappointAreas)
                lbAreas.Items.Add(area);

            if (selectedIndex != -1)
                lbAreas.SelectedIndex = selectedIndex;
        }

        private void LoadExistingAreas()
        {
            AreaCollection existingAreas = new AreaCollection(OrganizationId);
            if (existingAreas.Count > 0)
            {
                foreach (Area area in existingAreas)
                    cbAreas.Items.Add(area);
                cbAreas.SelectedIndex = 0;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel?", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
                System.Windows.Forms.Application.Exit();
            }
        }

        private void cbAreas_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            Area area = (Area)cbAreas.SelectedItem;
            if (area.Blob != null && area.Blob.ByteArray != null && area.Blob.ByteArray.Length > 0)
                picAreaMap.Image = AreaPolygon(area);
            else
            {
                area.Blob = area.getImageBlob();
                if (area.Blob != null && area.Blob.ByteArray != null && area.Blob.ByteArray.Length > 0)
                    picAreaMap.Image = AreaPolygon(area);
                else
                    picAreaMap.Image = null;
            }

            Cursor.Current = Cursors.Default;
        }

        private void lbAreas_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            Area area = mappointAreas[lbAreas.SelectedIndex];
            if (area.Blob != null && area.Blob.ByteArray == null)
            {
                area.Blob = area.getImageBlob();
                if (area.Blob == null)
                    area.Name = "Invalid Image";
            }

            if (area.Blob != null && area.Blob.ByteArray != null && area.Blob.ByteArray.Length > 0)
                picImportedMap.Image = AreaPolygon(area);
            else
                picImportedMap.Image = null;

            if (area.AreaID != -1)
            {
                for (int i = 0; i < cbAreas.Items.Count; i++)
                {
                    Area existingArea = (Area)cbAreas.Items[i];
                    if (existingArea.AreaID == area.AreaID)
                    {
                        cbAreas.SelectedIndex = i;
                        break;
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private void btnLink_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            Area importedArea = mappointAreas[lbAreas.SelectedIndex];
            Area existingArea = (Area)cbAreas.SelectedItem;

            importedArea.AreaID = existingArea.AreaID;
            importedArea.Name = existingArea.Name;
            importedArea.Description = existingArea.Description;
            importedArea.ForeignKey = existingArea.ForeignKey;
            importedArea.MapHeight = existingArea.MapHeight;
            importedArea.MapWidth = existingArea.MapWidth;
            importedArea.OrganizationID = existingArea.OrganizationID;
            importedArea.OutreachCoordinators = existingArea.OutreachCoordinators;
            importedArea.ThumbBlob = null;

            existingArea.Coordinates = importedArea.Coordinates;
            if (importedArea.Blob != null)
            {
                existingArea.Blob = importedArea.Blob;
                picAreaMap.Image = AreaPolygon(existingArea);
            }

            LoadMappointAreas(lbAreas.SelectedIndex);

            Cursor.Current = Cursors.Default;
        }


        private void btnCreateNew_Click(object sender, EventArgs e)
        {
            frmNewArea frmNewArea = new frmNewArea();
            frmNewArea.ShowDialog();
            string newAreaName = frmNewArea.AreaName.Trim();
            frmNewArea.Close();

            Cursor.Current = Cursors.WaitCursor;

            if (newAreaName != string.Empty)
            {
                Area importedArea = mappointAreas[lbAreas.SelectedIndex];

                importedArea.AreaID = -1;
                importedArea.Name = frmNewArea.AreaName;
                importedArea.MapHeight = 400;
                importedArea.MapWidth = 600;
                importedArea.OrganizationID = OrganizationId;
                importedArea.ThumbBlob = null;

                LoadMappointAreas(lbAreas.SelectedIndex);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            foreach (Area area in mappointAreas)
                if (area.AreaID != int.MinValue)
                {
                    area.Save("ImportMapPointAreas");
                    new Arena.DataLayer.Core.AreaData().DeleteAreaCoordinates(area.AreaID);
                    foreach (AreaCoordinate aCoordinate in area.Coordinates)
                    {
                        aCoordinate.AreaID = area.AreaID;
                        aCoordinate.Save();
                    }
                }

            MessageBox.Show("Areas have been imported", "Import Complete");
        }

        private void UpdateAddressAreas(bool reGeoCode)
        {
            Arena.DataLayer.Organization.OrganizationData oData = new Arena.DataLayer.Organization.OrganizationData();

            AreaCollection areas = new AreaCollection(OrganizationId);

            SqlDataReader rdr = new Arena.DataLayer.Core.AddressData().GetAddresses();
            
            while (rdr.Read())
            {
                Address address = new Address((int)rdr["address_id"]);
                if (address != null && address.AddressID != -1)
                {
                    if (reGeoCode && 
                        !address.StreetLine1.ToUpper().StartsWith("PO BOX") &&
                        address.Latitude == 0 &&
                        address.Longitude == 0 &&
                        address.Standardized &&
                        address.DateGeocoded.AddDays(30) < DateTime.Today)
                    {
                        bool active = false;
                        PersonAddressCollection pac = new PersonAddressCollection();
                        pac.LoadByAddressID(address.AddressID);
                        foreach (PersonAddress pa in pac)
                        {
                            Person person = new Person(pa.PersonID);
                            if (person.RecordStatus == Arena.Enums.RecordStatus.Active)
                            {
                                active = true;
                                break;
                            }
                        }

                        if (active)
                        {
                            status.Text = address.ToString().Replace("\n", " ");
                            address.Geocode("ImportMapPointAreas", true);
                            string breakstring = address.StreetLine1;
                        }
                    }
                    else if (address.Latitude != 0 && address.Longitude != 0)
                    {
                        status.Text = address.ToString().Replace("\n", " ");
                        address.UpdateArea(areas);
                    }

                    System.Windows.Forms.Application.DoEvents();
                }


            }
            rdr.Close();

            status.Text = string.Empty;

            MessageBox.Show("Addresses have been updated", "Import Complete");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult msgResult = MessageBox.Show("Do you want to attempt to geo-code addresses that have not yet been succesfully geo-coded?\n\nThis will only affect addresses that have been succcessfully standardized, are associated with an active person record, and have not been attempted in the last 30 days. Note: There is a financial cost involved in geo-coding addresses!", "Geo-code Addresses", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (msgResult != DialogResult.Cancel)
                UpdateAddressAreas(msgResult == DialogResult.Yes);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            mappointAreas.RemoveAt(lbAreas.SelectedIndex);

            LoadMappointAreas(lbAreas.SelectedIndex);

            Cursor.Current = Cursors.Default;
        }

        private Image AreaPolygon(Area area)
        {
            Image areaImage = area.Blob.GetImage(300, 200);
            Graphics graphics = Graphics.FromImage(areaImage);

            if (area.Coordinates.Count >= 3)
            {

                Point[] points = new Point[area.Coordinates.Count];
                for (int i = 0; i < area.Coordinates.Count; i++)
                {
                    AreaCoordinate coord = area.Coordinates[i];
                    points[i] = area.MapCoordToPixelCoord(coord.Latitude, coord.Longitude, 300, 200);
                }

                Pen pen = new Pen(Color.Red);
                pen.Width = 3;
                graphics.DrawPolygon(pen, points);
            }

            return areaImage;
        }
    }
}