using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Google.Apis.Storage.v1;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using netart.Models;

namespace netart.Services
{
    public class GoogleStorageService {
        private readonly StorageClient storage;
        private readonly string _BucketName;

        public GoogleStorageService(IBucketNameSetting BucketName)
        {
            storage = StorageClient.Create();
            _BucketName = BucketName.bucketName;
        }

        public async Task<bool> UploadFileGoogle(IFormFile file, string objectName, string fileType)
        {
            using var newMemoryStream = new MemoryStream();
            file.CopyTo(newMemoryStream);
            try {
                await storage.UploadObjectAsync(_BucketName, objectName, fileType, newMemoryStream);
                Console.WriteLine($"Uploaded {objectName}.");
                return true;
            }
            catch (Exception e) {
                Console.WriteLine("cannot upload to the google storage" + e.Message);
                return false;
            }
        }

        public async Task<MemoryStream> DownloadFileGoogle(string uuid)
        {
            var memoryStream = new MemoryStream();
            await storage.DownloadObjectAsync(_BucketName, uuid, memoryStream);
            return memoryStream;
        }
    }
}