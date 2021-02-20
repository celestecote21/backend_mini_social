using MongoDB.Driver;
using System.Collections.Generic;
using netart.Models;


namespace netart.Services
{
    public class PostService
    {
        private readonly IMongoCollection<Post> _post;

        // Constructeur that get the data in the appsetting file to create the connection and get the db
        // use the interface I think
        public PostService(IPostDatabaseSetting settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _post = database.GetCollection<Post>(settings.PostCollectionName);
        }
        public List<Post> GetAll()
        {
            return _post.Find(post => true).ToList();
        }
        public List<Post> GetFrom(string username)
        {
            return _post.Find(post => post.Username == username).ToList();
        }
        public Post Get(string userName)
        {
            return _post.Find<Post>(post => post.Username == userName).FirstOrDefault();
        }
        public Post Create(Post post)
        {
            _post.InsertOne(post);
            return post;
        }
        public void Remove(string Id)
        {
            _post.DeleteOne(post => post.BdId == Id);
        }
    }
}