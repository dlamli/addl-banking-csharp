<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmCustomer.aspx.cs" Inherits="ADDLBankingApp.Views.frmCustomerManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.12/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/1.2.2/css/buttons.dataTables.min.css" />
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.12/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.2.2/js/dataTables.buttons.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.2/pdfmake.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.2/vfs_fonts.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.html5.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('[id*=gvCustomer]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                dom: 'Bfrtip',
                'aoColumnDefs': [{ 'bSortable': false, 'aTargets': [0] }],
                'iDisplayLength': 20,
                buttons: [
                    { extend: 'copy', text: 'Copy to clipboard', className: 'exportExcel', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'excel', text: 'Export to Excel', className: 'exportExcel', filename: 'Customers_Excel', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'csv', text: 'Export to CSV', className: 'exportExcel', filename: 'Customers_Csv', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'pdf', text: 'Export to PDF', className: 'exportExcel', filename: 'Customers_Pdf', orientation: 'landscape', pageSize: 'LEGAL', exportOptions: { modifier: { page: 'all' }, columns: ':visible' } }
                ]
            });
        });
    </script>
    <!-- Bootstrap DatePicker -->
    <script type="text/javascript">
        $(function () {
            $('[id*=txtBirthDate]').datepicker({
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

        function openModalMsg() {
            $('#myModalMsg').modal('show'); //ventana de mensajes
        }

        function CloseModalMsg() {
            $('#myModalMsg').modal('hide');//cierra ventana de mensajes
        }

        //$(document).ready(function () { //filtrar el datagridview
        //    $("#myInput").on("keyup", function () {
        //        var value = $(this).val().toLowerCase();
        //        $("#MainContent_gvCustomer tr").filter(function () {
        //            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        //        });
        //    });
        //});

    </script>


    <h1>Customer Management</h1>
    <%--    <input
        id="myInput"
        placeholder="Search"
        class="form-control"
        type="text" />--%>
    <asp:GridView
        ID="gvCustomer"
        runat="server"
        AutoGenerateColumns="false"
        CssClass="table table-sm"
        HeaderStyle-CssClass="thead-dark"
        HeaderStyle-BackColor="#204969"
        HeaderStyle-ForeColor="#FFF7F7"
        AlternatingRowStyle-BackColor="#DADADA"
        OnRowCommand="gvCustomer_RowCommand">

        <Columns>

            <asp:BoundField
                HeaderText="Id"
                DataField="Id" />
            <asp:BoundField
                HeaderText="Identification"
                DataField="Identification" />
            <asp:BoundField
                HeaderText="Username"
                DataField="Username" />
            <asp:BoundField
                HeaderText="Name"
                DataField="Name" />
            <asp:BoundField
                HeaderText="MiddleName"
                DataField="MiddleName" />
            <asp:BoundField
                HeaderText="LastName"
                DataField="LastName" />
            <asp:BoundField
                HeaderText="Password"
                DataField="Password" />
            <asp:BoundField
                HeaderText="Email"
                DataField="Email" />
            <asp:BoundField
                HeaderText="Birthdate"
                DataField="Birthdate" />
            <asp:BoundField
                HeaderText="Status"
                DataField="Status" />
            <asp:BoundField
                HeaderText="RoleId"
                DataField="RoleId" />
            <asp:ButtonField
                HeaderText="Edit"
                CommandName="editCustomer"
                ControlStyle-CssClass="btn btn-primary"
                ButtonType="Button"
                Text="Edit" />
            <asp:ButtonField
                HeaderText="Remove"
                CommandName="removeCustomer"
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
        OnClick="btnNew_Click"
        CausesValidation="false" />
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
                                    ID="ltrIdentification"
                                    Text="Identification"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtIdentification"
                                    runat="server"
                                    CssClass="form-control"
                                    MaxLength="9
                                    " />
                                <asp:RequiredFieldValidator
                                    ID="rfvIdentification"
                                    runat="server"
                                    ForeColor="Red"
                                    ControlToValidate="txtIdentification"
                                    EnableClientScript="true"
                                    ErrorMessage="Identification is required"
                                    Display="Dynamic"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator
                                    ID="revCCV"
                                    runat="server"
                                    ForeColor="Red"
                                    ControlToValidate="txtIdentification"
                                    ValidationExpression="^\d+$"
                                    EnableClientScript="true"
                                    ErrorMessage="Please enter number only"
                                    Display="Dynamic"
                                    SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrUsername"
                                    Text="Username"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtUsername"
                                    runat="server"
                                    CssClass="form-control" />
                                <asp:RequiredFieldValidator
                                    ID="rfvUsername"
                                    runat="server"
                                    ForeColor="Red"
                                    ControlToValidate="txtUsername"
                                    EnableClientScript="true"
                                    ErrorMessage="Username is required"
                                    Display="Dynamic"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrName"
                                    Text="Name"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtName"
                                    runat="server"
                                    CssClass="form-control" />
                                <asp:RequiredFieldValidator
                                    ID="rfvName"
                                    runat="server"
                                    ForeColor="Red"
                                    ControlToValidate="txtName"
                                    EnableClientScript="true"
                                    ErrorMessage="Name is required"
                                    Display="Dynamic"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrMiddleName"
                                    Text="MiddleName"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtMiddleName"
                                    runat="server"
                                    CssClass="form-control" />
                                <asp:RequiredFieldValidator
                                    ID="rfvMiddleName"
                                    runat="server"
                                    ForeColor="Red"
                                    ControlToValidate="txtMiddleName"
                                    EnableClientScript="true"
                                    ErrorMessage="MiddleName is required"
                                    Display="Dynamic"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrLastName"
                                    Text="LastName"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastName"
                                    runat="server"
                                    CssClass="form-control" />
                                <asp:RequiredFieldValidator
                                    ID="rfvLastName"
                                    runat="server"
                                    ForeColor="Red"
                                    ControlToValidate="txtLastName"
                                    EnableClientScript="true"
                                    ErrorMessage="LastName is required"
                                    Display="Dynamic"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrPassword"
                                    Text="Password"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtPassword"
                                    runat="server"
                                    CssClass="form-control" />
                                <asp:RequiredFieldValidator
                                    ID="rfvPassword"
                                    runat="server"
                                    ForeColor="Red"
                                    ControlToValidate="txtPassword"
                                    EnableClientScript="true"
                                    ErrorMessage="Password is required"
                                    Display="Dynamic"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrEmail"
                                    Text="Email"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail"
                                    runat="server"
                                    CssClass="form-control" />
                                <asp:RequiredFieldValidator
                                    ID="rfvEmail"
                                    runat="server"
                                    ForeColor="Red"
                                    ControlToValidate="txtEmail"
                                    EnableClientScript="true"
                                    ErrorMessage="Email is required"
                                    Display="Dynamic"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator
                                    ID="revEmail"
                                    runat="server"
                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ControlToValidate="txtEmail"
                                    ErrorMessage="Invalid Email Format"
                                    SetFocusOnError="True"
                                    ForeColor="Red"
                                    Display="Dynamic"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrBirthdate"
                                    Text="Birthdate"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtBirthDate"
                                    runat="server"
                                    CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator
                                    ID="rfvBirthDate"
                                    runat="server"
                                    ForeColor="Red"
                                    ControlToValidate="txtBirthDate"
                                    EnableClientScript="true"
                                    ErrorMessage="Birthdate is required"
                                    Display="Dynamic"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
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
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrRoleId"
                                    Text="Role"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList
                                    ID="ddlRole"
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
                    <h4 class="modal-title">Account Management</h4>
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
                        CausesValidation="false"
                        Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Confirm" />
                    <asp:LinkButton
                        type="button"
                        CssClass="btn btn-danger"
                        ID="btnCancelModal"
                        OnClick="btnCancelModal_Click"
                        runat="server"
                        CausesValidation="false"
                        Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cancel" />
                </div>
            </div>
        </div>
    </div>

    <%--Modal Message--%>
    <div id="myModalMsg" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button
                        type="button"
                        class="close"
                        data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">System Message</h4>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Literal
                            ID="ltrModalMessage"
                            runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton
                        type="button"
                        CssClass="btn btn-success"
                        ID="btnModalMessage"
                        OnClick="btnModalMessage_Click"
                        runat="server"
                        CausesValidation="false"
                        Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Continue" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
