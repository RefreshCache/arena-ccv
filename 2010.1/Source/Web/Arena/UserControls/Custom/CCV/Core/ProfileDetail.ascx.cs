namespace ArenaWeb.UserControls.Custom.CCV.Core
{
	using System;
    using System.ComponentModel;
    using System.Collections.Generic;
	using System.Text;
	using System.Data;
	using System.Data.SqlClient;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Configuration;
    using System.Reflection;

	using Arena.Enums;
	using Arena.Core;
	using Arena.Marketing;
	using Arena.Organization;
	using Arena.Portal;
	using Arena.Portal.UI;
	using Arena.Security;
	using Arena.Exceptions;
	using Arena.DataLayer.Core;
	using Arena.DataLayer.Marketing;

    using ArenaWeb.UserControls.Core;
    using Arena.Custom.CCV.Core;
    using Arena.Event;

	/// <summary>
	///		Summary description for ProfileDetail.
	/// </summary>
	public partial class ProfileDetail : PortalControl
	{
		#region Module Settings

		// Module Settings
		[BooleanSetting("Popup View", "Flag indicating if this control is residing in a popup window.", false, false)]
		public string PopUpSetting { get { return Setting("popup", "", false); } }

		[PageSetting("Person Popup Search Page", "The page that is used for the popup search.", false, 16)]
		public string PopupSearchWindowSetting { get { return Setting("PopupSearchWindow", "16", false); } }

		[TextSetting("Experience Label", "The label to use if you want it to be something other than 'Experience Skills.'", false)]
		public string ExperienceLabelSetting { get { return Setting("ExperienceLabel", "Experience Skills", false); } }

		[TextSetting("QualifierCaption", "Profiles can optionally capture a qualifier value.  There must be a 'Profile Qualifier' Lookup type, and you must specify a caption here for the qualifier for the field to be displayed.", false)]
		public string QualifierCaptionSetting { get { return Setting("QualifierCaption", "", false); } }

		[PageSetting("Person Detail Page", "The page that is used for displaying person details.", false, 7)]
		public string PersonDetailsPageSetting { get { return Setting("PersonDetailsPage", "7", false); } }

		[ReportSetting("Roster Report", "The Report to use for the Roster. The Report's only parameter should be ProfileID. Default:'CheckInRoster_Tag'", false, SelectionMode.Reports, "/Arena/CheckIn/CheckInRoster_Tag")]
		public string RosterReportURLSetting { get { return Setting("RosterReportURL", "/Arena/CheckIn/CheckInRoster_Tag", false); } }

        #endregion

		#region Private Variables

		protected string tagTitle = "Tag";

		private bool ViewEnabled = false;
		private bool EditEnabled = false;
		private bool ApproveEnabled = false;

		private string StateID = "Profile";

		private Arena.Custom.CCV.Core.Profile parentProfile = null;
        private Arena.Custom.CCV.Core.Profile profile = null;
        private ServingProfile sProfile = null;

		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;

			btnRefresh.CausesValidation = false;

			if (!Page.IsPostBack)
			{
				Session["ProfileDetail"] = null;
				Session["SProfileDetail"] = null;
			}

			tstOverview.PageViewId = pvOverview.ClientID;
			tstServingCriteria.PageViewId = pvServingCriteria.ClientID;
			tstServingDetails.PageViewId = pvServingDetails.ClientID;
            tstFields.PageViewId = pvFields.ClientID;

			RegisterScripts();

			if (profile != null)
			{
				tagTitle = CurrentOrganization.GetProfileTypeName(profile.ProfileType);
				if (profile.ProfileType == ProfileType.Event)
					throw new ModuleException(CurrentPortalPage, CurrentModule, "Module does not support Events");
				
				reqProfileName.ErrorMessage = tagTitle + " Name is Required!";

				this.CurrentPortalPage.TemplateControl.Title = profile.Title;

				this.CurrentPortalPage.TemplateControl.ImageURL = string.Format("~/images/{0}_{1}.jpg",
					CurrentPortalPage.PortalPageID.ToString(),
					Enum.GetName(typeof(ProfileType), profile.ProfileType));

				if (profile.ProfileID == -1 && parentProfile == null)
				{
					ViewEnabled = false;
				}
				else
				{
					if (profile.ProfileID == -1 && parentProfile != null && parentProfile.ProfileID == -1)
					{
						EditEnabled = CurrentModule.Permissions.Allowed(OperationType.Edit, CurrentUser);
						ViewEnabled = true;
					}
					else
					{
						ApproveEnabled = profile.Allowed(OperationType.Approve, CurrentUser, CurrentPerson, true, false);
						EditEnabled = ApproveEnabled || profile.Allowed(OperationType.Edit, CurrentUser, CurrentPerson, true, false);
						ViewEnabled = EditEnabled || profile.Allowed(OperationType.View, CurrentUser, CurrentPerson, true, false);
					}
				}

				if (ViewEnabled)
				{
					StateID = "Profile" + profile.ProfileID;

					if (Page.IsPostBack)
					{
						if (ihFilePhoto.Value != string.Empty && ihFilePhoto.PostedFile != null)
						{
							byte[] fileBytes = new byte[ihFilePhoto.PostedFile.ContentLength];
							ihFilePhoto.PostedFile.InputStream.Read(fileBytes, 0, fileBytes.Length);
							profile.Blob.ByteArray = fileBytes;
							profile.Blob.SetFileInfo(ihFilePhoto.PostedFile.FileName);
							Session[profile.Blob.GUID.ToString()] = profile.Blob;
						}

						if (profile.ProfileType == ProfileType.Serving && ihFileServingPhoto.Value != string.Empty && ihFileServingPhoto.PostedFile != null)
						{
							byte[] fileBytes = new byte[ihFileServingPhoto.PostedFile.ContentLength];
							ihFileServingPhoto.PostedFile.InputStream.Read(fileBytes, 0, fileBytes.Length);
							sProfile.ServingBlob.ByteArray = fileBytes;
							sProfile.ServingBlob.SetFileInfo(ihFileServingPhoto.PostedFile.FileName);
							Session[sProfile.ServingBlob.GUID.ToString()] = sProfile.ServingBlob;
						}

					}
					else
					{
						if (profile.ProfileType == ProfileType.Serving)
						{
							new LookupType(SystemLookupType.ServingWeeklyCommitment).Values.LoadDropDownList(cblWeeklyCommitment);
							new LookupType(SystemLookupType.ServingTimeframe).Values.LoadDropDownList(cblTimeframe);
							new LookupType(SystemLookupType.ServingClassification).Values.LoadDropDownList(cblClassification);
							new LookupType(SystemLookupType.ContentCategory).Values.LoadDropDownList(cblContentCategory);
							new LookupType(SystemLookupType.ServingDuration).Values.LoadDropDownList(cblDuration);
                            new LookupType(SystemLookupType.SpiritualGifts).Values.LoadDropDownList(cblSpiritualGifts);
						}

						if (profile.ProfileID == -1 && parentProfile != null)
							ShowEdit();
						else
							ShowView();
					}
				}
				else
				{
					pnlView.Visible = false;
					pnlEdit.Visible = false;
				}
			}
		}

		private void RegisterScripts()
		{
			StringBuilder sbScript = new StringBuilder();
			sbScript.Append("\n\n<script language=\"javascript\">\n");
			sbScript.Append("\tfunction openSearchWindow(searchType)\n");
			sbScript.Append("\t{\n");
			sbScript.Append("\t\tvar tPos = window.screenTop + 100;\n");
			sbScript.Append("\t\tvar lPos = window.screenLeft + 100;\n");
			sbScript.AppendFormat("\t\tdocument.frmMain.ihPersonListID.value = '{0}';\n", ihPersonList.ClientID);
			sbScript.AppendFormat("\t\tdocument.frmMain.ihRefreshButtonID.value = '{0}';\n", btnRefresh.ClientID);
			sbScript.AppendFormat("\t\tvar searchWindow = window.open('Default.aspx?page={0}','Search','height=400,width=600,resizable=yes,scrollbars=yes,toolbar=no,location=no,directories=no,status=no,menubar=no,top=' + tPos + ',left=' + lPos);\n",PopupSearchWindowSetting);
			sbScript.Append("\t\tsearchWindow.focus();\n");
			sbScript.Append("\t}\n");
			sbScript.Append("</script>\n\n");

			Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OpenSearchWindow", sbScript.ToString());
		}

		private void ShowView()
		{
			foreach (Control ctrl in this.Parent.Controls)
				if (ctrl is ProfileMain)
					ctrl.Visible = true;

            lblProfileName.Text = profile.Name;
            
            string parentName = Arena.Utility.ArenaTextTools.Pluralize(tagTitle);
            Arena.Custom.CCV.Core.Profile parentProfile = new Arena.Custom.CCV.Core.Profile(profile.ParentProfileID);
			if (parentProfile.ProfileID != -1)
				lblParentProfile.Text = string.Format("<a href='default.aspx?page={0}&profile={1}'>{2}</a>",
					CurrentPortalPage.PortalPageID.ToString(),
					parentProfile.ProfileID.ToString(),
					parentProfile.Title);
			else
				lblParentProfile.Text = string.Format("<a href='default.aspx?page={0}&profiletype={1}'>{2}</a>",
					CurrentPortalPage.PortalPageID.ToString(),
					Enum.Format(typeof(ProfileType), profile.ProfileType, "D"),
					parentName);

            trCampus.Visible = CurrentOrganization.CampusesExist;
            lblCampus.Text = profile.Campus != null ? profile.Campus.Name : string.Empty;

			plOwner.Person = profile.Owner;
			plOwner.PersonPageID = Convert.ToInt32(PersonDetailsPageSetting);
			plOwner.PersonUrlTarget = "_blank";

			if (QualifierCaptionSetting != string.Empty)
			{
				trQualifier.Visible = true;
				lblQualifierLabel.Text = QualifierCaptionSetting;
				lblQualifier.Text = profile.Qualifier.Value;
			}
			else
				trQualifier.Visible = false;

			if (profile.Active)
				lblActive.Text = "Yes";
			else
				lblActive.Text = "No";

			trSummaryLabel.Visible = (profile.Summary != string.Empty);
			trSummaryValue.Visible = trSummaryLabel.Visible;
			lblSummary.Text = Utilities.replaceCRLF(profile.Summary);

			trNotesLabel.Visible = (profile.Notes != string.Empty);
			trNotesValue.Visible = trNotesLabel.Visible;
			lblNotes.Text = Utilities.replaceCRLF(profile.Notes);

			if (profile.Blob.BlobID != -1)
			{
				string photoLink = string.Format("<a target='_blank' href='CachedBlob.aspx?guid={0}' style='text-decoration:none'>" +
					"<img border='0' src='CachedBlob.aspx?width=250&height=250&guid={0}'></a>", HttpUtility.UrlEncode(profile.Blob.GUID.ToString()));
				lblPhoto.Text = photoLink;
				lblPhoto.Visible = true;
			}
			else
			{
				lblPhoto.Text = string.Empty;
				lblPhoto.Visible = false;
			}

			if (profile.ProfileType == ProfileType.Serving)
			{
				trHoursView.Visible = true;
                ServingProfile sProfile = new ServingProfile(profile.ProfileID);
				lblHours.Text = sProfile.DefaultHoursPerWeek.ToString();

				trCategoryLevel.Visible = true;
				if (profile.CategoryLevel)
					lblCategoryLevel.Text = "Yes";
				else
					lblCategoryLevel.Text = "No";
			}
			else
			{
				trHoursView.Visible = false;
				trCategoryLevel.Visible = false;
			}
			btnPrintRoster.Visible = (RosterReportURLSetting != string.Empty);

			btnEdit.Visible = EditEnabled;
			pnlEdit.Visible = false;
			pnlView.Visible = true;
		}

		private void ShowEdit()
		{
			foreach (Control ctrl in this.Parent.Controls)
				if (ctrl is ProfileMain)
					ctrl.Visible = false;

            if (CurrentOrganization.CampusesExist)
            {
                trCampusEdit.Visible = true;
                ddlCampus.Items.Clear();
                if (profile.Campus != null)
                    CurrentOrganization.Campuses.LoadDropDownList(ddlCampus, "All", profile.Campus.CampusId.ToString());
                else
                    CurrentOrganization.Campuses.LoadDropDownList(ddlCampus, "All");
            }
            else
                trCampusEdit.Visible = false;

			if (profile.Owner.PersonID != -1)
			{
				lblOwnerEdit.Text = profile.Owner.FullName;
				ihOwnerID.Value = profile.Owner.PersonID.ToString();
			}
			else
			{
				lblOwnerEdit.Text = string.Empty;
				ihOwnerID.Value = string.Empty;
			}

			lbRemoveOwner.Visible = (profile.Owner.PersonID != -1 && profile.ProfileType != ProfileType.Personal);

			if (QualifierCaptionSetting != string.Empty)
			{
				trQualifierEdit.Visible = true;
				lblQualifierEdit.Text = QualifierCaptionSetting;
				ddlQualifier.Items.Clear();
				new LookupType(SystemLookupType.ProfileQualifier).Values.LoadDropDownList(ddlQualifier, profile.Qualifier.LookupID);
			}
			else
				trQualifierEdit.Visible = false;
			
			//Make roots selectable.
			profilePicker.SelectableRoots = true;

			//Disable the profile id as it can't be it's own parent or it's children.
            foreach (Arena.Core.Profile pro in profile.ChildProfiles)
            {
                profilePicker.DisabledProfileIds += pro.ProfileID.ToString() + "|";
            }
            profilePicker.DisabledProfileIds += profile.ProfileID.ToString();

			//New Profile Picker Control.  Always set ProfileType before setting profile ID IF -1 is a possible 
			//profile ID value.  This allows us to know which "root" is selected. Because all roots are -1.
			//If this doesn't get set and the profile id is -1, expect problems.
			profilePicker.ProfileType = profile.ProfileType;
			profilePicker.ProfileID = profile.ParentProfileID;

			btnUpdate.Attributes.Add("onclick", string.Format("return typeChanged('{0}','{1}','{2}');", profilePicker.ClientID, Convert.ToInt32(profile.ProfileType), CurrentOrganization.GetProfileTypeName(ProfileType.Personal)));

			tbProfileName.Text = profile.Name;
			cbActive.Checked = profile.Active;
            tbOwnerRelationship.Text = profile.OwnerRelationshipStrength.ToString();
            tbPeerRelationship.Text = profile.PeerRelationshipStrength.ToString();
			cbCategoryLevel.Checked = profile.CategoryLevel;

			if (pnlPhotoUpdate.Attributes["onclick"] == null)
			{
				StringBuilder photoScript = new StringBuilder();
				photoScript.AppendFormat("javascript:");
				photoScript.AppendFormat("document.frmMain.{0}.style.display='inline';", ihFilePhoto.ClientID);
				photoScript.AppendFormat("document.frmMain.{0}.style.display='inline';", btnUploadPhoto.ClientID);
				photoScript.AppendFormat("document.frmMain.{0}.style.visibility='visible';", ihFilePhoto.ClientID);
				photoScript.AppendFormat("document.frmMain.{0}.style.visibility='visible';", btnUploadPhoto.ClientID);
				photoScript.Append("this.style.display='none';");
				photoScript.Append("this.style.visibility='hidden';");
				pnlPhotoUpdate.Attributes.Add("onclick", photoScript.ToString());
			}

			if (profile.ProfileType == ProfileType.Serving)
			{
				tstServingCriteria.Visible = true;
				tstServingDetails.Visible = true;

				rvHours.Enabled = true;
				rvVolunteersNeeded.Enabled = true;
				revContactEmail.Enabled = true;

				tbHours.Text = sProfile.DefaultHoursPerWeek.ToString();

				cbDisplayToPublic.Checked = sProfile.DisplayPublic;

				foreach (ListItem li in cblWeeklyCommitment.Items)
					li.Selected = (sProfile.Attributes.FindByID(Int32.Parse(li.Value)) != null);
				foreach (ListItem li in cblTimeframe.Items)
					li.Selected = (sProfile.Attributes.FindByID(Int32.Parse(li.Value)) != null);
				foreach (ListItem li in cblClassification.Items)
					li.Selected = (sProfile.Attributes.FindByID(Int32.Parse(li.Value)) != null);
				foreach (ListItem li in cblContentCategory.Items)
					li.Selected = (sProfile.Attributes.FindByID(Int32.Parse(li.Value)) != null);
				foreach (ListItem li in cblDuration.Items)
					li.Selected = (sProfile.Attributes.FindByID(Int32.Parse(li.Value)) != null);
                foreach (ListItem li in cblSpiritualGifts.Items)
                    li.Selected = (sProfile.Attributes.FindByID(Int32.Parse(li.Value)) != null);

				cbCriticalNeed.Checked = sProfile.CriticalNeed;
				tbVolunteersNeeded.Text = sProfile.VolunteersNeeded.ToString();
				tbContactInfo.Text = sProfile.ContactInfo;
				tbContactEmail.Text = sProfile.ContactEmail;
				tbServingSummary.Text = sProfile.Summary;
				tbServingDetails.Text = sProfile.Details;
				tbExperience.Text = sProfile.ExperienceSkills;
				tbScheduleNotes.Text = sProfile.ScheduleNotes;
				tbVideoLink.Text = sProfile.VideoLink;

				lblExperience.Text = ExperienceLabelSetting;

				if (pnlServingPhotoUpdate.Attributes["onclick"] == null)
				{
					StringBuilder photoScript = new StringBuilder();
					photoScript.AppendFormat("javascript:");
					photoScript.AppendFormat("document.frmMain.{0}.style.display='inline';", ihFileServingPhoto.ClientID);
					photoScript.AppendFormat("document.frmMain.{0}.style.display='inline';", btnUploadServingPhoto.ClientID);
					photoScript.AppendFormat("document.frmMain.{0}.style.visibility='visible';", ihFileServingPhoto.ClientID);
					photoScript.AppendFormat("document.frmMain.{0}.style.visibility='visible';", btnUploadServingPhoto.ClientID);
					photoScript.Append("this.style.display='none';");
					photoScript.Append("this.style.visibility='hidden';");
					pnlServingPhotoUpdate.Attributes.Add("onclick", photoScript.ToString());
				}
			}
			else
			{
				tstServingCriteria.Visible = false;
				tstServingDetails.Visible = false;

				rvHours.Enabled = false;
				rvVolunteersNeeded.Enabled = false;
				revContactEmail.Enabled = false;
			}

            // Custom Fields
            ShowFieldModuleList();
            dlFieldModules.EditItemIndex = -1;

            ShowFieldList();
            dlFields.EditItemIndex = -1;

			showImages();

			tbNotes.Text = profile.Notes;
			btnUpdate.Visible = EditEnabled;
            btnCancel.Visible = true;

			pnlView.Visible = false;
			pnlEdit.Visible = true;
		}

		public void showImages()
		{
			if (profile.Blob != null &&
				profile.Blob.ByteArray != null &&
				profile.Blob.ByteArray.Length > 0)
			{
				string photoLink = string.Format("<a target='_blank' href='CachedBlob.aspx?guid={0}' style='text-decoration:none'>" +
					"<img border='0' src='CachedBlob.aspx?width=120&height=120&guid={0}'></a>", HttpUtility.UrlEncode(profile.Blob.GUID.ToString()));
				lblEditPhoto.Text = photoLink;
				lblEditPhoto.Visible = true;
				lblRemovePhoto.Visible = true;
			}
			else
			{
				lblEditPhoto.Text = string.Empty;
				lblEditPhoto.Visible = false;
				lblRemovePhoto.Visible = false;
			}

			if (profile.ProfileType == ProfileType.Serving &&
				sProfile.ServingBlob != null &&
				sProfile.ServingBlob.ByteArray != null &&
				sProfile.ServingBlob.ByteArray.Length > 0)
			{
				string photoLink = string.Format("<a target='_blank' href='CachedBlob.aspx?guid={0}' style='text-decoration:none'>" +
					"<img border='0' src='CachedBlob.aspx?width=120&height=120&guid={0}'></a>", HttpUtility.UrlEncode(sProfile.ServingBlob.GUID.ToString()));
				lblEditServingPhoto.Text = photoLink;
				lblEditServingPhoto.Visible = true;
				lblRemoveServingPhoto.Visible = true;
			}
			else
			{
				lblEditServingPhoto.Text = string.Empty;
				lblEditServingPhoto.Visible = false;
				lblRemoveServingPhoto.Visible = false;
			}
		}

		private void parseProfileCollection(ProfileCollection profiles, ListControl listControl, int iCurrent, int iSkip, int indentCount)
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < indentCount; i++)
				sb.Append("--");

			indentCount += 1;

			foreach (Arena.Custom.CCV.Core.Profile profile in profiles)
			{
				if (profile.ProfileID != iSkip)
				{
					string name = profile.Title.Trim();
					if (name.Length > 3)
						if (name.Substring(name.Length - 3, 3) == "(S)")
							name = name.Substring(0, name.Length - 3);

					ListItem li = new ListItem(sb.ToString() + name, profile.ProfileID.ToString());
					if (indentCount == 1)
						li.Text = li.Text.ToUpper();
					li.Selected = (profile.ProfileID == iCurrent);
					listControl.Items.Add(li);
					parseProfileCollection(profile.ChildProfiles, listControl, iCurrent, iSkip, indentCount);
				}
			}
		}

		protected void btnEdit_Click(object sender, System.EventArgs e)
		{
            LoadProfile(true);
			ShowEdit();
		}

		protected void btnUpdate_Click(object sender, System.EventArgs e)
		{
			if (Page.IsValid)
			{
				if (ihOwnerID.Value != string.Empty)
					profile.Owner = new Person(Int32.Parse(ihOwnerID.Value));
				else
					profile.Owner = new Person();

				ProfileType oProfileType = profile.ProfileType;
				profile.ParentProfileID = profilePicker.ProfileID;
				profile.Name = tbProfileName.Text;

                if (CurrentOrganization.CampusesExist && ddlCampus.SelectedValue != string.Empty)
                    profile.Campus = new Campus(Int32.Parse(ddlCampus.SelectedValue));
                else
                    profile.Campus = null;

				if (QualifierCaptionSetting != string.Empty)
					profile.Qualifier = new Lookup(Int32.Parse(ddlQualifier.SelectedValue));
				else
					profile.Qualifier = null;

				profile.Active = cbActive.Checked;

				try { profile.OwnerRelationshipStrength = Int32.Parse(tbOwnerRelationship.Text); }
				catch { profile.OwnerRelationshipStrength = 0; }

				try { profile.PeerRelationshipStrength = Int32.Parse(tbPeerRelationship.Text); }
				catch { profile.PeerRelationshipStrength = 0; }

				profile.CategoryLevel = cbCategoryLevel.Checked;
				profile.Notes = tbNotes.Text;

				if (profile.Blob.ByteArray != null && profile.Blob.ByteArray.Length > 0)
					profile.Blob.Save(CurrentUser.Identity.Name);

				if (profile.ProfileType == ProfileType.Serving)
					profile.Summary = tbServingSummary.Text;

				if (profilePicker.ProfileType != oProfileType)
				{
					ChangeChildrenType(profilePicker.ProfileType, profile.ChildProfiles);
				}

				//Must set profile type AFTER getting children, as it gets based on the profile's current type.
				profile.ProfileType = profilePicker.ProfileType;
				if (profilePicker.ProfileType == ProfileType.Personal)
					profile.Owner = CurrentPerson;
				profile.Save(CurrentUser.Identity.Name);
                profile.SaveFields(CurrentUser.Identity.Name);
				
				//This save will only be called if the type was already serving, otherwise we are dealing with a new serving profile.                
				if (profile.ProfileType == ProfileType.Serving && oProfileType == ProfileType.Serving)
				{
					sProfile.ProfileID = profile.ProfileID;

					if (tbHours.Text != string.Empty)
						try { sProfile.DefaultHoursPerWeek = Decimal.Parse(tbHours.Text); }
						catch { sProfile.DefaultHoursPerWeek = 0; }
					else
						sProfile.DefaultHoursPerWeek = 0;

					sProfile.Attributes.Clear();

					foreach (ListItem item in cblWeeklyCommitment.Items)
						if (item.Selected)
							sProfile.Attributes.Add(new Lookup(Int32.Parse(item.Value)));
					foreach (ListItem item in cblTimeframe.Items)
						if (item.Selected)
							sProfile.Attributes.Add(new Lookup(Int32.Parse(item.Value)));
					foreach (ListItem item in cblClassification.Items)
						if (item.Selected)
							sProfile.Attributes.Add(new Lookup(Int32.Parse(item.Value)));
					foreach (ListItem item in cblContentCategory.Items)
						if (item.Selected)
							sProfile.Attributes.Add(new Lookup(Int32.Parse(item.Value)));
					foreach (ListItem item in cblDuration.Items)
						if (item.Selected)
							sProfile.Attributes.Add(new Lookup(Int32.Parse(item.Value)));
                    foreach (ListItem item in cblSpiritualGifts.Items)
                        if (item.Selected)
                            sProfile.Attributes.Add(new Lookup(Int32.Parse(item.Value)));

					sProfile.DisplayPublic = cbDisplayToPublic.Checked;
					sProfile.CriticalNeed = cbCriticalNeed.Checked;
					sProfile.VolunteersNeeded = Int32.Parse(tbVolunteersNeeded.Text);
					sProfile.ContactInfo = tbContactInfo.Text;
					sProfile.ContactEmail = tbContactEmail.Text;
					sProfile.Details = tbServingDetails.Text;
					sProfile.ExperienceSkills = tbExperience.Text;
					sProfile.ScheduleNotes = tbScheduleNotes.Text;
					sProfile.VideoLink = tbVideoLink.Text;

					if (sProfile.ServingBlob.ByteArray != null && sProfile.ServingBlob.ByteArray.Length > 0)
						sProfile.ServingBlob.Save(CurrentUser.Identity.Name);

					sProfile.Save();
				}
                else if (profile.ProfileType == ProfileType.Serving)
                {
                    //Type has been changed from something else to Serving, Create a default Serving Profile and save it.
                    //This will also load and save a profile that has been changed to and from serving keeping their values.
                    sProfile = new ServingProfile(profile.ProfileID);
                    sProfile.Save();
                }

				CurrentPerson.Cache.InvalidateProfileCache();

				Session[StateID] = profile;

				this.CurrentPortalPage.TemplateControl.Title = profile.Title;

				Response.Redirect(string.Format("~/default.aspx?page={0}&profile={1}",
					CurrentPortalPage.PortalPageID.ToString(),
					profile.ProfileID.ToString()), true);				
			}
			else
				Page.FindControl("valSummary").Visible = true;
		}

		protected void ChangeChildrenType(ProfileType type, ProfileCollection profiles)
		{
			foreach (Arena.Custom.CCV.Core.Profile child in profiles)
			{
				ChangeChildrenType(type, child.ChildProfiles);
				child.ProfileType = type;
				if (type == ProfileType.Personal)
					child.Owner = CurrentPerson;
                if (type == ProfileType.Serving)
                {
                    //Make a serving profile for it if none exist.
                    ServingProfile childServing = new ServingProfile(child.ProfileID);
                    childServing.Save();
                }                    
				child.Save(CurrentUser.Identity.Name);				
			}
		}

		protected void btnCancel_Click(object sender, System.EventArgs e)
		{
			int redirectProfileID = profile.ProfileID == -1 ? profile.ParentProfileID : profile.ProfileID;
			Response.Redirect(string.Format("~/default.aspx?page={0}&profile={1}&profiletype={2}",
				CurrentPortalPage.PortalPageID.ToString(),
				redirectProfileID.ToString(),
				Enum.Format(typeof(ProfileType), profile.ProfileType, "D")), true);
		}

		private void lbRemoveOwner_Click(object sender, System.EventArgs e)
		{
			profile.Owner = new Person();
			ihOwnerID.Value = string.Empty;

			Session[StateID] = Profile;

			lblOwnerEdit.Text = string.Empty;
			lbRemoveOwner.Visible = false;
		}

		protected void btnRefresh_Click(object sender, System.EventArgs e)
		{
			string[] NewPersonIDs = ihPersonList.Value.Split(',');
			foreach (string id in NewPersonIDs)
			{
				if (id.Trim() != string.Empty)
				{
					profile.Owner = new Person(Int32.Parse(id));
					lblOwnerEdit.Text = profile.Owner.FullName + "<br>";
					lbRemoveOwner.Visible = (profile.Owner.PersonID != -1 && profile.ProfileType != ProfileType.Personal);

					ihOwnerID.Value = id;
					Session[StateID] = Profile;
				}
			}
			ihPersonList.Value = string.Empty;
		}

		private void btnUploadPhoto_Click(object sender, EventArgs e)
		{
			showImages();
		}

		private void lblRemovePhoto_Click(object sender, EventArgs e)
		{
			profile.Blob.Delete();
			Session[profile.Blob.GUID.ToString()] = null;
			showImages();
		}

		private void btnUploadServingPhoto_Click(object sender, EventArgs e)
		{
			showImages();
		}

		private void lblRemoveServingPhoto_Click(object sender, EventArgs e)
		{
			sProfile.ServingBlob.Delete();
			Session[sProfile.ServingBlob.GUID.ToString()] = null;
			showImages();
		}

		void btnPrintRoster_Click(object sender, EventArgs e)
		{
			string script = string.Format("window.open('ReportPDFViewer.aspx?Report={0}&ProfileID={1}');", RosterReportURLSetting, this.profile.ProfileID.ToString());
			Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ReportViewer", script, true);
		}

		public override INavigable GetNavigableItem()
		{
			LoadProfile();
			return profile;
        }

        private void LoadProfile()
        {
            LoadProfile(false);
        }

		private void LoadProfile(bool reload)
		{
			ProfileType profileType = ProfileType.Undefined;

			if (profile == null || reload)
			{
                int ProfileID = -1;
                int ParentProfileID = -1;

                parentProfile = null;
                ServingProfile parentServingProfile = null;

                string[] keys;
                keys = Request.QueryString.AllKeys;
                foreach (string key in keys)
                {
                    switch (key.ToUpper())
                    {
                        case "PROFILE":
                            try
                            {
                                ProfileID = Int32.Parse(Request.QueryString.Get(key));
                            }
                            catch (System.Exception ex)
                            {
                                throw new ModuleException(CurrentPortalPage, CurrentModule, string.Format("The '{0}' ProfileDetail module requires a numeric" +
                                    "Profile ID. ", CurrentModule.Title, ex));
                            }
                            break;
                        case "PARENT":
                            try
                            {
                                ParentProfileID = Int32.Parse(Request.QueryString.Get(key));
                                parentProfile = new Arena.Custom.CCV.Core.Profile(ParentProfileID);
								if (parentProfile != null && parentProfile.ProfileID != -1)
								{
									profileType = parentProfile.ProfileType;
									parentServingProfile = new ServingProfile(ParentProfileID);
								}
                            }
                            catch (System.Exception ex)
                            {
                                throw new ModuleException(CurrentPortalPage, CurrentModule, string.Format("The '{0}' ProfileDetail module requires a numeric" +
                                    "Parent ID. ", CurrentModule.Title, ex));
                            }
                            break;
                        case "PROFILETYPE":
                            try
                            {
                                profileType = (ProfileType)Enum.Parse(typeof(ProfileType), Request.QueryString.Get(key));
                            }
                            catch
                            {
                                profileType = ProfileType.Undefined;
                            }
                            break;
                    }
                }

                string StateID = "ProfileDetail_" + ProfileID.ToString();

                if (Page.IsPostBack && !reload && Session[StateID] != null)
                {
                    profile = (Arena.Custom.CCV.Core.Profile)Session[StateID];
                    sProfile = (ServingProfile)Session["S" + StateID];
                }
                else
                {
                    profile = new Arena.Custom.CCV.Core.Profile(ProfileID);

                    if (profileType != ProfileType.Undefined)
                        profile.ProfileType = profileType;

                    if (profile.ProfileType == ProfileType.Serving)
                        sProfile = new ServingProfile(ProfileID);

                    if (profile.ProfileID == -1)
                    {
                        profile.OrganizationID = CurrentPortal.OrganizationID;
                        profile.ParentProfileID = ParentProfileID;
                        profile.DisplayOrder = 9999;

                        if (parentProfile != null && parentProfile.ProfileID != -1)
                        {
                            profile.Owner = parentProfile.Owner;
                            profile.Active = parentProfile.Active;

                            foreach (Field field in profile.Fields)
                            {
                                field.ProfileId = -1;
                                profile.Fields.Add(field);
                            }

                            foreach (FieldModule fieldModule in profile.FieldModules)
                            {
                                fieldModule.ProfileId = -1;
                                profile.FieldModules.Add(fieldModule);
                            }

                        }
                        else
                        {
                            profile.Owner = CurrentPerson;
                            profile.Active = true;
                        }

                        if (sProfile != null)
                        {
                            sProfile.CriticalNeed = false;
                            sProfile.DisplayPublic = false;
                            sProfile.VolunteersNeeded = 0;

                            if (parentServingProfile != null && parentServingProfile.ProfileID != -1)
                            {
                                sProfile.DefaultHoursPerWeek = parentServingProfile.DefaultHoursPerWeek;
                                sProfile.ContactEmail = parentServingProfile.ContactEmail;
                            }
                            else
                            {
                                sProfile.DefaultHoursPerWeek = 1;
                            }
                        }
                    }
                    Session[StateID] = profile;
                    Session["S" + StateID] = sProfile;
                }

				profile.CurrentPageID = CurrentPortalPage.PortalPageID;
			}
		}

        public string GetFormattedFieldType(object fieldType)
        {
            try
            {
                Type type = fieldType.GetType();
                object[] attrs = type.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
                return type.Name;
            }
            catch
            {
                return "<Unknown Type>";
            }
        }

        public string GetFormattedQualifier(object fieldType, string value)
        {
            try
            {
                return ((FieldType)fieldType).RenderQualifierDisplay(value);
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetFormattedFieldLocation(object enumCol)
        {
            FieldLocation sl = (FieldLocation)enumCol;
            return Enum.GetName(typeof(FieldLocation), sl);
        }

        public string GetFormattedSelectionType(object enumCol)
        {
            SelectionType st = (SelectionType)enumCol;
            return Enum.GetName(typeof(SelectionType), st);
        }

        public string GetFormattedString(object stringCol)
        {
            return Utilities.replaceCRLF(stringCol.ToString());
        }


        #region Fields Tab

        #region Field Module Events

        void dlFieldModules_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            ListItemType li = e.Item.ItemType;

            if (li == ListItemType.Footer)
            {
                CustomFieldModuleCollection fieldModules = new CustomFieldModuleCollection(CurrentPortal.OrganizationID);
                if (fieldModules.Count > 0)
                {
                    DropDownList ddlFieldModules = (DropDownList)e.Item.FindControl("ddlFieldModules");
                    foreach (CustomFieldModule fieldModule in fieldModules)

                        ddlFieldModules.Items.Add(new ListItem(fieldModule.Title, fieldModule.CustomFieldModuleId.ToString()));
                    pnlFieldModules.Visible = true;
                }
                else
                    pnlFieldModules.Visible = false;
            }
        }

        void dlFieldModules_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            profile.FieldModules.RemoveAt(e.Item.ItemIndex);
            ShowFieldModuleList();
        }

        void dlFieldModules_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                DropDownList ddlFieldModules = (DropDownList)e.Item.FindControl("ddlFieldModules");

                FieldModule fieldModule = new FieldModule(profile.ProfileID, Int32.Parse(ddlFieldModules.SelectedValue));
                if (fieldModule.CustomFieldModuleId != -1)
                    profile.FieldModules.Add(fieldModule);

                ShowFieldModuleList();
            }
        }

        #endregion

        #region Field Events

        void dlFields_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            ListItemType li = e.Item.ItemType;

            if (li == ListItemType.Item || li == ListItemType.AlternatingItem)
            {
                Field field = profile.Fields[e.Item.ItemIndex];

                System.Web.UI.WebControls.Image imgVisible = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgVisible");
                imgVisible.Visible = field.Visible;

                System.Web.UI.WebControls.Image imgRequired = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgRequired");
                imgRequired.Visible = field.Required;

                //if (field.RegistrantFieldCount > 0)
                //    e.Item.FindControl("ibDelete").Visible = false;
                if (field.FieldOrder == 1 || field.FieldOrder == profile.Fields.Count)
                {
                    string name = field.FieldOrder == 1 ? "ibMoveUp" : "ibMoveDown";
                    var moveArrow = e.Item.Controls[0].FindControl(name) as ImageButton;
                    if (moveArrow != null)
                        moveArrow.Visible = false;
                }
            }

            else if (li == ListItemType.EditItem)
            {
                Field field = profile.Fields[e.Item.ItemIndex];

                RequiredFieldValidator reqName = (RequiredFieldValidator)e.Item.FindControl("reqName");
                reqName.Enabled = field.FieldType.TitleRequired;

                DropDownList ddlFieldType = (DropDownList)e.Item.FindControl("ddlFieldType");

                // Load any Custom Types
                SortedDictionary<string, Type> _fieldTypes = Arena.Utility.ArenaReflection.GetTypes(typeof(FieldType));

                // Load Types from Arena.Portal.UI
                Arena.Utility.ArenaReflection.CheckAssembly(
                    _fieldTypes,
                    Assembly.GetAssembly(typeof(Arena.Portal.UI.FieldTypes.TextBoxField)),
                    typeof(FieldType));

                foreach (KeyValuePair<string, Type> type in _fieldTypes)
                {
                    ListItem item = new ListItem(type.Key, type.Value.AssemblyQualifiedName);
                    ddlFieldType.Items.Add(item);
                    if (field.FieldType != null && item.Value == field.FieldTypeAssemblyName)
                        item.Selected = true;
                }

                DropDownList ddlLocation = (DropDownList)e.Item.FindControl("ddlLocation");
                Utilities.LoadEnum(ddlLocation, typeof(FieldLocation), field.Location);

                CheckBox cbVisible = (CheckBox)e.Item.FindControl("cbVisible");
                CheckBox cbRequired = (CheckBox)e.Item.FindControl("cbRequired");
                CheckBox cbReadOnly = (CheckBox)e.Item.FindControl("cbReadOnly");
                CheckBox cbAutoFill = (CheckBox)e.Item.FindControl("cbAutoFill");
                CheckBox cbShowOnList = (CheckBox)e.Item.FindControl("cbShowOnList");

                cbVisible.Checked = field.Visible;
                cbRequired.Checked = field.Required;
                cbReadOnly.Checked = field.ReadOnly;
                cbAutoFill.Checked = field.AutoFill;
                cbShowOnList.Checked = field.ShowOnList;

                RenderQualifier(e.Item, ddlFieldType.SelectedValue, true, field.FieldTypeQualifier);
            }
        }

        private void RenderQualifier(DataListItem dlItem, string fieldTypeValue, bool setValue, string qualifierValue)
        {
            Literal ltQualifier = (Literal)dlItem.FindControl("ltQualifier");
            ltQualifier.Text = string.Empty;

            PlaceHolder phQualifier = (PlaceHolder)dlItem.FindControl("phQualifier");
            phQualifier.Controls.Clear();

            if (fieldTypeValue != string.Empty)
            {
                FieldType fieldType = CustomField.GetFieldTypeClass(fieldTypeValue);
                ltQualifier.Text = fieldType.QualifierCaption;
                fieldType.RenderQualifierPrompt(phQualifier, CurrentArenaContext, setValue, qualifierValue);

                RequiredFieldValidator reqName = (RequiredFieldValidator)dlItem.FindControl("reqName");
                reqName.Enabled = fieldType.TitleRequired;
            }

            HtmlTableRow trQualifer = (HtmlTableRow)dlItem.FindControl("trQualifier");
            trQualifer.Visible = (ltQualifier.Text != string.Empty || phQualifier.Controls.Count > 0);
        }

        void dlFields_EditCommand(object source, DataListCommandEventArgs e)
        {
            dlFields.EditItemIndex = e.Item.ItemIndex;
            ShowFieldList();
        }

        void dlFields_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            profile.Fields.RemoveAt(e.Item.ItemIndex);
            ShowFieldList();
        }

        void dlFields_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "MoveUp" || e.CommandName == "MoveDown")
            {
                var field = profile.Fields[e.Item.ItemIndex];
                if (field != null)
                {
                    int direction = e.CommandName == "MoveUp" ? -1 : 1;
                    var otherField = profile.Fields[e.Item.ItemIndex + direction];
                    if (otherField != null)
                    {
                        otherField.FieldOrder = field.FieldOrder;
                        field.FieldOrder = field.FieldOrder + direction;
                    }

                }
                // highlight the row it moved to, index expected
                ShowFieldList(field.FieldOrder - 1);
            }
            else if (e.CommandName == "ChangeType")
            {
                DropDownList ddlFieldType = (DropDownList)e.Item.FindControl("ddlFieldType");
                RenderQualifier(e.Item, ddlFieldType.SelectedValue, true, string.Empty);
            }
        }

        void dlFields_UpdateCommand(object source, DataListCommandEventArgs e)
        {
            if (Page.IsValid)
            {
                TextBox tbName = (TextBox)e.Item.FindControl("tbName");
                DropDownList ddlLocation = (DropDownList)e.Item.FindControl("ddlLocation");
                DropDownList ddlFieldType = (DropDownList)e.Item.FindControl("ddlFieldType");
                TextBox tbRows = (TextBox)e.Item.FindControl("tbRows");
                TextBox tbWidth = (TextBox)e.Item.FindControl("tbWidth");
                CheckBox cbVisible = (CheckBox)e.Item.FindControl("cbVisible");
                CheckBox cbRequired = (CheckBox)e.Item.FindControl("cbRequired");
                CheckBox cbReadOnly = (CheckBox)e.Item.FindControl("cbReadOnly");
                CheckBox cbAutoFill = (CheckBox)e.Item.FindControl("cbAutoFill");
                CheckBox cbShowOnList = (CheckBox)e.Item.FindControl("cbShowOnList");
                PlaceHolder phQualifier = (PlaceHolder)e.Item.FindControl("phQualifier");

                Field field = profile.Fields[dlFields.EditItemIndex];

                field.Title = tbName.Text;
                field.Location = (FieldLocation)Enum.Parse(typeof(FieldLocation), ddlLocation.SelectedValue);
                field.FieldType = CustomField.GetFieldTypeClass(ddlFieldType.SelectedValue);
                if (tbRows.Text.Trim() != string.Empty)
                    field.Height = Int32.Parse(tbRows.Text.Trim());
                else
                    field.Height = 0;
                if (tbWidth.Text.Trim() != string.Empty)
                    field.Width = Int32.Parse(tbWidth.Text.Trim());
                else
                    field.Width = 0;
                field.Visible = cbVisible.Checked;
                field.Required = cbRequired.Checked;
                field.ReadOnly = cbReadOnly.Checked;
                field.AutoFill = cbAutoFill.Checked;
                field.ShowOnList = cbShowOnList.Checked;

                field.FieldTypeQualifier = field.FieldType.GetQualifierPromptValue(phQualifier);

                dlFields.EditItemIndex = -1;
                ShowFieldList();
            }
        }


        void dlFields_CancelCommand(object source, DataListCommandEventArgs e)
        {
            dlFields.EditItemIndex = -1;
            ShowFieldList();
        }

        protected void btnAddField_Click(object sender, EventArgs e)
        {
            Field field = new Field();
            field.ProfileId = profile.ProfileID;
            field.Title = "New Field";
            field.FieldType = new Arena.Portal.UI.FieldTypes.TextBoxField();
            field.FieldOrder = profile.Fields.Count + 1;
            profile.Fields.Add(field);

            ShowFieldList();
        }

        private void ShowFieldModuleList()
        {
            dlFieldModules.DataSource = profile.FieldModules;
            dlFieldModules.DataBind();
        }

        private void ShowFieldList()
        {
            ShowFieldList(-1);
        }

        private void ShowFieldList(int index)
        {
            profile.Fields.Sort();
            dlFields.DataSource = profile.Fields;
            dlFields.DataBind();
            if (index != -1 && dlFields.Items.Count > index)
            {
                // find the table and highlight the passed in table row
                foreach (Control control in dlFields.Items[index].Controls)
                {
                    if (control is Table)
                    {
                        foreach (Control row in control.Controls)
                            if (row is TableRow)
                            {
                                ScriptManager.RegisterStartupScript(upGenericeLoadPanel, upGenericeLoadPanel.GetType(), "highlightRow", string.Format("highlightRow('{0}');", row.ClientID), true);
                                break;
                            }
                        break;
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();

            LoadProfile();

			base.OnInit(e);
		}

		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			lbRemoveOwner.Click += new EventHandler(lbRemoveOwner_Click);
			btnUploadPhoto.Click += new EventHandler(btnUploadPhoto_Click);
			lblRemovePhoto.Click += new EventHandler(lblRemovePhoto_Click);
			btnUploadServingPhoto.Click += new EventHandler(btnUploadServingPhoto_Click);
			lblRemoveServingPhoto.Click += new EventHandler(lblRemoveServingPhoto_Click);
			btnPrintRoster.Click += new EventHandler(btnPrintRoster_Click);

            dlFieldModules.ItemDataBound += new DataListItemEventHandler(dlFieldModules_ItemDataBound);
            dlFieldModules.DeleteCommand += new DataListCommandEventHandler(dlFieldModules_DeleteCommand);
            dlFieldModules.ItemCommand += new DataListCommandEventHandler(dlFieldModules_ItemCommand);

            dlFields.ItemDataBound += new DataListItemEventHandler(dlFields_ItemDataBound);
            dlFields.EditCommand += new DataListCommandEventHandler(dlFields_EditCommand);
            dlFields.DeleteCommand += new DataListCommandEventHandler(dlFields_DeleteCommand);
            dlFields.UpdateCommand += new DataListCommandEventHandler(dlFields_UpdateCommand);
            dlFields.CancelCommand += new DataListCommandEventHandler(dlFields_CancelCommand);
            dlFields.ItemCommand += new DataListCommandEventHandler(dlFields_ItemCommand);

        }
		#endregion

	}
}
