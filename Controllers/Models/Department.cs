using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AcmeCo_CRUD.Controllers
{
    
    [BsonIgnoreExtraElements]
    public class Department
    {
        private ObjectId Id { get; set; }
        public int DepartmentId {get; set;}
        public string DepartmentName { get; set; }
    }
}                                                           