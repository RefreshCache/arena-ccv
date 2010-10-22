<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ApplicantDetail.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.HumanResources.ApplicantDetail" %>

<script language="javascript">

	function validateRemovePerson(who)
	{
		event.returnValue = confirm('Are you sure you want to remove the ' + who + '?')
	}
	
</script>

<asp:Panel ID="pnlDetail" runat="server">

    <input type="hidden" id="ihPersonList" runat="server" NAME="ihPersonList">
    <Button ID="bRefresh" Runat="server" style="visibility:hidden;display:none" onserverclick="bRefresh_Click">Refresh</Button>

    <table style="width: 100%;">
        <tr>
            <td class="formLabel" style="width: 200px;">
                <asp:Label ID="lblFirstName" runat="server" Text="First Name:" AssociatedControlID="tbFirstName" />
            </td>
            <td class="formItem" style="text-align: left;">
                <asp:TextBox ID="tbFirstName" runat="server" Width="200" />
            </td>
        </tr>
        <tr>
            <td class="formLabel">
                <asp:Label ID="lblLastName" runat="server" Text="First Name:" AssociatedControlID="tbLastName" />
            </td>
            <td class="formItem" style="text-align: left;">
                <asp:TextBox ID="tbLastName" runat="server" Width="200" />
            </td>
        </tr>
        <tr>
            <td class="formLabel" nowrap>Person</td>
            <td class="formItem"  nowrap style="text-align: left;">
	            <input type="hidden" id="ihPersonID" runat="server" NAME="ihPersonID" />
	            <asp:Label ID="lblPersonEdit" Runat="server"></asp:Label>
	            <asp:PlaceHolder id="phChangePerson" runat="server"><a href="#" onclick='openSearchWindow("owner");'>Change...</a>&nbsp;</asp:PlaceHolder>
	            <asp:LinkButton ID="lbRemovePerson" Runat="server"><span onclick='validateRemovePerson("owner");'>Remove</span></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td class="formLabel" style="text-align: left;">
                <asp:Label ID="lblEmail" runat="server" Text="Email Address:" AssociatedControlID="tbEmail" />
            </td>
            <td class="formItem" style="text-align: left;">
                <asp:TextBox ID="tbEmail" runat="server" Width="200" />
            </td>
        </tr>
        <tr>
            <td class="formLabel" style="text-align: left;">
                <asp:Label ID="lblPosition" runat="server" Text="Position Title:" AssociatedControlID="tbPosition" />
            </td>
            <td class="formItem" style="text-align: left;">
                <asp:TextBox ID="tbPosition" runat="server" Width="200" />
            </td>
        </tr>
        <tr>
            <td class="formLabel" style="text-align: left;">
                <asp:Label ID="lblHeard" runat="server" Text="Heard about the position:" AssociatedControlID="tbHeard" />
            </td>
            <td class="formItem">
                <asp:TextBox ID="tbHeard" runat="server" Width="200" />
            </td>
        </tr>
        <tr>
            <td class="formLabel" style="text-align: left;">
                <asp:Label ID="lblChristian" runat="server" Text="How long Christian:" AssociatedControlID="tbChristian" />
            </td>
            <td class="formItem">
                <asp:TextBox ID="tbChristian" runat="server" Width="200" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="upClass100" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="formItem">
                                    <asp:CheckBox ID="cbClass100" runat="server" AutoPostBack="true" />
                                </td>
                                <td class="formLabel">
                                    <asp:Label ID="lblClass100" runat="server" Text="&nbsp; Has attended Class 100" AssociatedControlID="cbClass100" />
                                </td>
                                <td class="formLabel">
                                    <asp:Label ID="lblClass100Date" runat="server" Text="&nbsp; &nbsp; Date attended: " AssociatedControlID="dtbClass100Date" />
                                </td>
                                <td class="formItem">
                                    <Arena:DateTextBox ID="dtbClass100Date" runat="server" Width="150" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="cbMember" runat="server" CssClass="formItem" />
                <asp:Label ID="lblMember" runat="server" CssClass="formLabel" Text="Is a member of CCV" AssociatedControlID="cbMember" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="cbGroup" runat="server" CssClass="formItem" />
                <asp:Label ID="lblGroup" runat="server" CssClass="formLabel" Text="In a neighborhood group" AssociatedControlID="cbGroup" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="upServing" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="formItem">
                                    <asp:CheckBox ID="cbServing" runat="server" AutoPostBack="true" />
                                </td>
                                <td class="formLabel">
                                    <asp:Label ID="lblServing" runat="server" Text="&nbsp; Currently Serving" AssociatedControlID="cbServing" />
                                </td>
                                <td class="formLabel">
                                    <asp:Label ID="lblMinistry" runat="server" Text="&nbsp; &nbsp; Serving Ministry: " AssociatedControlID="tbMinistry" />
                                </td>
                                <td class="formItem">
                                    <asp:TextBox ID="tbMinistry" runat="server" Width="200" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="cbBaptized" runat="server" CssClass="formItem" />
                <asp:Label ID="lblBaptized" runat="server" CssClass="formLabel" Text="Baptized by submersion" AssociatedControlID="cbBaptized" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="cbTithe" runat="server" CssClass="formItem" />
                <asp:Label ID="lblTithe" runat="server" CssClass="formLabel" Text="Currently tithing" AssociatedControlID="cbTithe" />
            </td>
        </tr>
        <tr>
            <td class="formLabel" colspan="2" style="text-align: left;">
                <asp:Label ID="lblExperience" runat="server" Text="Experience:" AssociatedControlID="tbExperience" />
            </td>
        </tr>
        <tr>
            <td class="formItem" colspan="2" style="text-align: left;">
                <asp:TextBox ID="tbExperience" runat="server" TextMode="MultiLine" Width="500" Height="200" />
            </td>
        </tr>
        <tr>
            <td class="formLabel" colspan="2" style="text-align: left;">
                <asp:Label ID="lblLed" runat="server" Text="What led them to apply:" AssociatedControlID="tbLed" />
            </td>
        </tr>
        <tr>
            <td class="formItem" colspan="2">
                <asp:TextBox ID="tbLed" runat="server" TextMode="MultiLine" Width="500" Height="200" />
            </td>
        </tr>
        <tr>
            <td class="formLabel" colspan="2" style="text-align: left;">
                <asp:Label ID="lblCoverletter" runat="server" Text="Cover Letter:" AssociatedControlID="tbCoverletter" />
            </td>
        </tr>
        <tr>
            <td class="formItem" colspan="2" style="text-align: left;">
                <asp:TextBox ID="tbCoverletter" runat="server" TextMode="MultiLine" Width="500" Height="250" />
            </td>
        </tr>
        <tr>
            <td class="formLabel" colspan="2" style="text-align: left;">
                <asp:HyperLink ID="aResume" runat="server" Text="Applicant Resume" />
            </td>
        </tr>
        <tr>
            <td style="text-align: left;">
                <asp:CheckBox ID="cbRejection" runat="server" CssClass="formItem" />
                <asp:Label ID="lblRejection" runat="server" CssClass="formLabel" Text="Rejection Notice Sent" AssociatedControlID="cbRejection" />
            </td>
        </tr>
        <tr>
            <td style="text-align: left;">
                <asp:CheckBox ID="cbReviewed" runat="server" CssClass="formItem" />
                <asp:Label ID="lblReviewed" runat="server" CssClass="formLabel" Text="Reviewed By Human Resources" AssociatedControlID="cbReviewed" />
            </td>
        </tr>
        <tr>
            <td class="formItem" colspan="2">
                <asp:Button ID="btnUpdate" runat="server" Text="Update" />
                &nbsp; &nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
            </td>
        </tr>
    </table>
    
</asp:Panel>
