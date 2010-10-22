<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AreaNeedRequest.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.CCV.Area.AreaNeedRequest" %>

<asp:UpdatePanel UpdateMode="conditional"  ID="pnlUpdate" runat="server">
<ContentTemplate>

    <asp:Panel ID="pnlEntryForm" runat="server">
        <fieldset class="areaNeed">
        <legend><asp:Label ID="lblCaption" runat="server"></asp:Label></legend>

        <asp:Label ID="lblName" AssociatedControlID="tbName" runat="server">Name</asp:Label>
        <asp:TextBox ID="tbName" runat="server" CssClass="formItem" Columns="25"></asp:TextBox>

        <asp:Label ID="lblPhone" AssociatedControlID="tbPhone" runat="server">Phone</asp:Label>
        <asp:TextBox ID="tbPhone" runat="server" CssClass="formItem" Columns="18"></asp:TextBox>

        <asp:Label ID="lblEmail" AssociatedControlID="tbEmail" runat="server">Email</asp:Label>
        <asp:TextBox ID="tbEmail" runat="server" CssClass="formItem" Columns="30"></asp:TextBox>

        <asp:Label ID="lblNeed" AssociatedControlID="tbNeed" runat="server">Description</asp:Label>
        <asp:TextBox ID="tbNeed" runat="server" CssClass="formItem" TextMode="MultiLine" Rows="5" Columns="38"></asp:TextBox>
         
        <asp:Panel ID="pnlError" runat="server" Visible="false"></asp:Panel>
        
        <asp:Button ID="btnSubmit" runat="server" Text="Submit Need" CssClass="search" OnClick="btnSubmit_Click" />
        </fieldset>
    </asp:Panel>    
    
    <asp:Panel ID="pnlResultForm" runat="server" Visible="false">
        <asp:Label ID="lblResult" runat="server" CssClass=""></asp:Label><br />
        <asp:Button ID="btnSubmitAnother" runat="server" Text="Submit Another Need" CssClass="search" OnClick="btnSubmitAnother_Click" />
    </asp:Panel>
    
</ContentTemplate>
</asp:UpdatePanel>