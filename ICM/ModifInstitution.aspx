<%@ Page Title="Modifier institution" Language="C#" MasterPageFile="~/Institutions.master" AutoEventWireup="true" CodeBehind="ModifInstitution.aspx.cs" Inherits="ICM.ModifInstitution" %>
<asp:Content ID="Content1" ContentPlaceHolderID="InstitutionsContent" runat="server">
    <h2>
        Modifier institution
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
            <td><asp:DropDownList ID="langueList" runat="server" /></td>
        </tr>
        <tr>
            <td>Intérêt : </td>
            <td><asp:TextBox ID="interetText" Columns="15" runat="server" /></td>
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
