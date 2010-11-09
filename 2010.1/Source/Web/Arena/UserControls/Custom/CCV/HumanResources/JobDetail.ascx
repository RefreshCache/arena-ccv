<%@ Control Language="C#" AutoEventWireup="true" CodeFile="JobDetail.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.HumanResources.JobDetail" %>

<asp:Panel ID="pnlJobPosting" runat="server" CssClass="jobDetail">
    <input type="hidden" id="ihPostingID" runat="server" />
    <table>
        <tr>
            <td>
                <h2><asp:Literal ID="lJobTitle" runat="server" /></h2>
            </td>
            <td class="applyLink">
                <asp:HyperLink ID="hlApply" runat="server" Text="Apply Online" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Literal ID="lJobDesc" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <a href="JavaScript:history.back();">Back</a>
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Panel ID="pnlNoJobs" runat="server" CssClass="textWrap">

    <asp:Literal ID="lNoPositions" runat="server" />

</asp:Panel>
