<%@ Control Language="c#" Inherits="ArenaWeb.UserControls.Custom.CCV.SmallGroup.Attendance" CodeFile="Attendance.ascx.cs" %>
<%@ Register TagPrefix="Arena" Namespace="Arena.Portal.UI" Assembly="Arena.Portal.UI" %>
<asp:HiddenField ID="hfGroupDate" runat="server" />

<script language="javascript" type="text/javascript">

    function pageLoad() {

        if ($("input[id$='_cbNoMeet']").is(":checked"))
            $("ul.memberList").hide();

        $("input[id$='_cbNoMeet']").click(function () {

            if ($(this).is(":checked")) {
                $("ul.memberList").hide('fast');
            }
            else {
                $("ul.memberList").show('fast');
            }
        });

        $("ul.memberList input[id$='_ibDelete']").click(function () {

            var personName = $(this).prev().children('label:first').html();
            return confirm('Are you sure you want to remove ' + personName + ' from your group?');
            
        });

    }

</script>

<div class="module group-attendance">

    <asp:UpdatePanel ID="pnlContent" runat="server">
        <ContentTemplate>
    
            <asp:Panel ID="pnlMessage" runat="server" class="notice" Visible="false"></asp:Panel>
            
            <asp:PlaceHolder ID="phDetails" runat="server">

            <h4>Enter Attendance For: <asp:Label ID="lMtgDate" runat="server" CssClass="group-date"></asp:Label></h4>
            
            <asp:LinkButton ID="lbPrev" runat="server" CssClass="previous-date" Text="Prev" OnClick="lbPrev_Click"></asp:LinkButton>
            <asp:LinkButton ID="lbNext" runat="server" CssClass="next-date" Text="Next" OnClick="lbNext_Click"></asp:LinkButton>

            <div class="did-not-meet">
                <asp:CheckBox ID="cbNoMeet" runat="server" TextAlign="Right" Text="We Did Not Meet" />
            </div>

            <div class="group-roster">                
                <asp:ListView ID="lvMembers" runat="server" 
                    onitemcommand="lvMembers_ItemCommand">
                <LayoutTemplate>
                    <ul class="memberList">
                        <li id="itemPlaceHolder" runat="server"></li>
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li id="Li1" runat="server">
                        <asp:CheckBox ID="cbMember" runat="server" TextAlign="Right" memberid='<%#Eval("person_id")%>' Checked='<%#Eval("attended")%>' Text='<%#Eval("person_name")%>' />
                        <asp:ImageButton ID="ibDelete" runat="server" CausesValidation="false" CommandName="Remove" CommandArgument='<%#Eval("person_id")%>' ImageUrl="~/images/delete.gif" OnClientClick="return validateDelete(this.previousSibling.innerHTML, event);"  />
                    </li>
                </ItemTemplate>
                </asp:ListView>
            </div>
            <div class="buttons">
                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="save" Text="Save Attendance" OnClick="btnSubmit_Click"></asp:LinkButton>
                <a class="add-person" href="javascript:openAddWindow();">Add New Person to the Group</a> 
            </div>
            
            </asp:PlaceHolder>

        </ContentTemplate>
    </asp:UpdatePanel>
</div>
