using MongoDB.Driver;
using coreFormValidation.Models;

namespace coreFormValidation.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<ToDoItem> _toDoCollection;
        private readonly IMongoCollection<Account> _accountCollection;

        public MongoDbService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            var database = client.GetDatabase("ToDoList"); 
            _toDoCollection = database.GetCollection<ToDoItem>("ToDoItems");
            _accountCollection = database.GetCollection<Account>("Accounts");
        }

        public List<ToDoItem> GetAll() => _toDoCollection.Find(_ => true).ToList();
        public ToDoItem GetById(string _id) => _toDoCollection.Find(i => i._id == _id).FirstOrDefault();
        public void Add(ToDoItem item) => _toDoCollection.InsertOne(item);
        public void Update(string _id, ToDoItem item) => _toDoCollection.ReplaceOne(i => i._id == _id, item);
        public void Delete(string _id) => _toDoCollection.DeleteOne(i => i._id == _id);
        
        public List<Account> GetAllAccounts() => _accountCollection.Find(_ => true).ToList();
        public Account GetAccountById(string _id) => _accountCollection.Find(i => i._id == _id).FirstOrDefault();
        public void AddAccount(Account account) => _accountCollection.InsertOne(account);
        public void UpdateAccount(string _id, Account account) => _accountCollection.ReplaceOne(i => i._id == _id, account);
        public void DeleteAccount(string _id) => _accountCollection.DeleteOne(i => i._id == _id);
        public Account GetAccountByEmail(string email) => _accountCollection.Find(i => i.eMail == email).FirstOrDefault();

    }
}
