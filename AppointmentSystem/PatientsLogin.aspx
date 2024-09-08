<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PatientsLogin.aspx.cs" Inherits="AppointmentSystem.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <style>

         body {
             font-family: Arial, Helvetica, sans-serif;
             display: flex;
             justify-content: center;
             align-items: center;
             height: 100vh; 
             margin: 0; 
             background-color: #f4f4f4; 
         }

         form {
             border: 3px solid #f1f1f1;
             width: 500px;
             background-color: #fff; 
             border-radius: 8px;
             box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); 
         }

         h2 {
             text-align: center;
             color: #04AA6D;
             font-weight: bold;
         }

         input[type=text], input[type=password], input[type=email], input[type=number], input[type=date], select {
             width: 100%;
             padding: 12px 20px;
             margin: 8px 0;
             display: inline-block;
             border: 1px solid #ccc;
             box-sizing: border-box;
             border-radius: 4px; 
         }

         button {
             background-color: #04AA6D;
             color: white;
             padding: 14px 20px;
             margin: 8px 0;
             border: none;
             cursor: pointer;
             width: 100%;
             border-radius: 4px; 
         }

         button:hover {
             opacity: 0.8;
         }

         .container {
             padding: 16px;
         }

         .validator-placeholder {
            height: 18px; /* Adjust this height based on your preference */
            margin-top: -10px;
            margin-bottom: 8px;
            font-size: 12px;
            color: red;
        }

         span.psw {
             color: #04AA6D;
             display: flex;
            justify-content: center;
            align-items:center;
         }

         #btnLogin {
            background-color: #04AA6D; 
            color: white; 
            padding: 14px 20px; 
            margin: 8px 0; 
            border: none; 
            cursor: pointer; 
            width: 100%; 
            border-radius: 4px; 
            font-size: 16px; 
        }

        #btnLogin:hover {
            background-color: #038a58; 
            opacity: 0.9; 
        }

         @media screen and (max-width: 300px) {
             span.psw {
                 display: block;
                 float: none;
             }
             .cancelbtn {
                 width: 100%;
             }
         }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Login Form</h2>
            <label for="uname"><b>Email</b></label>
            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" placeholder="Enter Email"></asp:TextBox>
            <br />
            <asp:RequiredFieldValidator ID="EmailRequireValidator" runat="server" ControlToValidate="txtEmail" CssClass="validator-placeholder" ErrorMessage="Please enter email!!"></asp:RequiredFieldValidator>
            <br />
            <label for="psw"><b>Password</b></label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Enter Password"></asp:TextBox>
            <br />
            <asp:Label ID="lblError" runat="server" CssClass="validator-placeholder"></asp:Label>
            <br />
            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
            <span class="psw">
            <a href="#">Forgot password?</a></span>
        </div>
    </form>
    
</body>
</html>
