<%@ Page Title="Contrats" Language="C#" MasterPageFile="~/Contracts.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ICM._Default" %>


<asp:Content ID="Body" runat="server" ContentPlaceHolderID="ContractsContent">
    <h2>
        Rechercher un contrat
    </h2>
    <asp:Label ID="stateForm" Visible="false" runat="server" Text="-1" />
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
            <td>Année : </td>
            <td><asp:DropDownList ID="YearList" runat="server" /></td>
            
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
            </td>
        </tr>
        <tr>
            <td>Personne de contact : </td>
            <td>
                <asp:DropDownList ID="PersonneList" runat="server" />
            </td>
            
        </tr>
        <tr>
            <td>Contract archivé :</td>
            <td><asp:CheckBox runat="server" ID="ArchivedCheck" /></td>
        </tr>
        <tr>
            <td> </td>
            <td> 
                <asp:Button runat="server" id="SearchButton" text="Rechercher" onclick="Search_Click" />
            </td>
        </tr>
    </table>   

    <asp:ListView ID="ResultsView" runat="server" OnItemDeleting="ContractDeleting">
        <LayoutTemplate>
            <ul>
                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
            </ul>
        </LayoutTemplate>

        <ItemTemplate>
            <asp:Label ID="LabelID" runat="server" Visible="false" Text='<%# Eval("Id")%>' />
            <li>
                <a href='ShowContract.aspx?contract=<%# Eval("Id")%>'><%# Eval("Title") %></a>
                    (<a href='AddContract.aspx?contract=<%# Eval("Id")%>'>Edit</a> | 
                    <asp:LinkButton ID="ArchiveButton" runat="server" CommandName="Delete" Text="Archive" 
                        OnClientClick="return confirm('Are you sure you want to archive this contract ?');" />)
            </li>
        </ItemTemplate>

        <EmptyDataTemplate>
            <p>
                Aucun résultat
            </p>
        </EmptyDataTemplate>
    </asp:ListView>
    
    <asp:Label ID="ErrorLabel" ForeColor="Red" runat="server" Visible="false"></asp:Label>

     <script type="text/javascript">
         $(function () {
             $("[id$=SearchButton]").button();
         });

    </script>
</asp:Content>
