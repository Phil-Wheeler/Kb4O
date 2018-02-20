using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using Kb4o.Keybase;
using Kb4o.Api;

namespace Kb4o
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            KeybaseWebService webService = new KeybaseWebService();
            webService.Login("philwheeler", "'\"B ^ HeJ'SfKgIC*6m\"Wn");
            //Login login = new Login();
            //Salt salt = login.GetSalt("philwheeler");

            //login.GetUser(salt, "mypassword");
            //login.Invoke();

            //KbServiceBase kbbase = new KbServiceBase() { Endpoint = "login.json", Method = "POST" };
            //KbServiceBase kbbase = new KbServiceBase() { Endpoint = "user/lookup.json" };
            //kbbase.Params.Add("usernames", "philwheeler");
            //kbbase.Endpoint += "?usernames=philwheeler";

            //kbbase.Invoke();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Note: Outlook no longer raises this event. If you have code that 
            //    must run when Outlook shuts down, see http://go.microsoft.com/fwlink/?LinkId=506785
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
