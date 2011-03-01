<%@ Page Title="Contrats" Language="C#" MasterPageFile="~/Contracts.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ICM._Default" %>

<asp:Content ID="Body" runat="server" ContentPlaceHolderID="ContractsContent">
    <h2>
        Rechercher un contrat
    </h2>
    <p>
        Titre : <asp:TextBox ID="TitleLabel" Columns="15" runat="server" /> <br />
        Date de départ : <asp:Calendar ID="StartDateCalendar" runat="server" /> <br />
        Date de fin : <asp:Calendar ID="EndDateCalendar" runat="server" /> <br />
        Département : <asp:DropDownList ID="Departement" runat="server" /> <br />
        Université : <asp:DropDownList ID="DropDownList1" runat="server" /> <br />
    </p>
</asp:Content>
