using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

using Arena.Core;
using Arena.Portal;
using Arena.Organization;

using Arena.Custom.CCV.PCO;

public partial class Test : PortalControl
{
    [RoleSetting("Viewer PCO Roles", "Roles that are synced with Planning Center Online as a viewer", false, ListSelectionMode.Multiple)]
    public string PCORolesSetting { get { return Setting("PCORoles", "", false); } }

    [RoleSetting("Editor PCO Roles", "Roles that are synced with Planning Center Online as an editor", false, ListSelectionMode.Multiple)]
    public string EditorPCORolesSetting { get { return Setting("EditorPCORoles", "", false); } }

    [TextSetting("Public Arena URL", "The Public Arena URL that PCO can use to capture Arena images (i.e. 'http://www.ccvonline.com/arena'.", true)]
    public string PublicArenaURLSetting { get { return Setting("PublicArenaURL", "", true);  } }

    LookupType pcoAccounts = null;

    protected override void OnInit(EventArgs e)
    {
        ddlAccount.SelectedIndexChanged += new EventHandler(ddlAccount_SelectedIndexChanged);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lResults.Text = string.Empty;
        lResults2.Text = string.Empty;

        pcoAccounts = new LookupType(Arena.Custom.CCV.PCO.Core.SystemLookupType.PCOAuthorization);
        pcoAccounts.UseCache = false;
        if (!Page.IsPostBack)
            pcoAccounts.Values.LoadDropDownList(ddlAccount);
    }

    void ddlAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlArenaPeople.Items.Clear();
        ddlPcoPeople.Items.Clear();
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        Lookup pcoAccount = pcoAccounts.Values.FindByID(Convert.ToInt32(ddlAccount.SelectedValue));
        People pcoPeople = new People(CurrentOrganization.OrganizationID, pcoAccount, PublicArenaURLSetting);

        bool editor = false;
        Person person = null;
        if (ddlArenaPeople.Items.Count > 0 && ddlArenaPeople.SelectedValue != "-1")
        {
            person = new Person(Int32.Parse(ddlArenaPeople.SelectedValue));
            foreach (var id in EditorPCORolesSetting.Split(','))
                if (id.Trim() != string.Empty)
                {
                    Arena.Security.Role role = new Arena.Security.Role(Int32.Parse(id));
                    if (role.RoleMemberIds.Contains(person.PersonID))
                    {
                        editor = true;
                        break;
                    }
                }
        }

        int pcoID = -1;
        if (ddlPcoPeople.Items.Count > 0)
            pcoID = Int32.Parse(ddlPcoPeople.SelectedValue);

