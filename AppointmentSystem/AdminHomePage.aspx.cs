using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;

namespace AppointmentSystem
{
    public partial class AdminHomePage : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adPatients;
        SqlDataAdapter adDoctors;
        SqlDataAdapter adAppointments;
        SqlDataAdapter adFeedbacks;
        SqlCommandBuilder builder;
        DataSet ds;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("~/Signup.aspx");
            }
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            AddDoctorPanel.Visible = false;
            AppointmentListPanel.Visible = false;
            DoctorListPanel.Visible = false;    
            FeedbackListPanel.Visible = false;
            PatientListPanel.Visible = false;
            DeleteDoctorPanel.Visible = false;
        }

        void db_init()
        {
            con = new SqlConnection();
            con.ConnectionString = WebConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;

            adPatients = new SqlDataAdapter("select * from Patients",con);
            adDoctors = new SqlDataAdapter("select * from Doctors", con);
            adAppointments = new SqlDataAdapter("select * from Appointments", con);
            adFeedbacks = new SqlDataAdapter("select * from Feedbacks", con);

            ds = new DataSet();
            //builder = new SqlCommandBuilder(adapter);
        }

        protected void btnDoctorList_Click(object sender, EventArgs e)
        {
            AddDoctorPanel.Visible = false;
            DoctorListPanel.Visible = true;
            db_init();

            con.Open();
            adDoctors.Fill(ds, "Doctors");
            con.Close();

            DataTable dt = ds.Tables["Doctors"];
            dt.Columns.Add("Doctor Name", typeof(string), "FirstName + ' ' + LastName");
            DataTable doctorsTable = dt.DefaultView.ToTable(false, "DoctorId" ,"Doctor Name", "Password", "Email","Fees");
            gvDoctor.DataSource = doctorsTable;
            gvDoctor.DataBind();

        }

        protected void btnPatientList_Click(object sender, EventArgs e)
        {
            AddDoctorPanel.Visible = false;
            PatientListPanel.Visible = true;
            db_init();

            con.Open();
            adPatients.Fill(ds, "Patients");
            con.Close();

            DataTable dt = ds.Tables["Patients"];
            DataTable patientsTable = dt.DefaultView.ToTable(false, "PatientId", "FirstName", "LastName", "Email", "ContactNumber","Password");
            gvPatient.DataSource = patientsTable;
            gvPatient.DataBind();
        }

        protected void btnAppointmentList_Click(object sender, EventArgs e)
        {
            AddDoctorPanel.Visible = false;
            AppointmentListPanel.Visible = true;
            db_init();

            con.Open();
            adPatients.Fill(ds, "Patients");
            adDoctors.Fill(ds, "Doctors");
            con.Close();

            // add two column more...
            string joinQuery = $"SELECT CONCAT(p.FirstName,' ',p.LastName) as PatientName, p.Email, p.ContactNumber, CONCAT(d.FirstName,' ',d.LastName) as DoctorName, d.Fees, CONVERT(VARCHAR(10), a.AppointmentDate, 120) as Date, CONVERT(VARCHAR(8), a.AppointmentTime, 108) as Time, a.Status as CurrentStatus FROM Appointments a JOIN Patients p ON a.PatientID = p.PatientID JOIN Doctors d ON a.DoctorID = d.DoctorID";
            cmd = new SqlCommand(joinQuery, con);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            gvAppointment.DataSource = dr;
            gvAppointment.DataBind();
            dr.Close();
            con.Close();
        }

        protected void btnFeedbackList_Click(object sender, EventArgs e)
        {
            AddDoctorPanel.Visible = false;
            FeedbackListPanel.Visible = true;
            db_init();

            con.Open();
            adFeedbacks.Fill(ds, "Feedbacks");
            adPatients.Fill(ds, "Patients");
            con.Close();

            string joinQuery = "SELECT CONCAT(p.FirstName,' ',p.LastName) as Username, p.Email, p.ContactNumber, f.Feedback from Feedbacks f JOIN Patients p ON f.Email=p.Email";
            cmd = new SqlCommand(joinQuery, con);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            gvFeedback.DataSource = dr;
            gvFeedback.DataBind();
            dr.Close();
            con.Close();
        }

        protected void btnAddDoctor_Click(object sender, EventArgs e)
        {
            AddDoctorPanel.Visible = true;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string Username = txtUsername.Text; 
            string FirstName = txtFirstName.Text;
            string LastName = txtLastName.Text;
            string Specialization = txtSpecialization.Text;
            string Password = txtPassword.Text;
            string Email = txtEmail.Text;
            string ContactNumber = txtContact.Text;
            string Fees = txtFees.Text;

            db_init();

            builder = new SqlCommandBuilder(adDoctors);
            con.Open();
            adDoctors.Fill(ds, "Doctors");
            con.Close();

            DataTable dt = ds.Tables["Doctors"];
            DataRow dr = dt.NewRow();
            dr["Username"] = Username;
            dr["FirstName"] = FirstName;
            dr["LastName"] = LastName;
            dr["Password"] = Password;
            dr["Specialization"] = Specialization;
            dr["ContactNumber"] = ContactNumber;
            dr["Availability"] = "Mon-Fri 09:00 a.m to 16:00 p.m";
            dr["Email"] = Email;
            dr["Fees"] = Fees;

            dt.Rows.Add(dr);
            adDoctors.Update(dt);
        }

        protected void btnDeleteDoctor_Click(object sender, EventArgs e)
        { 
            AddDoctorPanel.Visible = false;
            DeleteDoctorPanel.Visible = true;  
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            db_init();

            string Email = txtDEmail.Text;
            string Username = txtDUsername.Text;

            con.Open();
            adDoctors.Fill(ds, "Doctors");
            con.Close();

            builder = new SqlCommandBuilder(adDoctors);

            DataTable dt = ds.Tables["Doctors"];
            DataView dv = new DataView(dt);
            dv.RowFilter = $"Email='{Email}' AND Username='{Username}'";

            if(dv.Count == 1)
            {
                //DataRowView drv = dv[0];
                DataRow dr = dv[0].Row;
                dr.Delete();
            }
            adDoctors.Update(ds, "Doctors");
        }

        
    }
}