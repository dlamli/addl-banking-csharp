<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmTimeDeposit.aspx.cs" Inherits="ADDLBankingApp.Views.frmTimeDeposit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
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
            $('[id*=gvTimeDeposit]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                dom: 'Bfrtip',
                'aoColumnDefs': [{ 'bSortable': false, 'aTargets': [0] }],
                'iDisplayLength': 20,
                buttons: [
                    { extend: 'copy', text: 'Copy to clipboard', className: 'exportExcel', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'excel', text: 'Export to Excel', className: 'exportExcel', filename: 'TimeDeposits_Excel', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'csv', text: 'Export to CSV', className: 'exportExcel', filename: 'TimeDeposits_Csv', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'pdf', text: 'Export to PDF', className: 'exportExcel', filename: 'TimeDeposits_Pdf', orientation: 'landscape', pageSize: 'LEGAL', exportOptions: { modifier: { page: 'all' }, columns: ':visible' } }
                ]
            });
        });
    </script>


    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>
    <!-- Bootstrap DatePicker -->
    <script type="text/javascript">
        $(function () {
            $('[id*=txtStartDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "yyyy/mm/dd",
                language: "tr"
            });
            $('[id*=txtEndDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "yyyy/mm/dd",
                language: "tr"
            });
        });

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

        $(document).ready(function () { //filtrar el datagridview
            $("#myInput").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#MainContent_gvTimeDeposit tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });


    </script>


    <h1>Time Deposit Management</h1>
    <asp:GridView
        ID="gvTimeDeposit"
        runat="server"
        AutoGenerateColumns="false"
        CssClass="table table-sm"
        HeaderStyle-CssClass="thead-dark"
        HeaderStyle-BackColor="#204969"
        HeaderStyle-ForeColor="#FFF7F7"
        AlternatingRowStyle-BackColor="#DADADA"
        OnRowCommand="gvTimeDeposit_RowCommand"
        >
        <Columns>
            <asp:BoundField
                HeaderText="Id"
                DataField="Id" 
                />
            <asp:BoundField
                HeaderText="AccountId"
                DataField="AccountId" 
                />
            <asp:BoundField
                HeaderText="Amount"
                DataField="Amount" 
                />
            <asp:BoundField
                HeaderText="StartDate"
                DataField="StartDate" 
                />
            <asp:BoundField
                HeaderText="EndDate"
                DataField="EndDate" 
                />
            <asp:BoundField
                HeaderText="Percentage"
                DataField="Percentage"   
                />
            <asp:ButtonField
                HeaderText="Edit"
                CommandName="editTimeDeposit"
                ControlStyle-CssClass="btn btn-primary"
                ButtonType="Button"
                Text="Edit" 
                />
            <asp:ButtonField
                HeaderText="Remove"
                CommandName="removeTimeDeposit"
                ControlStyle-CssClass="btn btn-danger"
                ButtonType="Button"
                Text="Remove" 
                />
        </Columns>

    </asp:GridView>
    <asp:LinkButton
        ID="btnNew"
        type="Button"
        CssClass="btn btn-success"
        runat="server"
        Text="<span aria-hidden='true' glyphicon glyphicon-plus ></span> New"
        OnClick="btnNew_Click"
        CausesValidation="false"
        />
    <asp:Label
        ID="lblStatus"
        ForeColor="#FFF7F7"
        runat="server"
        Visible="false"
        />


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
                                    ID="ltrAccount"
                                    Text="IBAN"
                                    runat="server" />
                            <td>
                                <asp:DropDownList
                                    ID="ddlAccount"
                                    CssClass="form-control"
                                    runat="server">
                                </asp:DropDownList>
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
                                    ForeColor="Red"
                                    ControlToValidate="txtAmount" 
                                    EnableClientScript="true"
                                    ErrorMessage="Amount is required"
                                    Display="Dynamic"
                                    SetFocusOnError="True" 
                                    ></asp:RequiredFieldValidator>   
                                <asp:RegularExpressionValidator
                                    ID="revAmount"
                                    runat="server"
                                    ForeColor="Red"
                                    ControlToValidate="txtAmount"
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
                                    ID="ltrStartDate"
                                    Text="DateStart"
                                    runat="server" />
                            <td>
                                <asp:TextBox
                                    ID="txtStartDate"
                                    runat="server"
                                    CssClass="form-control"
                                   />
                                <asp:RequiredFieldValidator 
                                    ID="rfvStartDate" 
                                    runat="server"
                                    ForeColor="Red"
                                    ControlToValidate="txtStartDate" 
                                    EnableClientScript="true"
                                    ErrorMessage="StartDate is required"
                                    Display="Dynamic"
                                    SetFocusOnError="True" 
                                    ></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrEndDate"
                                    Text="EndDate"
                                    runat="server" />
                            <td>
                                <asp:TextBox
                                    ID="txtEndDate"
                                    runat="server"
                                    CssClass="form-control"
                                    />
                                <asp:RequiredFieldValidator 
                                    ID="rfvEndDate" 
                                    runat="server"
                                    ForeColor="Red"
                                    ControlToValidate="txtEndDate" 
                                    EnableClientScript="true"
                                    ErrorMessage="EndDate is required"
                                    Display="Dynamic"
                                    SetFocusOnError="True" 
                                    ></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrPercentage"
                                    Text="Percentage"
                                    runat="server" />
                            </td>

                            <td>
                                <asp:TextBox
                                    ID="txtPercentage"
                                    runat="server"
                                    MaxLength="2"
                                    CssClass="form-control" />
                                <asp:RequiredFieldValidator 
                                    ID="rfvPercentage" 
                                    runat="server"
                                    ForeColor="Red"
                                    ControlToValidate="txtPercentage" 
                                    EnableClientScript="true"
                                    ErrorMessage="Percentage is required"
                                    Display="Dynamic"
                                    SetFocusOnError="True" 
                                    ></asp:RequiredFieldValidator>
                                 <asp:RegularExpressionValidator
                                    ID="revPercentage"
                                    runat="server"
                                    ForeColor="Red"
                                    ControlToValidate="txtPercentage"
                                    ValidationExpression="^\d+$"
                                    EnableClientScript="true"
                                    ErrorMessage="Please enter numbers only"
                                    Display="Dynamic"
                                    SetFocusOnError="True" />
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
                    <h4 class="modal-title">Time Deposit Management</h4>
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
