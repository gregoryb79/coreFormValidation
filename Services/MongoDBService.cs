using MongoDB.Driver;
using coreFormValidation.Models;

namespace coreFormValidation.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<ToDoItem> _toDoCollection;

        public MongoDbService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            var database = client.GetDatabase("ToDoList"); 
            _toDoCollection = database.GetCollection<ToDoItem>("ToDoItems");
        }

        public List<ToDoItem> GetAll() => _toDoCollection.Find(_ => true).ToList();
        public ToDoItem GetById(string _id) => _toDoCollection.Find(i => i._id == _id).FirstOrDefault();
        public void Add(ToDoItem item) => _toDoCollection.InsertOne(item);
        public void Update(string _id, ToDoItem item) => _toDoCollection.ReplaceOne(i => i._id == _id, item);
        public void Delete(string _id) => _toDoCollection.DeleteOne(i => i._id == _id);
        
    }
}
