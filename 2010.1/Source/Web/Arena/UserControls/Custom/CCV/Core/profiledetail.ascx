<%@ Reference Control="~/usercontrols/core/profilemain.ascx" %>
<%@ Control Language="c#" Inherits="ArenaWeb.UserControls.Custom.CCV.Core.ProfileDetail" CodeFile="ProfileDetail.ascx.cs" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<link href="css/TabStrip.css" type="text/css" rel="stylesheet">    
<script language="javascript">

	function validateRemovePerson(who)
	{
		event.returnValue = confirm('Are you sure you want to remove the ' + who + '?')
	}
	
	function typeChanged(picker, oldType, personalName)
	{	
	    var newType = document.getElementById(picker).value.split("|")[0];
	    var message = 'Changing this parent to a ' + personalName + ' will make you the owner of it and it\'s children.';
	    if(newType == 0 && oldType != 0)
	        return confirm(message);
	    else return true;
	}
	
</script>

<input type="hidden" id="ihPersonList" runat="server" NAME="ihPersonList">
<Button ID="btnRefresh" Runat="server" style="visibility:hidden;display:none" onserverclick="btnRefresh_Click">Refresh</Button>
<asp:Panel ID="pnlView" Runat="server">
<table cellpadding="0" cellspacing="0" border="0">
<tr>
	<td valign="top">
		<table cellpadding="0" cellspacing="3" border="0">
			<tr>
				<td class="formLabel" nowrap>Name</td>
				<td class="formItem" nowrap><asp:Label ID="lblProfileName" Runat="server"></asp:Label></td>
			</tr>
			<tr>
				<td class="formLabel" nowrap>Parent</td>
				<td class="formItem" nowrap><asp:Label ID="lblParentProfile" Runat="server"></asp:Label></td>
			</tr>
			<tr id="trCampus" runat="server">
				<td class="formLabel" nowrap>Campus</td>
				<td class="formItem" nowrap><asp:Label ID="lblCampus" Runat="server"></asp:Label></td>
			</tr>
			<tr>
				<td class="formLabel" nowrap>Owner</td>
				<td class="formItem" nowrap><Arena:PersonLabel ID="plOwner" runat="server" /></asp:Label></td>
			</tr>
			<tr id="trQualifier" runat="server">
				<td class="formLabel" nowrap><asp:Label ID="lblQualifierLabel" Runat="server"></asp:Label></td>
				<td class="formItem" nowrap><asp:Label ID="lblQualifier" Runat="server"></asp:Label></td>
			</tr>
			<tr>
				<td class="formLabel" nowrap>Active</td>
				<td class="formItem" nowrap><asp:Label ID="lblActive" Runat="server"></asp:Label></td>
			</tr>
			<tr id="trCategoryLevel" runat="server">
				<td class="formLabel" nowrap>Category Level</td>
				<td class="formItem" nowrap><asp:Label ID="lblCategoryLevel" Runat="server"></asp:Label></td>
			</tr>
			<tr id="trHoursView" runat="server">
				<td class="formLabel" nowrap>Default Hours/Week</td>
				<td class="formItem" nowrap><asp:Label ID="lblHours" Runat="server"></asp:Label></td>
			</tr>
		</table>
		<table cellpadding="0" cellspacing="3" border="0">
			<tr id="trSummaryLabel" runat="server">
				<td class="formLabel" nowrap style="padding-top:5px">Summary</td>
			</tr>
			<tr id="trSummaryValue" runat="server">
				<td class="formItem" style="padding:5px"><asp:Label ID="lblSummary" Runat="server"></asp:Label></td>
			</tr>
			<tr id="trNotesLabel" runat="server">
				<td class="formLabel" nowrap style="padding-top:5px">Internal Notes</td>
			</tr>
			<tr id="trNotesValue" runat="server">
				<td class="formItem" style="padding:5px"><asp:Label ID="lblNotes" Runat="server"></asp:Label></td>
			</tr>
			<tr>
				<td>
					<asp:Button ID="btnEdit" Runat="server" Text="Edit Details" CssClass="smallText" onclick="btnEdit_Click"></asp:Button>
					<asp:Button ID="btnPrintRoster" Runat="server" Text="Print Members" CssClass="smallText"></asp:Button>
				</td>
			</tr>
		</table>
	</td>
	<td valign="top" style="padding-left:10px">
		<asp:Label ID="lblPhoto" Runat="server" Visible="False"></asp:Label>
	</td>
