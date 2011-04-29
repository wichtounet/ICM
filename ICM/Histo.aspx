<%@ Page Title="Historique" Language="C#" MasterPageFile="~/Contracts.master" AutoEventWireup="true" CodeBehind="Histo.aspx.cs" Inherits="ICM.Histo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContractsContent" runat="server">
    <h2>
        Historique
    </h2>
    <p>
        Année : <asp:TextBox ID="YearTextBox" runat="server" /> <br />
        Institution : <asp:DropDownList ID="InstitutionList" runat="server" /> <br />
        Département : <asp:DropDownList ID="DepartementList" runat="server" /> 
        <asp:Button ID="SearchButton" runat="server" Text="Rechercher" OnClick="SearchHisto" /><br /> 
        <br />
    </p>

    <asp:Panel ID="HistoPanel" runat="server" Visible="false">
        <asp:ListView ID="ContractsView" runat="server">
            <LayoutTemplate>
                <p>
                    <h3>Contrats</h3>

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
                    <a href='ShowContract.aspx?person=<%# Eval("Id")%>'><%# Eval("Title") %></a>
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
                    <h3>Etudiants IN/OUT</h3>
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
    </asp:Panel>
</asp:Content>