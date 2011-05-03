<%@ Page Title="Ajouter contrat" Language="C#" MasterPageFile="~/Contracts.master" AutoEventWireup="true" CodeBehind="AddContract.aspx.cs" Inherits="ICM.AddContract" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContractsContent" runat="server">
   

    <h2>
        Nouveau contrat
    </h2>
    <table>
        <tr>
            <td>Title : </td>
            <td><asp:TextBox ID="titleText" Columns="15" runat="server" /></td>
            
        </tr>
        <tr>
            <td>Type : </td>
            <td><asp:DropDownList ID="typeContractList" runat="server" /></td>
            
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
            <td>Fichier binaire :</td>
            <td><asp:FileUpload ID="UploadImageFile" runat="server" /></td>
        </tr>
        <tr>
            <td> </td>
            <td> <asp:Button runat="server" id="submitForm" text="Ajouter" onclick="submitForm_Click" /></td>
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
             $("[id$=submitForm]").button();
         });

    </script>

</asp:Content>
