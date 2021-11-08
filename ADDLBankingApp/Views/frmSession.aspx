<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmSession.aspx.cs" Inherits="ADDLBankingApp.Views.frmSession" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>
    <!-- Bootstrap DatePicker -->
    <script type="text/javascript">
        $(function () {
            $('[id*=txtDateStart]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "yyyy/mm/dd",
                language: "tr"
            });
            $('[id*=txtDateExpiration]').datepicker({
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
                $("#MainContent_gvSession tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });

     </script>

    <h1>Session Management</h1>
    <input
        id="myInput"
        placeholder="Search"
        class="form-control"
        type="text" />
    <asp:GridView
        ID="gvSession"
        runat="server"
        AutoGenerateColumns="false"
        CssClass="table table-sm"
        HeaderStyle-CssClass="thead-dark"
        HeaderStyle-BackColor="#204969"
        HeaderStyle-ForeColor="#FFF7F7"
        AlternatingRowStyle-BackColor="#DADADA">

        <Columns>
            <asp:BoundField
                HeaderText="Id"
                DataField="Id" />
            <asp:BoundField
                HeaderText="UserId"
                DataField="UserId" />
            <asp:BoundField
                HeaderText="DateStart"
                DataField="DateStart" />
            <asp:BoundField
                HeaderText="DateExpiration"
                DataField="DateExpiration" />
            <asp:BoundField
                HeaderText="Status"
                DataField="Status" />
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
                                    ID="ltrUserId"
                                    Text="UserId"
                                    runat="server" />
                            <td>
                                <asp:DropDownList
                                    ID="ddlUser"
                                    CssClass="form-control"
                                    runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrDateStart"
                                    Text="DateStart"
                                    runat="server" />
                            </td>

                            <td>
                                <asp:TextBox
                                    ID="txtDateStart"
                                    runat="server"
                                    CssClass="form-control" />
                                <asp:RequiredFieldValidator 
                            ID="rfvDateStart" 
                            runat="server" 
                            ForeColor="Maroon"
                            ErrorMessage="DateStart is required"
                            ControlToValidate="txtDateStart"
                            />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrDateExpiration"
                                    Text="DateExpiration"
                                    runat="server" />
                            <td>
                                <asp:TextBox
                                    ID="txtDateExpiration"
                                    runat="server"
                                    CssClass="form-control" />
                           <asp:RequiredFieldValidator 
                            ID="rfvDateExpiration" 
                            runat="server" 
                            ForeColor="Maroon"
                            ErrorMessage="DateExpiration is required"
                            ControlToValidate="txtDateExpiration"
                            />
                                <asp:Label
                                    ID="lblDatemsg"
                                    ForeColor="#FFF7F7"
                                    Visible="False"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrStatus"
                                    Text="Status"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList
                                    ID="ddlStatus"
                                    CssClass="form-control"
                                    runat="server">
                                    <asp:ListItem Value="1">Active</asp:ListItem>
                                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                                </asp:DropDownList>

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

</asp:Content>
