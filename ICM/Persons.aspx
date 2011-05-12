<%@ Page Title="Rechercher une personne" Language="C#" MasterPageFile="~/Persons.Master" AutoEventWireup="true" CodeBehind="Persons.aspx.cs" Inherits="ICM.Persons" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="PersonsContent" runat="server">
    <h2>
        Rechercher une personne
    </h2>
    <table>
        <tr>
            <td>Nom : </td>
            <td>
                <asp:Label Visible="false" ID="IDLabel" Text="-1" runat="server" />
                <asp:TextBox ID="NameLabel" Columns="15" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Prénom : </td>
            <td><asp:TextBox ID="FirstNameLabel" Columns="15" runat="server" /></td>
        </tr>
        <tr>
            <td>Institution : </td>
            <td><asp:DropDownList ID="InstitutionList" runat="server" OnSelectedIndexChanged="InstitutionSelected" AutoPostBack="true" /></td>
        </tr>
        <tr>
            <td>Départment : </td>
            <td><asp:DropDownList ID="DepartmentList" runat="server" /></td>
        </tr>
        <tr>
            <td>Personne archivée :</td>
            <td><asp:CheckBox runat="server" ID="ArchivedCheckBox" /></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:Button ID="SearchButton" runat="server" Text="Rechercher" OnClick="SearchPerson" /></td>
        </tr>
    </table>

    <asp:ListView ID="ResultsView" runat="server" OnItemDeleting="PersonDeleting">
        <LayoutTemplate>
            <ul>
                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
            </ul>
                
            <p>
                Page:
                <asp:DataPager ID="SaisieDataPager" runat="server" PageSize="20">
                    <Fields>
                        <asp:NumericPagerField ButtonCount="5" />
                    </Fields>
                </asp:DataPager>
            </p>
        </LayoutTemplate>

        <ItemTemplate>
            <asp:Label ID="LabelID" runat="server" Visible="false" Text='<%# Eval("Id")%>' />
            <li>
                <a href='ShowPerson.aspx?person=<%# Eval("Id")%>'><%# Eval("Name") %> <%# Eval("FirstName") %></a>
                    (<a href='AddPerson.aspx?person=<%# Eval("Id")%>'>Edit</a> | 
                    <asp:LinkButton ID="ArchiveButton" runat="server" CommandName="Delete" Text="Archive" 
                        OnClientClick="return confirm('Etes vous sûr de vouloir archiver cette personne ?');" />)
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