<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProfileMemberFields.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.CCV.Core.ProfileMemberFields" %>

<table cellpadding="0" cellspacing="0" border="0" width="100%">
<tr>
    <th valign="middle" align="left" nowrap style="color:#999999; border-bottom:1px solid #cccccc" class="formItem">
        Custom Fields
    </th>
    <th valign="middle" align="right" nowrap class="smallText">&nbsp;
        <asp:LinkButton ID="lbEdit" Runat="server" CssClass="editButton" style="COLOR:#999999" onclick="lbEdit_Click">Edit</asp:LinkButton>
    </th>
</tr>
</table>

<table id="tblFields" runat="server" cellpadding="3" cellspacing="0" border="0" width="100%"/>

<table id="tblSave" runat="server" visible="false" cellpadding="0" cellspacing="0" border="0" width="100%">
<tr>
    <td valign="middle" align="right" nowrap class="smallText">
        <asp:LinkButton ID="lbSave" Runat="server" Text="Save" onclick="lbSave_Click"/>
        <asp:LinkButton ID="lbCancel" Runat="server" Text="Cancel" CausesValidation="False" onclick="lbCancel_Click"/>
    </td>
</tr>
</table>



