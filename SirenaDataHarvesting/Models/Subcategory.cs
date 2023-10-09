using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SirenaDataHarvesting.Models.EntityBase;
using SirenaDataHarvesting.Models.GeneralSettings;

namespace SirenaDataHarvesting.Models
{
    [BsonCollection("subcategories")]
    public class Subcategory : Document
    {
        public required string Name { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public required string CategoryId { get; set; }
    }
}
