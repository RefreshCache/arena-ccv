using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Arena.Core;
using Arena.Document;
using Arena.Enums;

//using DTI.ImageMan.Twain;
//using DTI.ImageMan.Codecs;
using Atalasoft.Twain;
using Atalasoft.Imaging;
using Atalasoft.Imaging.WinControls;
using Atalasoft.Imaging.ImageProcessing;
using Atalasoft.Imaging.ImageProcessing.Document;
using Atalasoft.Imaging.ImageProcessing.Transforms;

namespace DocumentScanner
{
    public partial class Main : Form
    {
        #region Private Variables

        private int organizationID = -1;
        private Arena.Organization.Organization organization = null;
        private Acquisition acquisition;
        private Device device = null;
        private ImageCollection ScannedImages = new ImageCollection();

        PersonSearchForm personSearchForm = null;

        #endregion

        #region Main

        public Main()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        #region Form Events

        private void Main_Load(object sender, EventArgs e)
        {
            RefreshMenu();

            if (getCheckedMenuItem(miScanner) == null)
            {
                MessageBox.Show("A scanner was not found.  Please connect and/or install a valid scanner before running this application.", "Scanner Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            if (getCheckedMenuItem(miDocumentType) == null)
            {
                MessageBox.Show("There are not any Person Attributes configured in Arena that are document attributes.  Please add at least one document type Person Attribute before running this application.", "No Document Types", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.device != null && this.device.State > TwainState.Loaded)
                this.device.Close();

            this.acquisition.Dispose();

            ToolStripMenuItem mi = getCheckedMenuItem(miScanner);
            if (mi != null)
                Properties.Settings.Default.DefaultScanner =  ((Device)mi.Tag).Identity.ProductName;

            mi = getCheckedMenuItem(miDocumentType);
            if (mi != null)
                Properties.Settings.Default.DefaultDocType = ((myDocType)mi.Tag).DocType.DocumentTypeId;

            Properties.Settings.Default.DefaultPageSize = getCheckedMenuItemIndex(miPageSize);
            Properties.Settings.Default.DefaultRotate = getCheckedMenuItemIndex(miRotation);
            Properties.Settings.Default.Save();
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (this.device != null) this.device.Close();
        //        if (this.acquisition != null) this.acquisition.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #endregion 

        #region Atalasoft Image/Scan Events

        private void acquisition_ImageAcquired(object sender, AcquireEventArgs e)
        {
            Bitmap bitmap = e.Image;
            switch (getCheckedMenuItemIndex(miRotation))
            {
                case 1: bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone); break;
                case 2: bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone); break;
                case 3: bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone); break;
            }
            int page = tViewer.Items.Count + 1;

            AtalaImage atalaImage = AtalaImage.FromBitmap(bitmap);
            ScannedImages.Add(atalaImage);

            Atalasoft.Imaging.WinControls.Thumbnail thumbnail = new Atalasoft.Imaging.WinControls.Thumbnail(atalaImage, "", "");
            thumbnail.Selected = true;

            tViewer.Items.Add(thumbnail);

            RefreshView(tViewer.Items.Count - 1);
        }

        void acquisition_AcquireFinished(object sender, EventArgs e)
        {
            this.device.Close();
        }

        private void acquisition_AcquireCanceled(object sender, System.EventArgs e)
        {
            this.device.Disable();
            this.device.Close();
        }

        private void acquisition_DeviceEvent(object sender, Atalasoft.Twain.DeviceEventArgs e)
        {
            // One of the many device events has fired.
            // You will only receive the events you have set using
            // the Device.DeviceEvents property.
            if (e.Event == DeviceEventFlags.PaperJam)
                MessageBox.Show(this, "Paper jam!!!");
            else
                MessageBox.Show(this, "Device Event:  " + e.Event.ToString());
        }

        private void acquisition_AsynchronousException(object sender, Atalasoft.Twain.AsynchronousExceptionEventArgs e)
        {
            // Make sure you close the connection when there is an error during a scan.
            this.device.Close();
            MessageBox.Show(this, e.Exception.Message, "Asynchronous Exception");
        }

        #endregion

        #region Menu Events

        private void miScanner_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            SetMenuChecked(miScanner, e.ClickedItem);
            this.device = (Device)e.ClickedItem.Tag;
            UpdateStatus();
        }

