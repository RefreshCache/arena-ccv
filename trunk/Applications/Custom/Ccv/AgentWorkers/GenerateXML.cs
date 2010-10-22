using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

using Agent;

using Arena.Core;
using Arena.Document;
using Arena.Portal;

namespace Arena.Custom.CCV.AgentWorkers
{
    [Serializable]
    [Description("Agent to generate XML cache for the website to use so that the XML doesn't need to generated on every request.")]
    public class GenerateXML : AgentWorker
    {
        const int STATE_OK = 0;
    
        // private fields
        private int _positionAttributeID = -1;
        private int _departmentAttributeID = -1;
        private int _departmentPositionAttributeID = -1;
        private int _staffDetailsAttributeGroupID = -1;
        private int _bioDocumentTypeID = -1;
        private int _assistantTypeID = -1;
        private string _xmlFolderPath = string.Empty;

        #region Agent Settings

        [NumericSetting("Position Attribute ID", "The ID of the 'Position' person attribute", true)]
        [Description("The ID of the 'Position' person attribute")]
        public int PositionAttributeID { get { return _positionAttributeID; } set { _positionAttributeID = value; } }

        [NumericSetting("Department Attribute ID", "The ID of the 'Department' person attribute", true)]
        [Description("The ID of the 'Department' person attribute")]
        public int DepartmentAttributeID { get { return _departmentAttributeID; } set { _departmentAttributeID = value; } }

        [NumericSetting("Department Position Attribute ID", "The ID of the 'Department Position' person attribute", true)]
        [Description("The ID of the 'Position' person attribute")]
        public int DepartmentPositionAttributeID { get { return _departmentPositionAttributeID; } set { _departmentPositionAttributeID = value; } }

        [NumericSetting("Staff Details Attribute Group ID", "The ID of the 'Staff Details' person attribute group", true)]
        [Description("The ID of the 'Staff Details' person attribute group")]
        public int StaffDetailsAttributeGroupID { get { return _staffDetailsAttributeGroupID; } set { _staffDetailsAttributeGroupID = value; } }

        [NumericSetting("Biography Document Type ID", "The ID of the 'Biography' document type", false)]
        [Description("The ID of the 'Biography' document type")]
        public int BioDocumentTypeID { get { return _bioDocumentTypeID; } set { _bioDocumentTypeID = value; } }

        [NumericSetting("Assistant Relationship Type ID", "The ID of the 'Assistant' relationship type", false)]
        [Description("The ID of the 'Assistant' relationship type")]
        public int AssistantTypeID { get { return _assistantTypeID; } set { _assistantTypeID = value; } }

        [TextSetting("XML Folder", "UNC path to XML folder (i.e. //Server/Share/Folder).", true)]
        [Description("UNC path to XML folder (i.e. //Server/Share/Folder).")]
        public string XMLFolderPath { get { return _xmlFolderPath; } set { _xmlFolderPath = value; } }

        #endregion

        public override WorkerResult Run(bool previousWorkersActive)
        {
            try
            {
                int state;
                string message;

                if (Convert.ToBoolean(Enabled))
                {
                    if (RunIfPreviousWorkersActive || !previousWorkersActive)
                    {
                        WorkerResultStatus status = ProcessXML(out message, out state);
                        return new WorkerResult(state, status, string.Format(Description), message);
                    }
                    else
                        return new WorkerResult(STATE_OK, WorkerResultStatus.Abort, string.Format(Description), "Did not run because previous worker instance still active.");
                }
                else
                    return new WorkerResult(STATE_OK, WorkerResultStatus.Abort, string.Format(Description), "Did not run because worker not enabled.");
            }
            catch (Exception e)
            {
                // handle special exceptions here...
                throw (e);
            }
        }

        public WorkerResultStatus ProcessXML(out string message, out int state)
        {
            Trace.Write("Starting ProcessXML Agent...\n");

            WorkerResultStatus workerResultStatus = WorkerResultStatus.Ok;
            message = string.Empty;
            state = STATE_OK;

            try 
            { 
                message += CreateStaffXML();
                if (message != string.Empty)
                    workerResultStatus = WorkerResultStatus.Warning;
            }
            catch (Exception ex)
            {
                workerResultStatus = WorkerResultStatus.Warning;
                message = "An error occured while generating Staff XML.\n\nMessage\n------------------------\n" + ex.Message + "\n\nStack Trace\n------------------------\n" + ex.StackTrace;
            }

             return workerResultStatus;
        }

