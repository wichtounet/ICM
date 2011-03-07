<%@ Page Title="Ajouter une personne" Language="C#" MasterPageFile="~/Persons.master" AutoEventWireup="true" CodeBehind="AddPerson.aspx.cs" Inherits="ICM.AddPerson" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PersonsContent" runat="server">
        <h2>
        Nouvelle personne
    </h2>
    <table>
        <tr>
            <td>Nom : </td>
            <td><asp:TextBox ID="nomText" Columns="15" runat="server" /></td>
        </tr>
        <tr>
            <td>Prenom : </td>
            <td><asp:TextBox ID="prenomText" Columns="15" runat="server" /></td>
        </tr>
        <tr>
            <td>Institution : </td>
            <td><asp:DropDownList ID="institutionList" runat="server" /></td>
        </tr>
        <tr>
            <td>Filière : </td>
            <td><asp:DropDownList ID="filiereList" runat="server" /></td>
        </tr>
        <tr>
            <td>Département : </td>
            <td><asp:TextBox ID="interetText" Columns="15" runat="server" /></td>
        </tr>
        <tr>
            <td>Téléphone : </td>
            <td><asp:TextBox ID="telText" Columns="15" runat="server" /></td>
        </tr>
        <tr>
            <td> </td>
            <td><asp:Button ID="ajouterButton" runat="server" Text="Ajouter" /></td>
        </tr>
    </table>        
    
     <script type="text/javascript">
         $(function () {
             $("[id$=ajouterButton]").button();
         });

    </script>
</asp:Content>