        private void mi_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            SetMenuChecked((ToolStripMenuItem)sender, e.ClickedItem);
            UpdateStatus();
        }

        private void miRefresh_Click(object sender, EventArgs e)
        {
            RefreshMenu();
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void miRotateRight_Click(object sender, EventArgs e)
        {
            Rotate(90);
        }

        private void miRotateLeft_Click(object sender, EventArgs e)
        {
            Rotate(270);
        }

        private void miImageDelete_Click(object sender, EventArgs e)
        {
            for (int i = tViewer.Items.Count - 1; i >= 0; i--)
                if (tViewer.Items[i].Selected)
                {
                    tViewer.Items.RemoveAt(i);
                    ScannedImages.RemoveAt(i);
                }

            RefreshView();
        }
        
        #endregion

        #region Toolbar Events

        private void picScan_MouseDown(object sender, MouseEventArgs e)
        {
            picScan.Image = Properties.Resources.scan_down;
        }

        private void picScan_MouseUp(object sender, MouseEventArgs e)
        {
            picScan.Image = Properties.Resources.scan_up;
            picScan.Refresh();
            Application.DoEvents();
            ScanDocument();
        }

        private void picScan_MouseLeave(object sender, EventArgs e)
        {
            picScan.Image = Properties.Resources.scan_up;
        }

        private void picAttach_MouseDown(object sender, MouseEventArgs e)
        {
            if (ScannedImages.Count > 0)
                picAttach.Image = Properties.Resources.attach_down;
        }

        private void picAttach_MouseUp(object sender, MouseEventArgs e)
        {
            if (ScannedImages.Count > 0)
            {
                picAttach.Image = Properties.Resources.attach_up;
                picAttach.Refresh();
                Application.DoEvents();
                AttachDocument();
            }
            else
                picAttach.Image = Properties.Resources.attach_disabled;
        }

        private void picAttach_MouseLeave(object sender, EventArgs e)
        {
            if (ScannedImages.Count > 0)
                picAttach.Image = Properties.Resources.attach_up;
            else
                picAttach.Image = Properties.Resources.attach_disabled;
        }

        #endregion

        #region Rotation Events

        private void picONormal_Click(object sender, EventArgs e)
        {
            SetMenuChecked(miRotation, miRotation.DropDownItems[0]);
            UpdateStatus();
        }

        private void picO90_Click(object sender, EventArgs e)
        {
            SetMenuChecked(miRotation, miRotation.DropDownItems[1]);
            UpdateStatus();
        }

        private void picO270_Click(object sender, EventArgs e)
        {
            SetMenuChecked(miRotation, miRotation.DropDownItems[3]);
            UpdateStatus();
        }

        private void picO180_Click(object sender, EventArgs e)
        {
            SetMenuChecked(miRotation, miRotation.DropDownItems[2]);
            UpdateStatus();
        }


        private void picRotateRight_Click(object sender, EventArgs e)
        {
            Rotate(90);
        }

        private void picRotateLeft_Click(object sender, EventArgs e)
        {
            Rotate(270);
        }

        #endregion

        private void tViewer_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshView();
        }

        #endregion

        #region Private Methods

