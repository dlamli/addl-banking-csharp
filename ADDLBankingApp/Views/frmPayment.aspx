<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableViewState="true" CodeBehind="frmPayment.aspx.cs" Inherits="ADDLBankingApp.Views.frmPayment" %>
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

        function openModalMsg() {
            $('#myModalMsg').modal('show'); //ventana de mensajes
        }

        function CloseModalMsg() {
            $('#myModalMsg').modal('hide');//cierra ventana de mensajes
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

        <h1>Payment Management</h1>
    <input
        id="myInput"
        placeholder="Search"
        class="form-control"
        type="text" />

    <asp:GridView 
        ID="gvPayment" 
        runat="server"
        AutoGenerateColumns="false"
        CssClass="table table-sm"
        HeaderStyle-CssClass="thead-dark"
        HeaderStyle-BackColor="#204969"
        HeaderStyle-ForeColor="#FFF7F7"
        AlternatingRowStyle-BackColor="#DADADA"
        OnRowCommand="gvPayment_RowCommand">

        <Columns>

            <asp:BoundField 
                HeaderText="Id"
                DataField="Id"/>
            <asp:BoundField 
                HeaderText="ServiceId"
                DataField="ServiceId"/>
            <asp:BoundField 
                HeaderText="UserId"
                DataField="UserId"/>
            <asp:BoundField 
                HeaderText="Date"
                DataField="Date"/>
            <asp:BoundField 
                HeaderText="Amount"
                DataField="Mount"/>
            <asp:ButtonField 
                HeaderText="Edit"
                CommandName="editPayment"
                ControlStyle-CssClass="btn btn-primary"
                ButtonType="Button"
                Text="Edit"/>
            <asp:ButtonField 
                HeaderText="Remove"
                CommandName="removePayment"
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
        OnClick="btnNew_Click"
        CausesValidation="false"/>

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
                                    ID="ltrServiceId"
                                    Text="ServiceId"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList
                                    ID="ddlService"
                                    CssClass="form-control"
                                    runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrUserId"
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
                                    ID="ltrDate"
                                    Text="Date"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox
                                    ID="txtDate"
                                    runat="server"
                                    CssClass="form-control" 
                                    Enabled="false"/>
                                 <asp:RequiredFieldValidator 
                                    ID="rfvDate" 
                                    runat="server"
                                    ForeColor="Red"
                                    ControlToValidate="txtDate" 
                                    EnableClientScript="true"
                                    ErrorMessage="Date is required"
                                    Display="Dynamic"
                                    SetFocusOnError="True" 
                                    ></asp:RequiredFieldValidator>   

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
                    <asp:LinkButton type="button" CssClass="btn btn-success" ID="btnConfirmModal" OnClick="btnConfirmModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" CausesValidation="false"/>
                    <asp:LinkButton type="button" CssClass="btn btn-danger" ID="btnCancelModal" OnClick="btnCancelModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" CausesValidation="false"/>
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
                            ID="ltrPaymentMessage"
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
