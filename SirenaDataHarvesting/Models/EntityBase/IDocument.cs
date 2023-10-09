using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SirenaDataHarvesting.Models.EntityBase
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }
    }
}
