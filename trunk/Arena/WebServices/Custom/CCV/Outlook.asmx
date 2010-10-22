<%@ WebService Language="C#" Class="Outlook" %>

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using Arena.Core;
using Arena.Phone;

public struct PhoneInfo
{
    public string Type;
    public string Number;
}

public struct PersonInfo
{
    public int ID;
    public Guid Guid;
    public string Name;
    public string PhotoUrl;
    public string Area;
    public string PeerChannel;
    public PhoneInfo[] Phones;
    public string Address;
    public string MemberStatus;
    public string RecordStatus;
}

public struct EmailSearchResults
{
    public PersonInfo PrimaryPerson;
    public PersonInfo[] AlternatePeople;
}

[WebService(Namespace = "http://www.AreanCHMS.Com/Custom/CCV/Outlook")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Outlook  : System.Web.Services.WebService {

    private int OrganizationID = Int32.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["organization"]);
    private string URLRoot = System.Web.Configuration.WebConfigurationManager.AppSettings["ApplicationURLPath"];
    
    [WebMethod]
    public int GetPersonByLogin(string loginID)
    {
        Person person = new Person(loginID);
        return person.PersonID;
    }

    [WebMethod]
    public EmailSearchResults PhoneSearch(int currentPersonID, string phoneNumber, int selectedPersonId)
    {
        return PhoneSearchWithImageAttributes(currentPersonID, phoneNumber, selectedPersonId, "125", "125", "", "");
        //"Polaroid", "Rotate=4;DropShadow=8,50,8,300,333333;Roundness=0;BottomPadding=4;TopPadding=4;SidePadding=4");
    }

    [WebMethod]
    public EmailSearchResults PhoneSearchWithImageAttributes(int currentPersonID, string phoneNumber, int selectedPersonId, string imageHeight, string imageWidth, string imageEffect, string imageEffectSettings)
    {
        PersonCollection people = new PersonCollection();

        string number = System.Text.RegularExpressions.Regex.Replace(phoneNumber.Trim(), @"\D", "");
        if (number.Trim() != string.Empty)
        {
            SqlDataReader rdr = new Arena.DataLayer.Core.PersonData().GetPrimaryPersonByPhone(number);
            while (rdr.Read())
                people.Add(new Person(rdr));
            rdr.Close();
        }
        
        return FindMatches(people, currentPersonID, selectedPersonId, number, imageHeight, imageWidth, imageEffect, imageEffectSettings);
    }

    [WebMethod]
    public EmailSearchResults EmailSearch(int currentPersonID, string emailAddress, int selectedPersonId)
    {
        return EmailSearchWithImageAttributes(currentPersonID, emailAddress, selectedPersonId, "125", "125", "", "");
            //"Polaroid", "Rotate=4;DropShadow=8,50,8,300,333333;Roundness=0;BottomPadding=4;TopPadding=4;SidePadding=4");
    }
    
    [WebMethod]
    public EmailSearchResults EmailSearchWithImageAttributes(int currentPersonID, string emailAddress, int selectedPersonId, string imageHeight, string imageWidth, string imageEffect, string imageEffectSettings)
    {
        PersonCollection people = new PersonCollection();
        
        if (emailAddress.Trim() != string.Empty)
        {
            SqlDataReader rdr = new Arena.DataLayer.Core.PersonData().GetPrimaryPersonByEmail(emailAddress);
            while (rdr.Read())
                people.Add(new Person(rdr));
            rdr.Close();
        }
        
        return FindMatches(people, currentPersonID, selectedPersonId, emailAddress, imageHeight, imageWidth, imageEffect, imageEffectSettings);
    }
    
    private EmailSearchResults FindMatches(PersonCollection people, int currentPersonID, int selectedPersonId, string key, string imageHeight, string imageWidth, string imageEffect, string imageEffectSettings)         
    {
        EmailSearchResults emailSearchResults = new EmailSearchResults();

        if (people.Count > 0)
        {
            if (selectedPersonId > 0)
            {
                // Look for specified person ID
                Person primaryPerson = people.FindByID(selectedPersonId);
                if (primaryPerson != null)
                {
                    // Save setting associating the selected person with the email address
                    PersonSetting ps = new PersonSetting(currentPersonID, OrganizationID, "PreferredPersonBy" + key);
                    ps.PersonId = currentPersonID;
                    ps.OrganizationId = OrganizationID;
                    ps.Key = "outlook_ws_" + key;
                    ps.Value = primaryPerson.PersonID.ToString();
                    ps.Save();                    
                }
                else
                    // Selected person with this email does not exist
                    selectedPersonId = 0;
            }
            else
            {
                // Look for previously saved setting associating the email with a specific person
                int savedPersonID = 0;
                PersonSetting ps = new PersonSetting(currentPersonID, OrganizationID, "PreferredPersonBy" + key);
                Int32.TryParse(ps.Value, out savedPersonID);
                if (savedPersonID > 0)
                {
                    // If setting found and person exists with this email, use that person
                    Person primaryPerson = people.FindByID(savedPersonID);
                    if (primaryPerson != null)
                        selectedPersonId = primaryPerson.PersonID;
                }
            }
            
            // If a person id was not specified, or doesn't exist with this email, use first person in collection
            if (selectedPersonId <= 0)
                selectedPersonId = people[0].PersonID;

            emailSearchResults.AlternatePeople = new PersonInfo[people.Count - 1];

            int pCount = 0;
            
            foreach (Person person in people)
            {

                PersonInfo personInfo = new PersonInfo();
                personInfo.ID = person.PersonID;
                personInfo.Name = person.FullName;

                if (person.PersonID == selectedPersonId)
                {
                    personInfo.Guid = person.PersonGUID;

                    if (person.Area != null)
                        personInfo.Area = person.Area.Name;
                    else
                        personInfo.Area = string.Empty;

                    if (person.Blob != null &&
                        person.Blob.ByteArray != null &&
                        person.Blob.ByteArray.Length > 0)
                        personInfo.PhotoUrl = string.Format(
                            "{0}CachedBlob.aspx?guid={1}&width={2}&height={3}&effect={4}&effectSettings={5}",
                            URLRoot, 
                            person.Blob.GUID.ToString(),
                            imageWidth,
                            imageHeight,
                            imageEffect,
                            imageEffectSettings);
                    else
                        personInfo.PhotoUrl = string.Empty;

                    if (person.Peers.Count > 0)
                        personInfo.PeerChannel = person.Peers[0].ObjectName;
                    else
                        personInfo.PeerChannel = string.Empty;
                                                
                    personInfo.Phones = new PhoneInfo[person.Phones.Count];
                    for (int i = 0; i < person.Phones.Count; i++)
                    {
                        PersonPhone personPhone = person.Phones[i];

                        PhoneInfo phoneInfo;
                        phoneInfo.Type = personPhone.PhoneType.Value;
                        phoneInfo.Number = personPhone.ToString();
                        personInfo.Phones[i] = phoneInfo;
                    }
                    if (person.PrimaryAddress != null)
                        personInfo.Address = person.PrimaryAddress.ToString();
                    else
                        personInfo.Address = string.Empty;
                    
                    personInfo.MemberStatus = person.MemberStatus.Value;
                    personInfo.RecordStatus = person.RecordStatus.ToString();
                    emailSearchResults.PrimaryPerson = personInfo;
                }
                else
                {
                    emailSearchResults.AlternatePeople[pCount] = personInfo;
                    pCount++;
                }
            }
        }
        
        return emailSearchResults;    
    }

}

