using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Azure
{
    public class CaptainEntity : TableEntity
    {
        public CaptainEntity()
        {
        }

        public CaptainEntity(string affected, string skill, int value)
        {
            RowKey = Guid.NewGuid().ToString();
            Affected = affected;
            PartitionKey = skill;
            Value = value;
        }

        public string Id => RowKey;
        public string Affected { get; set; }
        public string Skill => PartitionKey;        
        public int Value { get; set; }
    }
}
