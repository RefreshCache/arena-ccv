<%@ Control Language="c#" Inherits="ArenaWeb.UserControls.Custom.CCV.Core.ReassignTagOwner" CodeFile="ReassignTagOwner.ascx.cs" %>
<div class="heading2">Change Tag Ownership</div>
<br />
<asp:UpdatePanel ID="upReassign" runat="server">
    <ContentTemplate>
        <asp:Label ID="lblCurrentOwner" runat="server" AssociatedControlID="ddlCurrentOwner" CssClass="formItem">Change Tag Owner From:</asp:Label>
        <asp:DropDownList ID="ddlCurrentOwner" runat="server" AutoPostBack="true" CssClass="formItem"></asp:DropDownList>&nbsp;
        <asp:Label ID="lblNewOwner" runat="server" AssociatedControlID="ddlNewOwner" CssClass="formItem">To:</asp:Label>
        <asp:DropDownList ID="ddlNewOwner" runat="server" CssClass="formItem"></asp:DropDownList>
        <br /><br />
        <asp:Label ID="lblTag" runat="server" AssociatedControlID="ddlTag" CssClass="formItem">For this tag and all of it's child tags with the same owner:</asp:Label><br />
        <asp:DropDownList ID="ddlTag" runat="server" CssClass="formItem"></asp:DropDownList>
        <br /><br />
        <asp:Button ID="btnReassign" runat="server" CssClass="smallText" Text="Change Owner" />
    </ContentTemplate>
</asp:UpdatePanel>
<br /><br />
