using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppointmentSystem
{
    public partial class Signup : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        SqlCommandBuilder builder;
        DataSet ds;
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            if(!IsPostBack)
            DoctorAdminPanel.Visible = false;
        }

        void db_init(string table)
        {
            con = new SqlConnection();
            con.ConnectionString = WebConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;

            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from " + table;

            adapter = new SqlDataAdapter(cmd);
            ds = new DataSet();
            builder = new SqlCommandBuilder(adapter);
        }

        
       protected void IsEmailExist_Validator(object source, ServerValidateEventArgs args)
        {
            string Email = txtPEmail.Text.Trim();
            args.IsValid = !IsEmailExists(Email); 
        }

        protected bool IsEmailExists(string Email)
        {
            db_init("Patients");
            SqlCommand pcmd = new SqlCommand("Select count(*) from Patients where Email = @Email", con);
            pcmd.Parameters.AddWithValue("@Email", Email);

            con.Open();
            int rowCount = (int)pcmd.ExecuteScalar();  
            con.Close();

            return rowCount > 0;  
        }

        protected void IsContactExist_Validator(object source, ServerValidateEventArgs args)
        {
            string Contact = txtContact.Text.Trim();  
            args.IsValid = IsContactExists(Contact);        
        }

        protected bool IsContactExists(string ContactNumber)
        {
            string validNumberPattern = @"^[6-9]\d{9}$";
            if (!Regex.IsMatch(ContactNumber, validNumberPattern))
            {
                ContactNumberValidator.ErrorMessage = "Please enter a valid contact number!";
                return false;
            }

            db_init("Patients");
            SqlCommand pcmd = new SqlCommand("SELECT COUNT(*) FROM Patients WHERE ContactNumber = @ContactNumber", con);
            pcmd.Parameters.AddWithValue("@ContactNumber", ContactNumber);

            con.Open();
            int isExist = (int)pcmd.ExecuteScalar();  
            con.Close();

            if (isExist > 0)
            {
                ContactNumberValidator.ErrorMessage = "Contact number already exists!";
                return false;
            }

            return true; 
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string FirstName = txtFirstName.Text;
                string LastName = txtLastName.Text;
                string Email = txtPEmail.Text;
                string ContactNumber = txtContact.Text;
                DateTime DateOfBirth = Convert.ToDateTime(txtDob.Text);
                string Password = txtPPassword.Text;
                string Gender = ddlGender.SelectedItem.Text;

                /* Using SqlCommand...
                string insertQuery = "INSERT INTO Patients (FirstName, LastName, DateOfBirth, Gender, Contact, Email, Address) " + "VALUES (@FirstName, @LastName, @DateOfBirth, @Gender, @Contact, @Email, @Address)";

                cmd.Parameters.AddWithValue("@FirstName", FirstName);
                cmd.Parameters.AddWithValue("@LastName", LastName);
                cmd.Parameters.AddWithValue("@DateOfBirth", Dob);
                cmd.Parameters.AddWithValue("@Gender", Gender);
                cmd.Parameters.AddWithValue("@Contact", Contact);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Address", Address);
                */

                db_init("Patients");

                con.Open();
                adapter.Fill(ds, "Patients");
                con.Close();

                DataTable dt = ds.Tables["Patients"];
                DataRow dr = dt.NewRow();
                dr["FirstName"] = FirstName;
                dr["LastName"] = LastName;
                dr["Email"] = Email;
                dr["Password"] = Password;
                dr["ContactNumber"] = ContactNumber;
                dr["DateOfBirth"] = DateOfBirth;
                dr["Gender"] = Gender;

                dt.Rows.Add(dr);
                adapter.Update(ds, "Patients");

                Response.Redirect("~/PatientsLogin.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string Username = txtUsername.Text;
            string Password = txtPassword.Text;
            string table;

            if (rbRoleList.SelectedItem.Text == "Doctor")
            {
                table = "Doctors";
                Session["dUsername"] = Username;
                Session["dPassword"] = Password;
            }
            else
            {
                table = "Admin";
                Session["admin"] = Username;
            }
            
            if (!Regex.IsMatch(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"))
            {
                lblError.Text = "Password must be at least 8 characters long, contain an uppercase letter, a lowercase letter, a digit, and a special character.";
                return;
            }
            

            db_init(table);

            con.Open();
            adapter.Fill(ds, table);
            con.Close();

            DataTable dt = ds.Tables[table];
            DataView dv = new DataView(dt);
            dv.RowFilter = $"Username = '{Username}' AND Password = '{Password}'";

            if(dv.Count == 1)
            {
                if (rbRoleList.SelectedItem.Text == "Doctor")
                {
                    Response.Redirect("~/DoctorHomePage.aspx");
                }
                else
                {
                    Response.Redirect("~/AdminHomePage.aspx");
                }
            }
            else
            {
                lblError.Text = "Invalid Credential!!";
            }

        }

        protected void rbRoleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(rbRoleList.SelectedItem.Text == "Patient")
            {
                PatientPanel.Visible = true;
                DoctorAdminPanel.Visible = false;
            }
            else
            {
                PatientPanel.Visible = false;
                DoctorAdminPanel.Visible = true;
            }
        }

        
    }
}