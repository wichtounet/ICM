<%@ Page Title="Contrats" Language="C#" MasterPageFile="~/Contracts.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ICM._Default" %>


<asp:Content ID="Body" runat="server" ContentPlaceHolderID="ContractsContent">
    <h2>
        Rechercher un contrat
    </h2>
    <p>
        Titre : <asp:TextBox ID="TitleLabel" Columns="15" runat="server" /> <br />
        Date de départ : <asp:TextBox ID="dateDepart" Columns="15" runat="server" /><br />
        Date de fin : <asp:TextBox ID="dateFin" Columns="15" runat="server" /> <br />
        Département : <asp:DropDownList ID="DepartementList" runat="server" /> <br />
        Université : <asp:DropDownList ID="UniversityList" runat="server" /> <br />
        Type : <asp:DropDownList ID="TypeList" runat="server" /> <br />
        Personne : <asp:DropDownList ID="PersonsList" runat="server" /> <br />
        <asp:Button ID="SearchButton" runat="server" Text="Rechercher" /> <br /> 
    </p>

     <script type="text/javascript">
         $(function () {
             $("[id$=dateDepart]").datepicker();
         });
         $(function () {
             $("[id$=dateFin]").datepicker();
         });

         $(function () {
             $("[id$=SearchButton]").button();
         });

    </script>
</asp:Content>
