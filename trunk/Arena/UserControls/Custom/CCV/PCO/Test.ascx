<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Test.ascx.cs" Inherits="Test" %>

<div style="padding:10px">
<asp:UpdatePanel ID="pnlUpdate">
<ContentTemplate>
    <br />
    <table>
        <tr>
            <td style="vertical-align:top;width:40%" class="smallText">
                <h2>PCO Integration</h2><br /><br />
                <table>
                    <tr>
                        <td class="formLabel"  style="width:100px">PCO Account:</td>
                        <td><asp:DropDownList ID="ddlAccount" runat="server" CssClass="formItem" Width="150" AutoPostBack="true"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td class="formLabel" style="width:100px">Arena Person:</td>
                        <td><asp:DropDownList ID="ddlArenaPeople" runat="server" CssClass="formItem" Width="150"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td class="formLabel">PCO Person:</td>
                        <td><asp:DropDownList ID="ddlPcoPeople" runat="server" CssClass="formItem" Width="150"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td class="formLabel">Action:</td>
                        <td class="formItem">
                            <asp:DropDownList ID="ddlAction" runat="server" CssClass="formItem">
                                <asp:ListItem Text="Load Arena Names" Value="LoadArenaNames"></asp:ListItem>
                                <asp:ListItem Text="Load PCO Names" Value="LoadPCONames"></asp:ListItem>
                                <asp:ListItem Text="Get XML" Value="Xml"></asp:ListItem>
                                <asp:ListItem Text="PCO ID" Value="PCOID"></asp:ListItem>
                                <asp:ListItem Text="PCO Password" Value="PCOPassword"></asp:ListItem>
                                <asp:ListItem Text="Update PCO" Value="UpdatePCO"></asp:ListItem>
                                <asp:ListItem Text="Save Arena XML" Value="SaveArenaXml"></asp:ListItem>
                                <asp:ListItem Text="Save PCO XML" Value="SavePCOXml"></asp:ListItem>
                                <asp:ListItem Text="Link Records" Value="LinkRecords"></asp:ListItem>
                                <asp:ListItem Text="Login to PCO" Value="Login"></asp:ListItem>
                                <asp:ListItem Text="Sync Records" Value="Sync"></asp:ListItem>
                                <asp:ListItem Text="Show Unlinked Records" value="Unlinked"></asp:ListItem>
                                <asp:ListItem Text="Disable Old Users" value="DisableOldUsers"></asp:ListItem>
                                <asp:ListItem Text="Attempt Auto Link" value="AttemptAutoLink"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Button ID="btnGo" runat="server" Text="Go" onclick="btnGo_Click" />                          
                        </td>
                    </tr>
                </table>
            </td>
            <td style="vertical-align:top;width:60%" class="smallText">
                <h2>Actions</h2>
                <ul>
                    <li><b>Load Arena Names:</b> Loads all members of both the Viewer and Editor Arena roles into the Arena Person dropdown</li>
                    <li><b>Load PCO Names:</b> Retrieves all the account names from the PCO Account and loads them into the PCO Person dropdown</li>
                    <li><b>Get XML:</b> Displays the current and previously saved versions of both the Arena and PCO XML values</li>
                    <li><b>PCO ID:</b> Displays the PCO ID of the currently selected Arena Person dropdown</li>
                    <li><b>PCO Password:</b> Displays the PCO Password of the currently selected Arena Person dropdown</li>
                    <li><b>Save Arena XML:</b> Saves the current value of Arena XML</li>
                    <li><b>Save PCO XML:</b> Saves the current value of PCO XML</li>
                    <li><b>Link Records:</b> Updated the PCO ID of the currently selected Arena Person to be the value associated with the currently selected PCO Person</li>
                    <li><b>Login to PCO:</b> Attempts to login to PCO using the PCO ID and PCO Password of the currently selected Arena Person</li>
                    <li><b>Sync Records:</b> Performs a PCO sync for the currently selected Arena Person</li>
                    <li><b>Show Unlinked Records:</b> Loads all the members of both the Viewer and Editor Arena roles that do not have a saved PCO ID into the Arena Person dropdown</li>
                    <li><b>Disable Old Users:</b> Will change the PCO Permissions for any user in PCO that is no longer in any of the Viewer or Editor roles</li>
                    <li><b>Attempt Auto Link:</b> Will evaluate each person in the Arena Person dropdown and if one and only one PCO person exists in the PCO Person Dropdown with the same name, will set that Arena person's PCO ID to the matching PCO person.</li>
                </ul>
            </td>
        </tr>
    </table>
    <br />
    <table>
        <tr>
            <td class="formLabel" valign="top" style="width:100px">Result(s):</td>
            <td class="formItem" valign="top"><asp:Literal ID="lResults" runat="server"/></td>
            <td class="formItem" valign="top"><asp:Literal ID="lResults2" runat="server"/></td>
        </tr>
    </table>
</ContentTemplate>
</asp:UpdatePanel>
</div>


