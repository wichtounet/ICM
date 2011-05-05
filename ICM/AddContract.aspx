<%@ Page Title="Ajouter contrat" Language="C#" MasterPageFile="~/Contracts.master" AutoEventWireup="true" CodeBehind="AddContract.aspx.cs" Inherits="ICM.AddContract" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContractsContent" runat="server">
   <asp:Label ID="stateForm" Visible="false" runat="server" Text="-1" />

    <h2>
        Nouveau contrat
    </h2>
    <table>
        <tr>
            <td>Title : </td>
            <td><asp:TextBox ID="TitleText" Columns="15" runat="server" /></td>
            
        </tr>
        <tr>
            <td>Type : </td>
            <td><asp:DropDownList ID="ContractTypeList" runat="server" /></td>
            
        </tr>
        <tr>
            <td>Date début : </td>
            <td><asp:TextBox ID="StartDate" Columns="15" runat="server" /></td>
            
        </tr>
        <tr>
            <td>Date fin : </td>
            <td><asp:TextBox ID="EndDate" Columns="15" runat="server" /></td>
            
        </tr>
        <tr>
            <td>Institution : </td>
            <td>
                <asp:DropDownList ID="InstitutionList" runat="server" OnSelectedIndexChanged="InstitutionSelected"  AutoPostBack="true"/>
            </td>
        </tr>
        <tr>
            <td>Département :</td>
            <td>
                <asp:DropDownList ID="DepartmentList" runat="server" />
                <asp:Button ID="AddDepartment" runat="server" Text="+" OnClick="AddDepartment_Click" />    
                <asp:DropDownList ID="DepartmentSelectedList" runat="server" />
                <asp:Button ID="DeleteDepartment" runat="server" Text="-" OnClick="DeleteDepartment_Click" />    
                <asp:Label ID="DepartmentLabel" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Personne de contact : </td>
            <td>
                <asp:DropDownList ID="PersonList" runat="server" />
                Rôle : <asp:DropDownList ID="RoleList" runat="server" />
                <asp:Button ID="AddPerson" runat="server" Text="+" OnClick="AddPerson_Click" />    
                <asp:DropDownList ID="PersonSelectedList" runat="server" />
                <asp:Button ID="DeletePerson" runat="server" Text="-" OnClick="DeletePerson_Click" />    
                <asp:Label ID="PersonLabel" runat="server" />
            </td>
            
        </tr>
        <tr>
            <td>Fichier binaire :</td>
            <td><asp:FileUpload ID="UploadImageFile" runat="server" /></td>
        </tr>
        <tr>
            <td> </td>
            <td> 
                <asp:Button runat="server" id="Add" text="Ajouter" onclick="Add_Click"  Visible="false" />
                <asp:Button runat="server" id="Save" text="Sauvegarder" onclick="Save_Click"  Visible="false" />
            </td>
        </tr>
    </table>        
    
     <script type="text/javascript">
         $(function () {
             $("[id$=StartDate]").datepicker();
             //$("[id$=startDate]").datepicker("option", "dateFormat", "dd.mm.yy");
             //$("[id$=startDate]").datepicker("setDate", new Date(2008, 9, 03));
         });
         $(function () {
             $("[id$=EndDate]").datepicker();
             //$("[id$=endDate]").datepicker("option", "dateFormat", "dd.mm.yy");
         });

         $(function () {
             $("[id$=Add]").button();
         });
         $(function () {
             $("[id$=Save]").button();
         });

    </script>

</asp:Content>
