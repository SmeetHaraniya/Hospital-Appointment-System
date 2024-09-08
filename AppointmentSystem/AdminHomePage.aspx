<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminHomePage.aspx.cs" Inherits="AppointmentSystem.AdminHomePage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Home Page</title>
    <style>
        body {
            font-family: Arial, Helvetica, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f9f9f9;
        }

        .page-container {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }

        .container {
            display: flex;
            width: 80%;
            max-width: 1200px;
            height: 80vh;
            overflow: hidden;
        }

        .sidebar {
            width: 250px;
            background-color: #2c3e50;
            padding: 20px;
            color: white;
            box-shadow: 2px 0 5px rgba(0, 0, 0, 0.1);
            flex-shrink: 0;
        }

        .nav-btn {
            display: block;
            width: 100%;
            padding: 10px;
            margin-bottom: 10px;
            background-color: #34495e;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            text-align: center;
            font-size: 16px;
        }

        .nav-btn:hover {
            background-color: #1abc9c;
        }

        .center-container {
            flex: 1;
            display: flex;
            flex-direction: column;
            overflow: hidden;
        }

        .content {
            flex: 1;
            padding: 20px;
            background-color: #fff;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
            overflow: hidden;
        }

        .panel {
            padding: 20px;
            background-color: #f1f1f1;
            border-radius: 8px;
            margin-bottom: 20px;
            max-height: 93%;
            overflow: auto;
        }

        table {
            width: 100%;
            border-collapse: collapse;
        }

        th, td {
            padding: 12px;
            border: 1px solid #ddd;
            text-align: left;
        }

        th {
            background-color: #f4f4f4;
        }

        .form-cell {
            padding: 10px;
        }

        .form-cell label {
            display: inline-block;
            width: 150px;
            font-weight: bold;
        }

        .form-cell input, .form-cell select, .form-cell textarea {
            width: calc(100% - 160px); /* Adjust width according to label width */
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
        }

        .table-btn {
            width: 100%;
            padding: 10px 20px;
            background-color: #04AA6D;
            color: white;
            font-size:larger;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            margin-top: 10px;
        }

        .table-btn:hover {
            background-color: #039d54;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="page-container">
            <div class="container">
                <!-- Sidebar -->
                <div class="sidebar">
                    <asp:Button ID="btnDoctorList" runat="server" OnClick="btnDoctorList_Click" Text="Doctor List" CssClass="nav-btn" />
                    <asp:Button ID="btnPatientList" runat="server" OnClick="btnPatientList_Click" Text="Patient List" CssClass="nav-btn" />
                    <asp:Button ID="btnAppointmentList" runat="server" OnClick="btnAppointmentList_Click" Text="Appointment List" CssClass="nav-btn" />
                    <asp:Button ID="btnFeedbackList" runat="server" OnClick="btnFeedbackList_Click" Text="Feedback List" CssClass="nav-btn" />
                    <asp:Button ID="btnAddDoctor" runat="server" OnClick="btnAddDoctor_Click" Text="Add Doctor" CssClass="nav-btn" />
                    <asp:Button ID="btnDeleteDoctor" runat="server" OnClick="btnDeleteDoctor_Click" Text="Delete Doctor" CssClass="nav-btn" />
                </div>

                <!-- Main Content -->
                <div class="center-container">
                    <div class="content">
                        <asp:Panel ID="DoctorListPanel" runat="server" CssClass="panel">
                            <asp:GridView ID="gvDoctor" runat="server" CssClass="gridview"></asp:GridView>
                        </asp:Panel>

                        <asp:Panel ID="PatientListPanel" runat="server" CssClass="panel">
                            <asp:GridView ID="gvPatient" runat="server" CssClass="gridview"></asp:GridView>
                        </asp:Panel>

                        <asp:Panel ID="AppointmentListPanel" runat="server" CssClass="panel">
                            <asp:GridView ID="gvAppointment" runat="server" CssClass="gridview"></asp:GridView>
                        </asp:Panel>

                        <asp:Panel ID="FeedbackListPanel" runat="server" CssClass="panel">
                            <asp:GridView ID="gvFeedback" runat="server" CssClass="gridview"></asp:GridView>
                        </asp:Panel>

                        <asp:Panel ID="AddDoctorPanel" runat="server" CssClass="panel">
                            <table>
                                <tr>
                                    <td class="form-cell">
                                        <label for="username">Username:</label>
                                        <asp:TextBox ID="txtUsername" runat="server" placeholder="Enter username"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell">
                                        <label for="fname">First Name:</label>
                                        <asp:TextBox ID="txtFirstName" runat="server" placeholder="Enter first name"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell">
                                        <label for="lname">Last Name:</label>
                                        <asp:TextBox ID="txtLastName" runat="server" placeholder="Enter last name"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell">
                                        <label for="specialization">Specialization:</label>
                                        <asp:TextBox ID="txtSpecialization" runat="server" placeholder="Enter specialization"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell">
                                        <label for="email">Email:</label>
                                        <asp:TextBox ID="txtEmail" runat="server" placeholder="Enter email" TextMode="Email"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell">
                                        <label for="contact">Contact Number:</label>
                                        <asp:TextBox ID="txtContact" runat="server" placeholder="Enter contact number" TextMode="Number"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell">
                                        <label for="psw">Password:</label>
                                        <asp:TextBox ID="txtPassword" runat="server" placeholder="Enter password" TextMode="Password"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell">
                                        <label for="cpsw">Confirm Password:</label>
                                        <asp:TextBox ID="txtCPassword" runat="server" placeholder="Enter password" TextMode="Password"></asp:TextBox>
                                        <asp:CompareValidator ID="PasswordCompareValidator" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtCPassword" Display="Dynamic" ErrorMessage="Passwords must match" ForeColor="#CC0000" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell">
                                        <label for="fees">Consultancy Fees:</label>
                                        <asp:TextBox ID="txtFees" runat="server" placeholder="Enter consultancy fees"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add Doctor" CssClass="table-btn" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                        <asp:Panel ID="DeleteDoctorPanel" runat="server" CssClass="panel">
                            <table>
                                <tr>
                                    <td class="form-cell">
                                        <label for="email">Email:</label>
                                        <asp:TextBox ID="txtDEmail" runat="server" placeholder="Enter email" TextMode="Email"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-cell">
                                        <label for="fname">Username:</label>
                                        <asp:TextBox ID="txtDUsername" runat="server" placeholder="Enter username"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" CssClass="table-btn" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
