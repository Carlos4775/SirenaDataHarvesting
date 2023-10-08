namespace SirenaDataHarvesting.Models
{
    public class SirenaDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;

        public string SirenaCollectionName { get; set; } = null!;
    }
}
