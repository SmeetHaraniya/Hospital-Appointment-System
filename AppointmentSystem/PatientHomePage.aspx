<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PatientHomePage.aspx.cs" Inherits="AppointmentSystem.PatientHomePage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Patient Home Page</title>
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
        width: 80%; /* Adjust width as needed */
        max-width: 1200px;
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

    .content {
        flex: 1;
        padding: 20px;
        background-color: #fff;
        box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
    }

    .panel {
        padding: 20px;
        background-color: #f1f1f1;
        border-radius: 8px;
    }

    #CreateAppointmentPanel {
    flex: 1;
}

    #AppointmentsHistoryPanel {
        max-height: 400px; /* Adjust this value to your desired maximum height */
        overflow-y: auto; /* Enables vertical scrolling */
        margin-top: 20px; /* Optional: adds space above the history panel */
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
        padding: 8px;
    }

    .form-cell label {
        display: inline-block;
        width: 150px;
        font-weight: bold;
    }

    .form-cell input, .form-cell select {
        width: calc(100% - 160px); /* Adjust width for label */
        padding: 10px;
        border: 1px solid #ccc;
        border-radius: 4px;
        box-sizing: border-box;
    }

    .btn {
        background-color: #04AA6D;
        color: white;
        padding: 10px 20px;
        border: none;
        border-radius: 4px;
        cursor: pointer;
    }

    .btn:hover {
        background-color: #039d54;
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
</style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="page-container">
            <div class="container">
                <div class="sidebar">
                    <asp:Button ID="btnBookAppointment" runat="server" OnClick="btnBookAppointment_Click" Text="Book Appointment" CssClass="nav-btn" />
                    <asp:Button ID="btnAppointmentsHistory" runat="server" OnClick="btnAppointmentsHistory_Click" Text="Appointments History" CssClass="nav-btn" />
                </div>
                <div class="content">
                    <asp:Label ID="lblPatientName" runat="server"></asp:Label>
                    <asp:Panel ID="CreateAppointmentPanel" runat="server" CssClass="panel">
                        <table>
                            <tr>
                                <td class="form-cell">
                                    <label for="Specialization">Specialization:</label>
                                    <asp:DropDownList ID="ddlSpecialization" runat="server" AutoPostBack="True" OnTextChanged="SelectedSpecializationChanged" CssClass="dropdown">
                                        <asp:ListItem Selected="True">Select Specialization</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="form-cell">
                                    <label for="doctorName">Doctor Name:</label>
                                    <asp:DropDownList ID="ddlDoctor" runat="server" AutoPostBack="True" OnTextChanged="SelectedDoctorChanged" CssClass="dropdown">
                                        <asp:ListItem Selected="True">Select Doctor</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="form-cell">
                                    <label for="fees">Consultancy Fees:</label>
                                    <asp:TextBox ID="txtFees" runat="server" ReadOnly="True" CssClass="textbox" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="form-cell">
                                    <label for="date">Date:</label>
                                    <asp:TextBox ID="txtDate" runat="server" AutoPostBack="True" CssClass="textBox" OnTextChanged="SelectedDateChanged" TextMode="Date"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <!--<td class="form-cell">
                                    <label for="time">Time:</label>
                                    <asp:TextBox ID="txtTime" runat="server" TextMode="Time" CssClass="textbox"></asp:TextBox>
                                </td>-->
                                <td class="form-cell">
                                        <label for="time">Time:</label>
                                        <asp:DropDownList ID="ddlTimeSlots" runat="server" CssClass="dropdown">
                                            <asp:ListItem Selected="True">Select TimeSlot</asp:ListItem>
                                        </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="form-cell">
                                    <asp:Button ID="CreateAppointment" runat="server" OnClick="btnCreateAppointment_Click" Text="Create" CssClass="btn" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="AppointmentsHistoryPanel" runat="server" CssClass="panel">
                        <asp:GridView ID="GridView1" runat="server" CssClass="gridview" OnRowCommand="gvAppointment_CancleRowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:Button 
                                            ID="btnCancel" 
                                            runat="server" 
                                            CommandName="CancelAppointment" 
                                            CommandArgument='<%# Eval("AppointmentId") %>' 
                                            Text="Cancel" 
                                            Visible='<%# Eval("Status").ToString() == "Active" %>' 
                                            CssClass="btn" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
