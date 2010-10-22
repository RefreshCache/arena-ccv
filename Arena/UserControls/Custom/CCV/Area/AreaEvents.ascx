<%@ Control Language="c#" Inherits="ArenaWeb.UserControls.Custom.CCV.Area.AreaEvents" CodeFile="AreaEvents.ascx.cs" %>
<%@ Register TagPrefix="Arena" Namespace="Arena.Portal.UI" Assembly="Arena.Portal.UI" %>
<asp:Label ID="lblHeading" runat="server" CssClass="heading2"></asp:Label>
<Arena:DataGrid 
	id="dgOccurrences" 
	Runat="server" 
	AllowSorting="true">
	<Columns>
		<asp:boundcolumn 
			datafield="occurrence_id" 
			readonly="true"
			Visible="False"></asp:boundcolumn>
		<asp:boundcolumn
			HeaderText="Event"
			SortExpression="occurrence_name"
			DataField="occurrence_name"
			ItemStyle-Wrap="False"></asp:boundcolumn>
		<asp:TemplateColumn
			HeaderText="Start"
			SortExpression="occurrence_start_time"
			ItemStyle-VerticalAlign="Top">
			<ItemTemplate>
				<asp:Label ID="lblDate" Runat="server" Text='<%# GetFormattedDateTime(DataBinder.Eval(Container.DataItem, "occurrence_start_time")) %>'></asp:Label>
			</ItemTemplate>
			</asp:TemplateColumn>
		<asp:TemplateColumn
			HeaderText="End"
			SortExpression="occurrence_end_time"
			ItemStyle-VerticalAlign="Top">
			<ItemTemplate>
				<asp:Label ID="lblTime" Runat="server" Text='<%# GetFormattedDateTime(DataBinder.Eval(Container.DataItem, "occurrence_end_time")) %>'></asp:Label>
			</ItemTemplate>
			</asp:TemplateColumn>
		<asp:TemplateColumn
			HeaderText="Location"
			SortExpression="location"
			ItemStyle-VerticalAlign="Top">
			<ItemTemplate>
				<asp:Label ID="lblLocation" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "location") %>'></asp:Label>
			</ItemTemplate>
			</asp:TemplateColumn>
		<asp:TemplateColumn
			HeaderText="Notes"
			SortExpression="occurrence_description"
			ItemStyle-VerticalAlign="Top">
			<ItemTemplate>
				<asp:Label ID="lblDescription" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "occurrence_description") %>'></asp:Label>
			</ItemTemplate>
			</asp:TemplateColumn>
	</Columns>
</Arena:DataGrid>
