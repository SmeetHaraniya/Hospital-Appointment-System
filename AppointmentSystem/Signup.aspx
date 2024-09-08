
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="AppointmentSystem.Signup" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Signup</title>
    <style>
        body {
            font-family: Arial, Helvetica, sans-serif;
            display: flex;
            justify-content: center;
            align-items: center;
            height: auto; 
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

        #btnLogin, #btnRegister {
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

        #btnLogin:hover, #btnRegister:hover {
            background-color: #038a58; 
            opacity: 0.9; 
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
            align-items: center;
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
            <h2>SignUp Form</h2>
            <div>
                <asp:RadioButtonList ID="rbRoleList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rbRoleList_SelectedIndexChanged" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True">Patient</asp:ListItem>
                    <asp:ListItem>Doctor</asp:ListItem>
                    <asp:ListItem>Admin</asp:ListItem>
                </asp:RadioButtonList>
            </div>  
        </div>

        <asp:Panel ID="PatientPanel" runat="server">
            <div class="container">
                <label for="firstName"><b>First Name: </b></label>
                <asp:TextBox ID="txtFirstName" runat="server" placeholder="Enter First Name" required="required"></asp:TextBox>

                <label for="lastName"><b>
                    <br />
                    Last Name: </b></label>
                <asp:TextBox ID="txtLastName" runat="server" placeholder="Enter Last Name" required="required"></asp:TextBox>

                <label for="email"><b>
                    <br />
                    Email: </b></label>
                <asp:TextBox ID="txtPEmail" runat="server" placeholder="Enter Email" TextMode="Email" required="required"></asp:TextBox>
                <asp:CustomValidator ID="EmailVaidator" runat="server" ControlToValidate="txtPEmail" CssClass="validator-placeholder" ErrorMessage="Email is already exists!!" OnServerValidate="IsEmailExist_Validator"></asp:CustomValidator>
                <label for="contact"><b>
                    <br />
                    Contact Number: </b></label>
                <asp:TextBox ID="txtContact" runat="server" placeholder="Enter Contact Number" TextMode="Number" required="required"></asp:TextBox>
                <asp:CustomValidator ID="ContactNumberValidator" runat="server" ControlToValidate="txtContact" CssClass="validator-placeholder" OnServerValidate="IsContactExist_Validator"></asp:CustomValidator>
                <label for="dob">
                <br />
                <b>Date of Birth: </b>
                </label>
                <asp:TextBox ID="txtDob" runat="server" placeholder="Enter Date of Birth" required="required" TextMode="Date"></asp:TextBox>
                <label for="gender">
                <b>
                <br />
                Gender: </b>
                </label>
                <asp:DropDownList ID="ddlGender" runat="server" required="required">
                    <asp:ListItem Selected="True" Text="Select Gender" Value="" />
                    <asp:ListItem Text="Male" Value="Male" />
                    <asp:ListItem Text="Female" Value="Female" />
                    <asp:ListItem Text="Other" Value="Other" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="GenderValidator" runat="server" ControlToValidate="ddlGender" CssClass="validator-placeholder" ErrorMessage="Please select your gender" InitialValue="" />
                <label for="psw">
                <b>
                <br />
                Password: </b>
                </label>
                <asp:TextBox ID="txtPPassword" runat="server" placeholder="Enter Password" required="required" TextMode="Password"></asp:TextBox>
                <br />
                <asp:RegularExpressionValidator ID="PasswordValidator" runat="server" ControlToValidate="txtPPassword" ErrorMessage="Password must be at least 8 characters long, contain an uppercase letter, a lowercase letter, a digit, and a special character." CssClass="validator-placeholder" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&amp;])[A-Za-z\d@$!%*?&amp;]{8,}$" />

                <br />
                <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" />
                <span class="psw"><a href="PatientsLogin.aspx">Already have an account?</a></span>
            </div>    
        </asp:Panel>

        <asp:Panel ID="DoctorAdminPanel" runat="server">
            <div class="container">
                <label for="username"><b>
                    <br />
                    UserName: </b></label>
                <asp:TextBox ID="txtUsername" runat="server" placeholder="Enter Username"></asp:TextBox>
                <asp:RequiredFieldValidator ID="UsernameValidator" runat="server" ControlToValidate="txtUsername" ErrorMessage="Please enter a username" CssClass="validator-placeholder" />

                <label for="psw"><b>
                    <br />
                    Password: </b></label>
                <asp:TextBox ID="txtPassword" runat="server" placeholder="Enter Password" TextMode="Password"></asp:TextBox>

                <br />
                <asp:Label ID="lblError" runat="server" CssClass="validator-placeholder"></asp:Label>

                <br />
                <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Login" />
            </div>
        </asp:Panel>
    </form>
</body>
</html>


