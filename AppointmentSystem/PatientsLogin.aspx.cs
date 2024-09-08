using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.DynamicData;
using System.Reflection.Emit;

namespace AppointmentSystem
{
    public partial class login : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        SqlCommandBuilder builder;
        DataSet ds;

        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        }

        void db_init()
        {
            con = new SqlConnection();
            con.ConnectionString = WebConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;

            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from Patients";

            adapter = new SqlDataAdapter(cmd);
            ds = new DataSet();
            builder = new SqlCommandBuilder(adapter);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

            db_init();

            string Email = txtEmail.Text;
            string Password = txtPassword.Text;

            con.Open();
            adapter.Fill(ds, "Patients");
            con.Close();

            DataTable dt = ds.Tables["Patients"];
            DataView dv = new DataView(dt);
            dv.RowFilter = $"Email = '{Email}' AND Password = '{Password}'";

            if(!Regex.IsMatch(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"))
            {
                lblError.Text = "Password must be at least 8 characters long, contain an uppercase letter, a lowercase letter, a digit, and a special character.";
                return;
            }

            if (dv.Count == 1)
            {
                Session["patientEmail"] = Email;
                Session["patientPassword"] = Password;
                Response.Redirect("~/PatientHomePage.aspx");
            }
            else
            {
                lblError.Text = "Invalid Credentials!!";
            }
        }
    }
}