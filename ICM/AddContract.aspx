<%@ Page Title="Ajouter contrat" Language="C#" MasterPageFile="~/Contracts.master" AutoEventWireup="true" CodeBehind="AddContract.aspx.cs" Inherits="ICM.AddContract" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContractsContent" runat="server">
   <asp:Label ID="stateForm" Visible="false" runat="server" Text="-1" />
   <asp:Label ID="FileID" Visible="false" runat="server" Text="-1" />
    <h2>
        Nouveau contrat
    </h2>
    <table width="700px">
        <tr>
            <td>Title : </td>
            <td>
                <asp:TextBox ID="TitleText" Columns="15" runat="server" />
                <asp:RequiredFieldValidator runat="server" id="RequiredTitleValidator" ControlToValidate="TitleText" errormessage="Veuillez entrer un titre !" Enabled="false"/>
            </td>
            
        </tr>
        <tr>
            <td>Type : </td>
            <td>
                <asp:DropDownList ID="ContractTypeList" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredTypeValidator" runat="server" ControlToValidate="ContractTypeList" 
                    InitialValue="" Text="Veuillez choisir un type !" Enabled="false"/>
            </td>
            
        </tr>
        <tr>
            <td> </td>
            <td>
                
            </td>
        </tr>
        <tr>
            <td colspan=2>
                <fieldset>
                    <legend>Département :</legend>
                    <table>
                        <tr>
                            <td>Institution :</td>
                            <td>
                                <asp:DropDownList ID="InstitutionList" runat="server" OnSelectedIndexChanged="InstitutionSelected"  AutoPostBack="true"/>
                                Pas dans la liste? <a class="AddLink" href='AddInstitution.aspx' target="_blank">Ajouter</a>
                            </td>
                        </tr>
                        <tr>
                            <td>Département :</td>
                            <td>
                                <asp:DropDownList ID="DepartmentList" runat="server" />
                                <asp:Button ID="AddDepartment" runat="server" Text="+" OnClick="AddDepartment_Click" CausesValidation="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>Département choisis :</td>
                            <td>
                                <asp:DropDownList ID="DepartmentSelectedList" runat="server" />
                                <asp:Button ID="DeleteDepartment" runat="server" Text="-" OnClick="DeleteDepartment_Click" CausesValidation="false" />    
                                <asp:Label ID="DepartmentLabel" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredDepartmentValidator" runat="server" ControlToValidate="DepartmentSelectedList" 
                                    InitialValue="" Text="Veuillez choisir un département !" Enabled="false" />
                            </td>
                        </tr>
                    </table> 
                </fieldset>
            </td>
        </tr>
        <tr>
            <td colspan=2>
                <fieldset>
                    <legend>Contact :</legend>
                    <table>
                        <tr>
                            <td>Personne :</td>
                            <td>
                                <asp:DropDownList ID="PersonList" runat="server" />
                                Pas dans la liste? <a class="AddLink" href='AddPerson.aspx' target="_blank">Ajouter</a>                                
                            </td>
                        </tr>
                        <tr>
                            <td>Rôle : </td>
                            <td>
                                <asp:DropDownList ID="RoleList" runat="server" />
                                <asp:Button ID="AddPerson" runat="server" Text="+" OnClick="AddPerson_Click" CausesValidation="false"/>
                            </td>
                        </tr>
                        <tr>
                            <td>Personne choisies :</td>
                            <td>
                                <asp:DropDownList ID="PersonSelectedList" runat="server" />
                                <asp:Button ID="DeletePerson" runat="server" Text="-" OnClick="DeletePerson_Click" CausesValidation="false" />    
                                <asp:Label ID="PersonLabel" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredPersonValidator" runat="server" ControlToValidate="PersonSelectedList" 
                                InitialValue="" Text="Veuillez choisir une personne de contact !" Enabled="false" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>            
        </tr>
        <tr>
            <td>Date début : </td>
            <td>
                <asp:TextBox ID="StartDate" Columns="15" runat="server"  CausesValidation="false"/><asp:Label ID="StartValue" Visible="false" runat="server" Text="" />
            </td>
            
        </tr>
        <tr>
            <td>Date fin : </td>
            <td>
                <asp:TextBox ID="EndDate" Columns="15" runat="server" /><asp:Label ID="EndValue" Visible="false" runat="server" Text=""/>
                <asp:CompareValidator ID="CompareDate" runat="server" ControlToCompare="StartDate"
                    ControlToValidate="EndDate" ErrorMessage="Veuillez entrer une date supérieure à la date de début !" Operator="GreaterThan" Type="Date" Enabled="false" />
            </td>
            
        </tr>
        <tr>
            <td>Source pdf :</td>
            <td>
                <asp:HyperLink class="AddLink" ID="downloadFile" NavigateUrl="ContractFile.aspx" Text="Télécharger" Target="_blank" runat="server" />
                <asp:FileUpload ID="UploadImageFile" runat="server" />
                <asp:CustomValidator ID="CustomValidatorUpload" runat="server" 
                    ClientValidationFunction="ValidateFileUpload" ErrorMessage="Veuillez choisir un fichier .pdf" Enabled="false" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="Refresh" runat="server" Text="Rafraichir" OnClick="Refresh_Click" CausesValidation="false"/>
                <asp:Button runat="server" id="Add" text="Ajouter" onclick="Add_Click"  Visible="false" />
                <asp:Button runat="server" id="Save" text="Sauvegarder" onclick="Save_Click"  Visible="false" />
            </td>
        </tr>
    </table>
    
    <asp:Label ForeColor="Red" ID="ErrorLabel" Visible="false" runat="server" />
    
     <script type="text/javascript">
         
         function ValidateFileUpload(Source, args) {
             var fuData = document.getElementById('<%= UploadImageFile.ClientID %>');
             var FileUploadPath = fuData.value;

             if (FileUploadPath == '') {
                 // There is no file selected 
                 args.IsValid = false;
             }
             else {
                 var Extension = FileUploadPath.substring(FileUploadPath.lastIndexOf('.') + 1).toLowerCase();

                 if (Extension == "pdf") {
                     args.IsValid = true; // Valid file type
                 }
                 else {
                     args.IsValid = false; // Not valid file type
                 }
             }
         }

         $(function () {
             $("[id$=StartDate]").datepicker();
             $("[id$=StartDate]").datepicker("option", "dateFormat", "yy-mm-dd");
             $("[id$=EndDate]").datepicker();
             $("[id$=EndDate]").datepicker("option", "dateFormat", "yy-mm-dd");
             $("[id$=StartDate]").datepicker("setDate", new Date("<%= getStartDate() %>"));
             $("[id$=EndDate]").datepicker("setDate", new Date("<%= getEndDate() %>"));
         });
         $(function () {
             $("[id$=Refresh], [id$=MorePerson],[id$=MoreInstitution] ,[id$=Save] , [id$=Add], a", "#rightContent").button();
         });

         $(document).ready(function () {
             LoadDateControls();
         });

    </script>

</asp:Content>
