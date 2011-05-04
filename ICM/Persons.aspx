<%@ Page Title="Rechercher une personne" Language="C#" MasterPageFile="~/Persons.Master" AutoEventWireup="true" CodeBehind="Persons.aspx.cs" Inherits="ICM.Persons" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="PersonsContent" runat="server">
    <h2>
        Rechercher une personne
    </h2>
    <p>
        <asp:Label Visible="false" ID="IDLabel" Text="-1" runat="server" />
        Nom : <asp:TextBox ID="NameLabel" Columns="15" runat="server" /> <br />
        Prénom : <asp:TextBox ID="FirstNameLabel" Columns="15" runat="server" /> <br />
        Institution : <asp:DropDownList ID="InstitutionList" runat="server" OnSelectedIndexChanged="InstitutionSelected" AutoPostBack="true" />  <br />
        Départment : <asp:DropDownList ID="DepartmentList" runat="server" />  <br />
        <asp:CheckBox Text="Rechercher les personnes archivées ?" runat="server" ID="ArchivedCheckBox" />
        <asp:Button ID="SearchButton" runat="server" Text="Rechercher" OnClick="SearchPerson" />
    </p>

    <asp:ListView ID="ResultsView" runat="server" OnItemDeleting="PersonDeleting">
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
                <a href='ShowPerson.aspx?person=<%# Eval("Id")%>'><%# Eval("Name") %> <%# Eval("FirstName") %></a>
                    (<a href='AddPerson.aspx?person=<%# Eval("Id")%>'>Edit</a> | 
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

    <asp:Label ID="ErrorLabel" runat="server" Visible="false"></asp:Label>

     <script type="text/javascript">
         $(function () {
             $("[id$=SearchButton]").button();
         });
    </script>
</asp:Content>