        private void RefreshMenu()
        {
            try
            {
                int organizationID = Int32.Parse(ArenaContext.Current.AppSettings["Organization"]);
                organization = new Arena.Organization.Organization(organizationID);

                // Load the scanner devices and set the default
                this.acquisition = new Acquisition();

                if (this.acquisition.Devices.Count <= 0)
                {
                    MessageBox.Show("There were not any scanners found.  This application requires a scanner.", "No Scanner", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Application.Exit();
                }

                this.acquisition.AsynchronousException += new AsynchronousExceptionEventHandler(this.acquisition_AsynchronousException);
                this.acquisition.AcquireFinished += new EventHandler(acquisition_AcquireFinished);
                this.acquisition.ImageAcquired += new ImageAcquiredEventHandler(acquisition_ImageAcquired);
                this.acquisition.AcquireCanceled += new EventHandler(acquisition_AcquireCanceled);
                this.acquisition.DeviceEvent += new DeviceEventHandler(acquisition_DeviceEvent);

                miScanner.DropDownItems.Clear();
                foreach (Device twainDevice in this.acquisition.Devices)
                {
                    ToolStripMenuItem miDevice = new ToolStripMenuItem();
                    miDevice.Text = twainDevice.Identity.ProductName;
                    miDevice.Tag = twainDevice;
                    miScanner.DropDownItems.Add(miDevice);
                    if (twainDevice.Identity.ProductName == Properties.Settings.Default.DefaultScanner)
                    {
                        this.device = twainDevice;
                        miDevice.Checked = true;
                    }
                }

                if (this.device == null)
                {
                    this.device = this.acquisition.Devices.Default;
                    foreach (ToolStripItem item in miScanner.DropDownItems)
                        if (item.Text == this.device.Identity.ProductName)
                        {
                            ((ToolStripMenuItem)item).Checked = true;
                            break;
                        }
                }

                // Load document types and set default
                miDocumentType.DropDownItems.Clear();

                DocumentTypeCollection docTypes = new DocumentTypeCollection();
                docTypes.LoadByOrganizationID(organizationID);

                int defaultDocType = Properties.Settings.Default.DefaultDocType;
                foreach (AttributeGroup attrGroup in new AttributeGroupCollection(organizationID))
                    foreach (Arena.Core.Attribute attr in attrGroup.Attributes)
                        if (attr.AttributeType == Arena.Enums.DataType.Document &&
                            attr.TypeQualifier.Trim() != string.Empty)
                        {
                            try
                            {
                                DocumentType docType = new DocumentType(Int32.Parse(attr.TypeQualifier));
                                if (defaultDocType == 0)
                                    defaultDocType = docType.DocumentTypeId;

                                ToolStripMenuItem miDocument = new ToolStripMenuItem();
                                miDocument.Text = docType.TypeName;
                                miDocument.Tag = new myDocType(docType, attr.AttributeId);
                                miDocument.Checked = (docType.DocumentTypeId == defaultDocType);
                                miDocumentType.DropDownItems.Add(miDocument);
                            }
                            catch { }
                        }

                for (int i = 0; i < miPageSize.DropDownItems.Count; i++)
                    ((ToolStripMenuItem)miPageSize.DropDownItems[i]).Checked = (i == Properties.Settings.Default.DefaultPageSize);

                for (int i = 0; i < miRotation.DropDownItems.Count; i++)
                    ((ToolStripMenuItem)miRotation.DropDownItems[i]).Checked = (i == Properties.Settings.Default.DefaultRotate);

                UpdateStatus();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ScanDocument()
        {
            this.Cursor = Cursors.WaitCursor;

            if (device.TryOpen())
            {
                try
                {
                    ToolStripMenuItem mi = getCheckedMenuItem(miDocumentType);
                    if (mi != null)
                    {
                        DocumentType docType = ((myDocType)mi.Tag).DocType;

                        DeviceCapability[] capabilities = device.GetSupportedCapabilities();
                        foreach (DeviceCapability capability in capabilities)
                        {
                            switch (capability)
                            {
                                case DeviceCapability.ICAP_PIXELTYPE:

                                    if (docType.PreferredColorDepth != Arena.Enums.ColorDepth.Undefined)
                                    {
                                        ImagePixelType[] pixelTypes = device.GetSupportedPixelTypes();

                                        switch (docType.PreferredColorDepth)
                                        {
                                            case Arena.Enums.ColorDepth.Color_4_bit:
                                            case Arena.Enums.ColorDepth.Color_8_bit:
                                            case Arena.Enums.ColorDepth.Color_24_bit:
                                            case Arena.Enums.ColorDepth.Color_32_bit:
                                            case Arena.Enums.ColorDepth.Color_48_bit:

                                                foreach (ImagePixelType pixelType in pixelTypes)
                                                    if (pixelType == ImagePixelType.Color)
                                                    {
                                                        device.PixelType = pixelType;
                                                        break;
                                                    }
                                                break;

                                            case Arena.Enums.ColorDepth.Grayscale_8_bit:
                                            case Arena.Enums.ColorDepth.Grayscale_10_bit:
                                            case Arena.Enums.ColorDepth.Grayscale_24_bit:

                                                foreach (ImagePixelType pixelType in pixelTypes)
                                                    if (pixelType == ImagePixelType.Grayscale)
                                                    {
                                                        device.PixelType = pixelType;
                                                        break;
                                                    }
                                                break;

                                            case Arena.Enums.ColorDepth.Black_White:

                                                foreach (ImagePixelType pixelType in pixelTypes)
                                                    if (pixelType == ImagePixelType.BlackAndWhite)
                                                    {
                                                        device.PixelType = pixelType;
                                                        break;
                                                    }
                                                break;
                                        }
                                    }

                                    break;

                                case DeviceCapability.ICAP_BITDEPTH:

                                    if (docType.PreferredColorDepth != Arena.Enums.ColorDepth.Undefined)
                                    {
                                        int[] bitDepths = device.GetSupportedBitDepths();
                                        int selectedBithDepth = 0;

                                        switch (docType.PreferredColorDepth)
                                        {
                                            case Arena.Enums.ColorDepth.Black_White:
                                                selectedBithDepth = FindBestBitDepth(bitDepths, 1);
                                                break;
                                            case Arena.Enums.ColorDepth.Color_4_bit:
                                                selectedBithDepth = FindBestBitDepth(bitDepths, 4);
                                                break;
                                            case Arena.Enums.ColorDepth.Color_8_bit:
                                                selectedBithDepth = FindBestBitDepth(bitDepths, 8);
                                                break;
                                            case Arena.Enums.ColorDepth.Color_24_bit:
                                                selectedBithDepth = FindBestBitDepth(bitDepths, 24);
                                                break;
                                            case Arena.Enums.ColorDepth.Color_32_bit:
                                                selectedBithDepth = FindBestBitDepth(bitDepths, 32);
                                                break;
                                            case Arena.Enums.ColorDepth.Color_48_bit:
                                                selectedBithDepth = FindBestBitDepth(bitDepths, 48);
                                                break;
                                            case Arena.Enums.ColorDepth.Grayscale_8_bit:
                                                selectedBithDepth = FindBestBitDepth(bitDepths, 8);
                                                break;
                                            case Arena.Enums.ColorDepth.Grayscale_10_bit:
                                                selectedBithDepth = FindBestBitDepth(bitDepths, 10);
                                                break;
                                            case Arena.Enums.ColorDepth.Grayscale_24_bit:
                                                selectedBithDepth = FindBestBitDepth(bitDepths, 24);
                                                break;
                                        }

                                        if (selectedBithDepth > 0)
                                            device.BitDepth = selectedBithDepth;
                                    }

                                    break;

                                case DeviceCapability.ICAP_XRESOLUTION:

                                    if (docType.PreferredResolution != Resolution.Undefined)
                                    {
                                        TwainResolution[] resolutions = device.GetSupportedResolutions();
                                        TwainResolution selectedResolution = null;

                                        switch (docType.PreferredResolution)
                                        {
                                            case Resolution.DPI_72:
                                                selectedResolution = FindBestResolution(resolutions, 72);
                                                break;
                                            case Resolution.DPI_150:
                                                selectedResolution = FindBestResolution(resolutions, 150);
                                                break;
                                            case Resolution.DPI_300:
                                                selectedResolution = FindBestResolution(resolutions, 300);
                                                break;
                                            case Resolution.DPI_600:
                                                selectedResolution = FindBestResolution(resolutions, 600);
                                                break;
                                        }

                                        if (selectedResolution != null)
                                            device.Resolution = selectedResolution;
                                    }

                                    break;
                            }
                        }
                    }

                    switch (getCheckedMenuItemIndex(miPageSize))
                    {
                        case 0:
                            device.FrameSize = StaticFrameSizeType.LetterUS;
                            break;
                        case 1:
                            device.FrameSize = StaticFrameSizeType.LegalUS;
                            break;
                        case 2:
                            device.FrameSize = StaticFrameSizeType.USStatement;
                            break;
                    }

                    device.AutomaticDeskew = true;
                    device.AutoScan = true;

                    device.HideInterface = true;
                    device.DisplayProgressIndicator = false;
                    device.Acquire();
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    device.Close();
                    MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            this.Cursor = Cursors.Default;
        }

        private void AttachDocument()
        {
            Person person = null;
            using (personSearchForm = new PersonSearchForm())
            {
                personSearchForm.ShowDialog();
                person = personSearchForm.SelectedPerson;
                personSearchForm.Dispose();
            }

            if (person != null)
            {
                this.Cursor = Cursors.WaitCursor;

                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

                ToolStripMenuItem mi = getCheckedMenuItem(miDocumentType);
                if (mi != null)
                {
                    myDocType docType = (myDocType)mi.Tag;

                    PersonAttribute pa = new PersonAttribute(person.PersonID, docType.AttributeId);

                    if (pa.IntValue == -1 ||
                        MessageBox.Show(
                            string.Format("{0} already has a {1} document associated with their record. Do you want to overwrite the existing document?", person.NickName, docType.TypeName),
                            "Document Already Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    {

                        Arena.Utility.ArenaDataBlob blob = new Arena.Utility.ArenaDataBlob(pa.IntValue);

                        Atalasoft.Imaging.Codec.Pdf.PdfEncoder pdf = new Atalasoft.Imaging.Codec.Pdf.PdfEncoder();
                        pdf.Metadata = new Atalasoft.Imaging.Codec.Pdf.PdfMetadata(
                            person.FullName, "", docType.DocType.TypeName,
                            "", organization.Name, "", DateTime.Now, DateTime.Now);

                        string fileName = string.Format("{0}_{1}.pdf",
                            person.FullName.Replace(" ", ""),
                            docType.TypeName.Replace(" ", ""));

                        blob.SetFileInfo(fileName);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            ScannedImages.Save(ms, pdf, null);
                            blob.ByteArray = ms.ToArray();
                            ms.Close();
                        }

                        blob.Cache = false;
                        blob.DocumentTypeID = docType.DocType.DocumentTypeId;
                        blob.Title = docType.DocType.TypeName;

                        pa.IntValue = blob.Save(userName);
                        pa.PersonID = person.PersonID;
                        pa.AttributeId = docType.AttributeId;
                        pa.Save(organizationID, userName);

                        PersonDocument pDoc = new PersonDocument(person.PersonID, pa.IntValue);
                        pDoc.PersonID = person.PersonID;
                        pDoc.DocumentID = pa.IntValue;
                        pDoc.SaveRelationship(userName);

                        ScannedImages.Clear();
                        tViewer.Items.Clear();
                        RefreshView();
                    }
                }

                this.Cursor = Cursors.Default;
            }

        }

        private void Rotate(double angle)
        {
            RotateCommand rotate = new RotateCommand(angle);

            foreach (int i in tViewer.SelectedIndices)
            {
                ScannedImages[i] = rotate.Apply(ScannedImages[i]).Image;
                tViewer.Items[i].Image = rotate.Apply(tViewer.Items[i].Image).Image;
            }

            RefreshView();
        }

        private void SetMenuChecked(ToolStripMenuItem parentItem, ToolStripItem clickedItem)
        {
            foreach (ToolStripItem item in parentItem.DropDownItems)
                ((ToolStripMenuItem)item).Checked = (item.Text == clickedItem.Text);
        }

        private int getCheckedMenuItemIndex(ToolStripMenuItem parentMenuItem)
        {
            for (int i = 0; i < parentMenuItem.DropDownItems.Count; i++)
                if (((ToolStripMenuItem)parentMenuItem.DropDownItems[i]).Checked)
                    return i;
            return -1;
        }

        private ToolStripMenuItem getCheckedMenuItem(ToolStripMenuItem parentMenuItem)
        {
            for (int i = 0; i < parentMenuItem.DropDownItems.Count; i++)
            {
                ToolStripMenuItem item = (ToolStripMenuItem)parentMenuItem.DropDownItems[i];
                if (item.Checked)
                    return item;
            }
            return null;
        }

        // Return the lowest supported resolution that is at least the target resolution
        private int FindBestBitDepth(int[] bitDepths, int targetBitDepth)
        {
            int diff = int.MaxValue;

            int selectedBitDepth = 0;

            foreach (int bitDepth in bitDepths)
                if (bitDepth >= targetBitDepth && (bitDepth - targetBitDepth) < diff)
                {
                    diff = bitDepth - targetBitDepth;
                    selectedBitDepth = bitDepth;
                }

            return selectedBitDepth;
        }

        // Return the lowest supported bitmap that is at least the target bitmap
        private TwainResolution FindBestResolution(TwainResolution[] resolutions, int targetResolution)
        {
            float diff = int.MaxValue;

            TwainResolution selectedResolution = null;

            foreach (TwainResolution resolution in resolutions)
                if (resolution.X >= targetResolution && (resolution.X - targetResolution) < diff)
                {
                    diff = resolution.X - targetResolution;
                    selectedResolution = resolution;
                }


            return selectedResolution;
        }

        private void UpdateStatus()
        {
            ToolStripMenuItem miScannerItem = getCheckedMenuItem(miScanner);
            lblScanner.Text = miScannerItem != null ? miScannerItem.Text : "";

            ToolStripMenuItem miDocTypeItem = getCheckedMenuItem(miDocumentType);
            lblDocumentType.Text = miDocTypeItem != null ? string.Format("Document Type: {0}", miDocTypeItem.Text) : "";

            ToolStripMenuItem miPageSizeItem = getCheckedMenuItem(miPageSize);
            lblPageSize.Text = miPageSizeItem != null ? string.Format("Page Size:{0}", miPageSizeItem.Text) : "";

            picONormal.Image = ((ToolStripMenuItem)miRotation.DropDownItems[0]).Checked ? Properties.Resources.o_normal_enabled : Properties.Resources.o_normal_disabled;
            picO90.Image = ((ToolStripMenuItem)miRotation.DropDownItems[1]).Checked ? Properties.Resources.o_clock_enabled : Properties.Resources.o_clock_disabled;
            picO180.Image = ((ToolStripMenuItem)miRotation.DropDownItems[2]).Checked ? Properties.Resources.o_180_enabled : Properties.Resources.o_180_disabled;
            picO270.Image = ((ToolStripMenuItem)miRotation.DropDownItems[3]).Checked ? Properties.Resources.o_counter_enabled : Properties.Resources.o_counter_disabled;
        }

        private void RefreshView()
        {
            if (tViewer.SelectedIndices.Count() > 0)
            {
                foreach (int i in tViewer.SelectedIndices)
                {
                    RefreshView(i);
                    break;
                }

                picAttach.Image = Properties.Resources.attach_up;
            }
            else
            {
                iViewer.Visible = false;
                iViewer.Image = null;
                tViewer.Refresh();
                iViewer.Refresh();

                picAttach.Image = Properties.Resources.attach_disabled;
            }
        }

        private void RefreshView(int index)
        {
            if (index >= 0)
            {
                iViewer.Image = ScannedImages[index].ToBitmap();
                iViewer.Visible = true;
                picAttach.Image = Properties.Resources.attach_up;
            }
            else
            {
                iViewer.Visible = false;
                iViewer.Image = null;
                picAttach.Image = Properties.Resources.attach_disabled;
            }

            tViewer.Refresh();
            iViewer.Refresh();
        }

        #endregion

    }

    #region Helper Classes

    public class myDocType
    {
        public myDocType(DocumentType docType, int attributeId)
        {
            this.DocType = docType;
            this.TypeName = docType.TypeName;
            this.AttributeId = attributeId;
        }

        public DocumentType DocType { get; set; }
        public string TypeName { get; set; }
        public int AttributeId { get; set; }

        public override string ToString()
        {
            return this.TypeName;
        }
    }

    #endregion

}
