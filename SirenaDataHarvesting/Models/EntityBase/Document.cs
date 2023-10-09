using MongoDB.Bson;

namespace SirenaDataHarvesting.Models.EntityBase
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }
    }
}
