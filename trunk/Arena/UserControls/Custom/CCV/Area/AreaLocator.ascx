<%@ Control Language="c#" Inherits="ArenaWeb.UserControls.Custom.CCV.Area.AreaLocator" CodeFile="AreaLocator.ascx.cs" %>

<fieldset class="areaLocator">
<legend><asp:Label ID="lblAddressCaption" runat="server"></asp:Label></legend>

<asp:Label AssociatedControlID="tbAddress" runat="server">Address</asp:Label>
<asp:TextBox ID="tbAddress" runat="server" CssClass="formItem" Columns="50"></asp:TextBox>

<asp:Label AssociatedControlID="tbCity" runat="server">City</asp:Label>
<asp:TextBox ID="tbCity" runat="server" CssClass="formItem" Columns="30"></asp:TextBox>

<asp:Label AssociatedControlID="tbState" runat="server">State</asp:Label>
<asp:TextBox ID="tbState" runat="server" CssClass="formItem" Columns="10"></asp:TextBox>

<asp:Label AssociatedControlID="tbZip" runat="server">Zip</asp:Label>
<asp:TextBox ID="tbZip" runat="server" CssClass="formItem" Columns="15"></asp:TextBox>
 
<asp:Button ID="btnSubmit" runat="server" Text="Locate Groups" CssClass="search" />
</fieldset>