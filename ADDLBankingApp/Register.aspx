<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="ADDLBankingApp.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign Up</title>

    <style>
        body {
            font-family: Arial, Helvetica, sans-serif;
        }

        * {
            box-sizing: border-box;
        }

        h1 {
            text-align: center;
            color: #204969;
        }

        /* Full-width input fields */
        input[type=text], input[type=password] {
            width: 100%;
            padding: 15px;
            margin: 5px 0 22px 0;
            display: inline-block;
            border: none;
            background: #f1f1f1;
        }

            /* Add a background color when the inputs get focus */
            input[type=text]:focus, input[type=password]:focus {
                background-color: #ddd;
                outline: none;
            }

        /* Set a style for all buttons */
        button {
            background-color: #04AA6D;
            color: white;
            padding: 14px 20px;
            margin: 8px 0;
            border: none;
            cursor: pointer;
            width: 100%;
            opacity: 0.9;
        }

            button:hover {
                opacity: 1;
            }

        /* Extra styles for the cancel button */
        .cancelbtn {
            background-color: gray;
            color: white;
            padding: 14px 20px;
            margin: 8px 0;
            border: none;
            cursor: pointer;
            width: 100%;
            opacity: 0.9;
        }

        .normalbtn {
            background-color: #204969;
            color: white;
            padding: 14px 20px;
            margin: 5px 0;
            border: none;
            cursor: pointer;
            width: 100%;
            opacity: 0.9;
        }

        button:hover {
            opacity: 1;
        }

        /* Add padding to container elements */
        .container {
            padding: 16px;
        }

        /* The Modal (background) */
        .modal {
            display: normal; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: #202124;
            padding-top: 50px;
        }

        /* Modal Content/Box */
        .modal-content {
            background-color: #fefefe;
            margin: 5% auto 10% auto; /* 5% from the top, 15% from the bottom and centered */
            border: 1px solid #888;
            width: 30%; /* Could be more or less, depending on screen size */
        }

        /* Style the horizontal ruler */
        hr {
            border: 1px solid #f1f1f1;
            margin-bottom: 25px;
        }

        /* The Close Button (x) */
        .close {
            position: absolute;
            right: 35px;
            top: 15px;
            font-size: 40px;
            font-weight: bold;
            color: #f1f1f1;
        }

        .imgcontainer {
            text-align: center;
            margin: 24px 0 12px 0;
            position: relative;
        }

        .close:hover,
        .close:focus {
            color: #f44336;
            cursor: pointer;
        }

        /* Clear floats */
        .clearfix::after {
            content: "";
            clear: both;
            display: table;
        }

        /* Change styles for cancel button and signup button on extra small screens */
        @media screen and (max-width: 300px) {
            .cancelbtn, .signupbtn {
                width: 100%;
            }
        }

        #txtPassword, #txtUsername {
            font-size: 16px;
        }
    </style>

</head>
<body>
        <div id="myModal">
            <form class="modal-content animate" runat="server">
                <div class="imgcontainer">
                    <img src="Assets/Images/addc-logo.png" />
                </div>
                <div class="container">
                    <div class="container">
                        <h1>Sign Up</h1>
                        <asp:TextBox ID="txtIdentification" Placeholder="Type in your identification" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvIdentificacion" runat="server"
                            ErrorMessage="La identificacion es requerida" ControlToValidate="txtIdentification" ForeColor="Maroon"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtName" Placeholder="Type in your Name" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server"
                            ErrorMessage="Name is required" ControlToValidate="txtName" ForeColor="Maroon"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtMiddleName" Placeholder="Type in your MiddleName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvMiddleName" runat="server"
                            ErrorMessage="Middle Name is required" ControlToValidate="txtMiddleName" ForeColor="Maroon"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtLastName" Placeholder="Type in your last name" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvLastName" runat="server"
                            ErrorMessage="El nombre es requerido" ControlToValidate="txtLastName" ForeColor="Maroon"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtEmail" Placeholder="Ingrese su correo electronico" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                            ErrorMessage="El correo electronico es requerido" ControlToValidate="txtEmail" ForeColor="Maroon"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtFechaNacimiento" Placeholder="Select birthdate" runat="server"></asp:TextBox>
                        <asp:Button ID="btnFechaNac" OnClick="btnFechaNac_Click" runat="server" Text="Seleccionar fecha" CausesValidation="false" />
                        <asp:Calendar ID="cldFechaNacimiento" OnSelectionChanged="cldFechaNacimiento_SelectionChanged" runat="server" Visible="false"></asp:Calendar>
                        <asp:RequiredFieldValidator ID="rfvFechaNac" runat="server" ForeColor="Maroon"
                            ErrorMessage="Birthdate required" ControlToValidate="txtFechaNacimiento"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtUsername" Placeholder="Type in your username" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ForeColor="Maroon"
                            ErrorMessage="Username is required" ControlToValidate="txtUsername"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtPassword" Placeholder="Type in your password" TextMode="Password" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ForeColor="Maroon"
                            ErrorMessage="El password es requerido" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtConfirmPassword" Placeholder="Confirm password" TextMode="Password" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvConfirmarPassword" runat="server" ForeColor="Maroon"
                            ErrorMessage="Password is required" ControlToValidate="txtConfirmPassword"></asp:RequiredFieldValidator>
                        <br />
                        <asp:CompareValidator ID="cvPassword" runat="server" ErrorMessage="Password must be the same"
                            ControlToValidate="txtPassword" ControlToCompare="txtConfirmPassword" ForeColor="Maroon"></asp:CompareValidator>
                        <asp:Label ID="lblStatus" runat="server" Text="" Visible="false" ForeColor="Maroon"></asp:Label>
                    </div>

                    <div class="container">
                        <asp:Button ID="btnAceptar" Text="Aceptar" CssClass="normalbtn" OnClick="btnAceptar_Click" runat="server" />
                        <input type="reset" value="Limpiar" class="cancelbtn" />
                    </div>
                </div>
            </form>
        </div>
</body>
</html>
