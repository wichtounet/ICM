<%@ Page Title="Historique" Language="C#" MasterPageFile="~/Contracts.master" AutoEventWireup="true" CodeBehind="Histo.aspx.cs" Inherits="ICM.Histo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContractsContent" runat="server">
    <h2>
        Historique
    </h2>
    <p>
        Année : <asp:TextBox ID="YearTextBox" runat="server" /> <br />
        Institution : <asp:DropDownList ID="InstitutionList" runat="server" /> <br />
        Département : <asp:DropDownList ID="DepartementList" runat="server" /> 
        <asp:Button ID="SearchButton" runat="server" Text="Rechercher" /><br /> 
        <br />
    </p>

    <p>
        <h3>Contrats</h3>
        <ul>
            <li>Blah</li>
            <li>Bla</li>
            <li>Blah</li>
        </ul>
    </p>

    <p>
        <h3>Etudiants IN/OUT</h3>
        <ul>
            <li>Blah</li>
            <li>Bla</li>
            <li>Blah</li>
        </ul>
    </p>

</asp:Content>