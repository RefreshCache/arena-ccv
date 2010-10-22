<%@ Control Language="c#" Inherits="ArenaWeb.UserControls.Custom.CCV.Core.LossReport" CodeFile="LossReport.ascx.cs" %>
<%@ Register TagPrefix="Arena" Namespace="Arena.Portal.UI" Assembly="Arena.Portal.UI" %>
<div style="background-color:#F4F2F2;border-top: solid 1px #B1B1B1">
<asp:Panel ID="pnlFilter" runat="server">
<table cellpadding="3" cellspacing="0" border="0">
<tr>
	<td valign="top" align="left" style="padding-left:10px;padding-top:10px"><img src="images/filter.gif" border="0"></td>
	<td valign="top">
        <table cellpadding="0" cellspacing="3" border="0">
        <tr>
            <td class="formLabel" nowrap="nowrap">From</td>
            <td>
                <Arena:DateTextBox ID="tbFilterFrom" Runat="server" style="width:100px" CssClass="formItem" MaxLength="10" InvalidValueMessage="From Date must be a valid date!" />
            </td>
        </tr>
        <tr>
            <td class="formLabel" nowrap="nowrap">Through</td>
            <td>
                <Arena:DateTextBox ID="tbFilterTo" Runat="server" style="width:100px" CssClass="formItem" MaxLength="10" InvalidValueMessage="To Date must be a valid date!" />
            </td>
        </tr>
		<tr>
			<td colspan="2" align="left">
				<asp:Button ID="btnApply" Runat="server" CssClass="smallText" Text="Apply Filter"></asp:Button>
			</td>
		</tr>
		</table>
    </td>
	<td valign="top" style="padding-left:20px">
		<table cellpadding="0" cellspacing="3" border="0">
		<tr>
			<td class="formLabel" nowrap>Pastor</td>
			<td style="padding-left:5px">
				<asp:DropDownList ID="ddlPastor" Runat="server" CssClass="formItem"/>
			</td>
		</tr>
		<tr>
			<td class="formLabel" nowrap>Show Processed</td>
			<td style="padding-left:5px">
				<asp:CheckBox ID="cbProcessed" Runat="server" CssClass="formItem"/>
			</td>
		</tr>
		</table>
	</td>
</tr>
</table>
</asp:Panel>
</div>
<Arena:DataGrid 
	id="dgLosses" 
	Runat="server" DataKeyField="family_id" 
	AllowSorting="true">
	<Columns>
		<asp:boundcolumn 
			datafield="family_id" 
			Visible="False"></asp:boundcolumn>
		<Arena:SelectColumn 
		    HeaderText="Process"
		    DataField="processed"></Arena:SelectColumn>
		<Arena:SelectColumn 
		    HeaderText="Send<br/>Email"
		    DataField="send_email"></Arena:SelectColumn>
		<arena:BooleanImageColumn 
 			HeaderText="Sent" 
			datafield="sent" 
			SortExpression="sent"
			ReadOnly="True"></arena:BooleanImageColumn>
		<asp:TemplateColumn
			HeaderText="Family Head"
			ItemStyle-Wrap="False"
			ItemStyle-VerticalAlign="Top"
			SortExpression="person_name">
			<ItemTemplate>
				<Arena:PersonLabel ID="plPerson" runat="server"  
				    PersonPageID="7"
					PersonGUID='<%# Eval("person_guid") %>' 
					PersonName='<%# Eval("person_name") %>' 
					HasPhoto='<%# Eval("person_blob_id") != DBNull.Value %>'
				/>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:boundcolumn 
 			HeaderText="Family Members" 
			datafield="family_members"
			ReadOnly="True"></asp:boundcolumn>
<%--	<asp:HyperLinkColumn  
 			HeaderText="Area" 
			SortExpression="area_name"
			DataNavigateUrlField="area_id" 
			DataTextField="area_name" 
			DataNavigateUrlFormatString="default.aspx?page=1203&area={0}"></asp:HyperLinkColumn> --%>
		<asp:TemplateColumn
			HeaderText="Pastor"
			ItemStyle-Wrap="False"
			ItemStyle-VerticalAlign="Top"
			SortExpression="pastor_name">
			<ItemTemplate>
				<Arena:PersonLabel ID="plPastor" runat="server"  
				    PersonPageID="7"
					PersonGUID='<%# Eval("pastor_guid") %>' 
					PersonName='<%# Eval("pastor_name") %>' 
					HasPhoto='<%# Eval("pastor_blob_id") != DBNull.Value %>'
				/>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:boundcolumn 
 			HeaderText="Lost" 
			datafield="loss_date" 
			SortExpression="loss_date"
			DataFormatString="{0:MM/dd/yy}"
			ReadOnly="True"></asp:boundcolumn>
<%--		<asp:boundcolumn 
 			HeaderText="Times<br/>Lost" 
			datafield="times_lost" 
			SortExpression="times_lost"
			ReadOnly="True"></asp:boundcolumn>
--%>			
		<asp:boundcolumn 
 			HeaderText="First Attended" 
			datafield="first_attended" 
			SortExpression="first_attended"
			DataFormatString="{0:MM/dd/yy}"
			ReadOnly="True"></asp:boundcolumn>
		<asp:boundcolumn 
 			HeaderText="Last Attended" 
			datafield="last_attended" 
			SortExpression="last_attended"
			DataFormatString="{0:MM/dd/yy}"
			ReadOnly="True"></asp:boundcolumn>
		<asp:boundcolumn 
 			HeaderText="Last Gave" 
			datafield="last_gave" 
			SortExpression="last_gave"
			DataFormatString="{0:MM/dd/yy}"
			ReadOnly="True"></asp:boundcolumn>
		<asp:boundcolumn 
 			HeaderText="Times Given<br/>(12 mos)" 
			datafield="times_gave_last_year" 
			SortExpression="times_gave_last_year"
			ReadOnly="True"></asp:boundcolumn>
			
		<asp:boundcolumn 
 			HeaderText="Starting<br/>Point" 
			datafield="starting_point" 
			SortExpression="starting_point"
			DataFormatString="{0:MM/dd/yy}"
			ReadOnly="True"></asp:boundcolumn>
		<asp:boundcolumn 
 			HeaderText="Baptized"
			datafield="baptism"
			SortExpression="starting_point"
			DataFormatString="{0:MM/dd/yy}"
			ReadOnly="True"></asp:boundcolumn>
		<arena:BooleanImageColumn 
 			HeaderText="Group" 
			datafield="neighborhood_group" 
			SortExpression="neighborhood_group"
			ReadOnly="True"></arena:BooleanImageColumn>

	</Columns>
</Arena:DataGrid>
<asp:Button ID="btnProcess" Runat="server" CssClass="smallText" Text="Update"></asp:Button>