</tr>
</table>
</asp:Panel>

<asp:Panel ID="pnlEdit" Runat="server" Visible="False" style="padding-top:15px;padding-left:5px;padding-bottom:10px" DefaultButton="btnUpdate">

	<ComponentArt:TabStrip id="tabStrip" 
	CssClass="TopGroup"
	DefaultItemLookId="DefaultTabLook"
	DefaultSelectedItemLookId="SelectedTabLook"
	DefaultDisabledItemLookId="DisabledTabLook"
	DefaultGroupTabSpacing="1"
	ImagesBaseUrl="include/componentart/tabstrip/"
	MultiPageId="multiPage"
	runat="server">
	<Tabs>
		<componentart:TabStripTab ID="tstOverview" Runat="Server" Text="Overview"></componentart:TabStripTab>
		<componentart:TabStripTab ID="tstServingCriteria" Runat="Server" Text="Serving Criteria"></componentart:TabStripTab>
		<componentart:TabStripTab ID="tstServingDetails" Runat="Server" Text="Serving Details"></componentart:TabStripTab>
        <ComponentArt:TabStripTab ID="tstFields" runat="Server" Text="Fields"></ComponentArt:TabStripTab>
   	</Tabs>
	<ItemLooks>
		<ComponentArt:ItemLook LookId="DefaultTabLook" CssClass="DefaultTab" HoverCssClass="DefaultTabHover" LabelPaddingLeft="10" LabelPaddingRight="10" LabelPaddingTop="5" LabelPaddingBottom="4" LeftIconUrl="tab_left_icon.gif" RightIconUrl="tab_right_icon.gif" HoverLeftIconUrl="hover_tab_left_icon.gif" HoverRightIconUrl="hover_tab_right_icon.gif" LeftIconWidth="3" LeftIconHeight="21" RightIconWidth="3" RightIconHeight="21" />
		<ComponentArt:ItemLook LookId="SelectedTabLook" CssClass="SelectedTab" LabelPaddingLeft="10" LabelPaddingRight="10" LabelPaddingTop="4" LabelPaddingBottom="4" LeftIconUrl="selected_tab_left_icon.gif" RightIconUrl="selected_tab_right_icon.gif" LeftIconWidth="3" LeftIconHeight="21" RightIconWidth="3" RightIconHeight="21" />
	</ItemLooks>
	</ComponentArt:TabStrip>
	
	<ComponentArt:MultiPage id="multiPage" CssClass="MultiPage" runat="server" height="100%" Width="650px">

		<ComponentArt:PageView CssClass="PageContent" runat="server" id="pvOverview" >
			<table cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td valign="top">
						<table cellpadding="0" cellspacing="3" border="0">
						<tr>
							<td class="formLabel" nowrap>Name</td>
							<td class="formItem" nowrap>
								<asp:TextBox ID="tbProfileName" Runat="server" MaxLength="200" style="width:300px" CssClass="formItem"></asp:TextBox>
								<asp:RequiredFieldValidator ControlToValidate="tbProfileName" ID="reqProfileName" Runat= "server" CssClass="errorText" Display="Dynamic"> *</asp:RequiredFieldValidator>
							</td>
						</tr>
						<tr>
							<td class="formLabel" nowrap style="padding-top:5px">Parent</td>
							<td><Arena:ProfilePicker ID="profilePicker" runat="server" /></td>
						</tr>
						<tr id="trCampusEdit" runat="server">
							<td class="formLabel" nowrap style="padding-top:5px">Campus</td>
							<td><asp:DropDownList ID="ddlCampus" runat="server" CssClass="formItem"></asp:DropDownList></td>
						</tr>
						<tr>
							<td class="formLabel" nowrap valign="top" style="padding-top:5px">Owner</td>
							<td class="formItem" nowrap valign="top" style="padding-top:5px">
								<input type="hidden" id="ihOwnerID" runat="server">
								<asp:Label ID="lblOwnerEdit" Runat="server"></asp:Label>&nbsp;
								<a href="#" onclick='openSearchWindow("owner");'>Change...</a>&nbsp;
								<asp:LinkButton ID="lbRemoveOwner" Runat="server"><span onclick='validateRemovePerson("owner");'>Remove</span></asp:LinkButton>
							</td>
						</tr>
						<tr>
							<td class="formLabel" nowrap style="padding-top:25px">Relationships</td>
							<td class="formItem"  nowrap style="padding-top:25px">
							    <table>
							    <tr>
							        <td align="center">
							            <div class="smallText" style="padding-bottom:5px">Strength Between Owner & Members</div>
							            <asp:TextBox ID="tbOwnerRelationship" runat="server"></asp:TextBox>
							            <ajax:SliderExtender ID="slideOwnerRelationship" runat="server" TargetControlID="tbOwnerRelationship" 
							                Minimum="0" Maximum="4" Steps="5" Length="150"></ajax:SliderExtender>
							            <table width="180px" class="smallText">
							            <tr>
							                <td align="left">Weak</td>
							                <td align="center">Moderate</td>
							                <td align="right">Strong</td>
							            </tr>
							            </table>
							        </td>
							        <td style="padding-left:20px" align="center">
							            <div class="smallText" style="padding-bottom:5px">Strength Between Members</div>
							            <asp:TextBox ID="tbPeerRelationship" runat="server"></asp:TextBox>
							            <ajax:SliderExtender ID="slidePeerRelationship" runat="server" TargetControlID="tbPeerRelationship" 
							                Minimum="0" Maximum="4" Steps="5" Length="150"></ajax:SliderExtender>
							            <table width="180px" class="smallText">
							            <tr>
							                <td align="left">Weak</td>
							                <td align="center">Moderate</td>
							                <td align="right">Strong</td>
							            </tr>
							            </table>
							        </td>
							    </tr>
							    </table>
							</td>
						</tr>
						<tr id="trQualifierEdit" runat="server">
							<td class="formLabel" nowrap style="padding-top:5px"><asp:Label ID="lblQualifierEdit" Runat="server"></asp:Label></td>
							<td class="formItem"  nowrap style="padding-top:5px"><asp:DropDownList ID="ddlQualifier" Runat="server" CssClass="formItem"></asp:DropDownList></td>
						</tr>
						</table>
					</td>
					<td valign="top" style="padding-left:5px">
						<table cellpadding="0" cellspacing="3" border="0">
						<tr>
							<td class="formLabel" nowrap>Active</td>
							<td class="formItem" nowrap>
								<asp:CheckBox ID="cbActive" Runat="server" CssClass="formItem" Text=""></asp:CheckBox>
							</td>
						</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="formLabel" nowrap style="padding-top:25px" colspan="2">Internal Notes</td>
				</tr>
				<tr>
					<td class="formItem" colspan="2"><asp:TextBox ID="tbNotes" Runat="server" MaxLength="2000" TextMode="MultiLine" Rows="10" style="width:550px" CssClass="formItem"></asp:TextBox></td>
				</tr>
			</table>
		</ComponentArt:PageView>

		<ComponentArt:PageView CssClass="PageContent" runat="server" id="pvServingCriteria">
			<table cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td valign="top">
						<table cellpadding="0" cellspacing="3" border="0">
						<tr>
							<td class="formLabel" nowrap>Display to Public</td>
							<td class="formItem" nowrap><asp:CheckBox ID="cbDisplayToPublic" Runat="server" CssClass="formItem" Text=""></asp:CheckBox></td>
						</tr>
						<tr>
							<td class="formLabel" nowrap>Category Level</td>
							<td class="formItem" nowrap><asp:CheckBox ID="cbCategoryLevel" Runat="server" CssClass="formItem" Text=""></asp:CheckBox></td>
						</tr>
						<tr>
							<td class="formLabel" nowrap>Critical Need</td>
							<td class="formItem" nowrap><asp:CheckBox ID="cbCriticalNeed" Runat="server" CssClass="formItem" Text=""></asp:CheckBox></td>
						</tr>
						<tr>
							<td class="formLabel" nowrap>Default Hours/Week</td>
							<td class="formItem" nowrap>
								<asp:TextBox ID="tbHours" Runat="server" style="width:50px" CssClass="formItem"></asp:TextBox>
								<asp:RangeValidator ID="rvHours" Runat="server" ControlToValidate="tbHours" CssClass="errorText"
									Display="Dynamic" MinimumValue="0" MaximumValue="99" Type="Double" ErrorMessage="Hours/Week Must be Numeric!"> *</asp:RangeValidator>
							</td>
						</tr>
						<tr>
							<td class="formLabel" nowrap>Volunteers Needed</td>
							<td class="formItem" nowrap>
								<asp:TextBox ID="tbVolunteersNeeded" Runat="server" style="width:50px" CssClass="formItem"></asp:TextBox>
								<asp:RangeValidator ID="rvVolunteersNeeded" Runat="server" ControlToValidate="tbVolunteersNeeded" Type="Integer" ErrorMessage="Voluneers Needed must be a valid number!" MinimumValue="0" CssClass="errorText" MaximumValue="9999" Display="Dynamic"> *</asp:RangeValidator>
							</td>
						</tr>
						</table>
					</td>
					<td valign="top" style="padding-left:5px">
						<table cellpadding="0" cellspacing="3" border="0">
						<tr>
							<td class="formLabel" nowrap valign="top">Contact Info</td>
							<td class="formItem" valign="top"><asp:TextBox ID="tbContactInfo" Runat="server" TextMode="MultiLine" Rows="2" style="width:200px;" CssClass="formItem"></asp:TextBox></td>
						</tr>
						<tr>
							<td class="formLabel" nowrap>Contact Email</td>
							<td class="formItem" nowrap>
								<asp:TextBox ID="tbContactEmail" Runat="server" style="width:200px" CssClass="formItem"></asp:TextBox>
								<asp:RegularExpressionValidator id="revContactEmail" runat="server" ControlToValidate="tbContactEmail" CssClass="errorText"
									Display="Static" ValidationExpression="[\w\.\'_%-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+" 
									ErrorMessage="Invalid Contact Email Address"> *</asp:RegularExpressionValidator>
							</td>
						</tr>
						<tr>
							<td class="formLabel" nowrap>Video Link</td>
							<td class="formItem" nowrap>
								<asp:TextBox ID="tbVideoLink" Runat="server" style="width:200px" CssClass="formItem"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="formLabel" nowrap valign="top">Category Photo</td>
							<td class="formItem" nowrap valign="top">
								<div>
									<asp:Label ID="lblEditPhoto" Runat="server" Visible="False"></asp:Label>
									<asp:LinkButton id="lblRemovePhoto" Runat="server" Visible="False" CssClass="smallText">Remove Photo</asp:LinkButton>
								</div>
								<asp:Panel ID="pnlPhotoUpdate" Runat="server" CssClass="smallText" style="display:block;visibility:visible;cursor:pointer">Update Photo...</asp:Panel>
								<input type="file" id="ihFilePhoto" runat="server" class="smallText" style="width:200px;display:none;visibility:hidden" NAME="ihFilePhoto">
								<asp:Button ID="btnUploadPhoto" Runat="server" CssClass="smallText" Text="Upload" style="display:none;visibility:hidden"></asp:Button>
								<asp:Label ID="lblPhotoMessage" Runat="server" Visible="False"></asp:Label>
							</td>
						</tr>
						<tr>
							<td class="formLabel" nowrap valign="top">Serving Photo</td>
							<td class="formItem" nowrap valign="top">
								<div>
									<asp:Label ID="lblEditServingPhoto" Runat="server" Visible="False"></asp:Label>
									<asp:LinkButton id="lblRemoveServingPhoto" Runat="server" Visible="False" CssClass="smallText">Remove Serving Photo</asp:LinkButton>
								</div>
								<asp:Panel ID="pnlServingPhotoUpdate" Runat="server" CssClass="smallText" style="display:block;visibility:visible;cursor:pointer">Update Serving Photo...</asp:Panel>
								<input type="file" id="ihFileServingPhoto" runat="server" class="smallText" style="width:200px;display:none;visibility:hidden" NAME="ihFileServingPhoto">
								<asp:Button ID="btnUploadServingPhoto" Runat="server" CssClass="smallText" Text="Upload" style="display:none;visibility:hidden" CausesValidation="False"></asp:Button>
								<asp:Label ID="lblServingPhotoMessage" Runat="server" Visible="False"></asp:Label>
							</td>
						</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="formLabel" nowrap style="padding-top:20px" colspan="2">
						<table cellpadding="0" cellspacing="3" border="0">
						<tr>
							<td class="formLabel" nowrap valign="top">Weekly Commitment</td>
							<td class="formItem" valign="top"><asp:CheckBoxList ID="cblWeeklyCommitment" Runat="server" CssClass="formItem" RepeatLayout="Flow" RepeatDirection="Horizontal"></asp:CheckBoxList></td>
						</tr>
						<tr>
							<td class="formLabel" nowrap valign="top">Timeframe</td>
							<td class="formItem" valign="top"><asp:CheckBoxList ID="cblTimeframe" Runat="server" CssClass="formItem" RepeatLayout="Flow" RepeatDirection="Horizontal"></asp:CheckBoxList></td>
						</tr>
						<tr>
							<td class="formLabel" nowrap valign="top">Classification</td>
							<td class="formItem" valign="top"><asp:CheckBoxList ID="cblClassification" Runat="server" CssClass="formItem" RepeatLayout="Flow" RepeatDirection="Horizontal"></asp:CheckBoxList></td>
						</tr>
						<tr>
							<td class="formLabel" nowrap valign="top">Content Category</td>
							<td class="formItem" valign="top"><asp:CheckBoxList ID="cblContentCategory" Runat="server" CssClass="formItem" RepeatLayout="Flow" RepeatDirection="Horizontal"></asp:CheckBoxList></td>
						</tr>
						<tr>
							<td class="formLabel" nowrap valign="top">Duration</td>
							<td class="formItem" valign="top"><asp:CheckBoxList ID="cblDuration" Runat="server" CssClass="formItem" RepeatLayout="Flow" RepeatDirection="Horizontal"></asp:CheckBoxList></td>
						</tr>
						<tr>
							<td class="formLabel" nowrap valign="top">Spiritual Gifts</td>
							<td class="formItem" valign="top"><asp:CheckBoxList ID="cblSpiritualGifts" Runat="server" CssClass="formItem" RepeatLayout="Flow" RepeatColumns="6" RepeatDirection="Horizontal"></asp:CheckBoxList></td>
						</tr>
						</table>
					</td>
				</tr>
			</table>
		</ComponentArt:PageView>

		<ComponentArt:PageView CssClass="PageContent" runat="server" id="pvServingDetails">
			<table cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td class="formLabel" nowrap style="padding-top:5px" colspan="2">Summary</td>
				</tr>
				<tr>
					<td class="formItem" colspan="2"><asp:TextBox ID="tbServingSummary" Runat="server" MaxLength="5000" TextMode="MultiLine" Rows="3" style="width:550px" CssClass="formItem"></asp:TextBox></td>
				</tr>
				<tr>
					<td class="formLabel" nowrap style="padding-top:5px" colspan="2">Details</td>
				</tr>
				<tr>
					<td class="formItem" colspan="2"><asp:TextBox ID="tbServingDetails" Runat="server" TextMode="MultiLine" Rows="6" style="width:550px" CssClass="formItem"></asp:TextBox></td>
				</tr>
				<tr>
					<td class="formLabel" nowrap style="padding-top:5px" colspan="2">
						<asp:Label ID="lblExperience" Runat="server">Experience Skills</asp:Label>
					</td>
				</tr>
				<tr>
					<td class="formItem" colspan="2"><asp:TextBox ID="tbExperience" Runat="server" TextMode="MultiLine" Rows="4" style="width:550px" CssClass="formItem"></asp:TextBox></td>
				</tr>
				<tr>
					<td class="formLabel" nowrap style="padding-top:5px" colspan="2">Schedule Notes</td>
				</tr>
				<tr>
					<td class="formItem" colspan="2"><asp:TextBox ID="tbScheduleNotes" Runat="server" TextMode="MultiLine" Rows="4" style="width:550px" CssClass="formItem"></asp:TextBox></td>
				</tr>
			</table>
		</ComponentArt:PageView>

        <ComponentArt:PageView CssClass="PageContent" runat="server" ID="pvFields">
            <div class="heading2">
                Custom Fields</div>
            <div class="smallText">
                Custom Fields are fields that you can add to your <%=tagTitle%> so that information specific
                to your <%=tagTitle%> can be entered when adding a person.</div>
            <asp:UpdatePanel ID="upGenericeLoadPanel" runat="server">
                <ContentTemplate>
                    <asp:DataList ID="dlFields" runat="server" CssClass="list" CellPadding="3" CellSpacing="0"
                        ExtractTemplateRows="true">
                        <HeaderTemplate>
                            <asp:Table ID="Table6" runat="server">
                                <asp:TableRow CssClass="listHeader">
                                    <asp:TableCell CssClass="listHeader" VerticalAlign="Bottom" HorizontalAlign="left">Label</asp:TableCell>
                                    <asp:TableCell CssClass="listHeader" VerticalAlign="Bottom" HorizontalAlign="Center">Visible</asp:TableCell>
                                    <asp:TableCell CssClass="listHeader" VerticalAlign="Bottom" HorizontalAlign="Center">Required</asp:TableCell>
                                    <asp:TableCell CssClass="listHeader" VerticalAlign="Bottom" HorizontalAlign="left">Location</asp:TableCell>
                                    <asp:TableCell CssClass="listHeader" VerticalAlign="Bottom" HorizontalAlign="left">Type</asp:TableCell>
                                    <asp:TableCell CssClass="listHeader" VerticalAlign="Bottom" HorizontalAlign="left">Type Qualifier</asp:TableCell>
                                    <asp:TableCell CssClass="listHeader" VerticalAlign="Bottom" HorizontalAlign="right"></asp:TableCell>
                                    <asp:TableCell CssClass="listHeader" VerticalAlign="Bottom" HorizontalAlign="right"></asp:TableCell>
                                    <asp:TableCell CssClass="listHeader" VerticalAlign="Bottom" HorizontalAlign="right"></asp:TableCell>
                                    <asp:TableCell CssClass="listHeader" VerticalAlign="Bottom" HorizontalAlign="right"></asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Table ID="Table7" runat="server">
                                <asp:TableRow CssClass="listItem" ID="trField" runat="server">
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="left"><%# DataBinder.Eval(Container.DataItem, "Title" )%></asp:TableCell>
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="center">
                                        <asp:Image ID="imgVisible" runat="server" ImageUrl="~/images/check.gif" BorderWidth="0"
                                            Visible="false" /></asp:TableCell>
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="center">
                                        <asp:Image ID="imgRequired" runat="server" ImageUrl="~/images/check.gif" BorderWidth="0"
                                            Visible="false" /></asp:TableCell>
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="left"><%# GetFormattedFieldLocation(Eval("Location"))%></asp:TableCell>
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="left"><%# GetFormattedFieldType(Eval("FieldType"))%></asp:TableCell>
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="left"><%# GetFormattedQualifier(Eval("FieldType"), Eval("FieldTypeQualifier").ToString())%></asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="right">
                                        <asp:ImageButton ID="ibMoveDown" runat="server" CommandName="MoveDown" ImageUrl="~/images/down.gif" />
                                    </asp:TableCell>                      
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="right">
                                        <asp:ImageButton ID="ibMoveUp" runat="server" CommandName="MoveUp" ImageUrl="~/images/up.gif" />
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="right">
                                        <asp:ImageButton ID="ibEdit" runat="server" CommandName="Edit" ImageUrl="~/images/edit.gif" />
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="right">
                                        <asp:ImageButton ID="ibDelete" runat="server" CommandName="Delete" ImageUrl="~/images/delete.gif" />
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <asp:Table ID="Table8" runat="server">
                                <asp:TableRow ID="TableRow1" CssClass="listAltItem" runat="server">
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="left"><%# DataBinder.Eval(Container.DataItem, "Title" )%></asp:TableCell>
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="center">
                                        <asp:Image ID="imgVisible" runat="server" ImageUrl="~/images/check.gif" BorderWidth="0"
                                            Visible="false" /></asp:TableCell>
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="center">
                                        <asp:Image ID="imgRequired" runat="server" ImageUrl="~/images/check.gif" BorderWidth="0"
                                            Visible="false" /></asp:TableCell>
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="left"><%# GetFormattedFieldLocation(Eval("Location"))%></asp:TableCell>
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="left"><%# GetFormattedFieldType(Eval("FieldType"))%></asp:TableCell>
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="left"><%# GetFormattedQualifier(Eval("FieldType"), Eval("FieldTypeQualifier").ToString())%></asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="right">
                                        <asp:ImageButton ID="ibMoveDown" runat="server" CommandName="MoveDown" ImageUrl="~/images/down.gif" />
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="right">
                                        <asp:ImageButton ID="ibMoveUp" runat="server" CommandName="MoveUp" ImageUrl="~/images/up.gif" />
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="right">
                                        <asp:ImageButton ID="ibEdit" runat="server" CommandName="Edit" ImageUrl="~/images/edit.gif" />
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="right">
                                        <asp:ImageButton ID="ibDelete" runat="server" CommandName="Delete" ImageUrl="~/images/delete.gif" />
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </AlternatingItemTemplate>
                        <EditItemTemplate>
                            <asp:Table ID="Table9" runat="server">
                                <asp:TableRow CssClass="listEditItem">
                                    <asp:TableCell ColumnSpan="8" VerticalAlign="top" HorizontalAlign="left" Style="border: solid 1px #B1B1B1;
                                        background-color: LightGrey">
                                        <table cellpadding="0" cellspacing="3" border="0">
                                            <tr>
                                                <td class="formItem" nowrap valign="middle">
                                                    Label
                                                </td>
                                                <td class="formItem" nowrap valign="middle">
                                                    <asp:TextBox ID="tbName" runat="server" MaxLength="100" CssClass="formItem" Text='<%# DataBinder.Eval(Container.DataItem, "Title" )%>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ControlToValidate="tbName" ID="reqName" runat="server"
                                                        CssClass="errorText" Display="Dynamic" ErrorMessage="Label is Required"> *</asp:RequiredFieldValidator>
                                                </td>
                                                <td class="formItem" nowrap valign="middle">
                                                    Label Location
                                                </td>
                                                <td class="formItem" nowrap valign="middle">
                                                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="formItem">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItem" nowrap valign="middle">
                                                    Field Type
                                                </td>
                                                <td class="formItem" nowrap valign="middle">
                                                    <Arena:DropDownListWithCommand ID="ddlFieldType" runat="server" CommandName="ChangeType" CssClass="formItem" AutoPostBack="true" >
                                                    </Arena:DropDownListWithCommand>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td class="formItem" nowrap valign="middle">
                                                    Size
                                                </td>
                                                <td id="tdSize" runat="server" class="formItem" nowrap valign="middle">
                                                    <asp:TextBox ID="tbRows" runat="server" MaxLength="3" CssClass="formItem" Width="40px"
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "Height" )%>'></asp:TextBox>
                                                    Rows
                                                    <asp:RangeValidator ID="rvRows" runat="server" Display="Dynamic" ControlToValidate="tbRows"
                                                        MinimumValue="0" MaximumValue="999" ErrorMessage="Invalid value for Size/Rows"
                                                        Type="integer"> *</asp:RangeValidator>
                                                    &nbsp;&nbsp;
                                                    <asp:TextBox ID="tbWidth" runat="server" MaxLength="4" CssClass="formItem" Width="40px"
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "Width" )%>'></asp:TextBox>
                                                    Pixels Wide
                                                    <asp:RangeValidator ID="rvWidth" runat="server" Display="Dynamic" ControlToValidate="tbWidth"
                                                        MinimumValue="0" MaximumValue="9999" ErrorMessage="Invalid value for Size/Pixels Wide"
                                                        Type="integer"> *</asp:RangeValidator>
                                                </td>
                                            </tr>
                                            <tr id="trQualifier" runat="server">
                                                <td class="formItem" nowrap valign="top">
                                                    <asp:Literal ID="ltQualifier" runat="server"></asp:Literal>
                                                </td>
                                                <td class="formItem" nowrap valign="top" colspan="3">
                                                    <Arena:DynamicControlsPlaceholder ID="phQualifier" runat="server"></Arena:DynamicControlsPlaceholder>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td class="formLabel" nowrap valign="top" colspan="3">
                                                    <asp:CheckBox ID="cbVisible" runat="server" CssClass="formItem" Text="Visible" TextAlign="right">
                                                    </asp:CheckBox>&nbsp;&nbsp;&nbsp;
                                                    <asp:CheckBox ID="cbRequired" runat="server" CssClass="formItem" Text="Required"
                                                        TextAlign="right"></asp:CheckBox>&nbsp;&nbsp;&nbsp;
                                                    <asp:CheckBox ID="cbReadOnly" runat="server" CssClass="formItem" Text="Read-Only"
                                                        TextAlign="right"></asp:CheckBox>&nbsp;&nbsp;&nbsp;
                                                    <asp:CheckBox ID="cbAutoFill" runat="server" CssClass="formItem" Text="Enable Auto-Fill"
                                                        TextAlign="right"></asp:CheckBox>&nbsp;&nbsp;&nbsp;
                                                    <asp:CheckBox ID="cbShowOnList" runat="server" CssClass="formItem" Text="Show On List"
                                                        TextAlign="right"></asp:CheckBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="right">
                                        <asp:ImageButton ID="ibUpdate" runat="server" CommandName="Update" ImageUrl="~/images/update.gif" />
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="right">
                                        <asp:ImageButton ID="ibCancel" runat="server" CommandName="Cancel" ImageUrl="~/images/cancel.gif" />
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Table ID="Table10" runat="server">
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="10" CssClass="listPager" HorizontalAlign="right">
                                        <asp:ImageButton ID="btnAddField" runat="server" CausesValidation="false" ImageUrl="~/images/add_field.gif"
                                            CssClass="listPagerItem" OnClick="btnAddField_Click" />
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </FooterTemplate>
                    </asp:DataList>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <asp:Panel ID="pnlFieldModules" runat="server">
                <div class="heading2">
                    Custom Field Modules</div>
                <div class="smallText">
                    Field Modules are pre-configured groupings of Custom Fields that can be added to
                    your <%=tagTitle%>. If you have a group of fields that you consistently use,
                    these fields can be set up as a Field Module so that you can easily add them here.
                    The Arena Administrator(s) can set these field modules up for you.</div>
                <asp:UpdatePanel ID="upFieldModules" runat="server">
                    <ContentTemplate>
                        <asp:DataList ID="dlFieldModules" runat="server" CssClass="list" CellPadding="3"
                            CellSpacing="0" ExtractTemplateRows="true">
                            <HeaderTemplate>
                                <asp:Table ID="Table1" runat="server">
                                    <asp:TableRow CssClass="listHeader">
                                        <asp:TableCell CssClass="listHeader" VerticalAlign="Bottom" HorizontalAlign="left">Category</asp:TableCell>
                                        <asp:TableCell CssClass="listHeader" VerticalAlign="Bottom" HorizontalAlign="left">Module</asp:TableCell>
                                        <asp:TableCell CssClass="listHeader" VerticalAlign="Bottom" HorizontalAlign="left">Attributes</asp:TableCell>
                                        <asp:TableCell CssClass="listHeader" VerticalAlign="Bottom" HorizontalAlign="right">Remove</asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Table ID="Table2" runat="server">
                                    <asp:TableRow CssClass="listItem">
                                        <asp:TableCell VerticalAlign="top" HorizontalAlign="left"><%# DataBinder.Eval(Container.DataItem, "Category" )%></asp:TableCell>
                                        <asp:TableCell VerticalAlign="top" HorizontalAlign="left"><%# DataBinder.Eval(Container.DataItem, "Title" )%></asp:TableCell>
                                        <asp:TableCell VerticalAlign="top" HorizontalAlign="left"><%# GetFormattedString(DataBinder.Eval(Container.DataItem, "CustomFieldModuleFields"))%></asp:TableCell>
                                        <asp:TableCell VerticalAlign="top" HorizontalAlign="right">
                                            <asp:ImageButton ID="ibDelete" runat="server" CommandName="Delete" ImageUrl="~/images/delete.gif" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <asp:Table ID="Table3" runat="server">
                                    <asp:TableRow CssClass="listAltItem">
                                        <asp:TableCell VerticalAlign="top" HorizontalAlign="left"><%# DataBinder.Eval(Container.DataItem, "Category" )%></asp:TableCell>
                                        <asp:TableCell VerticalAlign="top" HorizontalAlign="left"><%# DataBinder.Eval(Container.DataItem, "Title" )%></asp:TableCell>
                                        <asp:TableCell VerticalAlign="top" HorizontalAlign="left"><%# GetFormattedString(DataBinder.Eval(Container.DataItem, "CustomFieldModuleFields"))%></asp:TableCell>
                                        <asp:TableCell VerticalAlign="top" HorizontalAlign="right">
                                            <asp:ImageButton ID="ibDelete" runat="server" CommandName="Delete" ImageUrl="~/images/delete.gif" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </AlternatingItemTemplate>
                            <FooterTemplate>
                                <asp:Table ID="Table5" runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell ColumnSpan="4" CssClass="listPager" HorizontalAlign="right">
                                            Add New Field Module:
                                            <asp:DropDownList ID="ddlFieldModules" runat="server" CssClass="listItem">
                                            </asp:DropDownList>
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnAddFieldModule" runat="server" CausesValidation="false" Text="Add"
                                                CssClass="listPagerItem" CommandName="Add" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </FooterTemplate>
                        </asp:DataList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </ComponentArt:PageView>

	</ComponentArt:MultiPage>
	<br />   
	<asp:Button ID="btnUpdate" Runat="server" Text="Update" CssClass="smallText" onclick="btnUpdate_Click"></asp:Button>&nbsp;
	<asp:Button ID="btnCancel" Runat="server" Text="Cancel" CssClass="smallText" CausesValidation="False" onclick="btnCancel_Click"></asp:Button><br>
	
</asp:Panel>

<asp:Label ID="lblMessage" Runat="server" CssClass="normalText"></asp:Label>
