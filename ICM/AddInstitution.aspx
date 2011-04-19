<%@ Page Title="" Language="C#" MasterPageFile="~/Institutions.master" AutoEventWireup="true" CodeBehind="AddInstitution.aspx.cs" Inherits="ICM.AddInstitution" %>
<asp:Content ID="Content1" ContentPlaceHolderID="InstitutionsContent" runat="server">
    <h2>
        Nouvelle institution
    </h2>
    <table>
        <tr>
            <td>Nom : </td>
            <td><asp:TextBox ID="nomText" Columns="15" runat="server" /></td>
        </tr>
        <tr>
            <td>Description : </td>
            <td><asp:TextBox ID="descrText" Columns="15" runat="server" /></td>
        </tr>
        <tr>
            <td>Emplacement : </td>
            <td><asp:DropDownList ID="continentList" runat="server" /><asp:DropDownList ID="countryList" runat="server" /></td>
        </tr>
        <tr>
            <td>linguistique : </td>
            <td><asp:DropDownList ID="LanguageList" runat="server" /></td>
        </tr>
        <tr>
            <td>Intérêt : </td>
            <td><asp:TextBox ID="interetText" Columns="15" runat="server" /></td>
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
