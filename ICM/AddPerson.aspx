<%@ Page Title="Ajouter une personne" Language="C#" MasterPageFile="~/Persons.master" AutoEventWireup="true" CodeBehind="AddPerson.aspx.cs" Inherits="ICM.AddPerson" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PersonsContent" runat="server">
    <h2>
        Ajouter une personne
    </h2>
    <table>
        <asp:Label ID="IDLabel" Visible="false" runat="server" Text="-1" />
        <tr>
            <td>Nom : </td>
            <td><asp:TextBox ID="NameTextBox" Columns="15" runat="server" /></td>
            <td><asp:RequiredFieldValidator runat="server" id="RequiredNameValidator" ControlToValidate="NameTextBox" errormessage="Veuillez entrer un nom !" /></td>
        </tr>
        <tr>
            <td>Prénom : </td>
            <td><asp:TextBox ID="FirstNameTextBox" Columns="15" runat="server" /></td>
            <td><asp:RequiredFieldValidator runat="server" ID="RequiredFirstNameValidator" ControlToValidate="FirstNameTextBox" errormessage="Veuillez entrer un prénom !" /></td>
        </tr>
        <tr>
            <td>Institution : </td>
            <td>
                <asp:DropDownList ID="InstitutionList" runat="server" OnSelectedIndexChanged="InstitutionSelected" AutoPostBack="true" />
            </td>
            <td>
                Pas dans la liste? <a class="AddLink" href='AddInstitution.aspx' target="_blank">Ajouter</a>
            </td>
        </tr>
        <tr>
            <td>Département : </td>
            <td><asp:DropDownList ID="DepartmentList" runat="server" /></td>
        </tr>
        <tr>
            <td>Téléphone : </td>
            <td><asp:TextBox ID="PhoneTextBox" Columns="15" runat="server" /></td>
            <td>
                <asp:RequiredFieldValidator runat="server" ID="RequiredPhoneValidator" ControlToValidate="PhoneTextBox" errormessage="Veuillez entrer un numéro de téléphone !" />
                <asp:RegularExpressionValidator runat="server" ID="ValidPhoneValidator" ControlToValidate="PhoneTextBox" ValidationExpression="[0-9]{3}/[0-9]{3}\.[0-9]{2}\.[0-9]{2}" errormessage="Numéro de téléphone invalide" />
            </td>
        </tr>
        <tr>
            <td>Email : </td>
            <td><asp:TextBox ID="MailTextBox" Columns="15" runat="server" /></td>
            <td>
                <asp:RequiredFieldValidator runat="server" ID="RequiredMailValidator" ControlToValidate="MailTextBox" errormessage="Veuillez entrer une adresse email !" />
                <asp:RegularExpressionValidator runat="server" ID="ValidEmailValidator" ControlToValidate="MailTextBox" ValidationExpression="^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$" errormessage="Email invalide !" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="Refresh" runat="server" Text="Rafraichir" OnClick="Refresh_Click" CausesValidation="false"/>
                <asp:Button ID="AddButton" runat="server" Text="Ajouter" OnClick="CreatePerson" Visible="false" />
                <asp:Button ID="SaveButton" runat="server" Text="Sauvegarder" OnClick="SavePerson" Visible="false" />
            </td>
        </tr>
    </table>
    
    <asp:Label ID="ErrorLabel" ForeColor="Red" runat="server" Visible="false" />

     <script type="text/javascript">
         $(function () {
             $("[id$=AddButton], [id$=SaveButton], [id$=Refresh], a", "#rightContent").button();
         });
    </script>
</asp:Content>
