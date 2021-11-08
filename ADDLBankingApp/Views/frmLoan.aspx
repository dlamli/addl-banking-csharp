<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmLoan.aspx.cs" Inherits="ADDLBankingApp.Views.frmLoan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function openModal() {
            $('#myModal').modal('show'); //messages window
        }

        function openModalManagement() {
            $('#myModalManagement').modal('show'); //maintenance window
        }

        function CloseModal() {
            $('#myModal').modal('hide');//closes messages window
        }

        function CloseManagement() {
            $('#myModalManagement').modal('hide'); //closes maintenance window
        }

        $(document).ready(function () { //filters datagridview
            $("#myInput").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#MainContent_gvPayment tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });

    </script>

        <h1>Loan Management</h1>
    <input
        id="myInput"
        placeholder="Search"
        class="form-control"
        type="text" />

    <asp:GridView 
        ID="gvLoan" 
        runat="server"
        AutoGenerateColumns="false"
        CssClass="table table-sm"
        HeaderStyle-CssClass="thead-dark"
        HeaderStyle-BackColor="#204969"
        HeaderStyle-ForeColor="#FFF7F7"
        AlternatingRowStyle-BackColor="#DADADA"
        OnRowCommand="gvLoan_RowCommand">

        <Columns>

            <asp:BoundField 
                HeaderText="Id"
                DataField="Id"/>
            <asp:BoundField 
                HeaderText="Type"
                DataField="Type"/>
            <asp:BoundField 
                HeaderText="Amount"
                DataField="Amount"/>
            <asp:BoundField 
                HeaderText="AccountId"
                DataField="AccountId"/>
            <asp:ButtonField 
                HeaderText="Edit"
                CommandName="editLoan"
                ControlStyle-CssClass="btn btn-primary"
                ButtonType="Button"
                Text="Edit"/>
            <asp:ButtonField 
                HeaderText="Remove"
                CommandName="removeLoan"
                ControlStyle-CssClass="btn btn-danger"
                ButtonType="Button"
                Text="Remove" />

        </Columns>

    </asp:GridView>

    <asp:LinkButton 
        ID="btnNew" 
        type="Button"
        CssClass="btn btn-success"
        runat="server"
        Text="<span aria-hidden='true' glyphicon-plus></span>New"
        OnClick="btnNew_Click"/>

    <asp:Label 
        ID="lblStatus"
        ForeColor="#FFF7F7"
        runat="server"
        Visible="false"/>

    <!--Management Window -->
    <div id="myModalManagement" class="modal fade" role="dialog">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <button 
                        type="button" 
                        class="close"
                        data-dismiss="modal"
                        >&times;
                    </button>
                    <h4 class="modal-title">
                        <asp:Literal 
                            ID="ltrTitleManagement"
                            runat="server"
                            ></asp:Literal>
                    </h4>
                </div>
                <div class="modal-body">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrIdManagement"
                                    Text="Id"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtIdManagement"
                                    runat="server"
                                    Enabled="false"
                                    CssClass="form-control" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrType"
                                    Text="Type"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox
                                    ID="txtType"
                                    runat="server"
                                    CssClass="form-control" />

                                <asp:RequiredFieldValidator 
                                    ID="rfvType" 
                                    runat="server"
                                    ErrorMessage="Type is required"
                                    ControlToValidate="txtType"
                                    ForeColor="Maroon"
                                    />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrAmount"
                                    Text="Amount"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox
                                    ID="txtAmount"
                                    runat="server"
                                    CssClass="form-control" />

                                <asp:RequiredFieldValidator 
                                    ID="rfvAmount" 
                                    runat="server"
                                    ErrorMessage="Amount is required"
                                    ControlToValidate="txtAmount"
                                    ForeColor="Maroon"
                                    />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrAccountId"
                                    Text="AccountId"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList
                                    ID="ddlAccount"
                                    CssClass="form-control"
                                    runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <asp:Label 
                        ID="lblResult"
                        ForeColor="#FFF7F7"
                        Visible="False" 
                        runat="server"
                        />
                </div>
                <div class="modal-footer">
                    <asp:LinkButton 
                        type="button"
                        OnClick="btnConfirmManagement_Click"
                        CssClass="btn btn-success" 
                        ID="btnConfirmManagement"
                        runat="server" 
                        Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Confirm"
                        />
                    <asp:LinkButton 
                        type="button" 
                        OnClick="btnCancelManagement_Click"
                        CssClass="btn btn-danger" 
                        ID="btnCancelManagement"
                        runat="server" 
                        Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cancel"
                        />
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Window -->
    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">Payment Management</h4>
            </div>
            <div class="modal-body">
                <p>
                    <asp:Literal ID="ltrModalMessage" runat="server"/>
                    <asp:Label  ID="lblRemoveCode" runat="server"/>
                </p>
            </div>
                <div class="modal-footer">
                    <asp:LinkButton type="button" CssClass="btn btn-success" ID="btnConfirmModal" OnClick="btnConfirmModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar"/>
                    <asp:LinkButton type="button" CssClass="btn btn-danger" ID="btnCancelModal" OnClick="btnCancelModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar"/>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
