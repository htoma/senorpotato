using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Azure
{
    public class CaptainEntity : TableEntity
    {
        public CaptainEntity()
        {
        }

        public CaptainEntity(string skill, string affected, int value)
        {
            PartitionKey = skill;
            RowKey = Guid.NewGuid().ToString();
            Affected = affected;
            Value = value;
        }

        public string Skill => PartitionKey;
        public string Affected { get; set; }
        public int Value { get; set; }
    }
}
