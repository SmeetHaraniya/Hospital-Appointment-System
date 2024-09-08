<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DoctorHomePage.aspx.cs" Inherits="AppointmentSystem.DoctorHomePage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Doctor Home Page</title>
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

        .btn {
            display: block;
            width: 100%;
            padding: 10px;
            margin-bottom: 10px;
            background-color:  #04AA6D;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            text-align: center;
            font-size: 16px;
        }

        .btn:hover {
            background-color: #039d54;
        }

        .center-container {
            flex: 1;
            display: flex;
            flex-direction: column;
            overflow: hidden; /* Ensures scrollbars are managed by child elements */
        }

        .content {
            flex: 1;
            padding: 20px;
            background-color: #fff;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
            overflow: hidden; /* Hides scrollbars on the content itself */
        }


        .panel {
            padding: 20px;
            background-color: #f1f1f1;
            border-radius: 8px;
            margin-bottom: 20px;
            max-height: 100%; /* Ensures the panel takes up full height available */
            overflow: auto; /* Enables scrolling within the panel if content overflows */
            box-sizing: border-box; /* Includes padding and border in the element's total width and height */
        }

        .gridview {
            width: 100%;
            border-collapse: collapse;
            margin: 20px 0;
        }

        .gridview th, .gridview td {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: left;
        }

        .gridview th {
            background-color: #f2f2f2;
        }

         /* Modal styles */
        .modal {
            display: none; /* Hidden by default */
            position: fixed; 
            z-index: 1; /* Sits on top */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            background-color: rgba(0, 0, 0, 0.5); /* Black background with opacity */
            justify-content: center;
            align-items: center;
        }

        /* Modal content */
        .modal-content {
            background-color: #fff;
            padding: 20px;
            border-radius: 8px;
            width: 40%; /* Adjust modal width */
            box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.2);
        }

        .modal-content h2{
            text-align: center;
            background-color: #f1f1f1;
            padding: 10px;
            border-radius: inherit;
        }

        /* Close button */
        .close-btn {
            color: #aaa;
            float: right;
            font-size: 28px;
            font-weight: bold;
            cursor: pointer;
        }

        .close-btn:hover {
            color: #000;
        }

        /* Table styling */
        table {
            background-color: #f1f1f1;
            width: 100%;
            border-collapse: collapse;
            margin: 20px 0;
            border-radius: inherit;
        }

        table td {
            padding: 10px;
            vertical-align: middle;
        }

        table td:first-child {
            text-align: right;
            padding-right: 20px;
        }

        table td:last-child {
            text-align: left;
        }

        table input[type="text"] {
            width: 100%; 
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box; 
            font-size: 14px;
        }

        table input[type="text"]:focus {
            border-color: #4CAF50;
            outline: none;
        }

        table .btn {
            background-color: #4CAF50;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
        }

        table .btn:hover {
            background-color: #45a049;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="page-container">
            <div class="container">
                <div class="sidebar">
                    <asp:Button ID="btnAppointment" runat="server" OnClick="btnAppointment_Click" Text="Appointments" CssClass="nav-btn" />
                    <asp:Button ID="btnPrescription" runat="server" OnClick="btnPrescription_Click" Text="Prescription" CssClass="nav-btn" />
                </div>
                <div class="center-container">
                    <div class="content">
                            <asp:Panel ID="AppointmentPanel" runat="server" CssClass="panel">
                                <asp:GridView ID="gvAppointment" runat="server" CssClass="gridview" OnRowCommand="gvAppointment_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:Button ID="btnCancel" runat="server" CommandArgument='<%# Eval("AppointmentId") %>' CommandName="CancelAppointment" Text="Cancel" Visible='<%# Eval("Status").ToString() == "Active" %>' CssClass="btn" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Prescribe">
                                            <ItemTemplate>
                                                <asp:Button ID="btnPrescribe" runat="server" CommandArgument='<%# Eval("AppointmentId") %>' CommandName="PrescribeAppointment" Text="Prescribe" Visible='<%# Eval("Status").ToString() == "Active" %>' CssClass="btn" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                            <asp:Panel ID="PrescriptionPanel" runat="server" CssClass="panel">
                                <asp:GridView ID="gvPrescription" runat="server" CssClass="gridview">
                                </asp:GridView>
                            </asp:Panel>
                    </div>
                </div>
            </div>
        </div>

        <!-- Prescription Modal -->
        <div id="prescriptionModal" class="modal" runat="server">
            <div class="modal-content">
                <span class="close-btn" onclick="closeModal()">&times;</span><br />
                <h2>Prescription Form</h2>
                <table>
                    <tr>
                        <td><label for="disease"><b>Disease:</b></label></td>
                        <td><asp:TextBox ID="txtDisease" runat="server" placeholder="Enter disease"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><label for="allergies"><b>Allergies:</b></label></td>
                        <td><asp:TextBox ID="txtAllergies" runat="server" placeholder="Enter allergies"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><label for="prescription"><b>Prescription:</b></label></td>
                        <td><asp:TextBox ID="txtPrescription" runat="server" placeholder="Enter prescription"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Button ID="btnSubmitPrescription" runat="server" Text="Submit" CssClass="btn" OnClick="btnSubmitPrescription_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <script>
            function showModal() {
                document.getElementById("prescriptionModal").style.display = "flex";
            }

            function closeModal() {
                document.getElementById("prescriptionModal").style.display = "none";
            }
    </script>
    </form>
</body>
</html>