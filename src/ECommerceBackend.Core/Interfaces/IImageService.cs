using System.IO;
using System.Threading.Tasks;
using ECommerceBackend.Core.Models;

namespace ECommerceBackend.Core.Interfaces
{
    public interface IImageService
    {
        /// <summary>
        /// Uploads a file
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        Task<UploadedFileResponse> UploadFileAsync(Stream stream, string fileName, string folder = "assets");

        /// <summary>
        /// Deletes an uploaded file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        Task<bool> DeleteFileAsync(string fileName, string folder = "assets");

        /// <summary>
        /// Deletes an uploaded file
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        Task<bool> DeleteFileAsync(string fileId);
    }
}
