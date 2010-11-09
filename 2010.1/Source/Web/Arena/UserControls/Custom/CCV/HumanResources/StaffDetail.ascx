<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StaffDetail.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.HumanResources.StaffDetail" %>
<%@ Register TagPrefix="Arena" Namespace="Arena.Portal.UI" Assembly="Arena.Portal.UI" %>

<script type="text/javascript">

	function validateRemovePerson(who)
	{
	    return confirm('Are you sure you want to remove the ' + who + '?');
	}
	
</script>

<asp:Panel ID="pnlContainer" runat="server">
    <input type="hidden" id="ihPersonList" runat="server" name="ihPersonList" />
    <input type="hidden" id="ihSearchType" runat="server" name="ihSearchType" />
    <button ID="bRefresh" Runat="server" style="visibility:hidden; display:none" onserverclick="bRefresh_Click">Refresh</button>
    <asp:UpdatePanel ID="upError" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div id="error" style="padding-top: 2px; padding-bottom: 4px;">
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="errorText" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <ajax:TabContainer ID="tcStaffInfo" runat="server">
        <ajax:TabPanel ID="tpDept" runat="server" HeaderText="Department">
            <ContentTemplate>
                
                <asp:UpdatePanel ID="upDept" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblClass" runat="server" Text="Class: " CssClass="formLabel" AssociatedControlID="ddlClassLU" />
                                </td>
                                <td style="white-space: nowrap;">
                                    <Arena:LookupDropDown ID="ddlClassLU" runat="server" CssClass="smallText" Width="150" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSubDept" runat="server" Text="Sub-Department: " CssClass="formLabel" AssociatedControlID="ddlSubDeptLU" />
                                </td>
                                <td style="white-space: nowrap;">
                                    <Arena:LookupDropDown ID="ddlSubDeptLU" runat="server" CssClass="smallText" Width="150" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTitle" runat="server" Text="Title: " CssClass="formLabel" AssociatedControlID="tbTitle" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbTitle" runat="server" CssClass="formItem" Width="200" />
                                </td>
                            </tr>
                            <tr>
                                <td class="formLabel" style="vertical-align: top;">Supervisor:</td>
                                <td class="formItem"  style="text-align: left;">
                                    <input type="hidden" id="ihSupervisorID" runat="server" name="ihSupervisorID" />
                                    <asp:Label ID="lblSupervisorEdit" runat="server"></asp:Label>
                                    <asp:PlaceHolder id="phSupervisor" runat="server"><a href="#" onclick="openSearchWindow('supervisor');">Change...</a>&nbsp;</asp:PlaceHolder>
                                    <asp:LinkButton ID="lbRemoveSupervisor" Runat="server" OnClientClick="return validateRemovePerson('supervisor');">Remove</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                
            </ContentTemplate>
        </ajax:TabPanel>
        
        <ajax:TabPanel ID="tpEmpStatus" runat="server" HeaderText="Employee Status">
            <ContentTemplate>
            
                <asp:UpdatePanel ID="upEmpDetail" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <input type="hidden" id="ihSalary" name="ihSalary" />
                        <asp:Panel ID="pnlEmpDetail" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblHireDate" runat="server" Text="Hire Date: " CssClass="formLabel" AssociatedControlID="dtbHireDate" />
                                    </td>
                                    <td>
                                        <Arena:DateTextBox ID="dtbHireDate" runat="server" CssClass="formItem" Width="80" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMinisterDate" runat="server" Text="Minister Date: " CssClass="formLabel" AssociatedControlID="dtbMinisterDate" />
                                    </td>
                                    <td>
                                        <Arena:DateTextBox ID="dtbMinisterDate" runat="server" CssClass="formItem" Width="80" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFica" runat="server" Text="FICA: " CssClass="formLabel" AssociatedControlID="tbFica" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbFica" runat="server" CssClass="formItem" Width="100" />
                                        <span class="formLabel"> %</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblHours" runat="server" Text="Weekly Hours: " CssClass="formLabel" AssociatedControlID="tbHours" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbHours" runat="server" CssClass="formItem" Width="100" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAccessLU" runat="server" Text="Access Areas: " CssClass="formLabel" AssociatedControlID="ddlAccessLU" />
                                    </td>
                                    <td style="white-space: nowrap;">
                                        <Arena:LookupDropDown ID="ddlAccessLU" runat="server" CssClass="formItem" Width="150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblKeys" runat="server" Text="Keys: " CssClass="formLabel" AssociatedControlID="tbKeys" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbKeys" runat="server" CssClass="formItem" Width="100" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblBadgeIssued" runat="server" Text="Badge Issued: " CssClass="formLabel" AssociatedControlID="dtbBadgeIssued" />
                                    </td>
                                    <td>
                                        <Arena:DateTextBox ID="dtbBadgeIssued" runat="server" CssClass="formItem" Width="80" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblBadgeFob" runat="server" Text="Badge FOB: " CssClass="formLabel" AssociatedControlID="tbBadgeFob" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbBadgeFob" runat="server" CssClass="formItem" Width="100" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTermDate" runat="server" Text="Termination Date: " CssClass="formLabel" AssociatedControlID="dtbTermDate" />
                                    </td>
                                    <td>
                                        <Arena:DateTextBox ID="dtbTermDate" runat="server" CssClass="formItem" Width="80" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTermType" runat="server" Text="Termination Type: " CssClass="formLabel" AssociatedControlID="ddlTermTypeLU" />
                                    </td>
                                    <td style="white-space: nowrap;">
                                        <Arena:LookupDropDown ID="ddlTermTypeLU" runat="server" CssClass="smallText" Width="150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNoticeType" runat="server" Text="Notice Type: " CssClass="formLabel" AssociatedControlID="ddlNoticeTypeLU" />
                                    </td>
                                    <td style="white-space: nowrap;">
                                        <Arena:LookupDropDown ID="ddlNoticeTypeLU" runat="server" CssClass="smallText" Width="150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblExitInt" runat="server" Text="Exit Interview: " CssClass="formLabel" AssociatedControlID="dtbExitInt" />
                                    </td>
                                    <td>
                                        <Arena:DateTextBox ID="dtbExitInt" runat="server" CssClass="formItem" Width="80" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                        <asp:Label ID="lblRehireable" runat="server" Text="Eligible For Rehire: " CssClass="formLabel" AssociatedControlID="cbRehireable" />
                                    </td>
                                    <td>
                                        <br />
                                        <asp:CheckBox ID="cbRehireable" runat="server" CssClass="smallText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRehireDate" runat="server" Text="Rehire Date: " CssClass="formLabel" AssociatedControlID="dtbRehireDate" />
                                    </td>
                                    <td>
                                        <Arena:DateTextBox ID="dtbRehireDate" runat="server" CssClass="formItem" Width="80" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblRehireNote" runat="server" Text="Notes:" CssClass="formLabel" AssociatedControlID="tbRehireNote" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox ID="tbRehireNote" runat="server" CssClass="formItem" Width="400" Height="150" TextMode="MultiLine" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                
            </ContentTemplate>
        </ajax:TabPanel>
        
        <ajax:TabPanel ID="tpSalary" runat="server" HeaderText="Salary">
            <ContentTemplate>
                
                <asp:UpdatePanel ID="upSalary" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlSalaryDetails" runat="server">
                            <input type="hidden" id="ihSalaryHistoryID" name="ihSalaryHistoryID" runat="server" />
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblFullTime" runat="server" Text="Full-Time Position: " CssClass="formLabel" AssociatedControlID="cbFullTime" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbFullTime" runat="server" CssClass="smallText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblHourlyRate" runat="server" Text="Current Hourly Rate: " CssClass="formLabel" AssociatedControlID="tbHourlyRate" />
                                    </td>
                                    <td>
                                        <span class="formLabel" style="padding-left: 5px;">$</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbHourlyRate" runat="server" CssClass="formItem" Width="100" ValidationGroup="SalaryHistory" />
                                        <span class="formLabel"> per hour</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCurrentSalary" runat="server" Text="Current Salary: " CssClass="formLabel" AssociatedControlID="tbCurrentSalary" />
                                    </td>
                                    <td>
                                        <span class="formLabel" style="padding-left: 5px;">$</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbCurrentSalary" runat="server" CssClass="formItem" Width="100" ValidationGroup="SalaryHistory" />
                                        <span class="formLabel"> per year</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblHousing" runat="server" Text="Housing Allowance: " CssClass="formLabel" AssociatedControlID="tbHousing" />
                                    </td>
                                    <td>
                                        <span class="formLabel" style="padding-left: 5px;">$</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbHousing" runat="server" CssClass="formItem" Width="100" ValidationGroup="SalaryHistory" />
                                        <span class="formLabel"> per year</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFuel" runat="server" Text="Fuel Allowance: " CssClass="formLabel" AssociatedControlID="tbFuel" />
                                    </td>
                                    <td>
                                        <span class="formLabel" style="padding-left: 5px;">$</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbFuel" runat="server" CssClass="formItem" Width="100" ValidationGroup="SalaryHistory" />
                                        <span class="formLabel"> per year</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblRaiseDate" runat="server" Text="Raise Date: " CssClass="formLabel" AssociatedControlID="dtbRaiseDate" />
                                    </td>
                                    <td>
                                        <Arena:DateTextBox ID="dtbRaiseDate" runat="server" CssClass="formItem" Width="80" ValidationGroup="SalaryHistory" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRaiseAmt" runat="server" Text="Raise Amount: " CssClass="formLabel" AssociatedControlID="tbRaiseAmt" />
                                    </td>
                                    <td>
                                        <span class="formLabel" style="padding-left: 5px;">$</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbRaiseAmt" runat="server" CssClass="formItem" Width="100" ValidationGroup="SalaryHistory" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblReviewScore" runat="server" Text="Review Score: " CssClass="formLabel" AssociatedControlID="ddlReviewScoreLU" />
                                    </td>
                                    <td style="white-space: nowrap;">
                                        <Arena:LookupDropDown ID="ddlReviewScoreLU" runat="server" CssClass="smallText" Width="150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblReviewDate" runat="server" Text="Review Date: " CssClass="formLabel" AssociatedControlID="dtbReviewDate" />
                                    </td>
                                    <td>
                                        <Arena:DateTextBox ID="dtbReviewDate" runat="server" CssClass="formItem" Width="80" ValidationGroup="SalaryHistory" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel" nowrap colspan="2">Reviewer:</td>
                                    <td class="formItem"  nowrap style="text-align: left;">
                                        <input type="hidden" id="ihReviewerID" runat="server" NAME="ihReviewerID" />
                                        <asp:Label ID="lblReviewerEdit" runat="server"></asp:Label>
                                        <asp:PlaceHolder id="phChangeReviewer" runat="server"><a href="#" onclick="openSearchWindow('reviewer');">Change...</a>&nbsp;</asp:PlaceHolder>
                                        <asp:LinkButton ID="lbRemoveReviewer" Runat="server" OnClientClick="return validateRemovePerson('reviewer')">Remove</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblNextReview" runat="server" Text="Next Review Date: " CssClass="formLabel" AssociatedControlID="dtbNextReview" />
                                    </td>
                                    <td>
                                        <Arena:DateTextBox ID="dtbNextReview" runat="server" CssClass="formItem" Width="80" ValidationGroup="SalaryHistory" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Button ID="btnSaveSalary" runat="server" Text="Save Salary" CssClass="formItem" ValidationGroup="SalaryHistory" />
                                        <asp:Button ID="btnCancelSalary" runat="server" Text="Cancel" CssClass="formItem" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        
                        <asp:Panel ID="pnlSalaryHistory" runat="server">
                            <Arena:DataGrid ID="dgSalaryHistory" runat="server" AllowSorting="false">
                                <Columns>
                                    <asp:BoundColumn 
                                        HeaderText="ID" 
                                        DataField="SalaryHistoryID" 
                                        Visible="false"></asp:BoundColumn>
                                    <asp:TemplateColumn 
                                        HeaderText="Full Time" 
                                        ItemStyle-Wrap="false" 
                                        HeaderStyle-HorizontalAlign="Center" 
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Image ID="imgFullTime" Runat="server" ImageUrl="~/images/check.gif" Visible='<%# Eval("FullTime") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn 
                                        HeaderText="Hourly" 
                                        DataFormatString="{0:C}"
                                        DataField="HourlyRate"></asp:BoundColumn>
                                    <asp:BoundColumn 
                                        HeaderText="Salary" 
                                        DataFormatString="{0:C}"
                                        DataField="Salary"></asp:BoundColumn>
                                    <asp:BoundColumn 
                                        HeaderText="Housing" 
                                        DataFormatString="{0:C}"
                                        DataField="Housing"></asp:BoundColumn>
                                    <asp:BoundColumn 
                                        HeaderText="Fuel" 
                                        DataFormatString="{0:C}"
                                        DataField="Fuel"></asp:BoundColumn>
                                    <asp:BoundColumn 
                                        HeaderText="Raise Date" 
                                        DataFormatString="{0:d}"
                                        DataField="RaiseDate"></asp:BoundColumn>
                                    <asp:BoundColumn 
                                        HeaderText="Raise Amount" 
                                        DataFormatString="{0:C}"
                                        DataField="RaiseAmount"></asp:BoundColumn>
                                    <asp:BoundColumn 
                                        HeaderText="Review Score" 
                                        DataField="ReviewScore"></asp:BoundColumn>
                                    <asp:BoundColumn 
                                        HeaderText="Review Date" 
                                        DataFormatString="{0:d}"
                                        DataField="ReviewDate"></asp:BoundColumn>
                                    <asp:BoundColumn 
                                        HeaderText="Reviewer" 
                                        DataField="Reviewer"></asp:BoundColumn>
                                    <asp:BoundColumn 
                                        HeaderText="Next Review" 
                                        DataFormatString="{0:d}"
                                        DataField="NextReview"></asp:BoundColumn>
                                </Columns>
                            </Arena:DataGrid>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelSalary" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="dgSalaryHistory" EventName="ReBind" />
                    </Triggers>
                </asp:UpdatePanel>
                
            </ContentTemplate>
        </ajax:TabPanel>
        
        <ajax:TabPanel ID="tpBenefits" runat="server" HeaderText="Benefits">
            <ContentTemplate>
                
                <asp:UpdatePanel ID="upBenefits" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblBenefits" runat="server" Text="Benefits Eligibility: " CssClass="formLabel" AssociatedControlID="ddlBenefitsLU" />
                                </td>
                                <td style="white-space: nowrap;">
                                    <Arena:LookupDropDown ID="ddlBenefitsLU" runat="server" CssClass="smallText" Width="150" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblBeneStart" runat="server" Text="Start Date: " CssClass="formLabel" AssociatedControlID="dtbBeneStartDate" />
                                </td>
                                <td>
                                    <Arena:DateTextBox ID="dtbBeneStartDate" runat="server" CssClass="formItem" Width="80" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblMedical" runat="server" Text="Medical Insurance: " CssClass="formLabel" AssociatedControlID="ddlMedicalLU" />
                                </td>
                                <td style="white-space: nowrap;">
                                    <Arena:LookupDropDown ID="ddlMedicalLU" runat="server" CssClass="smallText" Width="150" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblDental" runat="server" Text="Dental Insurance: " CssClass="formLabel" AssociatedControlID="ddlDentalLU" />
                                </td>
                                <td style="white-space: nowrap;">
                                    <Arena:LookupDropDown ID="ddlDentalLU" runat="server" CssClass="smallText" Width="150" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblVision" runat="server" Text="Vision Insurance: " CssClass="formLabel" AssociatedControlID="ddlVisionLU" />
                                </td>
                                <td style="white-space: nowrap;">
                                    <Arena:LookupDropDown ID="ddlVisionLU" runat="server" CssClass="smallText" Width="150" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblLife" runat="server" Text="Life Insurance: " CssClass="formLabel" AssociatedControlID="ddlLifeLU" />
                                </td>
                                <td style="white-space: nowrap;">
                                    <Arena:LookupDropDown ID="ddlLifeLU" runat="server" CssClass="smallText" Width="150" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblParticipant" runat="server" Text="Retirement Plan Participant: " CssClass="formLabel" AssociatedControlID="cbParticipant" />
                                </td>
                                <td class="smallText">&nbsp;</td>
                                <td>
                                    <asp:CheckBox ID="cbParticipant" runat="server" CssClass="smallText" AutoPostBack="true" />
                                </td>
                            </tr>
                            <tr id="tr403Contrib" runat="server">
                                <td>
                                    <asp:Label ID="lbl403Contrib" runat="server" Text="403(b) Contribution: " CssClass="formLabel" AssociatedControlID="tb403Contrib" />
                                </td>
                                <td class="smallText">&nbsp;</td>
                                <td>
                                    <asp:TextBox ID="tb403Contrib" runat="server" CssClass="formItem" Width="100" />
                                    <span class="formLabel">%</span>
                                </td>
                            </tr>
                            <tr id="tr403Match" runat="server">
                                <td colspan="2">
                                    <asp:Label ID="lbl403Match" runat="server" Text="403(b) Match: " CssClass="formLabel" AssociatedControlID="tb403Match" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tb403Match" runat="server" CssClass="formItem" Width="100" />
                                    <span class="formLabel">%</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblHSA" runat="server" Text="HSA Contribution: " CssClass="formLabel" AssociatedControlID="tbHSA" />
                                </td>
                                <td>
                                    <span class="formLabel" style="padding-left: 5px;">$</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbHSA" runat="server" CssClass="formItem" Width="100" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                
            </ContentTemplate>
        </ajax:TabPanel>
        
        <ajax:TabPanel ID="tpCobra" runat="server" HeaderText="COBRA">
            <ContentTemplate>
                
                <asp:UpdatePanel ID="upCobra" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblBeneEndDate" runat="server" Text="Benefit End Date: " CssClass="formLabel" AssociatedControlID="dtbBeneEndDate" />
                                </td>
                                <td>
                                    <Arena:DateTextBox ID="dtbBeneEndDate" runat="server" CssClass="formItem" Width="80" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblElectCobra" runat="server" Text="Elect Cobra: " CssClass="formLabel" AssociatedControlID="cbElectCobra" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbElectCobra" runat="server" CssClass="smallText" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCobraLetterSent" runat="server" Text="COBRA Letter Sent: " CssClass="formLabel" AssociatedControlID="dtbCobraLetterSent" />
                                </td>
                                <td>
                                    <Arena:DateTextBox ID="dtbCobraLetterSent" runat="server" CssClass="formItem" Width="80" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCobraEnddate" runat="server" Text="COBRA End Date: " CssClass="formLabel" AssociatedControlID="dtbCobraEndDate" />
                                </td>
                                <td>
                                    <Arena:DateTextBox ID="dtbCobraEndDate" runat="server" CssClass="formItem" Width="80" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                
            </ContentTemplate>
        </ajax:TabPanel>
        
        <ajax:TabPanel ID="tpLeave" runat="server" HeaderText="Leave">
            <ContentTemplate>
                
                <asp:UpdatePanel ID="upLeave" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <input type="hidden" id="ihLeave" name="ihLeave" />
                        <asp:Panel ID="pnlLeaveDetails" runat="server">
                            <input type="hidden" id="ihLeaveHistoryID" name="ihLeaveHistoryID" runat="server" />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblLeaveType" runat="server" Text="Leave Type: " CssClass="formLabel" AssociatedControlID="ddlLeaveTypeLU" />
                                    </td>
                                    <td class="formItem" style="white-space: nowrap;">
                                        <Arena:LookupDropDown ID="ddlLeaveTypeLU" runat="server" CssClass="smallText" Width="150" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblLeaveReason" runat="server" Text="Leave Reason: " CssClass="formLabel" AssociatedControlID="tbLeaveReason" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbLeaveReason" runat="server" CssClass="formItem" Width="200" ValidationGroup="LeaveHistory" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblLeaveDate" runat="server" Text="Leave Date: " CssClass="formLabel" AssociatedControlID="dtbLeaveDate" />
                                    </td>
                                    <td>
                                        <Arena:DateTextBox ID="dtbLeaveDate" runat="server" CssClass="formItem" Width="80" ValidationGroup="LeaveHistory" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblReturnDate" runat="server" Text="Return Date: " CssClass="formLabel" AssociatedControlID="dtbReturnDate" />
                                    </td>
                                    <td>
                                        <Arena:DateTextBox ID="dtbReturnDate" runat="server" CssClass="formItem" Width="80" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblLeaveNote" runat="server" Text="Notes:" CssClass="formLabel" AssociatedControlID="tbLeaveNote" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox ID="tbLeaveNote" runat="server" CssClass="formItem" Width="400" Height="150" TextMode="MultiLine" ValidationGroup="LeaveHistory" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnSaveLeave" runat="server" Text="Save Leave" CssClass="formItem" ValidationGroup="LeaveHistory" />
                                        <asp:Button ID="btnCancelLeave" runat="server" Text="Cancel" CssClass="formItem" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        
                        <asp:Panel ID="pnlLeaveHistory" runat="server">
                            <Arena:DataGrid ID="dgLeaveHistory" runat="server" AllowSorting="false">
                                <Columns>
                                     <asp:BoundColumn 
                                        HeaderText="ID" 
                                        DataField="LeaveHistoryID" 
                                        Visible="false"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Leave Type">
                                        <ItemTemplate>
                                            <%# Eval("LeaveType.Value") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn 
                                        HeaderText="Reason" 
                                        DataField="LeaveReason"></asp:BoundColumn>
                                    <asp:BoundColumn 
                                        HeaderText="Leave Date" 
                                        DataFormatString="{0:d}" 
                                        DataField="LeaveDate"></asp:BoundColumn>
                                    <asp:BoundColumn 
                                        HeaderText="Return Date" 
                                        DataFormatString="{0:d}" 
                                        DataField="ReturnDate"></asp:BoundColumn>
                                    <asp:BoundColumn 
                                        HeaderText="Notes" 
                                        DataField="Notes"></asp:BoundColumn>
                                </Columns>
                            </Arena:DataGrid>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelLeave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="dgLeaveHistory" EventName="ReBind" />
                    </Triggers>
                </asp:UpdatePanel>
                
            </ContentTemplate>
        </ajax:TabPanel>
    </ajax:TabContainer>
    
    <asp:Panel ID="pnlButtons" runat="server">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="formItem" />
    </asp:Panel>
</asp:Panel>