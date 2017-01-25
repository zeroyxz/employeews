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

        //You need the async attribute on the button click because you are invoking 
        private async void button1_Click(object sender, EventArgs e)
        {
            authContext = new AuthenticationContext(authority);

            //UserCredential cred = new UserCredential() - this is obsolete and replaced with UserPasswordCredential
            //UserPasswordCredential cred = new UserPasswordCredential(); - this expects a usernam and password though which we dont know
            //ClientCredential cred = new ClientCredential();//This nexpects a client_id and client_secret - this is for a two-legged approach where we are not impersonating a user to make the request to the web api we are using the identity of the AAD registered application

            //The AcquireToken method is only within version 2.28.2 of the ADAL
            //To get the rght version using package manager:
            //--Get-Project APITester | Install-package Microsoft.IdentityModel.Clients.ActiveDirectory -Version 2.28.2
            AuthenticationResult authResult = authContext.AcquireToken(apiResourceId, clientId, redirectUri);

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);

            //HttpResponseMessage response = await client.GetAsync(apiBaseAddress + "employees");
            HttpResponseMessage response = await client.GetAsync(apiBaseAddress + "employees");


            string responseString = await response.Content.ReadAsStringAsync();

            MessageBox.Show(responseString);
        }
    }
}
