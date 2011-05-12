<%@ Page Title="" Language="C#" MasterPageFile="~/Institutions.master" AutoEventWireup="true" CodeBehind="Institutions.aspx.cs" Inherits="ICM.Institutions" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="InstitutionsContent" runat="server">
    <h2>
        Recherche une institution
    </h2>
    <table>
        <tr>
            <td>Nom : </td>
            <td>
                <asp:TextBox ID="NameText" Columns="15" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Langue :  </td>
            <td>
                <asp:DropDownList ID="LanguagesList" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Continent : </td>
            <td>
                <asp:DropDownList ID="ContinentsList" runat="server" AutoPostBack="True" onselectedindexchanged="ContinentsList_SelectedIndexChanged" />
            </td>
        </tr>
        <tr>
            <td>Pays : </td>
            <td>
                <asp:DropDownList ID="CountriesList" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Institution archivées :</td>
            <td>
                <asp:CheckBox runat="server" ID="ArchivedCheckBox" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="SearchButton" runat="server" Text="Rechercher" 
                    onclick="SearchButton_Click" />
            </td>
        </tr>
    </table>    

    <asp:ListView ID="ResultsView" runat="server" OnItemDeleting="InstitutionArchiving">
        <LayoutTemplate>
            <ul>
                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
            </ul>
                
            <p>
                Page:
                <asp:DataPager ID="SaisieDataPager" runat="server" PageSize="5">
                    <Fields>
                        <asp:NumericPagerField ButtonCount="5" />
                    </Fields>
                </asp:DataPager>
            </p>
        </LayoutTemplate>

        <ItemTemplate>
            <asp:Label ID="LabelID" runat="server" Visible="false" Text='<%# Eval("Id")%>' />
            <li>
                <a href='ShowInstitution.aspx?institution=<%# Eval("Id")%>'><%# Eval("Name") %> </a>
                    (<a href='AddInstitution.aspx?institution=<%# Eval("Id")%>'>Edit</a> | 
                    <asp:LinkButton ID="ArchiveButton" runat="server" CommandName="Delete" Text="Archive" 
                        OnClientClick="return confirm('Are you sure you want to archive this institution ?');" />)
            </li>
        </ItemTemplate>

        <EmptyDataTemplate>
            <p>
                Aucun résultat
            </p>
        </EmptyDataTemplate>
    </asp:ListView>
    <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red"></asp:Label>
</asp:Content>
