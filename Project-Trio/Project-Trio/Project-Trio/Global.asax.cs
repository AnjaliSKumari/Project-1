using System;
using System.Web;

namespace Project_Trio
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Application startup code
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Session start code
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // Call the new method to track the exit time for the currently active page
            try
            {
                if (Session["UserId"] != null)
                {
                    ActivityTracker.TrackCurrentPageExitOnSessionEnd();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in Session_End: {ex.Message}");
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            // Application end code
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Global error handling
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                System.Diagnostics.Debug.WriteLine($"Application Error: {ex.Message}");
            }
        }
    }
}