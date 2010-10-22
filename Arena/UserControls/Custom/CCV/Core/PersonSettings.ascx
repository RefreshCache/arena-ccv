<%@ Control Language="c#" Inherits="ArenaWeb.UserControls.Custom.CCV.Core.PersonSettings" CodeFile="PersonSettings.ascx.cs" %>
<%@ Register TagPrefix="Arena" Namespace="Arena.Portal.UI" Assembly="Arena.Portal.UI" %>

<asp:UpdatePanel UpdateMode="Conditional"  ID="pnlHistory" Runat="server">
<ContentTemplate>
<table width="100%" cellpadding="0" cellspacing="0" border="0" noprint>
<tr>
	<td width="11" valign="top"><img src="images/bar_left.jpg" border="0"></td>
	<td width="25" style="background-image: url(images/bar_middle.jpg);" valign="top"><img src="images/whitepages/history.jpg" border="0"></td>
	<td width="*" style="background-image: url(images/bar_middle.jpg);" valign="middle" nowrap class="heading2" >
		<div>Person Settings</div>
	</td>
	<td width="155" valign="bottom" align="right" style="BACKGROUND-IMAGE:url(images/bar_middle.jpg)">
		&nbsp;&nbsp;
	</td>
	<td width="11" valign="top"><img src="images/bar_right.jpg" border="0"></td>
</tr>
</table>

<div>
<table cellpadding="1" cellspacing="0" border="0" width="100%" noprint bgcolor="#FFFFFF">

</table>
</div>
<br/><br/>
</ContentTemplate>
</asp:UpdatePanel>		

<asp:Panel ID="pnlPersonSettings" Runat="server">
<asp:LinkButton ID="lbAdd" Runat="server" CssClass="smallText" Visible="False">Add</asp:LinkButton>
<asp:Panel ID="pnlFilter" runat="server" style="background-color:#F4F2F2;border-top: solid 1px #B1B1B1">
<table cellpadding="3" cellspacing="0" border="0">
<tr>
	<td valign="top" align="left" style="padding-left:10px;padding-top:10px"><img src="images/filter.gif" border="0"></td>
	<td valign="top">
        <table cellpadding="5" cellspacing="0" border="0" style="padding-top:15px;padding-bottom:15px">
        <tr>
	        <td valign="middle" style="padding-left:10px;" class="formLabel">
		        Key
	        </td>
	        <td valign="middle">
	            <asp:TextBox ID="tbKeyFilter" runat="server" CssClass="formItem"></asp:TextBox>
		        <asp:Button ID="btnApply" Runat="server" CssClass="smallText" Text="Apply Filter"></asp:Button>
	        </td>
        </tr>
        </table>
	</td>
</tr>
</table>
</asp:Panel>
<Arena:DataGrid 
	id="dgPersonSettings" 
	Runat="server"
	AllowSorting="true">
	<Columns>
		<asp:boundcolumn 
 			HeaderText="ID" 
			datafield="key" 
			visible="false"></asp:boundcolumn>
		<Asp:ButtonColumn 
 			HeaderText="Setting" 
			CommandName="EditPersonSetting" 
			ButtonType="LinkButton" 
			SortExpression="Key"
			DataTextField="Key"
			ItemStyle-Wrap="false"></asp:ButtonColumn>
		<asp:TemplateColumn
 			HeaderText="Value"
 			ItemStyle-Wrap="false">
			<ItemTemplate><%# EncodedValue(DataBinder.Eval(Container.DataItem, "Value")) %></ItemTemplate>
			</asp:TemplateColumn>
	</Columns>
</Arena:DataGrid>
</asp:Panel>
<asp:Panel ID="pnlDetails" Runat="server">
<input type="hidden" id="ihPersonSettingID" runat="server">
<table cellpadding="0" cellspacing="5" border="0">
	<tr>
		<td class="formLabel" nowrap valign="middle">Key</td>
		<td class="formItem" nowrap valign=middle>
			<asp:TextBox ID="tbKey" Runat="server" MaxLength="200" Width="250" CssClass="formItem"></asp:TextBox>
			<asp:RequiredFieldValidator ControlToValidate="tbKey" ID="reqKey" Runat= "server" CssClass="errorText" Display="Dynamic" ErrorMessage="Key is Required"> *</asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr>
		<td class="formLabel" nowrap valign="middle">Value</td>
		<td class="formItem" nowrap valign="middle">
			<asp:TextBox ID="tbValue" Runat="server" Width="250" MaxLength="200" CssClass="formItem"></asp:TextBox>
		</td>
	</tr>
</table>
<table cellpadding="0" cellspacing="5" border="0">
	<tr>
		<td class="formItem"><asp:Label ID="lblMessage" Runat="server" CssClass="errorText"></asp:Label></td>
	</tr>
	<tr>
		<td>
			<asp:Button ID="btnUpdate" Runat="server" Text="Update" CssClass="smallText" onclick="btnUpdate_Click"></asp:Button>
			<asp:Button ID="btnCancel" Runat="server" Text="Cancel" CssClass="smallText" CausesValidation="False" onclick="btnCancel_Click"></asp:Button>
		</td>
	</tr>
</table>
</asp:Panel>