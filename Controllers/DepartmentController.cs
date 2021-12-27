using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AcmeCo_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            var dbList = dbClient.GetDatabase("AcmeCo").GetCollection<Department>("Department").AsQueryable();
            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Department department)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            int LastDepartmentId = dbClient.GetDatabase("AcmeCo").GetCollection<Department>("Department").AsQueryable().Count();
            department.DepartmentId = LastDepartmentId;
            dbClient.GetDatabase("AcmeCo").GetCollection<Department>("Department").InsertOne(department);
            return new JsonResult("Added successfully");
        }
        
        [HttpPut]
        public JsonResult Put(Department department)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            
            var filter = Builders<Department>.Filter.Eq("Department", department.DepartmentId);
            var update = Builders<Department>.Update.Set("DepartmentName",department.DepartmentName);
            
            dbClient.GetDatabase("AcmeCo").GetCollection<Department>("Department").UpdateOne(filter, update);
            return new JsonResult("Updated successfully");
        }
        
        
        [HttpDelete("{int}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            
            var filter = Builders<Department>.Filter.Eq("DepartmentId", id);
            
            
            dbClient.GetDatabase("AcmeCo").GetCollection<Department>("Department").DeleteOne(filter);
            return new JsonResult("Deleted successfully");
        }
    }
}