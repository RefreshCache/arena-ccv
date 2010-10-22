<%@ Control Language="c#" Inherits="ArenaWeb.UserControls.Custom.CCV.Phone.SpeedDialer" CodeFile="SpeedDialer.ascx.cs" %>
<div nowrap="true" class="smallText" style="border-bottom:1px solid #999999;margin-bottom:3px"><asp:Literal ID="lTagName" runat="server"></asp:Literal></div>
<asp:ListView ID="lvContacts" runat="server">
<LayoutTemplate>
    <table border="0" cellpadding="0" cellspacing="3" style="width:100%">
        <tr id="itemPlaceHolder" runat="server"></tr>
    </table>
</LayoutTemplate>
<ItemTemplate>
    <tr id="Tr1" runat="server" class="voicemail">
        <td style="text-align:right:width:16px"><asp:Literal ID="lCtcHome" runat="server"></asp:Literal></td>
        <td style="text-align:right:width:16px"><asp:Literal ID="lCtcWork" runat="server"></asp:Literal></td>
        <td style="text-align:right:width:16px"><asp:Literal ID="lCtcCell" runat="server"></asp:Literal></td>
        <td class="voicemailNameNew"><asp:HyperLink ID="hlName" runat="server"></asp:HyperLink></td>
    </tr>
</ItemTemplate>
</asp:ListView>
