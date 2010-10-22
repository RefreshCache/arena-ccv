<%@ Control Language="c#" Inherits="ArenaWeb.UserControls.Custom.CCV.Area.AreaLeadership" CodeFile="AreaLeadership.ascx.cs" %>
<table class="module fro">
<tbody>
<tr>
    <td id="tdImages" runat="server" valign="top" width="200">
    </td>  
    <td valign="top" align="left">
    <asp:Panel ID="pnlLeaderHeading" runat="server" CssClass="h3" />
	<asp:Table ID="tblLeaders" runat="server"></asp:Table>
    </td>
    <td rowspan="2" valign="top" class="rightCol" id="tdTeam" runat="server">
        <asp:Panel ID="pnlTeamHeading" runat="server" CssClass="h3" />
        <asp:Table ID="tblTeam" runat="server"></asp:Table>
    </td>
</tr>

</tbody>
</table>