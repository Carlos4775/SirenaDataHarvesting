using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SirenaDataHarvesting.Models.EntityBase;
using SirenaDataHarvesting.Models.GeneralSettings;

namespace SirenaDataHarvesting.Models
{
    [BsonCollection("products")]
    public class Product : Document
    {
        [BsonElement("name")]
        public required string Name { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("imageUrl")]
        public string? ImageUrl { get; set; }
    }
}
