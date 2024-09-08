using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Configuration;

namespace AppointmentSystem
{
    public partial class PatientHomePage : System.Web.UI.Page
    {
        SqlConnection con;
        SqlDataAdapter adAppointments;
        SqlDataAdapter adPatients;
        SqlDataAdapter adDoctors;
        SqlCommand cmd;
        SqlCommandBuilder builder;
        DataSet ds;

        void initialize_doctorlist(string specialization)
        {
            db_init();

            adDoctors = new SqlDataAdapter($"Select * from Doctors Where Specialization='{specialization}'", con);

            con.Open();
            adDoctors.Fill(ds, "Doctors");
            con.Close();

            DataTable dt = ds.Tables["Doctors"];
            foreach (DataRow row in dt.Rows)
            {
                string name = row["FirstName"].ToString() + " " + row["LastName"].ToString();
                string fees = row["Fees"].ToString();
                ListItem item = new ListItem(name, fees);
                ddlDoctor.Items.Add(item);
            }
        }

        void initialize_specializationlist()
        { 
            db_init();

            adDoctors = new SqlDataAdapter("SELECT DISTINCT Specialization FROM DOCTORS", con);
            con.Open();
            adDoctors.Fill(ds, "SpecializationCategory");
            con.Close();

            DataTable dt = ds.Tables["SpecializationCategory"];
            foreach (DataRow row in dt.Rows)
            {
                ListItem item = new ListItem(row["Specialization"].ToString());
                ddlSpecialization.Items.Add(item);
            }

        }

        void initialize_timeslots(string doctor,DateTime date)
        {
            // Fetch booked time slots from the database
            List<string> bookedSlots = GetBookedTimeSlots(doctor, date);

            // Define all possible time slots (assuming 1-hour slots)
            List<string> allSlots = new TimeSlots().GetAvailableTimeSlots();

            // Filter available slots by excluding the booked ones
            List<string> availableSlots = allSlots.Except(bookedSlots).ToList();

            // Bind available slots to DropDownList
            ddlTimeSlots.Items.Clear();
            ddlTimeSlots.Items.Add(new ListItem("Select TimeSlot", "")); // Default option
            foreach (string slot in availableSlots)
            {
                ddlTimeSlots.Items.Add(new ListItem(slot, slot));
            }

            // Disable DropDownList if no available slots
            ddlTimeSlots.Enabled = availableSlots.Count > 0;
        }

        private List<string> GetBookedTimeSlots(string doctorName, DateTime date)
        {
            db_init();
            List<string> bookedSlots = new List<string>();

            string []name = doctorName.Split(' ');
            string FirstName = name[0];
            string LastName = name[1];

            // Assuming you have a method to get booked appointments from the database
            using (con)
            {
                string query = "SELECT AppointmentTime FROM Appointments a JOIN Doctors d ON d.DoctorId = a.DoctorId WHERE d.FirstName = @FirstName AND d.LastName = @LastName AND AppointmentDate = @AppointmentDate";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@FirstName", FirstName);
                cmd.Parameters.AddWithValue("@AppointmentDate", date);
                cmd.Parameters.AddWithValue("@LastName", LastName);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DateTime appointmentTime = Convert.ToDateTime(reader["AppointmentTime"]);
                    string bookedSlot = appointmentTime.ToString("hh:mm tt") + " - " + appointmentTime.AddHours(1).ToString("hh:mm tt");
                    bookedSlots.Add(bookedSlot);
                }
                con.Close();
            }

            return bookedSlots;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
               
            if (Session["patientsEmail"] == null && Session["patientPassword"] == null)
            {
                Response.Redirect("~/PatientsLogin.aspx");
            }
            if(!IsPostBack)
            {
                initialize_specializationlist();
                ddlTimeSlots.Enabled = false;
                txtDate.Enabled = false;
            }

        }

        void db_init()
        {
            con = new SqlConnection();
            con.ConnectionString = WebConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;

            adPatients = new SqlDataAdapter("select * from Patients",con);
            adDoctors = new SqlDataAdapter("select * from Doctors", con);
            adAppointments = new SqlDataAdapter("select * from Appointments", con);

            ds = new DataSet();
            //builder = new SqlCommandBuilder(adapter);
        }

        protected void SelectedSpecializationChanged(object sender, EventArgs e)
        {
            txtDate.Enabled = false;
            txtDate.Text = "";
            ddlTimeSlots.Enabled = false;
            ddlTimeSlots.Items.Clear();
            ddlTimeSlots.Items.Add(new ListItem("Select TimeSlot"));
            ddlTimeSlots.Enabled = false;   
            ddlDoctor.Items.Clear();
            ddlDoctor.Items.Add(new ListItem("Select Doctor"));
            initialize_doctorlist(ddlSpecialization.SelectedItem.Text);
        }

        protected void SelectedDoctorChanged(object sender, EventArgs e)
        {
            ddlTimeSlots.Items.Clear();
            ddlTimeSlots.Items.Add(new ListItem("Select TimeSlot"));
            ddlTimeSlots.Enabled = false;

            if (ddlDoctor.SelectedItem.Text == "Select Doctor")
            {
                txtDate.Enabled = false;
                txtDate.Text = "";
                txtFees.Text = "";
            }
            else
            {
                txtDate.Enabled = true;
                txtFees.Text = ddlDoctor.SelectedItem.Value;
            }
            
        }
        protected void SelectedDateChanged(object sender, EventArgs e)
        {
            ddlTimeSlots.Enabled = true;
            initialize_timeslots(ddlDoctor.SelectedItem.Text, DateTime.Parse(txtDate.Text));
        }


