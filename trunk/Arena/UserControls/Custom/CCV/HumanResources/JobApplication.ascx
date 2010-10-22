<%@ Control Language="C#" AutoEventWireup="true" CodeFile="JobApplication.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.HumanResources.JobApplication" %>

<asp:Panel ID="pnlApplication" CssClass="appContainer" runat="server">
        <h2><asp:Literal ID="lHeader" runat="server" Text="Online Job Application" /></h2>
        
        <div class="infoContainer">
            <table width="100%" class="applicationTable">
                <tr>
                    <td class="formLabel" nowrap="nowrap">
                        <asp:Label ID="lblPosition" runat="server" AssociatedControlID="tbPosition">Position:</asp:Label></td>
                    <td nowrap="nowrap">
                        <asp:TextBox ID="tbPosition" runat="server" Width="250" CssClass="formItem"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvPosition" runat="server" 
                            ControlToValidate="tbPosition" 
                            SetFocusOnError="true"  
                            ErrorMessage="Position is required." 
                            ForeColor="Red"> *</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="formLabel" nowrap="nowrap">
                        <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="tbFirstName">First Name:</asp:Label></td>
                    <td nowrap="nowrap">
                        <asp:TextBox ID="tbFirstName" runat="server" Width="250" CssClass="formItem"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvFirstName" runat="server" 
                            ControlToValidate="tbFirstName" 
                            SetFocusOnError="true" 
                            ErrorMessage="Your first name is required." 
                            CssClass="errorText"> *</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="formLabel" nowrap="nowrap">
                        <asp:Label ID="lblLastName" runat="server" AssociatedControlID="tbLastName">Last Name:</asp:Label></td>
                    <td nowrap="nowrap">
                        <asp:TextBox ID="tbLastName" runat="server" Width="250" CssClass="formItem"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvLastName" runat="server" 
                            ControlToValidate="tbLastName" 
                            SetFocusOnError="true" 
                            ErrorMessage="Your last name is required." 
                            CssClass="errorText"> *</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="formLabel" nowrap="nowrap">
                        <asp:Label ID="lblEmail" runat="server" AssociatedControlID="tbEmail">Email:</asp:Label></td>
                    <td nowrap="nowrap">
                        <asp:TextBox ID="tbEmail" runat="server" Width="250" CssClass="formItem"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvEmail" runat="server" 
                            ControlToValidate="tbEmail" 
                            SetFocusOnError="true" 
                            ErrorMessage="Your email is required." 
                            CssClass="errorText">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator id="revEmail" runat="server" 
                            ControlToValidate="tbEmail" 
                            CssClass="errorText"
			                Display="Static" 
			                ValidationExpression="[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+" 
			                ErrorMessage="Invalid Email Address"> *</asp:RegularExpressionValidator>
                    </td>
                </tr>
            </table>
        </div>
        
        <div class="questionsContainer">
            <h2><asp:Literal ID="lSurvey" runat="server" Text="Applicant Survey:" /></h2>
            <p><asp:Literal ID="lDescription" runat="server" Text="Please answering the following questions:" /></p>
            
            <p><asp:Label ID="lblHear" runat="server" Text="1. How did you hear about this position?" AssociatedControlID="tbHear" /> <br />
            <asp:TextBox ID="tbHear" runat="server" Width="250" CssClass="formItem"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rvHear" runat="server" 
                ControlToValidate="tbHear" 
                SetFocusOnError="true" 
                ErrorMessage="How you heard about this position is required." 
                CssClass="errorText"> *</asp:RequiredFieldValidator></p>
            
            <p><asp:Label ID="lblChristian" runat="server" Text="2. How long have you been a Christian?" AssociatedControlID="tbChristian" /> <br />
            <asp:TextBox ID="tbChristian" runat="server" Width="250" CssClass="formItem"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rvChristian" runat="server" 
                ControlToValidate="tbChristian" 
                SetFocusOnError="true" 
                ErrorMessage="How long you've been a Christian is required." 
                CssClass="errorText> *</asp:RequiredFieldValidator></p>
            
            <p>
            <asp:Label ID="lblClass100" runat="server" Text="3. Have you taken Starting Point?" AssociatedControlID="ddlClass100" /> <br />
            <asp:DropDownList ID="ddlClass100" runat="server" CssClass="formItem"></asp:DropDownList>
            &nbsp; 
            <asp:Label ID="lblWhen" runat="server" Text="When?" AssociatedControlID="tbClass100" />
            &nbsp;
            <Arena:DateTextBox ID="tbClass100" runat="server" Width="120" class="formItem"></Arena:DateTextBox></p>
            
            <p><asp:Label ID="lblMember" runat="server" Text="4. Are you a member of CCV?" AssociatedControlID="ddlMember" /> <br />
            <asp:DropDownList ID="ddlMember" runat="server" CssClass="formItem"></asp:DropDownList></p>
            
            <p><asp:Label ID="lblGroup" runat="server" Text="5. Are you involved in a Neighborhood Group at CCV?" AssociatedControlID="ddlGroup" /> <br />
            <asp:DropDownList ID="ddlGroup" runat="server" CssClass="formItem"></asp:DropDownList></p>
            
            <p><asp:Label ID="lblServing" runat="server" Text="6. Are you involved in serving in a Ministry at CCV?" AssociatedControlID="ddlServing" /> <br />
            <asp:DropDownList ID="ddlServing" runat="server" CssClass="formItem"></asp:DropDownList>
            &nbsp; 
            <asp:Label ID="lblWhich" runat="server" Text="Which Ministry?" AssociatedControlID="tbServing" />
            &nbsp;
            <asp:TextBox ID="tbServing" runat="server" Width="100" CssClass="formItem"></asp:TextBox></p>
            
            <p><asp:Label ID="lblBaptized" runat="server" Text="7. Have you been baptized by immersion?" AssociatedControlID="ddlBaptized" /> <br />
            <asp:DropDownList ID="ddlBaptized" runat="server" CssClass="formItem"></asp:DropDownList></p>
            
            <p><asp:Label ID="lblTithe" runat="server" Text="8. Are you currently tithing at CCV?" AssociatedControlID="ddlTithe" /> <br />
            <asp:DropDownList ID="ddlTithe" runat="server" CssClass="formItem"></asp:DropDownList></p>
            
            <p><asp:Label ID="lblExperience" runat="server" Text="9. What experiences have you had that in the last 5 years would help you be successful in this position?" AssociatedControlID="tbExperience" /> <br />
            <asp:TextBox ID="tbExperience" runat="server" CssClass="formItem" Width="300" Height="75" TextMode="MultiLine"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rvExperience" runat="server" 
                ControlToValidate="tbExperience" 
                SetFocusOnError="true"  
                ErrorMessage="Prior experiences is required." 
                CssClass="errorText> *</asp:RequiredFieldValidator></p>
            
            <p><asp:Label ID="lblLed" runat="server" Text="10. What has led you to consider employment with Christ's Church of the Valley?" AssociatedControlID="tbLed" /> <br />
            <asp:TextBox ID="tbLed" runat="server" CssClass="formItem" Width="300" Height="75" TextMode="MultiLine"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rvLed" runat="server" 
                ControlToValidate="tbLed" 
                SetFocusOnError="true" 
                ErrorMessage="Description of your choice to seek employment with CCV is required." 
                CssClass="errorText> *</asp:RequiredFieldValidator></p>
            
            <p class="formBold"><asp:Label ID="lblCoverLetter" runat="server" Text="Please paste a cover letter in the text box below." AssociatedControlID="tbCoverLetter" /> <br />
            <asp:TextBox ID="tbCoverLetter" runat="server" CssClass="formItem" Width="300" Height="125" TextMode="MultiLine"></asp:TextBox></p>
            
            <p class="formBold"><asp:Label ID="lblResume" runat="server" Text="Please attach your resume." AssociatedControlID="fuResume" /> <br />
            <asp:FileUpload ID="fuResume" runat="server" Width="300" CssClass="formItem" />
            <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" 
                ControlToValidate="fuResume" 
                CssClass="errorText"
                Display="Static" 
                ValidationExpression="^.+\.(([dD][oO][cC])|([dD][oO][cC][xX])|([pP][dD][fF])|([tT][xX][tT])|([rR][tT][fF]))$" 
                ErrorMessage="Invalid File"> *</asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator ID="rvResume" runat="server" 
                ControlToValidate="fuResume" 
                SetFocusOnError="true" 
                ErrorMessage="Your resume is required." 
                CssClass="errorText"> *</asp:RequiredFieldValidator></p>
        </div>
        
            <asp:Button ID="btnSubmit" runat="server" CssClass="formItem" Text="Submit" CausesValidation="true" />
            &nbsp; &nbsp;
            <input type="reset" name="btnReset" value="Reset" />        
</asp:Panel>