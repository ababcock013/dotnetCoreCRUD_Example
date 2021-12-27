using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AcmeCo_CRUD.Controllers
{
    [BsonIgnoreExtraElements]
    public class Employee
    {
        private ObjectId Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        
        public string DateOfJoining { get; set; }          
        public string PhotoFileName { get; set; }
    }
}