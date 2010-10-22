<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PublicJobList.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.HumanResources.PublicJobList" %>
    
    <ul class="jobView">
        <asp:Repeater ID="rptrJobs" runat="server">
            <ItemTemplate>
                <li>
                    <a href="default.aspx?page=<%=JobPostingPageSetting %>&guid=<%# Eval("guid") %>"><%# Eval("title") %></a>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
