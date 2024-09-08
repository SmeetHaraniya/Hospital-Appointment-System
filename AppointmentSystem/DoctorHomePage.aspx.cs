using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppointmentSystem
{
    public partial class DoctorHomePage : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlCommandBuilder builder;
        SqlDataAdapter adAppointments;
        SqlDataAdapter adDoctors;
        SqlDataAdapter adPrescriptions;
        SqlDataAdapter adPatients;
        DataSet ds;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["dUsername"] == null && Session["dPassword"] == null)
            {
                Response.Redirect("~/Signup.aspx");
            }
            if (!IsPostBack)
            {
                PrescriptionPanel.Visible = false;
                AppointmentPanel.Visible = false;
            }
        }

        void db_init()
        {
            con = new SqlConnection();
            con.ConnectionString = WebConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;

            adAppointments = new SqlDataAdapter("SELECT * FROM Appointments", con);
            adDoctors = new SqlDataAdapter("SELECT * FROM Doctors", con);
            adPrescriptions = new SqlDataAdapter("SELECT * FROM Prescriptions", con);
            adPatients = new SqlDataAdapter("SELECT * FROM Patients", con);

            ds = new DataSet();
        }

        protected void btnAppointment_Click(object sender, EventArgs e)
        {
            prescriptionModal.Style["display"] = "none";

            AppointmentPanel.Visible = true;
            db_init();

            string Username = Session["dUsername"].ToString();
            string Password = Session["dPassword"].ToString();
            //debug
            //string Username = "Nidhi1662";
            //string Password = "Nidhi@1662";

            con.Open();
            adDoctors.Fill(ds, "Doctors");
            adAppointments.Fill(ds, "Appointments");
            adPatients.Fill(ds, "Patients");
            con.Close();

            string joinQuery = $"SELECT p.PatientId, a.AppointmentId, p.FirstName, p.LastName, p.Gender, p.Email, p.ContactNumber, CONVERT(VARCHAR(10), a.AppointmentDate, 120) as Date, CONVERT(VARCHAR(8), a.AppointmentTime, 108) as Time, a.Status FROM Appointments a JOIN Patients p ON a.PatientID = p.PatientID JOIN Doctors d ON a.DoctorID = d.DoctorID WHERE d.Username='{Username}' AND d.Password='{Password}'";
            cmd = new SqlCommand(joinQuery, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                Response.Write("No Data");

            }
            else
            {
                gvAppointment.DataSource = dr;
                gvAppointment.DataBind();
                dr.Close();

            }
            con.Close();

        }

        protected void btnPrescription_Click(object sender, EventArgs e)
        {
            prescriptionModal.Style["display"] = "none";

            AppointmentPanel.Visible = false;
            PrescriptionPanel.Visible = true;
            db_init();

            string Username = Session["dUsername"].ToString();
            string Password = Session["dPassword"].ToString();
            //debug
            //string Username = "Nidhi1662";
            //string Password = "Nidhi@1662";

            con.Open();
            adDoctors.Fill(ds, "Doctors");
            adAppointments.Fill(ds, "Appointments");
            adPatients.Fill(ds, "Patients");
            adPrescriptions.Fill(ds, "Prescriptions");
            con.Close();

            string joinQuery = $"SELECT p.PatientId, a.AppointmentId, p.FirstName, p.LastName, CONVERT(VARCHAR(10), a.AppointmentDate, 120) as Date, CONVERT(VARCHAR(8), a.AppointmentTime, 108) as Time, pr.Disease, pr.Allergies, pr.Prescription FROM Appointments a JOIN Patients p ON a.PatientID = p.PatientID JOIN Doctors d ON a.DoctorID = d.DoctorID JOIN Prescriptions pr ON pr.AppointmentId = a.AppointmentId WHERE d.Username='{Username}'";
            cmd = new SqlCommand(joinQuery, con);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                Response.Write("No Data");

            }
            else
            {
                gvPrescription.DataSource = dr;
                gvPrescription.DataBind();
                dr.Close();

            }
            con.Close();
        }

        

        protected void gvAppointment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CancelAppointment")
            {
                db_init();
                int appointmentID = Convert.ToInt32(e.CommandArgument);

                // Perform the cancellation appointment...
                string cancelQuery = "UPDATE Appointments SET Status = 'Cancelled' WHERE AppointmentID = @AppointmentID";
                cmd = new SqlCommand(cancelQuery, con);
                cmd.Parameters.AddWithValue("@AppointmentID", appointmentID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                // Re-bind the GridView to reflect the changes
                btnAppointment_Click(sender, e);

            }

            if (e.CommandName == "PrescribeAppointment")
            {
                int appointmentID = Convert.ToInt32(e.CommandArgument);
                Session["PresForAppoId"] = appointmentID;
                //Response.Redirect("~/Prescription.aspx");
                prescriptionModal.Style["display"] = "flex";
            }
        }


        protected void btnSubmitPrescription_Click(object sender, EventArgs e)
        {
            db_init();

            con.Open();
            adPrescriptions.Fill(ds, "Prescriptions");
            adAppointments.Fill(ds, "Appointments");
            con.Close();

            int appointmentId = Convert.ToInt32(Session["PresForAppoId"].ToString());
            string Disease = txtDisease.Text;
            string Allergies = txtAllergies.Text;
            string Prescription = txtPrescription.Text;

            DataTable dt = ds.Tables["Prescriptions"];
            DataRow dr = dt.NewRow();
            dr["AppointmentId"] = appointmentId;
            dr["Disease"] = Disease;
            dr["Allergies"] = Allergies;
            dr["Prescription"] = Prescription;
            builder = new SqlCommandBuilder(adPrescriptions);
            dt.Rows.Add(dr);
            adPrescriptions.Update(ds, "Prescriptions");

            string updateQuery = $"UPDATE Appointments SET Status='Done' WHERE AppointmentId='{appointmentId}'";
            cmd = new SqlCommand(updateQuery, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();


            txtDisease.Text = string.Empty;
            txtAllergies.Text = string.Empty;
            txtPrescription.Text = string.Empty;

            //Response.Redirect("~/DoctorHomePage.aspx");

            // Re-bind the GridView to reflect the changes
            btnAppointment_Click(sender, e);
        }
    }
}
