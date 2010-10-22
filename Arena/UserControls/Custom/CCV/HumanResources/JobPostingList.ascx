<%@ Control Language="C#" AutoEventWireup="true" CodeFile="JobPostingList.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.HumanResources.JobPostingList" %>
<%@ Register TagPrefix="Arena" Namespace="Arena.Portal.UI" Assembly="Arena.Portal.UI" %>
<%@ Register TagPrefix="Telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Panel ID="pnlList" runat="server" CssClass="appContainer">
    <asp:LinkButton ID="lbAdd" Runat="server" CssClass="smallText" Visible="False">Add</asp:LinkButton>
    <div align="right" style="padding-top:3px;padding-bottom:5px">
        <asp:CheckBox ID="cbShowExternal" Runat="server" CssClass="smallText"
            Text="Only display open positions publically visible on the website. &nbsp;&nbsp;" AutoPostBack="True" />
    </div>	
    <Arena:DataGrid 
        id="dgList" 
        Runat="server"
        AllowSorting="true"
        datakeyfield="job_posting_id">
        <Columns>
            <asp:boundcolumn 
                HeaderText="ID" 
                datafield="job_posting_id" 
                visible="false"></asp:boundcolumn>
            <asp:BoundColumn 
                HeaderText="Guid" 
                DataField="guid" 
                Visible="false"></asp:BoundColumn>
            <asp:HyperLinkColumn 
                HeaderText="Title" 
                SortExpression="title" 
                DataTextField="title" 
                DataNavigateUrlField="guid" 
                ItemStyle-Wrap="false"></asp:HyperLinkColumn>
            <asp:BoundColumn 
                HeaderText="Applicants" 
                Datafield="applicant_count" 
                SortExpression="applicant_count" 
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
            <asp:TemplateColumn
                HeaderText="Full Time Position"
                SortExpression="full_time"
                ItemStyle-Wrap="False" 
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center">
                <ItemTemplate><asp:Image ID="imgFullTime" Runat="server" ImageUrl="~/images/check.gif" Visible='<%# Eval("full_time") %>' />
                </ItemTemplate></asp:TemplateColumn>
            <asp:TemplateColumn
                HeaderText="Paid Position"
                SortExpression="paid_position"
                ItemStyle-Wrap="False" 
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center">
                <ItemTemplate><asp:Image ID="imgPaidPosition" Runat="server" ImageUrl="~/images/check.gif" Visible='<%# Eval("paid_position") %>' />
                </ItemTemplate></asp:TemplateColumn>
            <asp:TemplateColumn
                HeaderText="Show Externally"
                SortExpression="show_external"
                ItemStyle-Wrap="False" 
                HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center">
                <ItemTemplate><asp:Image ID="imgShowExternal" Runat="server" ImageUrl="~/images/check.gif" Visible='<%# Eval("show_external") %>' />
                </ItemTemplate></asp:TemplateColumn>
            <asp:TemplateColumn 
                HeaderText="Posting Date" 
                SortExpression="date_posted">
                <ItemTemplate>
                    <%# Eval("date_posted", "{0:d}")%>
                </ItemTemplate>	
            </asp:TemplateColumn>
        </Columns>
    </Arena:DataGrid>
</asp:Panel>

<asp:Panel ID="pnlDetails" Runat="server">
    <input type="hidden" id="ihPositionID" runat="server" />
    <table cellpadding="0" cellspacing="5" border="0">
        <tr>
            <td class="formLabel" nowrap valign="top">
                <asp:Label ID="lblTitle" runat="server" Text="Title" AssociatedControlID="tbTitle" /></td>
            <td class="formItem" nowrap valign="top">
                <asp:TextBox ID="tbTitle" Runat="server" MaxLength="50" style="width:250px" CssClass="formItem"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="tbTitle" ID="reqTitle" Runat= "server" CssClass="errorText" Display="Dynamic" ErrorMessage="Title is required!"> *</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="formLabel" nowrap valign="top">
                <asp:Label ID="lblFullTime" runat="server" Text="Full Time Position" AssociatedControlID="cbFullTime" /></td>
            <td class="formItem" nowrap valign="top">
                <asp:CheckBox ID="cbFullTime" Runat="server" CssClass="formItem"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td class="formLabel" nowrap valign="top">
                <asp:Label ID="lblPaidPosition" runat="server" Text="Paid Position" AssociatedControlID="cbPaidPosition" /></td>
            <td class="formItem" nowrap valign="top">
                <asp:CheckBox ID="cbPaidPosition" Runat="server" CssClass="formItem"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td class="formLabel" nowrap valign="top">
                <asp:Label ID="lblShowExternalEdit" runat="server" Text="Show Externally" AssociatedControlID="cbShowExternalEdit" /></td>
            <td class="formItem" nowrap valign="top">
                <asp:CheckBox ID="cbShowExternalEdit" Runat="server" CssClass="formItem"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="formItem" nowrap>
                <asp:Label ID="lblDescription" runat="server" Text="Description" AssociatedControlID="radDescription" CssClass="formLabel" />
                <Telerik:RadEditor ID="radDescription" runat="server" Width="650px" Height="600px">
                </Telerik:RadEditor>
                <Telerik:RadSpell ID="radSpellDetails" runat="server" ControlToCheck="radDescription" Visible="false">
                </Telerik:RadSpell>
            </td>
        </tr>
    </table>

    <table cellpadding="0" cellspacing="5" border="0">
        <tr>
            <td>
                <asp:Button ID="btnUpdate" Runat="server" Text="Update" CssClass="smallText" onclick="btnUpdate_Click"></asp:Button>
                <asp:Button ID="btnCancel" Runat="server" Text="Cancel" CssClass="smallText" CausesValidation="False" onclick="btnCancel_Click"></asp:Button>
            </td>
        </tr>
        <tr>
            <td class="formItem"><asp:Label ID="lblMessage" Runat="server" CssClass="errorText"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
