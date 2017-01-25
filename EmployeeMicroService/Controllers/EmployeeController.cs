using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using EmployeeMicroService.Models;

namespace EmployeeMicroService.Controllers
{
    public class EmployeeController : ApiController
    {
        private IList<Employee> _employees;

        public EmployeeController(IList<Employee> employees)
        {
            _employees = employees;
        }
        
        [Route("employees")]
        [HttpGet]
        public HttpResponseMessage GetEmployees()
        {
            //Using HttpResponseMessage gives finer control over the HTTPResponse so you can set caching or return an HTTP error code
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, _employees);

            response.Headers.CacheControl = new CacheControlHeaderValue(){
                MaxAge = TimeSpan.FromMinutes(1)
            };

            return response;           

        }

        [Route("managers")]
        [HttpGet]
        public HttpResponseMessage GetManagers()
        {
            var managers =
                from mgr in _employees
                where (mgr.IsManager == true)
                select mgr;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, managers);

            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromMinutes(1)
            };

            return response;

        }

        [Route("employee/manager")]
        public HttpResponseMessage GetApprovingManager(Int32 EmployeeID)
        {
            HttpResponseMessage response = null;
           
            try
            {
                var employee =
                    (from emp in _employees
                     where (emp.ID == EmployeeID)
                     select emp).FirstOrDefault();

                if (employee != null)
                {
                    if (employee.IsManager)
                        response = Request.CreateResponse(HttpStatusCode.OK, employee);
                    else
                    {
                        var mgr =
                            (from emp in _employees
                             where (emp.ID == employee.ID - 2)
                             select emp).FirstOrDefault();

                        if (mgr != null)
                            response = Request.CreateResponse(HttpStatusCode.OK, mgr);
                        else                        
                            response = Request.CreateResponse(HttpStatusCode.NotFound);

                    }
                }

                return response;
                
            }

            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound,ex.ToString());
                
                return response;
            }            
        }
    }
}
