using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AcmeCo_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            var dbList = dbClient.GetDatabase("AcmeCo").GetCollection<Employee>("Employee").AsQueryable();
            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            int LastEmployeeId = dbClient.GetDatabase("AcmeCo").GetCollection<Employee>("Employee").AsQueryable().Count();
            employee.EmployeeId = LastEmployeeId;
            dbClient.GetDatabase("AcmeCo").GetCollection<Employee>("Employee").InsertOne(employee);
            return new JsonResult("Added successfully");
        }
        
        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            
            var filter = Builders<Employee>.Filter.Eq("EmployeeId", employee.EmployeeId);
            var update = Builders<Employee>.Update.Set("EmployeeName", employee.EmployeeName)
                .Set("Department", employee.Department)
                .Set("DateOfJoining", employee.DateOfJoining)
                .Set("PhotoFileName", employee.PhotoFileName);
            
            dbClient.GetDatabase("AcmeCo").GetCollection<Employee>("Employee").UpdateOne(filter, update);
            return new JsonResult("Updated successfully");
        }
        
        
        [HttpDelete("{int}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            
            var filter = Builders<Employee>.Filter.Eq("EmployeeId", id);
            
            
            dbClient.GetDatabase("AcmeCo").GetCollection<Employee>("Employee").DeleteOne(filter);
            return new JsonResult("Deleted successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postFile = httpRequest.Files[0];
                string fileName = postFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }
    }
}