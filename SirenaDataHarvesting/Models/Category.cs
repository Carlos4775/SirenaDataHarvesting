﻿using SirenaDataHarvesting.Models.EntityBase;
using SirenaDataHarvesting.Models.GeneralSettings;

namespace SirenaDataHarvesting.Models
{
    [BsonCollection("categories")]
    public class Category : Document
    {
        public required string Name { get; set; }
    }
}
