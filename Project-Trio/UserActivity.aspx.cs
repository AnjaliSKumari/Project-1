using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project_Trio
{
    public partial class UserActivity : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["UserConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Only show regular users (RoleId = 1), exclude admin users (RoleId = 2)
                string sql = "SELECT Id, Username, Email, Gender, Password FROM UserDetails WHERE RoleId = 1";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Add masked password column for display
                dt.Columns.Add("EncryptedPassword", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    string pwd = row["Password"].ToString();
                    row["EncryptedPassword"] = MaskPassword(pwd);
                }

                gvUsers.DataSource = dt;
                gvUsers.DataBind();
            }
        }

        private string MaskPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return "";

            if (password.Length <= 4)
                return new string('*', password.Length);

            return password.Substring(0, 2) + new string('*', password.Length - 4) + password.Substring(password.Length - 2);
        }

        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            BindGrid();
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvUsers.Rows[e.RowIndex];
            int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

            string newUsername = ((TextBox)row.FindControl("txtUsername")).Text.Trim();
            string newEmail = ((TextBox)row.FindControl("txtEmail")).Text.Trim();
            string newGender = ((DropDownList)row.FindControl("ddlGender")).SelectedValue;

            Label lblUsernameError = (Label)row.FindControl("lblUsernameError");
            Label lblEmailError = (Label)row.FindControl("lblEmailError");

            lblUsernameError.Visible = false;
            lblEmailError.Visible = false;

            // Validate Username uniqueness
            if (UsernameExists(newUsername, userId))
            {
                lblUsernameError.Text = "Username already exists.";
                lblUsernameError.Visible = true;
                return;
            }

            // Validate email format regex
            if (!Regex.IsMatch(newEmail, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                lblEmailError.Text = "Invalid email format.";
                lblEmailError.Visible = true;
                return;
            }

            // Validate email uniqueness
            if (EmailExists(newEmail, userId))
            {
                lblEmailError.Text = "Email already exists.";
                lblEmailError.Visible = true;
                return;
            }

            // Update DB
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "UPDATE UserDetails SET Username=@Username, Email=@Email, Gender=@Gender WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Username", newUsername);
                cmd.Parameters.AddWithValue("@Email", newEmail);
                cmd.Parameters.AddWithValue("@Gender", newGender);
                cmd.Parameters.AddWithValue("@Id", userId);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();

                if (rows > 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "User updated successfully.";
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Error updating user.";
                }
            }

            gvUsers.EditIndex = -1;
            BindGrid();
        }

        private bool UsernameExists(string username, int currentUserId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Only check among regular users (RoleId = 1), exclude admin users
                string sql = "SELECT COUNT(*) FROM UserDetails WHERE Username=@Username AND Id<>@Id AND RoleId = 1";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Id", currentUserId);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                conn.Close();
                return count > 0;
            }
        }

        private bool EmailExists(string email, int currentUserId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Only check among regular users (RoleId = 1), exclude admin users
                string sql = "SELECT COUNT(*) FROM UserDetails WHERE Email=@Email AND Id<>@Id AND RoleId = 1";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Id", currentUserId);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                conn.Close();
                return count > 0;
            }
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "DELETE FROM UserDetails WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", userId);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();

                if (rows > 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "User deleted successfully.";
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Error deleting user.";
                }
            }

            BindGrid();
        }

        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Highlight edited row
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    e.Row.CssClass = "edit-row";
                }
            }

            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddlGender = (DropDownList)e.Row.FindControl("ddlGender");
                if (ddlGender != null)
                {
                    string gender = DataBinder.Eval(e.Row.DataItem, "Gender").ToString();
                    ddlGender.SelectedValue = gender;
                }
            }
        }
    }
}