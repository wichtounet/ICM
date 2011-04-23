<%@ Page Title="Ajouter une personne" Language="C#" MasterPageFile="~/Persons.master" AutoEventWireup="true" CodeBehind="ShowPerson.aspx.cs" Inherits="ICM.ShowPerson" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PersonsContent" runat="server">
    <table>
        <asp:Label ID="IDLabel" Visible="false" runat="server" />
        <tr>
            <td>Nom : </td>
            <td><asp:Label ID="NameLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>Prénom : </td>
            <td><asp:Label ID="FirstNameLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>Institution : </td>
            <td><asp:Label ID="InstitutionLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>Département : </td>
            <td><asp:Label ID="DepartmentLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>Téléphone : </td>
            <td><asp:Label ID="PhoneLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>Email : </td>
            <td><asp:Label ID="MailLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>Archivé : </td>
            <td><asp:Label ID="StateLabel" runat="server" /></td>
        </tr>
    </table>

    <p>
        <asp:Button ID="EditButton" runat="server" Text="Modifier" OnClick="EditPerson" /> 
        <asp:Button ID="DeleteButton" runat="server" Text="Archiver" OnClick="DeletePerson" />
    </p>

     <script type="text/javascript">
         $(function () {
             $("[id$=EditButton]").button();
             $("[id$=DeleteButton]").button();
         });
    </script>
</asp:Content>