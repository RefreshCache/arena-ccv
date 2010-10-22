using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Principal;
using System.Configuration;

using Outlook = Microsoft.Office.Interop.Outlook;

using Arena.Core;
using Arena.Enums;
using Arena.Exceptions;

namespace OutlookSync
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnSync;
		private System.Windows.Forms.CheckedListBox clbTags;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.StatusBar sbStatus;
		private System.Windows.Forms.CheckBox cbClose;
        private CheckBox cbOverwritePicture;
        private CheckBox cbStaff;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			try
			{
				WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
				Person sPerson = new Person(wp.Identity.Name);
				if (sPerson.PersonID == -1)
				{
					MessageBox.Show("Your Login ID is not associated with a person record in Arena.  In order to import tag members, you must have a valid person record in Arena.");
					Application.Exit();
				}
				else
				{
					ProfileCollection pColl = new ProfileCollection();

					// Load Current User's Personal Tags
					pColl.LoadPrivateProfiles(
						Int32.Parse(ConfigurationSettings.AppSettings["Organization"]),
						sPerson.PersonID,
						true);

					// Load Current User's Subscribed Tags
					pColl.LoadSubscribedProfiles(
						Int32.Parse(ConfigurationSettings.AppSettings["Organization"]),
						sPerson.PersonID);

					// Remove any duplicate tag names
					for(int i = pColl.Count - 1; i >= 0; i--)
						for(int j = 0; j < pColl.Count; j++)
							if(j != i && pColl[j].ProfileID == pColl[i].ProfileID)
							{
								pColl.RemoveAt(i);
								break;
							}

					// Add each tag to the checkbox list
					foreach(Profile profile in pColl)
						clbTags.Items.Add(new CheckItem(profile.Title, profile.ProfileID), CheckState.Checked);
				}
			}
			catch(SystemException ex)
			{
				MessageBox.Show("An error occurred while attempting to retrieve your private tags:\n\n" + ex.Message, "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnSync = new System.Windows.Forms.Button();
            this.clbTags = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.sbStatus = new System.Windows.Forms.StatusBar();
            this.cbClose = new System.Windows.Forms.CheckBox();
            this.cbOverwritePicture = new System.Windows.Forms.CheckBox();
            this.cbStaff = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnSync
            // 
            this.btnSync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSync.Location = new System.Drawing.Point(149, 258);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(54, 20);
            this.btnSync.TabIndex = 4;
            this.btnSync.Text = "&Go";
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // clbTags
            // 
            this.clbTags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.clbTags.CheckOnClick = true;
            this.clbTags.Location = new System.Drawing.Point(3, 68);
            this.clbTags.Name = "clbTags";
            this.clbTags.Size = new System.Drawing.Size(200, 154);
            this.clbTags.TabIndex = 3;
            this.clbTags.SelectedIndexChanged += new System.EventHandler(this.clbTags_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select the additional Arena tags that you would like to import into Outlook:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // sbStatus
            // 
            this.sbStatus.Location = new System.Drawing.Point(0, 289);
            this.sbStatus.Name = "sbStatus";
            this.sbStatus.Size = new System.Drawing.Size(206, 19);
            this.sbStatus.TabIndex = 1;
            // 
            // cbClose
            // 
            this.cbClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbClose.Checked = true;
            this.cbClose.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbClose.Location = new System.Drawing.Point(3, 257);
            this.cbClose.Name = "cbClose";
            this.cbClose.Size = new System.Drawing.Size(120, 21);
            this.cbClose.TabIndex = 0;
            this.cbClose.Text = "Close After Sync";
            // 
            // cbOverwritePicture
            // 
            this.cbOverwritePicture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbOverwritePicture.Checked = true;
            this.cbOverwritePicture.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOverwritePicture.Location = new System.Drawing.Point(3, 232);
            this.cbOverwritePicture.Name = "cbOverwritePicture";
            this.cbOverwritePicture.Size = new System.Drawing.Size(120, 20);
            this.cbOverwritePicture.TabIndex = 5;
            this.cbOverwritePicture.Text = "Overwrite Pictures";
            // 
            // cbStaff
            // 
            this.cbStaff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbStaff.Checked = true;
            this.cbStaff.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbStaff.Location = new System.Drawing.Point(3, 11);
            this.cbStaff.Name = "cbStaff";
            this.cbStaff.Size = new System.Drawing.Size(120, 20);
            this.cbStaff.TabIndex = 6;
            this.cbStaff.Text = "Import All Staff";
            this.cbStaff.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnSync;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(206, 308);
            this.Controls.Add(this.cbStaff);
            this.Controls.Add(this.cbOverwritePicture);
            this.Controls.Add(this.cbClose);
            this.Controls.Add(this.sbStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clbTags);
            this.Controls.Add(this.btnSync);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Outlook Sync";
            this.Closed += new System.EventHandler(this.MainForm_Closed);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}

		private void btnSync_Click(object sender, System.EventArgs e)
		{
            //try
            //{
				btnSync.Enabled = false;
				this.Cursor = Cursors.WaitCursor;

				sbStatus.Text = "Connecting to Outlook...";
				Application.DoEvents();

                //Outlook.Application oApp = new Microsoft.Office.Interop.Outlook.ApplicationClass();
				Outlook.Application oApp = new Outlook.Application();
				Outlook.NameSpace oNS = oApp.GetNamespace("mapi");
				oNS.Logon("Outlook", Missing.Value, false, true);

				Outlook.MAPIFolder oContacts = oNS.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderContacts);
				Outlook.Items oItems = oContacts.Items;

                if (cbStaff.Checked)
                {
                    PersonCollection staff = new PersonCollection();
                    staff.LoadStaffMembers();
                    SyncPeople(oApp, oItems, "Staff", staff);
                }

				foreach(Object pItem in clbTags.CheckedItems)
				{
					Profile profile = new Profile(((CheckItem)pItem).ID);
                    SyncPeople(oApp, oItems, profile.Title, profile.Members);
					SyncChildProfiles(oApp, oItems, profile, true);
				}
				oNS.Logoff();

				oApp = null;
				oNS = null;
				oItems = null;

				this.Cursor = Cursors.Default;
				MessageBox.Show("Outlook contacts have been updated", "Import Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch(System.Exception ex)
            //{
            //    this.Cursor = Cursors.Default;
            //    MessageBox.Show("An error occurred while updating your contacts:\n\n" + ex.Message, "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
			
            //finally
            //{
            //}
		
			sbStatus.Text = string.Empty;
			setButtonEnabled();
			if(cbClose.Checked)
				this.Close();
		}

		private void SyncChildProfiles(Outlook.Application oApp, Outlook.Items oItems, Profile profile, bool Prompt)
		{
			bool Continue = true;
			if(profile.ChildProfiles.Count > 0)
			{
				if(Prompt)
				{
					StringBuilder sb = new StringBuilder();
					foreach(Profile ChildProfile in profile.ChildProfiles)
						sb.AppendFormat("\t{0}\n", ChildProfile.Title);
					Continue = (MessageBox.Show(
						string.Format("You have selected the '{0}' tag.  This tag also contains the following child tags:\n\n{1}\n" +
						"Do you want to sync the members of these child tags also?", profile.Title, sb.ToString()),
						"Sync Child Tag Members",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Question) == DialogResult.Yes);
				}

				if (Continue)
					foreach(Profile ChildProfile in profile.ChildProfiles)
					{
                        SyncPeople(oApp, oItems, ChildProfile.Title, ChildProfile.Members);
						SyncChildProfiles(oApp, oItems, ChildProfile, false);
					}
			}
		}

		private void SyncPeople(Outlook.Application oApp, Outlook.Items oItems, string title, PersonCollection people)
		{
            DirectoryInfo cacheRoot = null;
            FileInfo cacheFile = null;

            // get image cache path
            if (ConfigurationManager.AppSettings["ImagePath"] != null)
                cacheRoot = new DirectoryInfo(ConfigurationManager.AppSettings["ImagePath"]);
            else
                throw new ArenaApplicationException("'ImagePath' configuration file setting is not set");

            // Save the image to the cache file
            if (cacheRoot != null)
            {

                // if the root directory provided doesn't exist then create it
                if (!cacheRoot.Exists)
                {
                    try
                    {
                        cacheRoot.Create();
                    }
                    catch (System.Exception ex)
                    {
                        throw new ArenaApplicationException("Could not create '" + cacheRoot.FullName + "' directory!", ex);
                    }
                }

                cacheFile = new FileInfo(cacheRoot.FullName + "\\TempSocsImage.jpg");
            }

			Outlook.ContactItem oCt;

            foreach(Person person in people)
            {
				sbStatus.Text = "Syncing " + person.FullName + "...";
				Application.DoEvents();

				oCt = (Outlook.ContactItem)oItems.Find("[OrganizationalIDNumber] = " + person.PersonID.ToString());
				if (oCt == null)
					oCt = (Outlook.ContactItem)oApp.CreateItem(Outlook.OlItemType.olContactItem);
						
				oCt.OrganizationalIDNumber = person.PersonID.ToString();

				string categories = oCt.Categories;
				if (categories == null)
					categories = string.Empty;
				if (categories.IndexOf(title) < 0)
				{
					if (categories.Length > 0)
						categories += "; ";
					categories += title;
					oCt.Categories = categories;
				}

				oCt.FirstName = person.FirstName;
				oCt.LastName = person.LastName;

                if (!oCt.HasPicture || cbOverwritePicture.Checked)
                    if (person.Blob != null && 
                        person.Blob.ByteArray != null &&
                        person.Blob.ByteArray.Length > 0)
                    {
                        if (oCt.HasPicture)
                            oCt.RemovePicture();

                        // delete the cache file if it exists (blob has been updated)
                        if (cacheFile != null && cacheFile.Exists)
                        {
                            try { cacheFile.Delete(); }
                            catch (System.Exception ex) {throw new ArenaApplicationException("Could not delete '" + cacheFile.FullName + "' file!", ex); }
                        }

                        // write cache file
                        if (cacheFile != null)
                        {
                            FileStream fs = null;

                            try
                            {
                                Image image = person.Blob.GetImage(0, 0);

                                EncoderParameters eps = new EncoderParameters(1);
                                eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                                ImageCodecInfo ici = GetEncoderInfo("image/jpeg");

                                if (ici != null)
                                {
                                    fs = cacheFile.OpenWrite();
                                    image.Save(fs, ici, eps);
                                }
                            }
                            catch (System.Exception ex)
                            {
                                throw new ArenaApplicationException("ArenaImage: Could not save cache file!", ex);
                            }
                            finally
                            {
                                if (fs != null)
                                    fs.Close();
                            }

                            oCt.AddPicture(cacheFile.FullName);
                        }
                    }

				foreach(PersonPhone phone in person.Phones)
				{
					switch (phone.PhoneType.Value)
					{
						case "Main/Home":
							oCt.HomeTelephoneNumber = phone.Number;
							break;
						case "Internal Phone":
							oCt.BusinessTelephoneNumber = phone.Number;
							break;
						case "Personal":
							oCt.Home2TelephoneNumber = phone.Number;
							break;
						case "FAX":
							oCt.BusinessFaxNumber = phone.Number;
							break;
						case "Messages":
							oCt.CallbackTelephoneNumber = phone.Number;
							break;
						case "Pager":
							oCt.PagerNumber = phone.Number;
							break;
						case "Cell":
							oCt.MobileTelephoneNumber = phone.Number;
							break;
						case "Other Phone 1":
							oCt.OtherTelephoneNumber = phone.Number;
							break;
						case "Home Fax":
							oCt.HomeFaxNumber = phone.Number;
							break;
						case "alpha pager":
							oCt.PagerNumber = phone.Number;
							break;
						case "Voice mail":
							oCt.CallbackTelephoneNumber = phone.Number;
							break;
					}
				}
					
				foreach(PersonAddress address in person.Addresses)
				{
					switch (address.AddressType.Value)
					{
						case "Main/Home Address":
							oCt.HomeAddressStreet = address.Address.StreetLine1;
							oCt.HomeAddressCity = address.Address.City;
							oCt.HomeAddressState = address.Address.State;
							oCt.HomeAddressPostalCode = address.Address.PostalCode;
							break;
						default:
							oCt.OtherAddressStreet = address.Address.StreetLine1;
							oCt.OtherAddressCity = address.Address.City;
							oCt.OtherAddressState = address.Address.State;
							oCt.OtherAddressPostalCode = address.Address.PostalCode;
							break;
					}
				}

				if (person.Emails.Active.Count > 0)
				{
					oCt.Email1Address = person.Emails.Active[0].Email;
					oCt.Email1DisplayName = person.FullName;
				}

				switch (person.Gender)
				{
					case Gender.Female:
						oCt.Gender = Outlook.OlGender.olFemale;
						break;
					case Gender.Male:
						oCt.Gender = Outlook.OlGender.olMale;
						break;
					default:
						oCt.Gender = Outlook.OlGender.olUnspecified;
						break;
				}

				oCt.NickName = person.NickName;
				oCt.Save();
			}

			oCt = null;

		}

		private void MainForm_Closed(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            setButtonEnabled();
        }

		private void clbTags_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			setButtonEnabled();
		}
		private void setButtonEnabled()
		{
			btnSync.Enabled = (clbTags.CheckedItems.Count > 0 || cbStaff.Checked);
		}

        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType.ToLower() == mimeType.ToLower())
                    return encoders[j];
            }
            return null;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }

	public class CheckItem : Object
	{
		public string Name;
		public int ID;

		public CheckItem(string name, int id)
		{
			Name = name;
			ID = id;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
