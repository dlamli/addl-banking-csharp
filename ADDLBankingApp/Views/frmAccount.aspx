<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmAccount.aspx.cs" Inherits="ADDLBankingApp.Views.frmAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

        function openModal() {
            $('#myModal').modal('show'); //ventana de mensajes
        }

        function openModalMsg() {
            $('#myModalMsg').modal('show'); //ventana de mensajes
        }

        function openModalManagement() {
            $('#myModalManagement').modal('show'); //ventana de mantenimiento
        }

        function CloseModal() {
            $('#myModal').modal('hide');//cierra ventana de mensajes
        }

        function CloseModalMsg() {
            $('#myModalMsg').modal('hide');//cierra ventana de mensajes
        }

        function CloseManagement() {
            $('#myModalManagement').modal('hide'); //cierra ventana de mantenimiento
        }

        $(document).ready(function () { //filtrar el datagridview
            $("#myInput").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#MainContent_gvAccount tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });

    </script>

    <h1>Account Management</h1>
    <input
        id="myInput"
        placeholder="Search"
        class="form-control"
        type="text" />
    <asp:GridView
        ID="gvAccount"
        runat="server"
        AutoGenerateColumns="false"
        CssClass="table table-sm"
        HeaderStyle-CssClass="thead-dark"
        HeaderStyle-BackColor="#204969"
        HeaderStyle-ForeColor="#FFF7F7"
        AlternatingRowStyle-BackColor="#DADADA"
        OnRowCommand="gvAccount_RowCommand"
        >

        <Columns>

            <asp:BoundField
                HeaderText="Id"
                DataField="Id" />
            <asp:BoundField
                HeaderText="UserId"
                DataField="UserId" />
            <asp:BoundField
                HeaderText="CurrencyId"
                DataField="CurrencyId" />
            <asp:BoundField
                HeaderText="CardId"
                DataField="CardId" />
            <asp:BoundField
                HeaderText="Description"
                DataField="Description" />
            <asp:BoundField
                HeaderText="IBAN"
                DataField="IBAN" />
            <asp:BoundField
                HeaderText="Balance"
                DataField="Balance" />
            <asp:BoundField
                HeaderText="PhoneNumber"
                DataField="PhoneNumber" />
            <asp:BoundField
                HeaderText="Status"
                DataField="Status" />
            <asp:ButtonField
                HeaderText="Edit"
                CommandName="editAccount"
                ControlStyle-CssClass="btn btn-primary"
                ButtonType="Button"
                Text="Edit" />
            <asp:ButtonField
                HeaderText="Remove"
                CommandName="removeAccount"
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
        ForeColor="Red"
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
                                    Text="UserId"
                                    runat="server" />
                            </td>
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
                                    Text="CurrencyId"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList
                                    ID="ddlCurrency"
                                    CssClass="form-control"
                                    runat="server">
                                </asp:DropDownList>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    Text="CardId"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList
                                    ID="ddlCard"
                                    CssClass="form-control"
                                    runat="server">
                                </asp:DropDownList>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrDescription"
                                    Text="Description"
                                    runat="server" />
                                </td>
                            <td>
                                <asp:TextBox
                                    ID="txtDescription"
                                    runat="server"
                                    CssClass="form-control" />
                                <asp:RequiredFieldValidator 
                                    ID="RequiredFieldValidator1" 
                                    runat="server"
                                    ForeColor="Red"
                                    ErrorMessage="Description is required"
                                    ControlToValidate="txtDescription" 
                                    EnableClientScript="False"
                                    ></asp:RequiredFieldValidator>   
                                </td>

                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrIban"
                                    Text="IBAN"
                                    runat="server" />
                            </td>
                            <td>

                                <asp:TextBox
                                    ID="txtIban"
                                    MaxLength="22"
                                    runat="server"
                                    CssClass="form-control" />
                                <asp:RequiredFieldValidator 
                                    ID="RequiredFieldValidator2" 
                                    runat="server"
                                    ForeColor="Red"
                                    ErrorMessage="IBAN is required"
                                    ControlToValidate="txtIban" 
                                    EnableClientScript="False"
                                    ></asp:RequiredFieldValidator>   
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrBalance"
                                    Text="Balance"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox
                                    ID="txtBalance"
                                    runat="server"
                                    CssClass="form-control" />
                                <asp:RequiredFieldValidator 
                                    ID="RequiredFieldValidator3" 
                                    runat="server"
                                    ForeColor="Red"
                                    ErrorMessage="Balance is required"
                                    ControlToValidate="txtBalance" 
                                    EnableClientScript="False"
                                    ></asp:RequiredFieldValidator>   
                                <asp:RegularExpressionValidator
                                    runat="server"
                                    ID="RegularExpressionValidator"
                                    ControlToValidate="txtBalance"
                                    ValidationExpression="^\d+$"
                                    EnableClientScript="true"
                                    ErrorMessage="Please enter numbers only"
                                    Display="Dynamic"
                                    SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrPhoneNumber"
                                    Text="Phone Number"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox
                                    ID="txtPhoneNumber"
                                    runat="server"
                                    MaxLength="8"
                                    CssClass="form-control"/>
                                <asp:RequiredFieldValidator 
                                    ID="RequiredFieldValidator4" 
                                    runat="server"
                                    ForeColor="Red"
                                    ErrorMessage="Phone number is required"
                                    ControlToValidate="txtPhoneNumber" 
                                    EnableClientScript="False"
                                    ></asp:RequiredFieldValidator> 
                                <asp:RegularExpressionValidator
                                    runat="server"
                                    ID="RegularExpressionValidator1"
                                    ControlToValidate="txtPhoneNumber"
                                    ValidationExpression="^\d+$"
                                    EnableClientScript="true"
                                    ErrorMessage="Please enter numbers only"
                                    Display="Dynamic"
                                    SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
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
                        Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Continue" />
                </div>
            </div>
        </div>
    </div>









</asp:Content>
