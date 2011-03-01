<%@ Page Title="Rechercher une personne" Language="C#" MasterPageFile="~/Persons.Master" AutoEventWireup="true" CodeBehind="Persons.aspx.cs" Inherits="ICM.Persons" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="PersonsContent" runat="server">
    <h2>
        Rechercher une personne
    </h2>
    <p>
        Nom : <asp:TextBox ID="NameLabel" Columns="15" runat="server" /> <br />
        Prénom : <asp:TextBox ID="FirstNameLabel" Columns="15" runat="server" /> <br />
        Institution : <asp:DropDownList ID="InstitutionList" runat="server" /> <asp:Button ID="SearchButton" runat="server" Text="Rechercher" />
    </p>
</asp:Content>