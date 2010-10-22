<%@ Control Language="c#" Inherits="ArenaWeb.UserControls.Custom.CCV.Data.DataUpdateActions" CodeFile="DataUpdateActions.ascx.cs" %>
<asp:UpdatePanel ID="upActions" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
		<Arena:DataGrid 
            id="dgActions" 
            Runat="server"
            Width="100%"
            DataKeyField="action_id">
            <Columns>
				<asp:boundcolumn 
 					HeaderText="ID" 
					datafield="action_id" 								
					ReadOnly="True" 
					Visible="false"
					ItemStyle-VerticalAlign="Top"></asp:boundcolumn>
                <asp:BoundColumn 
	                HeaderText="Action" 
	                ItemStyle-Wrap="False"
	                SortExpression="action_name"
	                DataField="action_name"
	                ItemStyle-VerticalAlign="Top"></asp:BoundColumn>
                <asp:TemplateColumn 
	                HeaderText="Assembly" 
	                ItemStyle-Wrap="False"
	                ItemStyle-VerticalAlign="Top">
	                <ItemTemplate>
						<%# FormatAction((int)Eval("action_id")) %>
	                </ItemTemplate>	
                </asp:TemplateColumn>
            </Columns>
        </Arena:DataGrid>
        
		<input type="hidden" id="hdnActionID" runat="server" />
		
        <Arena:ModalPopup ID="mpEditAction" runat="server" CancelControlID="btnCancelAction" Title="Data Update Action" DefaultFocusControlID="tbActionName">
            <Content DefaultButton="btnSaveAction">
            <div style="width:530px;height:350px;overflow:auto">
            <table cellpadding="0" cellspacing="5" border="0" width="500px">
                <tr>
	                <td class="formLabel" valign="top" nowrap>Action Type</td>
                    <td valign="top">
		                <asp:DropDownList ID="ddlActions" runat="server" CssClass="formItem" Width="300px" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
	                <td class="formLabel" valign="top" nowrap>Name</td>
	                <td class="formItem" valign="top" nowrap><asp:TextBox ID="tbActionName" runat="server" Width="300px" MaxLength="100" />
		                <asp:RequiredFieldValidator ControlToValidate="tbActionName" ID="rfvActionName" runat="server"
			                CssClass="errorText" Display="Dynamic" ErrorMessage="Name is required" SetFocusOnError="true"> *</asp:RequiredFieldValidator>
	                </td>
                </tr>
                <tr>
	                <td class="formLabel" valign="top" nowrap>Description</td>
	                <td class="formItem" valign="top" nowrap><asp:TextBox ID="tbActionDescription" runat="server" Width="300px" MaxLength="1000" TextMode="MultiLine" Rows="4" />
	                </td>
                </tr>
                <tr>
	                <td colspan="4" class="formLabel" valign="top" nowrap>Action Settings</td>
                </tr>
                <tr>
                    <td colspan="4" class="formItem" valign="top">
                        <Arena:SettingGrid ID="sgActionSettings" runat="server" Width="500px" ></Arena:SettingGrid>
                    </td>
                </tr>
            </table>
            </div>
            </Content>
            <Buttons>
                <asp:Button ID="btnSaveAction" runat="server" Text="Save" Width="75" OnClick="btnSaveAction_Click" CssClass="smallText" />
                <asp:Button ID="btnCancelAction" runat="server" Text="Cancel" Width="75" OnClick="btnCancelAction_Click" CausesValidation="false" CssClass="smallText" />
            </Buttons>			
        </Arena:ModalPopup>

    </ContentTemplate>
</asp:UpdatePanel>
