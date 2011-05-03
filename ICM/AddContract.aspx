<%@ Page Title="Ajouter contrat" Language="C#" MasterPageFile="~/Contracts.master" AutoEventWireup="true" CodeBehind="AddContract.aspx.cs" Inherits="ICM.AddContract" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContractsContent" runat="server">
   <asp:Label ID="stateForm" Visible="false" runat="server" Text="-1" />

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
            <td>
                <asp:DropDownList ID="institutionList" runat="server" />
                <asp:Button ID="addInstitution" runat="server" Text="Ajouter" OnClick="addInstitution_Click" />    
                <asp:DropDownList ID="institutionChooseList" runat="server" />
                <asp:Button ID="deleteInstitution" runat="server" Text="Supprimer" OnClick="deleteInstitution_Click" />    
                <asp:Label ID="institutionLabel" runat="server" />
            </td>
            
        </tr>
        <tr>
            <td>Personne de contact : </td>
            <td>
                <asp:DropDownList ID="personneList" runat="server" />
                Rôle : <asp:DropDownList ID="roleList" runat="server" />
                <asp:Button ID="addPerson" runat="server" Text="Ajouter" OnClick="addPerson_Click" />    
                <asp:DropDownList ID="choosePersonsList" runat="server" />
                <asp:Button ID="deletePerson" runat="server" Text="Supprimer" OnClick="deletePerson_Click" />    
                <asp:Label ID="personLabel" runat="server" />
            </td>
            
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
