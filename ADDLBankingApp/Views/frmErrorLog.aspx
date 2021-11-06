<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmErrorLog.aspx.cs" Inherits="ADDLBankingApp.Views.frmErrorLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">




    <h1>ErrorLog</h1>

    <input
        id="myInput"
        placeholder="Search"
        class="form-control"
        type="text" />
    <asp:GridView
        ID="gvErrorlog"
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
                HeaderText="Source"
                DataField="Source" />
            <asp:BoundField
                HeaderText="Number"
                DataField="Number" />
            <asp:BoundField
                HeaderText="Description"
                DataField="Description" />
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