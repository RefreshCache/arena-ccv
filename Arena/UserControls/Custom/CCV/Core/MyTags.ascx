<%@ Control Language="c#" Inherits="ArenaWeb.UserControls.Custom.CCV.Core.MyTags" CodeFile="MyTags.ascx.cs" %>
<%@ Register TagPrefix="Arena" Namespace="Arena.Portal.UI" Assembly="Arena.Portal.UI" %>
<br/>
<div class="smallText" style="border-bottom:1px solid #999999;margin-bottom:3px">My Tags</div>
<asp:Repeater ID="rptTags" runat="server">
    <ItemTemplate>
	    <div class="smallText"><asp:HyperLink ID="hlTag" runat="server" BorderWidth="0"></asp:HyperLink></div>
    </ItemTemplate> 
</asp:Repeater>
