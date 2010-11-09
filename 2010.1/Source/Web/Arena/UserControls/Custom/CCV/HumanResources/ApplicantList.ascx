<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ApplicantList.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.HumanResources.ApplicantList" %>
<%@ Register TagPrefix="Arena" Namespace="Arena.Portal.UI" Assembly="Arena.Portal.UI" %>

<script type="text/javascript">
	function toggleCbSelection(cbImage)
	{
		if (cbImage.src.indexOf("gridCbBlank") >= 0)
			cbImage.src = cbImage.src.replace("gridCbBlank","gridCbInclude");
		else
			cbImage.src = cbImage.src.replace("gridCbInclude","gridCbBlank");
		BuildSelectList();
	}
	
	function CheckAllUpdate(cbImage)
	{
		toggleCbSelection(cbImage);
		
		var coll = document.getElementsByTagName('IMG');
		if (coll != null)
			for (i = 0; i < coll.length; i++)
				if (coll[i].src.indexOf("gridCb") >= 0)
					if(coll[i] != cbImage)
						coll[i].src = cbImage.src;
		BuildSelectList();
	}

	function BuildSelectList()
	{
		var includeList = '';
		
		var imageList = document.getElementsByTagName("IMG");
		for (var i = 0; i < imageList.length; i++)
			if (imageList[i].src.indexOf("CbInclude") >= 0 && imageList[i].attributes["applicant_id"] != null)
			{
				if (includeList != '') includeList += ',';
				includeList += imageList[i].attributes["applicant_id"].value;
			}
		
		SaveSelectList(includeList);
	}
</script>

<asp:UpdatePanel ID="upContainer" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlList" runat="server" CssClass="container">
            <input type="hidden" id="ihIncludeList" runat="server" name="ihIncludeList" />

                <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnApply" CssClass="listFilter">
                    <table cellpadding="0" cellspacing="3" border="0">
                        <tr>
                            <td valign="top" rowspan="4" align="left" style="padding-left:10px;padding-top:10px">
                                <img src="images/filter.gif" border="0" />
                            </td>
                            <td class="formLabel">
                                Job Posting:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPostings" runat="server" CssClass="formItem" />
                            </td>
                        </tr>
                        <tr>
                            <td class="formLabel">
                                Rejection Notice Sent:
                            </td>
                            <td>
                                <asp:CheckBox ID="cbRejectionSent" runat="server" CssClass="formItem" />
                            </td>
                        </tr>
                        <tr>
                            <td class="formLabel">
                                Reviewed By HR:
                            </td>
                            <td>
                                <asp:CheckBox ID="cbReviewed" runat="server" CssClass="formItem" />
                            </td>
                        </tr>
                        <tr>
                            <td><asp:Button ID="btnApply" Runat="server" CssClass="smallText" Text="Apply Filter" /></td>
                            <td>&nbsp;</td>
                            <td style="width:100px;">&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>

            <Arena:DataGrid 
                id="dgApplicants" 
                Runat="server"
                AllowSorting="true"
                Width="100%">
                <Columns>
                    <asp:boundcolumn 
                        HeaderText="ID" 
                        DataField="applicant_id"
                        Visible="False"></asp:boundcolumn>
                    <asp:boundcolumn 
                        HeaderText="Guid" 
                        DataField="guid"
                        Visible="False"></asp:boundcolumn>
                    <asp:TemplateColumn
                        HeaderStyle-CssClass="reportHeader" 
                        ItemStyle-Wrap="False">
                        <HeaderTemplate>
                            <asp:Image ID="imgSelect" Runat="server" ImageUrl="~/images/gridCbBlank.gif" onclick="javascript:CheckAllUpdate(this);"></asp:Image>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Image ID="imgSelect" Runat="server" ImageUrl="~/images/gridCbBlank.gif" onclick="javascript:toggleCbSelection(this);"></asp:Image>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:HyperLinkColumn HeaderText="Name"
                        SortExpression="full_name"
                        DataTextField="full_name"
                        DataNavigateUrlField="guid"
                        ItemStyle-Wrap="false"></asp:HyperLinkColumn>
                    <asp:boundcolumn
                        HeaderText="Email" 
                        datafield="email"
                        SortExpression="email"></asp:boundcolumn>
                    <asp:boundcolumn
                        HeaderText="Job Title" 
                        datafield="title"
                        SortExpression="title"></asp:boundcolumn>
                    <asp:TemplateColumn
                        HeaderText="Reviewed By HR"
                        ItemStyle-Wrap="False" 
                        HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Center"
                        SortExpression="reviewed_by_hr">
                        <ItemTemplate>
                            <asp:Image ID="imgReviewedByHR" Runat="server" 
                                ImageUrl="~/images/check.gif" Visible='<%# Eval("reviewed_by_hr") %>'></asp:Image>
                        </ItemTemplate>
                        </asp:TemplateColumn>
                    <asp:TemplateColumn
                        HeaderText="Rejection Sent"
                        ItemStyle-Wrap="False" 
                        HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Center"
                        SortExpression="rejection_notice_sent">
                        <ItemTemplate>
                            <asp:Image ID="imgRejectionSent" Runat="server" 
                                ImageUrl="~/images/check.gif" Visible='<%# Eval("rejection_notice_sent") %>'></asp:Image>
                        </ItemTemplate>
                        </asp:TemplateColumn>
                    <asp:boundcolumn
                        HeaderText="Date Applied" 
                        datafield="date_created"
                        SortExpression="date_created"></asp:boundcolumn>
                </Columns>
            </Arena:DataGrid>
            <asp:Button ID="btnSendEmail" runat="server" CssClass="formItem" Text="Send Rejection Notice" />
            <asp:Button ID="btnReviewed" runat="server" CssClass="formItem" Text="Reviewed By HR" />
            <asp:Button ID="btnBack" runat="server" CssClass="formItem" Text="Back" />
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger EventName="ReBind" ControlID="dgApplicants" />
        <asp:AsyncPostBackTrigger EventName="Click" ControlID="btnApply" />
        <asp:AsyncPostBackTrigger EventName="DeleteCommand" ControlID="dgApplicants" />
    </Triggers>
</asp:UpdatePanel>