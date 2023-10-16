using MongoDB.Bson;
using MongoDB.Driver;
using SirenaDataHarvesting.Models.EntityBase;
using SirenaDataHarvesting.Models.GeneralSettings;
using System.Linq.Expressions;

namespace SirenaDataHarvesting.Services.GenericRepository
{
    public class GenericRepository<TDocument> : IGenericRepository<TDocument> where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;
        private readonly IConfiguration _configuration;

        public GenericRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            string connectionString = _configuration["WebScrapingDatabase:ConnectionString"]!;
            string databaseName = _configuration["WebScrapingDatabase:DatabaseName"]!;
            IMongoDatabase database = new MongoClient(connectionString).GetDatabase(databaseName);

            _collection = database.GetCollection<TDocument>(GenericRepository<TDocument>.GetCollectionName(typeof(TDocument)));
        }

        private protected static string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).First()).CollectionName;
        }

        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual TDocument FindById(string id)
        {
            ObjectId objectId = new(id);
            FilterDefinition<TDocument> filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);

            return _collection.Find(filter).SingleOrDefault();
        }

        public virtual Task<TDocument> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                ObjectId objectId = new(id);
                FilterDefinition<TDocument> filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);

                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }

        public virtual void InsertOne(TDocument document)
        {
            _collection.InsertOne(document);
        }

        public virtual Task InsertOneAsync(TDocument document)
        {
            return Task.Run(() => _collection.InsertOneAsync(document));
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            _collection.InsertMany(documents);
        }

        public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public void ReplaceOne(TDocument document)
        {
            FilterDefinition<TDocument> filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceOneAsync(TDocument document)
        {
            FilterDefinition<TDocument> filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById(string id)
        {
            ObjectId objectId = new(id);
            FilterDefinition<TDocument> filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);

            _collection.FindOneAndDelete(filter);
        }

        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                ObjectId objectId = new(id);
                FilterDefinition<TDocument> filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);

                _collection.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        }
    }
}
