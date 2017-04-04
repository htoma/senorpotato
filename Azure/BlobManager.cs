using System.IO;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace Azure
{
    public class BlobManager
    {
        private const string BlobContainer = "games";
        private const string IdSequenceBlock = "id";

        private static readonly object IdSequenceLock = new object();

        public static T Get<T>(string blobName)
        {
            var block = GetBlock(blobName);
            return Get<T>(block);
        }        

        public static void Upload<T>(string blobName, T value)
        {
            var block = GetBlock(blobName);
            Upload(value, block);
        }        

        public static int GetNextId()
        {
            lock (IdSequenceLock)
            {
                var block = GetBlock(IdSequenceBlock);
                int value = 1;
                if (block.Exists())
                {
                    value = Get<int>(block) + 1;
                }

                Upload(value, block);
                return value;
            }            
        }

        public static void Delete(string blobName)
        {
            var block = GetBlock(blobName);
            block.DeleteIfExists();
        }

        public static void DeleteWithPrefix(string prefix)
        {
            var container = GetContainer();
            foreach (var blobItem in container.ListBlobs(prefix))
            {
                var gameBlock = blobItem as CloudBlockBlob;
                gameBlock?.DeleteIfExists();
            }
        }

        private static CloudBlobContainer GetContainer()
        {
            var storageAccount =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(BlobContainer);
            container.CreateIfNotExists();
            return container;
        }

        private static CloudBlockBlob GetBlock(string blockName)
        {
            return GetContainer().GetBlockBlobReference(blockName);
        }

        private static T Get<T>(CloudBlockBlob block)
        {
            using (var stream = new MemoryStream())
            using (var reader = new StreamReader(stream))
            {
                block.DownloadToStream(stream);
                stream.Position = 0;
                var content = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        private static void Upload<T>(T value, CloudBlockBlob block)
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(JsonConvert.SerializeObject(value));
                writer.Flush();
                stream.Position = 0;
                block.UploadFromStream(stream);
            }
        }        
    }
}