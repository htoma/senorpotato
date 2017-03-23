using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Azure
{
    public static class TableManager
    {
        public static IEnumerable<T> Get<T>(string tableName) where T : ITableEntity, new()
        {
            try
            {
                var table = GetTable(tableName, false);
                return table.ExecuteQuery(new TableQuery<T>()).ToList();
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        public static void Insert<T>(string tableName, ICollection<T> entities) where T : ITableEntity, new()
        {
            var table = GetTable(tableName, true);

            foreach (var country in entities.GroupBy(x => x.PartitionKey))
            {
                var maxItemsPerBatch = 50;

                for (int i = 0; i <= country.Count() / maxItemsPerBatch; i++)
                {
                    var op = new TableBatchOperation();
                    var toInsert = country.Skip(i * maxItemsPerBatch).Take(maxItemsPerBatch).ToList();
                    toInsert.ForEach(y => op.Insert(y));
                    table.ExecuteBatch(op);
                }
            }
        }

        public static void DeleteTable(string tableName)
        {
            var table = GetTable(tableName, false);
            table.DeleteIfExists();
        }

        private static CloudTable GetTable(string tableName, bool createIfNotExists)
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            if (createIfNotExists)
            {
                table.CreateIfNotExists();
            }
            return table;
            ;
        }
    }
}