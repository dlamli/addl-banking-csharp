<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmCard.aspx.cs" Inherits="ADDLBankingApp.Views.frmCard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Bootstrap -->
    <!-- Bootstrap DatePicker -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>
    <!-- Bootstrap DatePicker -->
    <script type="text/javascript">
        $(function () {
            $('[id*=txtDueDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "yyyy/mm/dd",
                language: "tr"
            });
        });
    </script>

    <script type="text/javascript">

        function openModal() {
            $('#myModal').modal('show'); //ventana de mensajes
        }

        function openModalManagement() {
            $('#myModalManagement').modal('show'); //ventana de mantenimiento
        }

        function CloseModal() {
            $('#myModal').modal('hide');//cierra ventana de mensajes
        }

        function CloseManagement() {
            $('#myModalManagement').modal('hide'); //cierra ventana de mantenimiento
        }

        $(document).ready(function () { //filtrar el datagridview
            $("#myInput").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#MainContent_gvCard tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });

    </script>



    <h1>Card Management</h1>
    <input
        id="myInput"
        placeholder="Search"
        class="form-control"
        type="text" />
    <asp:GridView
        ID="gvCard"
        runat="server"
        AutoGenerateColumns="false"
        CssClass="table table-sm"
        HeaderStyle-CssClass="thead-dark"
        HeaderStyle-BackColor="#204969"
        HeaderStyle-ForeColor="#FFF7F7"
        AlternatingRowStyle-BackColor="#DADADA"
        OnRowCommand="gvCard_RowCommand"
        >

        <Columns>

            <asp:BoundField
                HeaderText="Id"
                DataField="Id" />
            <asp:BoundField
                HeaderText="CardType"
                DataField="CardType" />
            <asp:BoundField
                HeaderText="CardNumber"
                DataField="CardNumber" />
            <asp:BoundField
                HeaderText="CCV"
                DataField="CCV" />
            <asp:BoundField
                HeaderText="DueDate"
                DataField="DueDate" />
            <asp:BoundField
                HeaderText="Provider"
                DataField="Provider" />
            <asp:ButtonField
                HeaderText="Edit"
                CommandName="editCard"
                ControlStyle-CssClass="btn btn-primary"
                ButtonType="Button"
                Text="Edit" />
            <asp:ButtonField
                HeaderText="Remove"
                CommandName="removeCard"
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
        Text="<span aria-hidden='true' glyphicon glyphicon-plus ></span> New"
        OnClick="btnNew_Click" />
    <asp:Label
        ID="lblStatus"
        ForeColor="#FFF7F7"
        runat="server"
        Visible="false" />



    <!--Management Window -->
    <div id="myModalManagement" class="modal fade" role="dialog">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <button
                        type="button"
                        class="close"
                        data-dismiss="modal">
                        &times;
                    </button>
                    <h4 class="modal-title">
                        <asp:Literal
                            ID="ltrTitleManagement"
                            runat="server"></asp:Literal>
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
                                    ID="ltrCardType"
                                    Text="CardType"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList
                                    ID="ddlCardType"
                                    CssClass="form-control"
                                    runat="server">
                                    <asp:ListItem Value="Credit">Credit</asp:ListItem>
                                    <asp:ListItem Value="Debit">Debit</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrCard"
                                    Text="CardNumber"
                                    runat="server" />
                            <td>
                                <asp:TextBox
                                    ID="txtCardNumber"
                                    runat="server"
                                    CssClass="form-control"
                                    MaxLength="16" />
                                <asp:RegularExpressionValidator
                                    runat="server"
                                    ID="RegularExpressionValidator2"
                                    ControlToValidate="txtCardNumber"
                                    ValidationExpression="^\d+$"
                                    EnableClientScript="true"
                                    ErrorMessage="Please enter numbers only"
                                    Display="Dynamic"
                                    SetFocusOnError="True" />
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrCCV"
                                    Text="CCV"
                                    runat="server" />
                            <td>
                                <asp:TextBox
                                    ID="txtCCV"
                                    runat="server"
                                    CssClass="form-control"
                                    MaxLength="3"
                                    />
                                <asp:RegularExpressionValidator
                                    runat="server"
                                    ID="RegularExpressionValidator3"
                                    ControlToValidate="txtCCV"
                                    ValidationExpression="^\d+$"
                                    EnableClientScript="true"
                                    ErrorMessage="Please enter numbers only"
                                    Display="Dynamic"
                                    SetFocusOnError="True" />
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrDate"
                                    Text="Date"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDueDate" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                                <asp:RequiredFieldValidator ID="rfvBirthdate" runat="server"
                                    ForeColor="Red"
                                    ErrorMessage="DudeDate is required"
                                    ControlToValidate="txtDueDate" 
                                    EnableClientScript="False"
                                    ></asp:RequiredFieldValidator>      
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrProvider"
                                    Text="Provider"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox
                                    ID="txtProvider"
                                    MaxLength="50"
                                    runat="server"
                                    CssClass="form-control" />
                            </td>
                        </tr>
                    </table>
                    <asp:Label
                        ID="lblResult"
                        ForeColor="#FFF7F7"
                        Visible="False"
                        runat="server" />
                </div>
                <div class="modal-footer">
                    <asp:LinkButton
                        type="button"
                        OnClick="btnConfirmManagement_Click"
                        CssClass="btn btn-success"
                        ID="btnConfirmManagement"
                        runat="server"
                        Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Confirm" />
                    <asp:LinkButton
                        type="button"
                        OnClick="btnCancelManagement_Click"
                        CssClass="btn btn-danger"
                        ID="btnCancelManagement"
                        runat="server"
                        Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cancel" />
                </div>
            </div>
        </div>
    </div>



    <!-- Modal Window -->
    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button
                        type="button"
                        class="close"
                        data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Card Management</h4>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Literal
                            ID="ltrModalMsg"
                            runat="server" />
                        <asp:Literal
                            ID="lblIdRemove"
                            runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton
                        type="button"
                        CssClass="btn btn-success"
                        ID="btnConfirmModal"
                        OnClick="btnConfirmModal_Click"
                        runat="server"
                        Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Confirm" />
                    <asp:LinkButton
                        type="button"
                        CssClass="btn btn-danger"
                        ID="btnCancelModal"
                        OnClick="btnCancelModal_Click"
                        runat="server"
                        Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cancel" />
                </div>
            </div>
        </div>
    </div>






</asp:Content>
