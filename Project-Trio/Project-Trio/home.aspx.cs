using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace Project_Trio
{
    public partial class Home : Page
    {
        protected global::System.Web.UI.WebControls.Literal litGenderInitial;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                LoadUserDetails();
            }
        }
        protected void lnkUse_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Use.aspx");
        }

        protected void btnUse_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Use.aspx");
        }
        private void LoadUserDetails()
        {
            int userId = Convert.ToInt32(Session["UserId"]);

            string connStr = ConfigurationManager.ConnectionStrings["WebDbConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Get Username from UserActivity (latest) or fallback to UserCredentials Email if needed
                string query = @"
                    SELECT TOP 1 ua.Username, uc.Gender
                    FROM UserActivity ua
                    INNER JOIN UserCredentials uc ON ua.UserId = uc.Id
                    WHERE ua.UserId = @UserId
                    ORDER BY ua.VisitTime DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string username = reader["Username"]?.ToString() ?? "User";
                    string gender = reader["Gender"]?.ToString() ?? "Unknown";

                    litUserName.Text = username;

                    if (gender.Equals("Male", StringComparison.OrdinalIgnoreCase))
                    {
                        imgProfileIcon.ImageUrl = "~/images/Male.png";
                        imgProfileIcon.AlternateText = "Male Profile Icon";
                        litGenderInitial.Text = "M";
                    }
                    else if (gender.Equals("Female", StringComparison.OrdinalIgnoreCase))
                    {
                        imgProfileIcon.ImageUrl = "~/images/Female.png";
                        imgProfileIcon.AlternateText = "Female Profile Icon";
                        litGenderInitial.Text = "F";
                    }
                    else
                    {
                        imgProfileIcon.ImageUrl = "~/images/default.png";
                        imgProfileIcon.AlternateText = "Default Profile Icon";
                        litGenderInitial.Text = "?";
                    }
                }
                else
                {
                    // No activity found — fallback: get Gender from UserCredentials only
                    reader.Close();

                    cmd.CommandText = "SELECT Gender FROM UserCredentials WHERE Id = @UserId";
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string gender = reader["Gender"]?.ToString() ?? "Unknown";
                        litUserName.Text = "User";

                        if (gender.Equals("Male", StringComparison.OrdinalIgnoreCase))
                        {
                            imgProfileIcon.ImageUrl = "~/images/Male.png";
                            imgProfileIcon.AlternateText = "Male Profile Icon";
                            litGenderInitial.Text = "M";
                        }
                        else if (gender.Equals("Female", StringComparison.OrdinalIgnoreCase))
                        {
                            imgProfileIcon.ImageUrl = "~/images/Female.png";
                            imgProfileIcon.AlternateText = "Female Profile Icon";
                            litGenderInitial.Text = "F";
                        }
                        else
                        {
                            imgProfileIcon.ImageUrl = "~/images/default.png";
                            imgProfileIcon.AlternateText = "Default Profile Icon";
                            litGenderInitial.Text = "?";
                        }
                    }
                }

                reader.Close();
            }
        }

        protected void btnAdmin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx");
        }

        protected void btnNormalUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProfilePage.aspx");
        }
    }
}
