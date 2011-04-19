<%@ Page Title="Modifier contrat" Language="C#" MasterPageFile="~/Contracts.master" AutoEventWireup="true" CodeBehind="ModifContract.aspx.cs" Inherits="ICM.ModifContract" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContractsContent" runat="server">
   

    <h2>
        Modifier contrat
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
            <td>Date début : </td>
            <td><asp:TextBox ID="dateDebut" Columns="15" runat="server" /></td>
        </tr>
        <tr>
            <td>Date fin : </td>
            <td><asp:TextBox ID="dateFin" Columns="15" runat="server" /></td>
        </tr>
        <tr>
            <td>Institution : </td>
            <td><asp:DropDownList ID="institutionList" runat="server" /></td>
        </tr>
        <tr>
            <td>Personne de contact : </td>
            <td><asp:DropDownList ID="personneList" runat="server" /></td>
        </tr>
        <tr>
            <td> </td>
            <td><asp:Button ID="modifierButton" runat="server" Text="Modifier" /></td>
        </tr>
    </table>        
    
     <script type="text/javascript">
         $(function () {
             $("[id$=dateDebut]").datepicker();
         });
         $(function () {
             $("[id$=dateFin]").datepicker();
         });

         $(function () {
             $("[id$=modifierButton]").button();
         });

    </script>

</asp:Content>
