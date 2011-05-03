<%@ Page Title="Contract" Language="C#" MasterPageFile="~/Contracts.master" AutoEventWireup="true" CodeBehind="ShowContract.aspx.cs" Inherits="ICM.ShowContract" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContractsContent" runat="server">
    <table>
        <asp:Label ID="IDLabel" Visible="false" runat="server" />
        <tr>
            <td>Titre : </td>
            <td><asp:Label ID="TitreLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>Personne de contact : </td>
            <td><asp:DropDownList ID="personList" runat="server" /></td>
        </tr>
        <tr>
            <td>Département : </td>
            <td><asp:DropDownList ID="departmentList" runat="server" /></td>
        </tr>
        <tr>
            <td>Date début : </td>
            <td><asp:Label ID="dateDebutLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>Date fin : </td>
            <td><asp:Label ID="dateFinLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>fichier source : </td>
            <td><asp:HyperLink ID="downloadFile" NavigateUrl="ContractFile.aspx" Text="Télécharger" Target="_blank" runat="server" /></td>
        </tr>
        <tr>
            <td>Type de Contrat : </td>
            <td><asp:Label ID="typeLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>Réalisé par : </td>
            <td><asp:Label ID="userLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>Archivé : </td>
            <td><asp:Label ID="StateLabel" runat="server" /></td>
        </tr>
    </table>
    
    <p>
        <asp:Button ID="EditButton" runat="server" Text="Modifier" OnClick="EditContract" /> 
        <asp:Button ID="DeleteButton" runat="server" Text="Archiver" OnClick="DeleteContract" />
    </p>

     <script type="text/javascript">
         $(function () {
             $("[id$=EditButton]").button();
             $("[id$=DeleteButton]").button();
         });
    </script>
</asp:Content>