<%@ Control Language="c#" Inherits="ArenaWeb.UserControls.Custom.CCV.Core.NewFamily" CodeFile="NewFamily.ascx.cs" %>
<script language="javascript">

	function formatDate(dateField, centuryCutoff)
	{
		var oldDate = '';
		var newDate = '';
		
		oldDate = dateField.value;
		
		if (oldDate.indexOf("/") < 0 && oldDate.indexOf("-") < 0)
		{
			var digitCount = 0;

			for(var i = 0; i < oldDate.length; i++)
			{
				var ch = oldDate.substr(i,1);
				
				if (digitCount < 8)
				{
					if (ch == "0" || ch == "1" || ch == "2" || ch == "3" || ch == "4" || 
						ch == "5" || ch == "6" || ch == "7" || ch == "8" || ch == "9")
					{
						newDate += ch;
						digitCount++;
					}
				}
				else
					break;
			}

			if (digitCount == 4)
				newDate = newDate.substr(0,2) + '/' + newDate.substr(2,2) + '/' + dToday.getYear().toString();
				
			else if (digitCount == 6)
			{
				var century = "20";
				var year = parseInt(newDate.substr(4,2));
				if (year > centuryCutoff)
					century = "19"
				newDate = newDate.substr(0,2) + '/' + newDate.substr(2,2) + '/' + century + newDate.substr(4,2);
			}	
			else if (digitCount == 8)
				newDate = newDate.substr(0,2) + '/' + newDate.substr(2,2) + '/' + newDate.substr(4,4);
			else
				newDate = oldDate;

			dateField.value = newDate;
		}
	}
	
	function formatPhone(phoneField)
	{
		var oldPhone = '';
		var newPhone = '';
		
		oldPhone = phoneField.value;
		
		var digitCount = 0;

		for(var i = 0; i < oldPhone.length; i++)
		{
			var ch = oldPhone.substr(i,1);
			
			if (digitCount < 10)
			{
				if (ch == "0" || ch == "1" || ch == "2" || ch == "3" || ch == "4" || 
					ch == "5" || ch == "6" || ch == "7" || ch == "8" || ch == "9")
				{
					newPhone += ch;
					digitCount++;
				}
				if (digitCount == 10)
					newPhone = '(' + newPhone.substr(0,3) + ') ' + newPhone.substr(3,3) + '-' + newPhone.substr(6,4);
			}
			else
				newPhone += ch;
		}

		if (digitCount >= 10)
			phoneField.value = newPhone;
	}

</script>

<input type="hidden" id="iRedirect" runat="server" name="iRedirect">