        switch (ddlAction.SelectedValue)
        {
            case "LoadArenaNames":
            case "Unlinked":

                ddlArenaPeople.Items.Clear();
                ddlArenaPeople.Items.Add(new ListItem("", "-1"));

                try
                {
                    if (string.IsNullOrEmpty(PCORolesSetting))
                        lResults.Text = "Invalid role(s) specified in the 'PCO Roles' module setting' organization setting!";
                    else
                    {
                        Dictionary<int, Person> roleMembers = new Dictionary<int, Person>();

                        foreach (var id in EditorPCORolesSetting.Split(','))
                            if (id.Trim() != string.Empty)
                            {
                                Arena.Security.Role role = new Arena.Security.Role(Int32.Parse(id));
                                foreach (int memberID in role.RoleMemberIds)
                                    if (!roleMembers.ContainsKey(memberID))
                                        roleMembers.Add(memberID, new Person(memberID));
                            }
                        foreach (var id in PCORolesSetting.Split(','))
                            if (id.Trim() != string.Empty)
                            {
                                Arena.Security.Role role = new Arena.Security.Role(Int32.Parse(id));
                                foreach (int memberID in role.RoleMemberIds)
                                    if (!roleMembers.ContainsKey(memberID))
                                        roleMembers.Add(memberID, new Person(memberID));
                            }

                        var sortedMembers = (from entry in roleMembers
                                             orderby entry.Value.FullName
                                             select entry);

                        foreach(KeyValuePair<int, Person> member in sortedMembers)
                            if (ddlAction.SelectedValue == "LoadArenaNames" || People.GetPcoID(pcoAccount, member.Value) == -1)
                                ddlArenaPeople.Items.Add(new ListItem(member.Value.FullName, member.Value.PersonID.ToString()));

                        lResults.Text = "Names Loaded";
                    }
                }
                catch (System.Exception ex)
                {
                    lResults.Text = "Could not load Arena people.  Make sure a valid Arena role is specified in the 'PCO_Sync_Role_ID' organization setting!<br/><br/>" +
                        ex.Message;
                }

                break;

            case "UpdatePCO":

                try
                {
                    if (person != null)
                        lResults.Text = "Record Updated!<br/><br/>" +
                            formatResult(pcoPeople, pcoPeople.UpdatePerson(person, "pcoTest", editor));
                    else
                        lResults.Text = "Select an Arena Person first!";
                }
                catch (System.Exception ex)
                {
                    lResults.Text = "Could not update PCO Record!<br/><br/>" +
                        ex.Message;
                }

                break;

            case "Login":
                try
                {
                    if (person != null)
                        Response.Redirect(string.Format("~/PCOLogin.aspx?email={0}&password={1}",
                            CurrentPerson.Emails.FirstActive,
                            People.GetPcoPassword(pcoAccount, CurrentPerson)), true);
                    else
                        lResults.Text = "Select an Arena Person first!";
                }
                catch (System.Exception ex)
                {
                    lResults.Text = "Could not login to PCO.!<br/><br/>" +
                        ex.Message;
                }

                break;

            case "PCOID":

                try
                {
                    if (person != null)
                        lResults.Text = "PCO ID: " + People.GetPcoID(pcoAccount, person).ToString();
                    else
                        lResults.Text = "Select an Arena Person first!";
                }
                catch (System.Exception ex)
                {
                    lResults.Text = "Could not get PCO ID!<br/><br/>" +
                        ex.Message;
                }

                break;

            case "PCOPassword":

                try
                {
                    if (person != null)
                        lResults.Text = "PCO Password: " + People.GetPcoPassword(pcoAccount, person).ToString();
                    else
                        lResults.Text = "Select an Arena Person first!";
                }
                catch (System.Exception ex)
                {
                    lResults.Text = "Could not get PCO Password!<br/><br/>" +
                        ex.Message;
                }

                break;

            case "AttemptAutoLink":

                try
                {
                    foreach (ListItem liArena in ddlArenaPeople.Items)
                    {
                        Person member = new Person(Int32.Parse(liArena.Value));
                        if (People.GetPcoID(pcoAccount, member) == -1)
                        {
                            int matches = 0;
                            int lastMatch = 0;

                            foreach (ListItem liPCO in ddlPcoPeople.Items)
                            {
                                if (liPCO.Text == liArena.Text)
                                {
                                    matches++;
                                    lastMatch = Int32.Parse(liPCO.Value);
                                }
                            }

                            if (matches == 1)
                            {
                                People.SavePcoID(pcoAccount, member, lastMatch, "PCOTest");
                                lResults.Text += string.Format("'{0}' ({1}) in Arena has been associated with '{2}' ({3}) in PCO<br/>",
                                    member.FullName, member.PersonID.ToString(),
                                    ddlPcoPeople.Items.FindByValue(lastMatch.ToString()).Text, lastMatch.ToString());
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    lResults.Text = "Could not Auto Link All Records.!<br/><br/>" +
                        ex.Message;
                }

                break;

            case "LinkRecords":

                try
                {
                    if (person == null || pcoID == -1)
                        lResults.Text = "Select both an Arena and PCO Person first!";
                    else
                    {
                        People.SavePcoID(pcoAccount, person, pcoID, "PCOTest");
                        lResults.Text = string.Format("'{0}' ({1}) in Arena has been associated with '{2}' ({3}) in PCO",
                            person.FullName, person.PersonID.ToString(),
                            ddlPcoPeople.SelectedItem.Text, pcoID.ToString());
                    }
                }
                catch (System.Exception ex)
                {
                    lResults.Text = "Could not Link Records.!<br/><br/>" +
                        ex.Message;
                }

                break;

            case "LoadPCONames":

                ddlPcoPeople.Items.Clear();
                ddlPcoPeople.Items.Add(new ListItem("", "-1"));

                try
                {
                    XDocument xdocResults = pcoPeople.GetPeople();
                    if (xdocResults != null)
                    {
                        foreach (XElement xPerson in xdocResults.Descendants("person"))
                        {
                            ddlPcoPeople.Items.Add(new ListItem(
                                string.Format("{0} {1}",
                                    xPerson.Descendants("first-name").First().Value,
                                    xPerson.Descendants("last-name").First().Value),
                                xPerson.Descendants("id").First().Value));
                        }
                        lResults.Text = "Names Loaded";
                    }
                    else

                        lResults.Text = "Could not get list of people:<br/><br/>" +
                            formatResult(pcoPeople, xdocResults);
                }
                catch (System.Exception ex)
                {
                    lResults.Text = "Could not load PCO people!<br/><br/>" +
                        ex.Message;
                }

                break;

            case "Xml":

                try
                {
                    if (person != null)
                    {
                        lResults.Text = "<b>Current Arena XML:</b><br/>" +
                            formatResult(pcoPeople, People.PersonXML(person, PublicArenaURLSetting, editor));

                        lResults2.Text = "<b>Previous Arena XML:</b><br/>" +
                            formatResult(pcoPeople, People.GetPcoLastSyncArena(pcoAccount, person));

                        pcoID = People.GetPcoID(pcoAccount, person);
                        if (pcoID != -1)
                        {
                            lResults.Text += "<br/><br/><b>Current PCO XML:</b><br/>" +
                                formatResult(pcoPeople, pcoPeople.GetPerson(pcoID.ToString()));

                            lResults2.Text += "<br/><br/><b>Previous PCO XML:</b><br/>" +
                                formatResult(pcoPeople, People.GetPcoLastSync(pcoAccount, person));
                        }
                        else
                            lResults.Text += "<br/><br/><b>Current PCO XML:</b><br/>" +
                                "Arena person is not linked to a PCO Person!";
                    }
                    else
                        lResults.Text = "Select an Arena first!";
                }
                catch (System.Exception ex)
                {
                    lResults.Text = "Could not get PCO person XML!<br/><br/>" +
                        ex.Message;
                }

                break;

            case "SavePCOXml":

                try
                {
                    if (person != null)
                    {
                        pcoID = People.GetPcoID(pcoAccount, person);
                        if (pcoID != -1)
                        {
                            XDocument xmlPCO = pcoPeople.GetPerson(pcoID.ToString());
                            if (xmlPCO != null)
                                People.SavePcoLastSync(pcoAccount, person, xmlPCO, "PCOTest");

                            lResults.Text = "<b>Current PCO XML:</b><br/>" +
                                formatResult(pcoPeople, xmlPCO);

                            lResults2.Text = "<b>Previous PCO XML (for Arena Person):</b><br/>" +
                                formatResult(pcoPeople, People.GetPcoLastSync(pcoAccount, person));
                        }
                        else
                            lResults.Text = "Arena person is not linked to a PCO Person!";
                    }
                    else
                        lResults.Text = "Select an Arena Person first!";
                }
                catch (System.Exception ex)
                {
                    lResults.Text = "Could not get PCO person XML!<br/><br/>" +
                        ex.Message;
                }

                break;

            case "SaveArenaXml":

                try
                {
                    if (person != null)
                    {
                        pcoID = People.GetPcoID(pcoAccount, person);
                        if (pcoID != -1)
                        {
                            XDocument xmlArena = People.PersonXML(person, PublicArenaURLSetting, editor);
                            if (xmlArena != null)
                                People.SavePcoLastSyncArena(pcoAccount, person, xmlArena, "PCOTest");

                            lResults.Text = "<b>Current Arena XML:</b><br/>" +
                                formatResult(pcoPeople, xmlArena);

                            lResults2.Text = "<b>Previous Arena XML (for Arena Person):</b><br/>" +
                                formatResult(pcoPeople, People.GetPcoLastSyncArena(pcoAccount, person));
                        }
                        else
                            lResults.Text = "Arena person is not linked to a PCO Person!";
                    }
                    else
                        lResults.Text = "Select an Arena Person first!";
                }
                catch (System.Exception ex)
                {
                    lResults.Text = "Could not get Arena person XML!<br/><br/>" +
                        ex.Message;
                }

                break;

            case "Sync":

                try
                {
                    if (person != null)
                        lResults.Text = pcoPeople.SyncPerson(person, "PCOTest", editor);
                    else
                        lResults.Text = "Select an Arena Person first!";
                }
                catch (System.Exception ex)
                {
                    lResults.Text = "Could not sync records!<br/><br/>" +
                        ex.Message;
                }

                break;

            case "DisableOldUsers":

                try
                {
                    List<int> activeUsers = new List<int>();

                    foreach (var id in EditorPCORolesSetting.Split(','))
                        if (id.Trim() != string.Empty)
                        {
                            Arena.Security.Role role = new Arena.Security.Role(Int32.Parse(id));
                            foreach (int memberID in role.RoleMemberIds)
                                if (!activeUsers.Contains(memberID))
                                    activeUsers.Add(memberID);
                        }
                    foreach (var id in PCORolesSetting.Split(','))
                        if (id.Trim() != string.Empty)
                        {
                            Arena.Security.Role role = new Arena.Security.Role(Int32.Parse(id));
                            foreach (int memberID in role.RoleMemberIds)
                                if (!activeUsers.Contains(memberID))
                                    activeUsers.Add(memberID);
                        }

                    pcoPeople.Disable(activeUsers);
                }
                catch (System.Exception ex)
                {
                    lResults.Text = "Could not disble old users!<br/><br/>" +
                        ex.Message;
                }

                break;
        }
    }

    private string formatResult(People pcoPeople, XDocument XDocument)
    {
        if (XDocument != null)
            return HttpUtility.HtmlEncode(XDocument.ToString()).Replace("\n", "<br/>").Replace(" ", "&nbsp;&nbsp;");
        else
            return HttpUtility.HtmlEncode(pcoPeople.HTMLResponse);
    }
}