        protected void btnAppointmentsHistory_Click(object sender, EventArgs e)
        {
            AppointmentsHistoryPanel.Visible = true;
            CreateAppointmentPanel.Visible = false;

            db_init();

            string Email = Session["patientEmail"].ToString();
            string Password = Session["patientPassword"].ToString();

            //debug
            //string Email = "arpit@gmail.com";


            con.Open();
            adAppointments.Fill(ds, "Appointments");
            adDoctors.Fill(ds, "Doctors");
            adPatients.Fill(ds, "Patients");
            con.Close();

            DataTable dt = ds.Tables["Appointments"];

            string joinQuery = $"select a.AppointmentId, d.Username as Doctor_Name, d.Fees as Consultancy_Fees, CONVERT(VARCHAR(10), a.AppointmentDate, 120) as Date, CONVERT(VARCHAR(8), a.AppointmentTime, 108) as Time, a.Status FROM Appointments a JOIN Patients p ON a.PatientID = p.PatientID JOIN Doctors d ON a.DoctorID = d.DoctorID WHERE p.Email='{Email}' AND p.Password='{Password}'";

            cmd = new SqlCommand(joinQuery, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            GridView1.DataSource = dr;
            GridView1.DataBind();
            con.Close();
        }

        protected void btnBookAppointment_Click(object sender, EventArgs e)
        {
            CreateAppointmentPanel.Visible = true;
            AppointmentsHistoryPanel.Visible = false;
        }

        protected void btnCreateAppointment_Click(object sender, EventArgs e)
        {
            string DoctorFullName = ddlDoctor.SelectedItem.Text;
            string[] name = DoctorFullName.Split(' ');
            string DoctorFirstName = name[0];
            string DoctorLastName = name[1];

            string Email = Session["patientEmail"].ToString();
            string Password = Session["patientPassword"].ToString();

            //debug
            //string patientEmail = "arpit@gmail.com";

            DateTime date;
            DateTime.TryParse(txtDate.Text, out date);
            DateTime time = date;
            //DateTime.TryParse(txtTime.Text, out time);
            
            // Time range string
            string timeRange = ddlTimeSlots.SelectedItem.Text;

            // Split the time range string
            string[] timeParts = timeRange.Split(new string[] { " - " }, StringSplitOptions.None);

            // Extract start and end time strings
            string startTimeString = timeParts[0].Trim();
            string endTimeString = timeParts[1].Trim();

            DateTime startTime = DateTime.ParseExact(startTimeString, "hh:mm tt", CultureInfo.InvariantCulture);

            
            db_init();
            
            con.Open();
            adDoctors.Fill(ds, "Doctors");
            adAppointments.Fill(ds, "Appointments");
            adPatients.Fill(ds, "Patients");
            con.Close();

            DataTable dtDoctors = ds.Tables["Doctors"];
            DataView dvDoctors = new DataView(dtDoctors);
            dvDoctors.RowFilter = $"FirstName='{DoctorFirstName}' AND LastName='{DoctorLastName}'";

            int doctorId = int.Parse(dvDoctors[0].Row["DoctorId"].ToString());

            DataTable dtPatients = ds.Tables["Patients"];
            DataView dvPatients = new DataView(dtPatients);
            dvPatients.RowFilter = $"Email='{Email}' AND Password='{Password}'";

            int patientId = int.Parse(dvPatients[0].Row["PatientId"].ToString()) ;

            DataTable dtAppointments = ds.Tables["Appointments"];
            DataRow dr = dtAppointments.NewRow();
            builder = new SqlCommandBuilder(adAppointments); // Automatically generate the InsertCommand, UpdateCommand, and DeleteCommand
            dr["PatientId"] = patientId;
            dr["DoctorId"] = doctorId;
            dr["AppointmentDate"] = date;
            dr["AppointmentTime"] = startTime;
            dr["Status"] = "Active";
            dr["Action"] = 1;
            dtAppointments.Rows.Add(dr);
            adAppointments.Update(ds, "Appointments");

            // reset all input fields...
            ddlSpecialization.SelectedIndex = 0;
            txtDate.Text = string.Empty;    
            txtFees.Text = string.Empty;
            ddlDoctor.SelectedIndex = 0;
            ddlTimeSlots.Items.Clear();
            ddlTimeSlots.Items.Add(new ListItem("Select TimeSlot"));
            ddlTimeSlots.Enabled = false; 
            txtDate.Enabled = false;

        }

        protected void gvAppointment_CancleRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CancelAppointment")
            {
                db_init();
                int appointmentID = Convert.ToInt32(e.CommandArgument);

                // Perform the cancellation appointment...
                string cancelQuery = "UPDATE Appointments SET Status = 'Cancelled by you' WHERE AppointmentID = @AppointmentID";
                cmd = new SqlCommand(cancelQuery, con);
                cmd.Parameters.AddWithValue("@AppointmentID", appointmentID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            // Re-bind the GridView to reflect the changes
            btnAppointmentsHistory_Click(sender, e);
        }

    }
}