using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace Project_Trio
{
    public partial class UserActivity : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["UserConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Set welcome username label
            if (Session["Username"] != null)
            {
                lblUsername.Text = Session["Username"].ToString();
            }
            else
            {
                lblUsername.Text = "User";
            }

            if (!IsPostBack)
            {
                LoadUsers();
            }
        }


        private void LoadUsers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Load all users except admins (RoleId != 2) and order by Id
                    string query = @"
                        SELECT Id, Username, Email, Gender, CreatedAt 
                        FROM UserDetails 
                        WHERE RoleId != 2 
                        ORDER BY Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    gvUsers.DataSource = dt;
                    gvUsers.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading users: " + ex.Message, true);
            }
        }
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            // Make sure to call ActivityTracker to mark the current page's exit before logging out
            ActivityTracker.TrackCurrentPageExitOnSessionEnd();

            Session.Clear();
            Session.Abandon();
            // Clear the session cookie as well
            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            Response.Redirect("Login.aspx"); // Redirect to your login page
        }

        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            LoadUsers();

            // Set the dropdown value for gender
            GridViewRow row = gvUsers.Rows[e.NewEditIndex];
            DropDownList ddlGender = row.FindControl("ddlGender") as DropDownList;

            if (ddlGender != null)
            {
                // Debugging: Check if Gender exists in DataKeys
                var genderValue = gvUsers.DataKeys[e.NewEditIndex]["Gender"];
                if (genderValue != null)
                {
                    string currentGender = genderValue.ToString();
                    Console.WriteLine("Gender found in DataKeys: " + currentGender);
                    if (!string.IsNullOrEmpty(currentGender))
                    {
                        ddlGender.SelectedValue = currentGender;
                    }
                    else
                    {
                        ddlGender.SelectedIndex = 0; // Set default value or leave empty
                    }
                }
                else
                {
                    // Log if Gender is missing from DataKeys
                    Console.WriteLine("Gender not found in DataKeys at index " + e.NewEditIndex);
                }
            }
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            LoadUsers();
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvUsers.Rows[e.RowIndex];
                int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

                // Get the updated values
                TextBox txtUsername = row.FindControl("txtUsername") as TextBox;
                TextBox txtEmail = row.FindControl("txtEmail") as TextBox;
                DropDownList ddlGender = row.FindControl("ddlGender") as DropDownList;

                // Get error labels
                Label lblUsernameError = row.FindControl("lblUsernameError") as Label;
                Label lblEmailError = row.FindControl("lblEmailError") as Label;

                bool hasError = false;

                // Validate username
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    lblUsernameError.Text = "Username is required.";
                    lblUsernameError.Visible = true;
                    hasError = true;
                }
                else
                {
                    // Check if username already exists (excluding current user)
                    if (IsUsernameExists(txtUsername.Text.Trim(), userId))
                    {
                        lblUsernameError.Text = "Username already exists.";
                        lblUsernameError.Visible = true;
                        hasError = true;
                    }
                    else
                    {
                        lblUsernameError.Visible = false;
                    }
                }

                // Validate email
                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    lblEmailError.Text = "Email is required.";
                    lblEmailError.Visible = true;
                    hasError = true;
                }
                else if (!IsValidEmail(txtEmail.Text.Trim()))
                {
                    lblEmailError.Text = "Invalid email format.";
                    lblEmailError.Visible = true;
                    hasError = true;
                }
                else
                {
                    // Check if email already exists (excluding current user)
                    if (IsEmailExists(txtEmail.Text.Trim(), userId))
                    {
                        lblEmailError.Text = "Email already exists.";
                        lblEmailError.Visible = true;
                        hasError = true;
                    }
                    else
                    {
                        lblEmailError.Visible = false;
                    }
                }

                if (hasError)
                {
                    return; // Don't proceed with update
                }

                // Update the user
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        UPDATE UserDetails 
                        SET Username = @Username, Email = @Email, Gender = @Gender 
                        WHERE Id = @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@Id", userId);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        gvUsers.EditIndex = -1;
                        LoadUsers();
                        ShowMessage("User updated successfully!", false);
                    }
                    else
                    {
                        ShowMessage("Error updating user.", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error updating user: " + ex.Message, true);
            }
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // First, delete related activity records
                    SqlCommand deleteActivityCmd = new SqlCommand(
                        "DELETE FROM UserActivityTracking WHERE UserId = @UserId", conn);
                    deleteActivityCmd.Parameters.AddWithValue("@UserId", userId);
                    deleteActivityCmd.ExecuteNonQuery();

                    // Then delete the user
                    SqlCommand deleteUserCmd = new SqlCommand(
                        "DELETE FROM UserDetails WHERE Id = @Id", conn);
                    deleteUserCmd.Parameters.AddWithValue("@Id", userId);

                    int result = deleteUserCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        LoadUsers();
                        ShowMessage("User deleted successfully!", false);
                    }
                    else
                    {
                        ShowMessage("Error deleting user.", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting user: " + ex.Message, true);
            }
        }

        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            LoadUsers();
        }

        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // To highlight the row in edit mode
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == gvUsers.EditIndex)
            {
                e.Row.CssClass = "edit-row";
            }
        }

        private bool IsUsernameExists(string username, int excludeUserId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM UserDetails WHERE Username = @Username AND Id != @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Id", excludeUserId);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private bool IsEmailExists(string email, int excludeUserId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM UserDetails WHERE Email = @Email AND Id != @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Id", excludeUserId);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Use simple regex for email validation
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern);
            }
            catch
            {
                return false;
            }
        }

        private void ShowMessage(string message, bool isError)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isError ? "alert alert-danger" : "alert alert-success";
            lblMessage.Visible = true;
        }
    }
}
