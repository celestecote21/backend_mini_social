using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using MongoDB.Driver;
using netart.Models;


namespace netart.Services
{
    public class PostService
    {
        private readonly IMongoCollection<Post> _posts;

        // Constructeur that get the data in the appsetting file to create the connection and get the db
        // use the interface I think
        public PostService(IPostDatabaseSetting settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _posts = database.GetCollection<Post>(settings.PostCollectionName);
        }
        public List<Post> GetAll()
        {
            return _posts.Find(post => true).ToList();
        }
        public List<Post> GetFrom(string username)
        {
            return _posts.Find(post => post.Username == username).ToList();
        }

        public async Task<List<Post>> GetLast(int nb)
        {
            return await _posts.Find(post => true).SortBy(post => post.CreationDate).Limit(nb).ToListAsync();
        }
        public Post Get(string userName)
        {
            return _posts.Find<Post>(post => post.Username == userName).FirstOrDefault();
        }
        public Post Create(Post post)
        {
            _posts.InsertOne(post);
            return post;
        }
        public void Remove(string Id)
        {
            _posts.DeleteOne(post => post.BdId == Id);
        }
    }
}