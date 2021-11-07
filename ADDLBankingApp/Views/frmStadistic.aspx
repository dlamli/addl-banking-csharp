<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmStadistic.aspx.cs" Inherits="ADDLBankingApp.Views.frmStadistic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Stadistic</h1>

<input
        id="myInput"
        placeholder="Search"
        class="form-control"
        type="text" />
    <asp:GridView
        ID="gvStadistic"
        runat="server"
        AutoGenerateColumns="false"
        CssClass="table table-sm"
        HeaderStyle-CssClass="thead-dark"
        HeaderStyle-BackColor="#204969"
        HeaderStyle-ForeColor="#FFF7F7"
        AlternatingRowStyle-BackColor="#DADADA"
        >

        <Columns>
             <asp:BoundField
                HeaderText="Id"
                DataField="Id" />
            <asp:BoundField
                HeaderText="UserId"
                DataField="UserId" />
             <asp:BoundField
                HeaderText="Date"
                DataField="Date" />
            <asp:BoundField
                HeaderText="Platform"
                DataField="Platform" />
            <asp:BoundField
                HeaderText="Browser"
                DataField="Browser" />
            <asp:BoundField
                HeaderText="Page"
                DataField="Page" />
            <asp:BoundField
                HeaderText="Action"
                DataField="Action" />

        
        </Columns>

    </asp:GridView>

    <asp:Label
        ID="lblStatus"
        ForeColor="#FFF7F7"
        runat="server"
        Visible="false" />




</asp:Content>