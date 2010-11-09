<%@ Control Language="c#" Inherits="ArenaWeb.UserControls.Custom.CCV.SmallGroup.GroupMap" CodeFile="GroupMap.ascx.cs" %>
<asp:Panel ID="pnlMap" CssClass="groupMap" Runat="server"></asp:Panel>
<div class="notice important">
Clicking on the group name below will center &amp; display it's information in the above map. If a group name below is not clickable, it simply means the address is not yet accessible through our system.
</div>
<asp:GridView ID="gvGroups" runat="server" AllowPaging="false" AllowSorting="true" 
    CssClass="list" AutoGenerateColumns="false" GridLines="None" 
    PagerSettings-Visible="false" DataKeyNames="GroupID" >
    <HeaderStyle CssClass="listHeader" VerticalAlign="Bottom" />
    <RowStyle CssClass="listItem" VerticalAlign="Top" />
    <AlternatingRowStyle CssClass="listAltItem" />
    <Columns>
        <asp:TemplateField HeaderText="Groups" SortExpression="Title">
            <ItemTemplate>
                <a id="aGroupTitle" runat="server"><%# DataBinder.Eval(Container.DataItem, "Title" )%></a><asp:Label ID="lblGroupTitle" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "Title" )%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="Description" DataField="Description" SortExpression="Description" />
        <asp:BoundField HeaderText="Group Type" DataField="GroupType" SortExpression="GroupType" ItemStyle-Wrap="false" />
        <asp:BoundField HeaderText="Meeting Day" DataField="MeetingDay" SortExpression="MeetingDay" ItemStyle-Wrap="false" />
        <asp:BoundField HeaderText="Meeting Time" DataField="Schedule" SortExpression="Schedule" ItemStyle-Wrap="false" />
        <asp:BoundField HeaderText="Topic" DataField="Topic" SortExpression="Topic" ItemStyle-Wrap="false" />
        <asp:BoundField HeaderText="Marital Preference" DataField="PrimaryMaritalStatus" 
            SortExpression="PrimaryMaritalStatus" ItemStyle-Wrap="false" />
        <asp:BoundField HeaderText="Age Group" DataField="PrimaryAge" SortExpression="PrimaryAge" ItemStyle-Wrap="false" />
        <asp:BoundField HeaderText="Average Age" DataField="AverageAge" SortExpression="AverageAge" ItemStyle-Wrap="false"  
            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
        <asp:TemplateField HeaderText="City" ItemStyle-Wrap="false" >
            <ItemTemplate><%# Eval("TargetLocation.City")%></ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="Notes" DataField="Notes" SortExpression="Notes" />
        <asp:ButtonField HeaderText="Register" ButtonType="Image" CommandName="Register" 
            ImageUrl="~/images/register_now.gif" HeaderStyle-HorizontalAlign="Center" 
            ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" />
    </Columns>
</asp:GridView>

