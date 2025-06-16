using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;

namespace Project_Trio
{
    public partial class ProfilePage : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["WebDbConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // For testing: set UserId if session is empty (remove in production)
                if (Session["UserId"] == null)
                {
                    Session["UserId"] = 1; // Make sure user with Id=1 exists in DB
                }

                LoadUserData();
                SetReadOnlyState(true);
            }
        }

        private void LoadUserData()
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            int userId = Convert.ToInt32(Session["UserId"]);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Username, Email, Gender FROM UserCredentials WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", userId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtUsername.Text = reader["Username"]?.ToString() ?? "";
                    txtEmail.Text = reader["Email"]?.ToString() ?? "";
                    txtGender.Text = reader["Gender"]?.ToString() ?? "";

                    string gender = txtGender.Text.Trim().ToLower();

                    if (gender == "male")
                    {
                        litGenderInitial.Text = "M";
                        genderCircle.Style["background-color"] = "#007bff";  // Blue
                    }
                    else if (gender == "female")
                    {
                        litGenderInitial.Text = "F";
                        genderCircle.Style["background-color"] = "#e83e8c";  // Pink
                    }
                    else
                    {
                        litGenderInitial.Text = "?";
                        genderCircle.Style["background-color"] = "#6c757d";  // Gray
                    }
                }
                reader.Close();
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            SetReadOnlyState(false);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LoadUserData();
            SetReadOnlyState(true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            int userId = Convert.ToInt32(Session["UserId"]);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "UPDATE UserCredentials SET Username=@Username, Email=@Email, Gender=@Gender WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Gender", txtGender.Text.Trim());
                cmd.Parameters.AddWithValue("@Id", userId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            lblStatus.Text = "Profile updated successfully!";
            LoadUserData();
            SetReadOnlyState(true);
        }

        private void SetReadOnlyState(bool readOnly)
        {
            txtUsername.ReadOnly = readOnly;
            txtEmail.ReadOnly = readOnly;
            txtGender.ReadOnly = readOnly;

            btnEdit.Visible = readOnly;
            btnSave.Visible = !readOnly;
            btnCancel.Visible = !readOnly;
        }
    }
}
