<%@ Control Language="c#" Inherits="ArenaWeb.UserControls.Custom.CCV.Core.NextSteps" CodeFile="NextSteps.ascx.cs" %>

<asp:UpdatePanel ID="upNextStep" runat="server">
<ContentTemplate>

    <div style="color:black; font-size:.8em;padding:10px"><img src="images/NextSteps/1.jpg"><asp:Image 
        ID="img2" runat="server" ImageUrl="~/images/NextSteps/2d.jpg" /><asp:Image
        ID="img3" runat="server" ImageUrl="~/images/NextSteps/3d.jpg" /><asp:Image
        ID="img4" runat="server" ImageUrl="~/images/NextSteps/4d.jpg" /><asp:Image
        ID="img4_1" runat="server" ImageUrl="~/images/NextSteps/4-1d.jpg" /><asp:Image
        ID="img4_2" runat="server" ImageUrl="~/images/NextSteps/4-2d.jpg" /><asp:Image
        ID="img4_3" runat="server" ImageUrl="~/images/NextSteps/4-3d.jpg" /><asp:Image
        ID="img4_4" runat="server" ImageUrl="~/images/NextSteps/4-4d.jpg" /><asp:Image
        ID="img5" runat="server" ImageUrl="~/images/NextSteps/5d.jpg" /><asp:Image
        ID="img6" runat="server" ImageUrl="~/images/NextSteps/6d.jpg" /><asp:ImageButton
        ID="img7" runat="server" ImageUrl="~/images/NextSteps/7.png"/></div>

    <Arena:ModalPopup ID="mpAddNextStep" runat="server" CancelControlID="btnCancelAdd" Title="Add To" DefaultFocusControlID="ddlStartingPoing">
        <Content>
            <div class="formLabel">Starting Point</div>
            <div style="padding:5px"><asp:DropDownList ID="ddlStartingPoint" runat="server" CssClass="smallText"></asp:DropDownList></div>
            <div style="text-align:right"><asp:Button ID="btnStartingPoint" runat="server" CssClass="smallText" Text="Add"/></div>
            <div style="padding-top:10px" class="formLabel">Baptism</div>
            <div style="padding:5px"><asp:DropDownList ID="ddlBaptism" runat="server" CssClass="smallText"></asp:DropDownList></div>
            <div style="text-align:right"><asp:Button ID="btnBaptism" runat="server" CssClass="smallText" Text="Add"/></div>
            <div style="padding-top:10px" class="formLabel">Foundations</div>
            <div style="padding:5px"><asp:DropDownList ID="ddlFoundations" runat="server" CssClass="smallText"></asp:DropDownList></div>
            <div style="text-align:right"><asp:Button ID="btnFoundations" runat="server" CssClass="smallText" Text="Add"/></div>
            <div style="padding-top:10px" class="formLabel">Neighborhood Groups</div>
            <div style="padding:5px"><asp:DropDownList ID="ddlGroups" runat="server" CssClass="smallText"></asp:DropDownList></div>
            <div style="text-align:right"><asp:Button ID="btnGroups" runat="server" CssClass="smallText" Text="Add"/></div>
        </Content>
        <Buttons>
            <asp:Button ID="btnCancelAdd" runat="server" Text="Cancel" Width="75" CausesValidation="false" CssClass="smallText" />
        </Buttons>			
    </Arena:ModalPopup>

</ContentTemplate>
</asp:UpdatePanel>
