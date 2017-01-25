using Microsoft.Owin;
using Microsoft.Owin.Security.ActiveDirectory;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(EmployeeMicroService.Startup))]
namespace EmployeeMicroService
{

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureAuth(app);

            WebApiConfig.Register(config);

            app.UseWebApi(config);
        }

        private void ConfigureAuth(IAppBuilder app)
        {
            //any Api Controller added to this Web Api and decorated with [Authorize] attribute will only understand bearer tokens issued from this specified Active Directory 
            //Tenant and Application, any other form of tokens will be rejected and HTTP status code 401 will be returned
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    Audience = ConfigurationManager.AppSettings["Audience"],
                    Tenant = ConfigurationManager.AppSettings["Tenant"]
                });
        }
    }

}