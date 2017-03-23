using Microsoft.WindowsAzure.Storage.Table;

namespace Azure
{
    public class PlayerEntity : TableEntity
    {
        public PlayerEntity()
        {
        }

        public PlayerEntity(string name, string description, string country)
        {
            PartitionKey = country;
            RowKey = name;
            Description = description;
        }

        public string Name => RowKey;
        public string Country => PartitionKey;

        public string Description { get; set; }
    }
}
