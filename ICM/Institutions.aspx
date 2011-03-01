<%@ Page Title="" Language="C#" MasterPageFile="~/Institutions.master" AutoEventWireup="true" CodeBehind="Institutions.aspx.cs" Inherits="ICM.Institutions" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="InstitutionsContent" runat="server">
    <h2>
        Rechercher une institution
    </h2>
    <p>
        Nom : <asp:TextBox ID="NameLabel" Columns="15" runat="server" /> <br />
        Langue : <asp:TextBox ID="LanguageLabel" Columns="15" runat="server" /> <br />
        Continent : <asp:DropDownList ID="Continent" runat="server" /> <br />
        Pays : <asp:DropDownList ID="CountryList" runat="server" /> 
        <asp:Button ID="SearchButton" runat="server" Text="Rechercher" /><br /> 
    </p>
</asp:Content>
