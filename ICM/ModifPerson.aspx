<%@ Page Title="Modifier une personne" Language="C#" MasterPageFile="~/Persons.master" AutoEventWireup="true" CodeBehind="ModifPerson.aspx.cs" Inherits="ICM.ModifPerson" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PersonsContent" runat="server">
        <h2>
        Modifier personne
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
            <td><asp:Button ID="modifierButton" runat="server" Text="Modifier" /></td>
        </tr>
    </table>        
    
     <script type="text/javascript">
         $(function () {
             $("[id$=modifierButton]").button();
         });

    </script>
</asp:Content>