<asp:Panel ID="pnlEdit" Runat="server" CssClass="module normalText">
	<table cellpadding="0" cellspacing="0" border="0">
	<tr>
		<td style="padding:15px">
			<table cellpadding="2" cellspacing="0" border="0">
			<tr>
				<td align="right" class="formLabel" nowrap="nowrap">First Name</td>
				<td class="formItem">
					<asp:TextBox ID="tbFirstName" Runat="server" CssClass="formItem" size="22" maxlength="50"></asp:TextBox>
					<asp:RequiredFieldValidator ID="reqFirstName" Runat="server" ControlToValidate="tbFirstName" CssClass="errorText" 
						Display="Static" ErrorMessage="First Name is required"> *</asp:RequiredFieldValidator>
				</td>
				<td align="right" class="formLabel" nowrap="nowrap">Spouse</td>
				<td class="formItem">
					<asp:TextBox ID="tbSpouseFirstName" Runat="server" CssClass="formItem" size="22" maxlength="50"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td align="right" class="formLabel" nowrap="nowrap">Last Name</td>
				<td class="formItem">
					<asp:TextBox ID="tbLastName" Runat="server" CssClass="formItem" size="22" maxlength="50"></asp:TextBox>
					<asp:RequiredFieldValidator ID="reqLastName" Runat="server" ControlToValidate="tbLastName" CssClass="errorText" 
						Display="Static" ErrorMessage="Last Name is required"> *</asp:RequiredFieldValidator>
				</td>
				<td></td>
				<td class="formItem">
					<asp:TextBox ID="tbSpouseLastName" Runat="server" CssClass="formItem" size="22" maxlength="50"></asp:TextBox>
			</tr>
			<tr>
				<td align="right" class="formLabel" nowrap="nowrap" style="padding-top:10px">Home Phone</td>
				<td class="formItem" style="padding-top:10px">
					<asp:TextBox ID="tbHomePhone" Runat="server" CssClass="formItem" size="15" maxlength="50" onchange="formatPhone(this);"></asp:TextBox>
					<asp:RequiredFieldValidator ID="reqHomePhone" Runat="server" ControlToValidate="tbHomePhone" CssClass="errorText" 
						Display="Static" ErrorMessage="Home Phone is required"> *</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="revHomePhone" runat="server" ControlToValidate="tbHomePhone" CssClass="errorText"
						Display="Static" ValidationExpression="\(?\d{3}\)?[\s\.\-]?\d{3}[\s\.\-]?\d{4}.*" 
						ErrorMessage="Invalid Home Phone (Area Code is Required)"> *</asp:RegularExpressionValidator>
				</td>
				<td></td>
				<td class="formItem" style="padding-top:10px">
					<asp:TextBox ID="tbSpouseHomePhone" Runat="server" CssClass="formItem" size="15" maxlength="50" onchange="formatPhone(this);"></asp:TextBox>
					<asp:RegularExpressionValidator id="revSpouseHomePhone" runat="server" ControlToValidate="tbSpouseHomePhone" CssClass="errorText"
						Display="Static" ValidationExpression="\(?\d{3}\)?[\s\.\-]?\d{3}[\s\.\-]?\d{4}.*" 
						ErrorMessage="Invalid Home Phone (Area Code is Required)"> *</asp:RegularExpressionValidator>
				</td>
			</tr>
			<tr>
				<td align="right" class="formLabel" nowrap="nowrap" style="vertical-align: top;">Cell Phone</td>
				<td class="formItem">
					<asp:TextBox ID="tbCellPhone" Runat="server" CssClass="formItem" size="15" maxlength="50" onchange="formatPhone(this);"></asp:TextBox>
					<asp:RegularExpressionValidator id="revCellPhone" runat="server" ControlToValidate="tbCellPhone" CssClass="errorText"
						Display="Static" ValidationExpression="\(?\d{3}\)?[\s\.\-]?\d{3}[\s\.\-]?\d{4}.*" 
						ErrorMessage="Invalid Cell Phone (Area Code is Required)"> *</asp:RegularExpressionValidator><br />
					<asp:CheckBox ID="cbCellSMS" runat="server" CssClass="smallText" Text="Would you like to receive text messages?" />
				</td>
				<td></td>
				<td class="formItem">
					<asp:TextBox ID="tbSpouseCellPhone" Runat="server" CssClass="formItem" size="15" maxlength="50" onchange="formatPhone(this);"></asp:TextBox>
					<asp:RegularExpressionValidator id="revSpouseCellPhone" runat="server" ControlToValidate="tbSpouseCellPhone" CssClass="errorText"
						Display="Static" ValidationExpression="\(?\d{3}\)?[\s\.\-]?\d{3}[\s\.\-]?\d{4}.*" 
						ErrorMessage="Invalid Cell Phone (Area Code is Required)"> *</asp:RegularExpressionValidator><br />
					<asp:CheckBox ID="cbSpouseSMS" runat="server" CssClass="smallText" Text="Would you like to receive text messages?" />
				</td>
			</tr>
			<tr>
				<td align="right" class="formLabel" nowrap>E-mail</td>
				<td class="formItem">
					<asp:TextBox ID="tbEmail" Runat="server" CssClass="formItem" size="22" maxlength="100"></asp:TextBox>
					<asp:RequiredFieldValidator ID="reqEmail" Runat="server" ControlToValidate="tbEmail" CssClass="errorText" 
						Display="Static" ErrorMessage="Email is required"> *</asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator id="revEmail" runat="server" ControlToValidate="tbEmail" CssClass="errorText"
						Display="Static" ValidationExpression="[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+" 
						ErrorMessage="Invalid Email Address"> *</asp:RegularExpressionValidator>
				</td>
				<td></td>
				<td class="formItem">
					<asp:TextBox ID="tbSpouseEmail" Runat="server" CssClass="formItem" size="22" maxlength="100"></asp:TextBox>
					<asp:RegularExpressionValidator id="revSpouseEmail" runat="server" ControlToValidate="tbSpouseEmail" CssClass="errorText"
						Display="Static" ValidationExpression="[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+" 
						ErrorMessage="Invalid Email Address"> *</asp:RegularExpressionValidator>
				</td>
			</tr>
			<tr>
				<td align="right" class="formLabel" nowrap style="padding-top:10px">Street Address</td>
				<td class="formItem" style="padding-top:10px" colspan="3">
					<asp:TextBox ID="tbStreetAddress" Runat="server" CssClass="formItem" size="22" maxlength="100"></asp:TextBox>
					<asp:RequiredFieldValidator ID="reqStreetAddress" Runat="server" ControlToValidate="tbStreetAddress" CssClass="errorText" 
						Display="Static" ErrorMessage="Street Address is required"> *</asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td align="right" class="formLabel" nowrap>City</td>
				<td class="formItem" colspan="3">
					<asp:TextBox ID="tbCity" Runat="server" CssClass="formItem" size="22" maxlength="64"></asp:TextBox>
					<asp:RequiredFieldValidator ID="reqCity" Runat="server" ControlToValidate="tbCity" CssClass="errorText" 
						Display="Static" ErrorMessage="City is required"> *</asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td align="right" class="formLabel" nowrap>State</td>
				<td class="formItem" colspan="3">
					<asp:DropDownList ID="ddlState" Runat="server" CssClass="formItem">
						<asp:ListItem value=""></asp:ListItem>
						<asp:ListItem value="AL">Alabama</asp:ListItem>
						<asp:ListItem value="AK">Alaska</asp:ListItem>
						<asp:ListItem value="AZ">Arizona</asp:ListItem>
						<asp:ListItem value="AR">Arkansas</asp:ListItem>
						<asp:ListItem value="CA">California</asp:ListItem>
						<asp:ListItem value="CO">Colorado</asp:ListItem>
						<asp:ListItem value="CT">Connecticut</asp:ListItem>
						<asp:ListItem value="DE">Delaware</asp:ListItem>
						<asp:ListItem value="DC">District of Columbia</asp:ListItem>
						<asp:ListItem value="FL">Florida</asp:ListItem>
						<asp:ListItem value="GA">Georgia</asp:ListItem>
						<asp:ListItem value="GU">Guam</asp:ListItem>
						<asp:ListItem value="HI">Hawaii</asp:ListItem>
						<asp:ListItem value="ID">Idaho</asp:ListItem>
						<asp:ListItem value="IL">Illinois</asp:ListItem>
						<asp:ListItem value="IN">Indiana</asp:ListItem>
						<asp:ListItem value="IA">Iowa</asp:ListItem>
						<asp:ListItem value="KS">Kansas</asp:ListItem>
						<asp:ListItem value="KY">Kentucky</asp:ListItem>
						<asp:ListItem value="LA">Louisiana</asp:ListItem>
						<asp:ListItem value="ME">Maine</asp:ListItem>
						<asp:ListItem value="MD">Maryland</asp:ListItem>
						<asp:ListItem value="MA">Massachusetts</asp:ListItem>
						<asp:ListItem value="MI">Michigan</asp:ListItem>
						<asp:ListItem value="MN">Minnesota</asp:ListItem>
						<asp:ListItem value="MS">Mississippi</asp:ListItem>
						<asp:ListItem value="MO">Missouri</asp:ListItem>
						<asp:ListItem value="MT">Montana</asp:ListItem>
						<asp:ListItem value="NE">Nebraska</asp:ListItem>
						<asp:ListItem value="NV">Nevada</asp:ListItem>
						<asp:ListItem value="NH">New Hampshire</asp:ListItem>
						<asp:ListItem value="NJ">New Jersey</asp:ListItem>
						<asp:ListItem value="NM">New Mexico</asp:ListItem>
						<asp:ListItem value="NY">New York</asp:ListItem>
						<asp:ListItem value="NC">North Carolina</asp:ListItem>
						<asp:ListItem value="ND">North Dakota</asp:ListItem>
						<asp:ListItem value="OH">Ohio</asp:ListItem>
						<asp:ListItem value="OK">Oklahoma</asp:ListItem>
						<asp:ListItem value="OR">Oregon</asp:ListItem>
						<asp:ListItem value="PA">Pennsylvania</asp:ListItem>
						<asp:ListItem value="PR">Puerto Rico</asp:ListItem>
						<asp:ListItem value="RI">Rhode Island</asp:ListItem>
						<asp:ListItem value="SC">South Carolina</asp:ListItem>
						<asp:ListItem value="SD">South Dakota</asp:ListItem>
						<asp:ListItem value="TN">Tennessee</asp:ListItem>
						<asp:ListItem value="TX">Texas</asp:ListItem>
						<asp:ListItem value="UT">Utah</asp:ListItem>
						<asp:ListItem value="VT">Vermont</asp:ListItem>
						<asp:ListItem value="VI">Virgin Islands</asp:ListItem>
						<asp:ListItem value="VA">Virginia</asp:ListItem>
						<asp:ListItem value="WA">Washington</asp:ListItem>
						<asp:ListItem value="WV">West Virginia</asp:ListItem>
						<asp:ListItem value="WI">Wisconsin</asp:ListItem>
						<asp:ListItem value="WY">Wyoming</asp:ListItem>
						<asp:ListItem value="AA">AA</asp:ListItem>
						<asp:ListItem value="AE">AE</asp:ListItem>
						<asp:ListItem value="AP">AP</asp:ListItem>
					</asp:DropDownList>
					<asp:RequiredFieldValidator ID="reqState" Runat="server" ControlToValidate="ddlState" CssClass="errorText" 
						Display="Static" ErrorMessage="State is required"> *</asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td align="right" class="formLabel" nowrap>Zip Code</td>
				<td class="formItem" colspan="3">
					<asp:TextBox ID="tbZipCode" Runat="server" CssClass="formItem" size="22" maxlength="24"></asp:TextBox>
					<asp:RequiredFieldValidator ID="reqZipCode" Runat="server" ControlToValidate="tbZipCode" CssClass="errorText" 
						Display="Static" ErrorMessage="Zip Code is required"> *</asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td align="right" class="formLabel" nowrap style="padding-top:5px"></td>
				<td class="formItem" style="padding-top:5px" colspan="3">
					<asp:Button ID="btnSubmit" Runat="server" CssClass="smallText" Text="Submit"></asp:Button>
				</td>
			</tr>
			</table>
		</td>
	</tr>
	</table>

</asp:Panel>

