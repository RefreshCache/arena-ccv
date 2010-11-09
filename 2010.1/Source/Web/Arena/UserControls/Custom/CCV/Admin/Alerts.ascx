<%@ Control Language="c#" Inherits="ArenaWeb.UserControls.Custom.CCV.Admin.Alerts" CodeFile="Alerts.ascx.cs" %>
<br/>
<asp:ListView ID="lvAlerts" runat="server">

    <LayoutTemplate>
        <div runat="server" id="itemPlaceHolder"/>
    </LayoutTemplate>
    
    <ItemTemplate>
        <div>
            <div class="alert">
                <h4>ALERT: <%# Eval("AlertKey") %></h4>
                <%# Eval("AlertMessage") %>
            </div>
            <div style="clear:both"></div>
        </div>
    </ItemTemplate>

</asp:ListView>
<asp:LinkButton ID="lbRefresh" runat="server" Text="Refresh" CssClass="tinyText"></asp:LinkButton>
<asp:Label ID="lblError" runat="server" Visible="false"></asp:Label>