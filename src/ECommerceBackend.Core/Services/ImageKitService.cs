using System;
using System.IO;
using System.Threading.Tasks;
using ECommerceBackend.Core.Interfaces;
using ECommerceBackend.Core.Models;
using ECommerceBackend.Core.UserSecrets;
using Imagekit;
using Microsoft.Extensions.Configuration;

namespace ECommerceBackend.Core.Services
{
    public class ImageKitService : IImageService
    {
        private readonly IConfiguration _configuration;
        private ServerImagekit imagekit;

        public ImageKitService(IConfiguration configuration)
        {
            _configuration = configuration;

            string publicKey = Environment.GetEnvironmentVariable("ECommerce_IMAGEKIT_PUBLIC_KEY");
            string privateKey = Environment.GetEnvironmentVariable("ECommerce_IMAGEKIT_PRIVATE_KEY");
            string urlEndPoint = Environment.GetEnvironmentVariable("ECommerce_IMAGEKIT_URL_ENDPOINT");

            if (string.IsNullOrEmpty(publicKey) || string.IsNullOrEmpty(privateKey) || string.IsNullOrEmpty(urlEndPoint))
            {
                ImageKit imageKit = _configuration.GetSection("ImageKit").Get<ImageKit>();
                if (imageKit != null)
                {
                    publicKey = imageKit.PublicKey;
                    privateKey = imageKit.PrivateKey;
                    urlEndPoint = imageKit.UrlEndPoint;
                }
            }
       
            imagekit = new ServerImagekit(publicKey, privateKey, urlEndPoint, "path");
        }

        

        /// <summary>
        /// Deletes a file from the specified folder/container
        /// </summary>
        /// <param name="fileId">File Id</param>
        /// <returns></returns>
        public async Task<bool> DeleteFileAsync(string fileId)
        {
            if (!string.IsNullOrEmpty(fileId))
            {
                var resp = await imagekit.DeleteFileAsync(fileId);
                if (resp.StatusCode == 204)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes a file from the specified folder/container
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="folder">Folder/Container Name</param>
        /// <returns></returns>
        public Task<bool> DeleteFileAsync(string fileName, string folder = "assets")
        {
            throw new NotImplementedException();
        }

        private static byte[] ConvertToBytes(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Uploads a file stream to the specified folder/container
        /// </summary>
        /// <param name="stream">File Stream</param>
        /// <param name="fileName">File Name</param>
        /// <param name="folder">Folder/Container Name</param>
        /// <returns></returns>
        public async Task<UploadedFileResponse> UploadFileAsync(Stream stream, string fileName, string folder = "assets")
        {
            ImagekitResponse resp = await imagekit
                    .Folder(folder)
                    .FileName(fileName)
                    .UploadAsync(ConvertToBytes(stream));

            UploadedFileResponse uploadedFileResponse = new UploadedFileResponse()
            {
                UploadedFileId = resp.FileId,
                Title = resp.Name,
                FileName = resp.Name,
                Name = resp.Name,
                URL = resp.URL,
                MediaUrl = resp.URL,
                Thumbnail = resp.Thumbnail,
                FilePath = resp.FilePath,
                FileType = resp.FileType,
                Height = resp.Height,
                Width = resp.Width,
                Size = resp.Size,
            };

            return uploadedFileResponse;
        }
    }
}
