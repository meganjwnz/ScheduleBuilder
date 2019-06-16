using ScheduleBuilder.Controllers;
using ScheduleManager.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ScheduleBuilder
{
    public partial class Login : System.Web.UI.Page
    {
        private LoginController loginController;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.loginController = new LoginController();
            ErrorLabel.Visible = false;
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {

            DataTable dataTable = this.loginController.GetLogin(UsernameTextBox.Text, PasswordTextBox.Text);
            if (dataTable.Rows.Count > 0)
            {
                if ((string)dataTable.Rows[0]["roleTitle"] == "Employee")
                {
                    Response.Redirect("~/Home/Index");
                }
                else if ((string)dataTable.Rows[0]["roleTitle"] == "Manager")
                {
                    Response.Redirect("~/Home/Index");
                }
                else if ((string)dataTable.Rows[0]["roleTitle"] == "Administrator")
                {
                    Response.Redirect("~/Home/Index");
                }
            }
            else
            {
                ErrorLabel.Visible = true;
            }
        }
    }
}