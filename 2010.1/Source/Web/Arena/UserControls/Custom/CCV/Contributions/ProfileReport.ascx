<%@ Control Language="c#" Inherits="ArenaWeb.UserControls.Custom.CCV.Contributions.ProfileReport" CodeFile="ProfileReport.ascx.cs" %>
<%@ Register TagPrefix="Arena" Namespace="Arena.Portal.UI" Assembly="Arena.Portal.UI" %>

<table>
    <tr>
        <td><asp:Label ID="lblProfileID" runat="server" AssociatedControlID="tbProfileID" Text="Profile ID" CssClass="formLabel"></asp:Label></td>
        <td><asp:TextBox ID="tbProfileID" runat="server" CssClass="formItem"></asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblStartDate" runat="server" AssociatedControlID="tbStartDate" Text="Start Date" CssClass="formLabel"></asp:Label></td>
        <td><Arena:DateTextBox ID="tbStartDate" runat="server" CssClass="formItem"></Arena:DateTextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblEndDate" runat="server" AssociatedControlID="tbEndDate" Text="End Date" CssClass="formLabel"></asp:Label></td>
        <td><Arena:DateTextBox ID="tbEndDate" runat="server" CssClass="formItem"></Arena:DateTextBox></td>
    </tr>
</table>
<asp:Button ID="btnSubmit" runat="server" Text="Submit" />
<asp:Label ID="lblError" runat="server" CssClass="errorText" Visible="false"></asp:Label>
<Arena:DataGrid 
	id="dgTransactions" 
	Runat="server" 
	AllowSorting="true"
	Visible="false">
</Arena:DataGrid>
