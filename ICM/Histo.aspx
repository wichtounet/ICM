<%@ Page Title="Historique" Language="C#" MasterPageFile="~/Contracts.master" AutoEventWireup="true" CodeBehind="Histo.aspx.cs" Inherits="ICM.Histo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContractsContent" runat="server">
    <h2>
        Historique
    </h2>

    <table>
        <asp:Label ID="IDLabel" Visible="false" runat="server" Text="-1" />
        <tr>
            <td>Année : </td>
            <td><asp:TextBox ID="YearTextBox" runat="server" /></td>
            <td>
                <asp:RequiredFieldValidator runat="server" id="RequiredNameValidator" ControlToValidate="YearTextBox" errormessage="Veuillez entrer une année valide !" />
                <asp:CompareValidator id="CompareValidator1" runat="server" ErrorMessage="Veuillez entrer une année valide !" ControlToValidate="YearTextBox" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td>Institution : </td>
            <td><asp:DropDownList ID="InstitutionList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="InstitutionSelected" /></td>
        </tr>
        <tr>
            <td>Département : </td>
            <td><asp:DropDownList ID="DepartmentList" runat="server" /></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="SearchButton" runat="server" Text="Rechercher" OnClick="SearchHisto" />
            </td>
        </tr>
    </table>
    
    <asp:Label ID="ErrorLabel" ForeColor="Red" runat="server" Visible="false"></asp:Label>

    <asp:Panel ID="HistoPanel" runat="server" Visible="false">
        <asp:ListView ID="ContractsView" runat="server">
            <LayoutTemplate>
                <p>
                    <h3><strong>Contrats</strong></h3>

                    <ul>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </ul>
                </p>
            </LayoutTemplate>

            <ItemTemplate>
                <asp:Label ID="LabelID" runat="server" Visible="false" Text='<%# Eval("Id")%>' />
                <li>
                    <a href='ShowContract.aspx?contract=<%# Eval("Id")%>'><%# Eval("Title") %></a>
                </li>
            </ItemTemplate>

            <EmptyDataTemplate>
                <p>
                    Aucun contrat
                </p>
            </EmptyDataTemplate>
        </asp:ListView>

        <asp:ListView ID="PersonsView" runat="server">
            <LayoutTemplate>
                <p>
                    <h3><strong>Etudiants IN/OUT</strong></h3>
                    <ul>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </ul>
                </p>

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
                </li>
            </ItemTemplate>

            <EmptyDataTemplate>
                <p>
                    Aucun étudiant IN / OUT
                </p>
            </EmptyDataTemplate>
        </asp:ListView>

        <asp:Button Text="Télécharger PDF" runat="server" ID="GeneratePDFButton" OnClick="GeneratePDF" />
    </asp:Panel>

     <script type="text/javascript">
         $(function () {
             $("[id$=SearchButton]").button();
             $("[id$=GeneratePDFButton]").button();
         });
    </script>
</asp:Content>