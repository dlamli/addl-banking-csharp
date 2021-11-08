<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmMarchamo.aspx.cs" Inherits="ADDLBankingApp.Views.Marchamo" %>
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

        <h1>Marchamo Management</h1>
    <input
        id="myInput"
        placeholder="Search"
        class="form-control"
        type="text" />

    <asp:GridView 
        ID="gvMarchamo" 
        runat="server"
        AutoGenerateColumns="false"
        CssClass="table table-sm"
        HeaderStyle-CssClass="thead-dark"
        HeaderStyle-BackColor="#204969"
        HeaderStyle-ForeColor="#FFF7F7"
        AlternatingRowStyle-BackColor="#DADADA"
        OnRowCommand="gvMarchamo_RowCommand">

        <Columns>

            <asp:BoundField 
                HeaderText="Id"
                DataField="Id"/>
            <asp:BoundField 
                HeaderText="AccountId"
                DataField="AccountId"/>
            <asp:BoundField 
                HeaderText="Name"
                DataField="Name"/>
            <asp:BoundField 
                HeaderText="Amount"
                DataField="Amount"/>
            <asp:BoundField 
                HeaderText="NumberPlate"
                DataField="NumberPlate"/>
            <asp:BoundField 
                HeaderText="VehicleType"
                DataField="VehicleType"/>
            <asp:ButtonField 
                HeaderText="Edit"
                CommandName="editMarchamo"
                ControlStyle-CssClass="btn btn-primary"
                ButtonType="Button"
                Text="Edit"/>
            <asp:ButtonField 
                HeaderText="Remove"
                CommandName="removeMarchamo"
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
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrName"
                                    Text="Name"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox
                                    ID="txtName"
                                    runat="server"
                                    CssClass="form-control" />

                                <asp:RequiredFieldValidator 
                                    ID="rfvName" 
                                    runat="server"
                                    ErrorMessage="Name is required"
                                    ControlToValidate="txtName"
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
                                    ID="ltrNumberPlate"
                                    Text="Number Plate"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:TextBox
                                    ID="txtNumberPlate"
                                    runat="server"
                                    CssClass="form-control" />

                                <asp:RequiredFieldValidator 
                                    ID="rfvNumberPlate" 
                                    runat="server"
                                    ErrorMessage="Plate Number is required"
                                    ControlToValidate="txtNumberPlate"
                                    ForeColor="Maroon"
                                    />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal
                                    ID="ltrVehicleType"
                                    Text="Vehicle Type"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList
                                    ID="ddlVehicleType"
                                    CssClass="form-control"
                                    runat="server">
                                    <asp:ListItem Value="A1">A1</asp:ListItem>
                                    <asp:ListItem Value="A2">A2</asp:ListItem>
                                    <asp:ListItem Value="A3">A3</asp:ListItem>
                                    <asp:ListItem Value="B1">B1</asp:ListItem>
                                    <asp:ListItem Value="B2">B2</asp:ListItem>
                                    <asp:ListItem Value="B3">B3</asp:ListItem>
                                    <asp:ListItem Value="B4">B4</asp:ListItem>
                                    <asp:ListItem Value="C1">C1</asp:ListItem>
                                    <asp:ListItem Value="C2">C2</asp:ListItem>
                                    <asp:ListItem Value="C3">C3</asp:ListItem>
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
