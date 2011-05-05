<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="ICM.Account.Login" %>

<asp:Content ID="HeadContentLogin" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="MainContentLogin" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Log In
    </h2>
    <p>
        Saisis ici ton login et password.
    </p>
    <span class="failureNotification">
        <asp:Literal ID="FailureLiteral" runat="server"></asp:Literal>
    </span>
    <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification" 
            ValidationGroup="LoginUserValidationGroup"/>
    <div class="accountInfo">
        <p>
            Username:<br />
            <asp:TextBox ID="UserName" runat="server" CssClass="textEntry" />
            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                    CssClass="failureNotification" ErrorMessage="Saisis ton login." ToolTip="Saisis votre login." 
                    ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
        </p>
        <p>
            Password:<br />
            <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password" />
            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                    CssClass="failureNotification" ErrorMessage="Saisis ton password." ToolTip="Saisis votre password." 
                    ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
        </p>
        <p>
            <asp:CheckBox ID="RememberCheckBox" runat="server"/>
            <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberCheckBox" CssClass="inline">Garder ma session active</asp:Label>
        </p>
        <p class="submitButton">
            <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" 
                ValidationGroup="LoginUserValidationGroup" onclick="LoginButton_Click"/>
        </p>
    </div>
</asp:Content>
