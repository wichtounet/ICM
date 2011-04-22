<%@ Page Title="Rechercher une personne" Language="C#" MasterPageFile="~/Persons.Master" AutoEventWireup="true" CodeBehind="Persons.aspx.cs" Inherits="ICM.Persons" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="PersonsContent" runat="server">
    <h2>
        Rechercher une personne
    </h2>
    <p>
        Nom : <asp:TextBox ID="NameLabel" Columns="15" runat="server" /> <br />
        Prénom : <asp:TextBox ID="FirstNameLabel" Columns="15" runat="server" /> <br />
        Institution : <asp:DropDownList ID="InstitutionList" runat="server" /> 
        <asp:CheckBox Text="Rechercher les personnes archivées ?" runat"server" ID="ArchivedCheckBox" />
        <asp:Button ID="SearchButton" runat="server" Text="Rechercher" OnClick="SearchPerson" />
    </p>

    <asp:ListView ID="ResultsView" runat="server" OnItemDeleting="PersonDeleting">
        <LayoutTemplate>
            <table border="0">
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
            </table>
        </LayoutTemplate>

        <ItemTemplate>
            <asp:Label ID="LabelID" runat="server" Visible="false" Text='<%# Eval("Id")%>' />
            <li>
                <%# Eval("Name") %> <%# Eval("FirstName") %> 
                    (Edit | 
                    <asp:LinkButton ID="ArchiveButton" runat="server" CommandName="Delete" Text="Archive" 
                        OnClientClick="return confirm('Are you sure you want to archive this person ?');" />)
            </li>
        </ItemTemplate>

        <EmptyDataTemplate>
            <p>
                Aucun résultat
            </p>
        </EmptyDataTemplate>
    </asp:ListView>
</asp:Content>