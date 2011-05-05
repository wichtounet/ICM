<%@ Page Title="Visualiser une institution" Language="C#" MasterPageFile="~/Institutions.master" AutoEventWireup="true" CodeBehind="ShowInstitution.aspx.cs" Inherits="ICM.ShowInstitution" %>
<asp:Content ID="Content1" ContentPlaceHolderID="InstitutionsContent" runat="server">
    <asp:Label ID="IDLabel" Visible="false" runat="server" />
    <table>
        <tr>
            <td>Nom : </td>
            <td><asp:Label ID="NameLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>Description : </td>
            <td><asp:Label ID="DescriptionLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>Ville : </td>
            <td><asp:Label ID="CityLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>Interêt : </td>
            <td><asp:Label ID="InterestLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>Language : </td>
            <td><asp:Label ID="LanguageLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>Pays : </td>
            <td><asp:Label ID="CountryLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>Continent : </td>
            <td><asp:Label ID="ContinentLabel" runat="server" /></td>
        </tr>
        <tr>
            <td>Archivée : </td>
            <td><asp:Label ID="StateLabel" runat="server" /></td>
        </tr>
    </table>
    <br />
    Départements:
    <asp:ListView ID="DepartmentsListView" runat="server">
        <LayoutTemplate>
            <table cellpadding="0" cellspacing="0">
                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Container.DataItem %>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
    <br />


    <br />
    <p>
        <asp:Button ID="EditButton" runat="server" Text="Modifier" 
            onclick="EditButton_Click"/> 
        <asp:Button ID="ArchiveButton" runat="server" Text="Archiver" 
            onclick="ArchiveButton_Click"/>
    </p>

     <script type="text/javascript">
         $(function () {
             $("[id$=EditButton]").button();
             $("[id$=ArchiveButton]").button();
         });
    </script>
</asp:Content>
