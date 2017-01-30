using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http;
using System.Net.Http.Headers;

namespace APITester
{
    public partial class Form1 : Form
    {
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        //Client Id is this application registered in AAD - you must give the app rights to access other AAD registered applications
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        Uri redirectUri = new Uri(ConfigurationManager.AppSettings["ida:RedirectUri"]);

        private static string authority = String.Format(aadInstance, tenant);

        private static string apiResourceId = ConfigurationManager.AppSettings["ApiResourceId"];
        private static string apiBaseAddress = ConfigurationManager.AppSettings["ApiBaseAddress"];

        private AuthenticationContext authContext = null;

        public Form1()
        {
            InitializeComponent();
        }

        //You need the async attribute on the button click because you are invoking async methods in the function where you are await-ing
        private async void button1_Click(object sender, EventArgs e)
        {
            //authority is made up of instance and tenant https://login.windows.net/peterworlinhotmail.onmicrosoft.com
            MessageBox.Show("authority=" + authority);

            authContext = new AuthenticationContext(authority);

            
            //The AcquireToken method is only within version 2.28.2 of the ADAL
            //To get the rght version using package manager:
            //--Get-Project APITester | Install-package Microsoft.IdentityModel.Clients.ActiveDirectory -Version 2.28.2

            //We request a token for the resource id - which is either dev, test or live version of https://peterworlinhotmail.onmicrosoft.com/employeewstest
            MessageBox.Show("apiResourceId=" + apiResourceId);
            MessageBox.Show("clientid=" + clientId);
            MessageBox.Show("redirectUri=" + redirectUri);
            AuthenticationResult authResult = authContext.AcquireToken(apiResourceId, clientId, redirectUri);

            HttpClient client = new HttpClient();

            //Add the autorisation token we recieve to the header of the HTTP request
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);

            //Make a call to the right base address for example https://employeews-test.azurewebsites.net/
            HttpResponseMessage response = await client.GetAsync(apiBaseAddress + "employees");

            //Read the result from the content
            string responseString = await response.Content.ReadAsStringAsync();

            //Display the content
            MessageBox.Show(responseString);
        }
    }
}
