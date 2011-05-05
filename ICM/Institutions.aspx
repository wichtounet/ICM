<%@ Page Title="" Language="C#" MasterPageFile="~/Institutions.master" AutoEventWireup="true" CodeBehind="Institutions.aspx.cs" Inherits="ICM.Institutions" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="InstitutionsContent" runat="server">
    <p>
        Nom : <asp:TextBox ID="NameText" Columns="15" runat="server" /> <br />
        Langue :  
        <asp:DropDownList ID="LanguagesList" runat="server">
        </asp:DropDownList>
        <br />
        Continent : <asp:DropDownList ID="ContinentsList" runat="server" AutoPostBack="True" onselectedindexchanged="ContinentsList_SelectedIndexChanged" />
        &nbsp;&nbsp; Pays : <asp:DropDownList ID="CountriesList" runat="server" />
        <br />
        <asp:CheckBox Text="Rechercher aussi les institutions archivées ?" runat="server" ID="ArchivedCheckBox" />
        <br />
        <br /> 
        <asp:Button ID="SearchButton" runat="server" Text="Rechercher" 
            onclick="SearchButton_Click" />
    </p>
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
</asp:Content>
