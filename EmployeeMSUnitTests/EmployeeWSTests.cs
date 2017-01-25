using System;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Results;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployeeMicroService.Controllers;
using EmployeeMicroService.Models;
using System.Web.Http;

namespace EmployeeMSUnitTests
{
    [TestClass]
    public class EmployeeWSTests
    {
        private List<Employee> _employees = new List<Employee>(){new Employee(){FirstName="Peter", LastName="Worlin", Grade=5, ID=1, IsManager=true},
                                                        new Employee(){FirstName="Clare", LastName="Worlin", Grade=3, ID=2, IsManager=true},
                                                        new Employee{FirstName="Lauren", LastName="Worlin", Grade=2, ID=3, IsManager=false},
                                                        new Employee{FirstName="Sophie", LastName="Worlin", Grade=1, ID=4, IsManager=false}};

        EmployeeController controller;

        //Note this is unit testing the methods only - it is not testing routes etc
        [TestInitialize]
        public void Init()
        {
            //It was over twice as fast to have an initialize method as not
            controller = new EmployeeController(_employees);
            controller.Request = new HttpRequestMessage(); //Need this or the Request object is undefined in the Controller
            controller.Configuration = new HttpConfiguration(); //Need this otherwise the Request object in the Controller has no configuration
        }

        [TestMethod]
        public void TestGetAllEmployees()
        {
            //Act
            var response = controller.GetEmployees();
           
            //Assert - random change
            Assert.IsNotNull(response);

            var emps = response.Content.ReadAsAsync<IEnumerable<Employee>>().Result; //as IList<Employee>;
            Assert.AreEqual(4, emps.Count()); //Count is an extension method provided by System.Linq         
        }

        [TestMethod]
        public void TestGetAllManagers()
        {
            //Act
            var response = controller.GetManagers();

            //Assert
            Assert.IsNotNull(response);

            var mgrs = response.Content.ReadAsAsync<IEnumerable<Employee>>().Result;
            Assert.AreEqual(2, mgrs.Count());


            Assert.IsTrue(mgrs.ElementAt(0).IsManager);

            Assert.IsTrue(mgrs.ElementAt(1).IsManager);
        }

        [TestMethod]
        public void TestGetApprovingManager()
        {

            var response = controller.GetApprovingManager(3);

            Assert.IsNotNull(response);

            var mgr = response.Content.ReadAsAsync<Employee>().Result;
            Assert.AreEqual(1, mgr.ID);
            Assert.IsTrue(mgr.IsManager);
           
            
        }
    
    
    }
}
