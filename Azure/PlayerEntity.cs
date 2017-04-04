using Microsoft.WindowsAzure.Storage.Table;

namespace Azure
{
    public class PlayerEntity : TableEntity
    {
        public PlayerEntity()
        {
        }

        public PlayerEntity(int id, string name, string description, string country)
        {
            RowKey = id.ToString();
            PartitionKey = country;
            Name = name;
            Description = description;
        }

        public string Id => RowKey;

        public string Country => PartitionKey;

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
