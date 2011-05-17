<%@ Page Title="" Language="C#" MasterPageFile="~/Institutions.master" AutoEventWireup="true" CodeBehind="AddInstitution.aspx.cs" Inherits="ICM.AddInstitution" %>
<asp:Content ID="Content1" ContentPlaceHolderID="InstitutionsContent" runat="server">
    <h2>
        &nbsp;<asp:Label ID="EditAddLabel" runat="server" Text="Nouvelle"></asp:Label> 
&nbsp;institution
    </h2>
    <table>
        <tr>
            <td>Nom : </td>
            <td>
                <asp:TextBox ID="NameText" Columns="15" runat="server" />
                <asp:RequiredFieldValidator runat="server" id="RequiredTitleValidator" ControlToValidate="NameText" errormessage="Veuillez entrer un nom !"/>
            </td>
        </tr>
        <tr>
            <td>Description : </td>
            <td>
                <asp:TextBox ID="DescriptionText" Columns="15" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Ville : </td>
            <td><asp:TextBox ID="CityText" Columns="15" runat="server" /></td>

        </tr>
        <tr>
            <td>Pays : </td>
            <td><asp:DropDownList ID="ContinentList" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="ContinentList_SelectedIndexChanged" /><asp:DropDownList ID="CountryList" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="CountryList" ErrorMessage="Pays doit être spécifié" 
                    ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>linguistique : </td>
            <td><asp:DropDownList ID="LanguageList" runat="server" /></td>
        </tr>
        <tr>
            <td>Intérêt : </td>
            <td><asp:TextBox ID="InterestText" Columns="15" runat="server" /></td>
        </tr>
        <tr>
            <td>Nouveau département : </td>
            <td>
                <asp:TextBox ID="DepartmentText" runat="server" Width="150px"></asp:TextBox>
            &nbsp;&nbsp;
                <asp:Button ID="AddDepartmentButton" runat="server" 
                    Text="Ajouter" onclick="AddDepartmentButton_Click" />
                <asp:Label ID="DepartmentLabel" runat="server" ForeColor="Red" 
                    Text="Le nom du départment doit être spécifié" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Départements : </td>
            <td>
                
                <asp:DropDownList ID="DepartmentList" runat="server" Width="150px">
                </asp:DropDownList>
&nbsp;&nbsp;
                <asp:Button ID="RemoveDepartmentButton" runat="server" Text="Supprimer" 
                    onclick="RemoveDepartmentButton_Click" />
                
            </td>
        </tr>
        <tr>
            <td> </td>
            <td> </td>
        </tr>
        <tr>
            <td> &nbsp;</td>
            <td><asp:Button ID="AddButton" runat="server" Text="Ajouter l'institution" 
                    onclick="AddButton_Click" />
                <asp:Button ID="EditButton" runat="server" onclick="EditButton_Click" 
                    Text="Sauvegarder" />
            </td>
        </tr>
        <tr><td></td><td>
            <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red"></asp:Label>
            </td></tr>
    </table>        
    
     <script type="text/javascript">
         $(function () {
             $("[id$=AddButton], [id$=EditButton], a", "#rightContent").button();
         });

    </script>
</asp:Content>
