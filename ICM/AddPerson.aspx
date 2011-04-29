<%@ Page Title="Ajouter une personne" Language="C#" MasterPageFile="~/Persons.master" AutoEventWireup="true" CodeBehind="AddPerson.aspx.cs" Inherits="ICM.AddPerson" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PersonsContent" runat="server">
        <h2>
        Nouvelle personne
    </h2>
    <table>
        <tr>
            <td>Nom : </td>
            <td><asp:TextBox ID="NameTextBox" Columns="15" runat="server" /></td>
        </tr>
        <tr>
            <td>Prenom : </td>
            <td><asp:TextBox ID="FirstNameTextBox" Columns="15" runat="server" /></td>
        </tr>
        <tr>
            <td>Institution : </td>
            <td><asp:DropDownList ID="InstitutionList" runat="server" /></td>
        </tr>
        <tr>
            <td>Département : </td>
            <td><asp:DropDownList ID="DepartmentList" runat="server" /></td>
        </tr>
        <tr>
            <td>Téléphone : </td>
            <td><asp:TextBox ID="PhoneTextBox" Columns="15" runat="server" /></td>
        </tr>
        <tr>
            <td>Email : </td>
            <td><asp:TextBox ID="MailTextBox" Columns="15" runat="server" /></td>
        </tr>
        <tr>
            <td> </td>
            <td><asp:Button ID="AddButton" runat="server" Text="Ajouter" OnClick="CreatePerson" /></td>
        </tr>
    </table>

     <script type="text/javascript">
         $(function () {
             $("[id$=AddButton]").button();
         });

    </script>
</asp:Content>