        private string CreateStaffXML()
        {
            StringBuilder sbMessages = new StringBuilder();

            AttributeGroup StaffDetailsGroup = new AttributeGroup(StaffDetailsAttributeGroupID);

            Arena.Core.Attribute departmentAttribute = new Arena.Core.Attribute(DepartmentAttributeID);
            LookupType departments = new LookupType(Convert.ToInt32(departmentAttribute.TypeQualifier));

            List<StaffMember> staff = new List<StaffMember>();
            PersonCollection people = new PersonCollection();
            people.LoadStaffMembers();
            foreach (Person person in people)
            {
                string title = string.Empty;
                Lookup department = null;
                Lookup departmentPosition = null;

                PersonAttribute pa = (PersonAttribute)person.Attributes.FindByID(PositionAttributeID);
                if (pa != null)
                    title = pa.StringValue;

                pa = (PersonAttribute)person.Attributes.FindByID(DepartmentAttributeID);
                if (pa != null && pa.IntValue != -1)
                    department = new Lookup(pa.IntValue);

                pa = (PersonAttribute)person.Attributes.FindByID(DepartmentPositionAttributeID);
                if (pa != null && pa.IntValue != -1)
                    departmentPosition = new Lookup(pa.IntValue);

                if (department != null && departmentPosition != null)
                    staff.Add(new StaffMember(
                        person.PersonID,
                        person.PersonGUID,
                        person.NickName,
                        person.LastName,
                        person.Blob != null ? person.Blob.GUID : Guid.Empty,
                        person.Emails.FirstActive,
                        title,
                        department,
                        departmentPosition));
            }

            staff.Sort();

            // Delete any existing department XML files in the staff folder
            DirectoryInfo staffFolder = new DirectoryInfo(Path.Combine(XMLFolderPath, "Staff"));
            if (staffFolder.Exists)
                foreach (FileInfo fi in staffFolder.GetFiles())
                    try
                    {
                        fi.Delete();
                    }
                    catch (System.Exception ex)
                    {
                        sbMessages.AppendFormat("Could not delete {0} file: {1}\n", fi.FullName, ex.Message);
                    }
            else
                staffFolder.Create();

            if (staff.Count > 0)
            {
                LookupCollection activeDepartments = new LookupCollection();

                Lookup currentDepartment = new Lookup();
                XmlDocument xdoc = null;
                XmlNode departmentNode = null;

                foreach (StaffMember StaffMember in staff)
                {
                    if (currentDepartment.LookupID != StaffMember.Department.LookupID)
                    {
                        if (xdoc != null)
                        {
                            string path = Path.Combine(staffFolder.FullName, currentDepartment.Guid.ToString() + ".xml");
                            try
                            {
                                xdoc.Save(path);
                            }
                            catch (System.Exception ex)
                            {
                                sbMessages.AppendFormat("Could not save {0} file: {1}\n", path, ex.Message);
                            }
                        }

                        currentDepartment = StaffMember.Department;
                        activeDepartments.Add(currentDepartment);

                        xdoc = new XmlDocument();
                        departmentNode = xdoc.CreateNode(XmlNodeType.Element, "department", xdoc.NamespaceURI);
                        XmlAttribute xattr = xdoc.CreateAttribute("", "name", xdoc.NamespaceURI);
                        xattr.Value = currentDepartment.Value;
                        departmentNode.Attributes.Append(xattr);
                        xdoc.AppendChild(departmentNode);
                    }

                    departmentNode.AppendChild(StaffMember.XMLNode(xdoc));

                    if (StaffMember.DepartmentPosition.Qualifier2 == "1")
                    {
                        XmlDocument xdocStaff = new XmlDocument();
                        XmlNode xnodeStaff = StaffMember.XMLNode(xdocStaff);
                        xdocStaff.AppendChild(xnodeStaff);

                        if (_assistantTypeID != -1)
                        {
                            RelationshipCollection relationships = new RelationshipCollection(StaffMember.ID);
                            foreach (Relationship relationship in relationships)
                                if (relationship.RelationshipTypeId == _assistantTypeID)
                                {
                                    XmlNode xnodeAssistant = xdocStaff.CreateNode(XmlNodeType.Element, "assistant", xdocStaff.NamespaceURI);

                                    XmlAttribute xattr = xdocStaff.CreateAttribute("", "fn", xdocStaff.NamespaceURI);
                                    xattr.Value = relationship.RelatedPerson.NickName;
                                    xnodeAssistant.Attributes.Append(xattr);

                                    xattr = xdocStaff.CreateAttribute("", "ln", xdocStaff.NamespaceURI);
                                    xattr.Value = relationship.RelatedPerson.LastName;
                                    xnodeAssistant.Attributes.Append(xattr);

                                    xattr = xdocStaff.CreateAttribute("", "email", xdocStaff.NamespaceURI);
                                    xattr.Value = relationship.RelatedPerson.Emails.FirstActive;
                                    xnodeAssistant.Attributes.Append(xattr);

                                    xnodeStaff.AppendChild(xnodeAssistant);

                                    break;
                                }
                        }

                        PersonAttributeCollection pAttributes = new PersonAttributeCollection();
                        pAttributes.LoadByGroup(StaffDetailsGroup, StaffMember.ID);
                        foreach (PersonAttribute pa in pAttributes)
                        {
                            if (pa.AttributeType == Arena.Enums.DataType.Document)
                            {
                                if (BioDocumentTypeID != -1 && pa.TypeQualifier == BioDocumentTypeID.ToString())
                                {
                                    Arena.Utility.ArenaDataBlob bioDoc = new Arena.Utility.ArenaDataBlob(pa.IntValue);
                                    if (bioDoc.FileExtension == "txt")
                                    {
                                        ASCIIEncoding enc = new ASCIIEncoding();
                                        string bio = enc.GetString(bioDoc.ByteArray);

                                        if (bio != string.Empty)
                                        {
                                            XmlNode xnodeBio = xdocStaff.CreateNode(XmlNodeType.Element, "biography", xdocStaff.NamespaceURI);
                                            xnodeBio.AppendChild(xdocStaff.CreateCDataSection(bio));
                                            xnodeStaff.AppendChild(xnodeBio);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                XmlNode xnodeAttribute = xdocStaff.CreateNode(XmlNodeType.Element, "attribute", xdocStaff.NamespaceURI);

                                XmlAttribute xattr = xdocStaff.CreateAttribute("", "name", xdocStaff.NamespaceURI);
                                xattr.Value = pa.AttributeName;
                                xnodeAttribute.Attributes.Append(xattr);

                                xattr = xdocStaff.CreateAttribute("", "value", xdocStaff.NamespaceURI);
                                xattr.Value = pa.ToString();
                                xnodeAttribute.Attributes.Append(xattr);

                                xnodeStaff.AppendChild(xnodeAttribute);
                            }
                        }

                        string path = Path.Combine(staffFolder.FullName, StaffMember.Guid.ToString() + ".xml");
                        try
                        {
                            xdocStaff.Save(path);
                        }
                        catch (System.Exception ex)
                        {
                            sbMessages.AppendFormat("Could not save {0} file: {1}\n", path, ex.Message);
                        }
                    }
                }

                if (xdoc != null)
                {
                    string path = Path.Combine(staffFolder.FullName, currentDepartment.Guid.ToString() + ".xml");
                    try
                    {
                        xdoc.Save(path);
                    }
                    catch (System.Exception ex)
                    {
                        sbMessages.AppendFormat("Could not save {0} file: {1}\n", path, ex.Message);
                    }
                }

                XmlDocument xdocDepartments = new XmlDocument();
                XmlNode xnodeDepartments = xdocDepartments.CreateNode(XmlNodeType.Element, "departments", xdocDepartments.NamespaceURI);
                xdocDepartments.AppendChild(xnodeDepartments);

                foreach (Lookup activeDepartment in activeDepartments) 
                {
                    XmlNode xnodeDepartment = xdocDepartments.CreateNode(XmlNodeType.Element, "department", xdocDepartments.NamespaceURI);

                    XmlAttribute xattr = xdocDepartments.CreateAttribute("", "guid", xdocDepartments.NamespaceURI);
                    xattr.Value = activeDepartment.Guid.ToString();
                    xnodeDepartment.Attributes.Append(xattr);

                    xattr = xdocDepartments.CreateAttribute("", "name", xdocDepartments.NamespaceURI);
                    xattr.Value = activeDepartment.Value;
                    xnodeDepartment.Attributes.Append(xattr);

                    XmlNode xnodeDeptDescription = xdocDepartments.CreateNode(XmlNodeType.Element, "description", xdocDepartments.NamespaceURI);
                    xnodeDeptDescription.InnerText = activeDepartment.Qualifier8;
                    xnodeDepartment.AppendChild(xnodeDeptDescription);

                    xnodeDepartments.AppendChild(xnodeDepartment);
                }

                try
                {
                    xdocDepartments.Save(Path.Combine(staffFolder.FullName, "departments.xml"));
                }
                catch (System.Exception ex)
                {
                    sbMessages.AppendFormat("Could not save {0} file: {1}\n", Path.Combine(staffFolder.FullName, "departments.xml"), ex.Message);
                }
            }

            return sbMessages.ToString();
        }
    }

    public class StaffMember : IComparable
    {
        public int ID { get; set; }
        public Guid Guid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid Image { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public Lookup Department { get; set; }
        public Lookup DepartmentPosition { get; set; }

        public StaffMember(
            int id,
            Guid guid,
            string firstName,
            string lastName,
            Guid image,
            string email,
            string title,
            Lookup department,
            Lookup departmentPosition)
        {
            ID = id;
            Guid = guid;
            FirstName = firstName;
            LastName = lastName;
            Image = image;
            Email = email;
            Title = title;
            Department = department;
            DepartmentPosition = departmentPosition;
        }

        public int CompareTo(object obj)
        {
            StaffMember staff2 = (StaffMember)obj;
            int compareResult = 0;

            // Compare Department
            compareResult = Department.Order.CompareTo(staff2.Department.Order);
            if (compareResult != 0) 
                return compareResult;

            // Compare Department Position
            compareResult = DepartmentPosition.Order.CompareTo(staff2.DepartmentPosition.Order);
            if (compareResult != 0) 
                return compareResult;

            // Compare Last Name
            compareResult = LastName.CompareTo(staff2.LastName);
            if (compareResult != 0) 
                return compareResult;

            // Compare First Name
            return FirstName.CompareTo(staff2.FirstName);
		}

        public XmlNode XMLNode(XmlDocument xdoc)
        {
            XmlNode xnode = xdoc.CreateNode(XmlNodeType.Element, "staff", xdoc.NamespaceURI);

            XmlAttribute xattr = xdoc.CreateAttribute("", "fn", xdoc.NamespaceURI);
            xattr.Value = FirstName;
            xnode.Attributes.Append(xattr);

            xattr = xdoc.CreateAttribute("", "ln", xdoc.NamespaceURI);
            xattr.Value = LastName;
            xnode.Attributes.Append(xattr);

            if (DepartmentPosition != null)
            {
                xattr = xdoc.CreateAttribute("", "position", xdoc.NamespaceURI);
                xattr.Value = DepartmentPosition.Value;
                xnode.Attributes.Append(xattr);
            }

            // Include Details
            if (DepartmentPosition.Qualifier2 == "1")
            {
                xattr = xdoc.CreateAttribute("", "guid", xdoc.NamespaceURI);
                xattr.Value = Guid.ToString();
                xnode.Attributes.Append(xattr);

            }

            if (Image != Guid.Empty && DepartmentPosition.Qualifier == "1")
            {
                xattr = xdoc.CreateAttribute("", "image", xdoc.NamespaceURI);
                xattr.Value = Image.ToString();
                xnode.Attributes.Append(xattr);
            }

            xattr = xdoc.CreateAttribute("", "email", xdoc.NamespaceURI);
            xattr.Value = Email;
            xnode.Attributes.Append(xattr);

            xattr = xdoc.CreateAttribute("", "title", xdoc.NamespaceURI);
            xattr.Value = Title;
            xnode.Attributes.Append(xattr);

            return xnode;
        }
    }
}