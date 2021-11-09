<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ADDLBankingApp.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ADDLBanking | Sign In</title>

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
           background: url("/Assets/Images/background.jpg");
            padding-top: 50px;
        }

        /* Modal Content/Box */
        .modal-content {
            background-color: #fefefe;
            margin: 5% auto 10% auto; /* 5% from the top, 15% from the bottom and centered */
            width: 30%; /* Could be more or less, depending on screen size */
            border-radius:15px;
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

        #txtPassword, #txtUsername,#btnSignIn {
            font-size: 16px;
        }
    </style>

</head>
<body>

    <div id="myModal" class="modal">

        <form class="modal-content animate" runat="server">
            <div class="imgcontainer">
                <img src="Assets/Images/addc-logo.png" />
            </div>
            <div class="container">
                <h1>Sign In</h1>
                <asp:TextBox
                    ID="txtUsername"
                    runat="server"
                    Placeholder="Enter username" />
                <asp:RequiredFieldValidator
                    ID="rfvUsername"
                    runat="server"
                    ErrorMessage="Username is required"
                    ControlToValidate="txtUsername"
                    ForeColor="Red" />
                <asp:TextBox
                    ID="txtPassword"
                    runat="server"
                    Placeholder="Enter password"
                    type="password" />
                <asp:RequiredFieldValidator
                    ID="rfvPassword"
                    runat="server"
                    ErrorMessage="Password is required"
                    ControlToValidate="txtPassword"
                    ForeColor="Red" />
                <br />
                <asp:Label
                    ID="lblStatus"
                    runat="server"
                    Text=""
                    ForeColor="Red" />
                <div class="container">
                    <asp:Button ID="btnSignIn" runat="server" Text="Sign in" CssClass="normalbtn" OnClick="btnSignIn_Click" />
                    <a>Don't have an account?
                        <asp:HyperLink runat="server" NavigateUrl="~/Register.aspx"> Register here</asp:HyperLink></a>
                </div>
            </div>
        </form>

    </div>

</body>
</html>
