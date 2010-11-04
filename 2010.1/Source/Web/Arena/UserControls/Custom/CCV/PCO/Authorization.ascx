<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Authorization.ascx.cs" Inherits="Authorization" %>

<div class="heading2" style="padding-bottom:10px">PCO Authorizations</div>

<asp:ListView ID="lvAccounts" runat="server" onitemcommand="lvAccounts_ItemCommand">

    <LayoutTemplate>
        <table class="list" cellpadding="3" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
            <tr class="listHeader">
                <td class="listHeader" valign="bottom" align="left">Account</td>
                <td class="listHeader" valign="bottom" align="left">Status</td>
                <td class="listHeader" valign="bottom" align="right"></td>
            </tr>
            <tr id="itemPlaceHolder" runat="server"></tr>
        </table>
    </LayoutTemplate>

    <ItemTemplate>
        <tr id="trItem" runat="server" class="listItem">
            <td valign="top" align="left"><%# Eval("Value") %>:</td>
            <td valign="top" align="left"><%# GetAuthStatus(Eval("Qualifier").ToString(), Eval("Qualifier8").ToString()) %></td> 
            <td valign="top" align="right"><asp:LinkButton ID="lbAuthorize" runat="server" CommandName="Authorize" Text="Authorize" CommandArgument='<%# Eval("LookupID") %>'></asp:LinkButton></td>
        </tr>
    </ItemTemplate>
    
    <AlternatingItemTemplate>
        <tr id="trItem" runat="server" class="listAltItem">
            <td valign="top" align="left"><%# Eval("Value") %>:</td>
            <td valign="top" align="left"><%# GetAuthStatus(Eval("Qualifier").ToString(), Eval("Qualifier8").ToString()) %></td> 
            <td valign="top" align="right"><asp:LinkButton ID="lbAuthorize" runat="server" CommandName="Authorize" Text="Authorize" CommandArgument='<%# Eval("LookupID") %>'></asp:LinkButton></td>
        </tr>
    </AlternatingItemTemplate>

</asp:ListView>

<div class="smallText" style="padding-top:10px">
    Note: If you need to sync with more than one default PCO Account, you can add additional values to the 
    "<asp:Literal ID="lTypeTitle" runat="server"></asp:Literal>" Lookup Type.  Once the value has been added, 
    return to this page to authorize and link it to a new PCO Account.
</div>
