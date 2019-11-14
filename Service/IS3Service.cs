using S3TestWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3TestWebApi.Service
{
    public interface IS3Service
    {
        Task<S3Response> CreateBucketAsync(string bucketName);

        Task UploadFileAsync(string bucketName);

        Task GetObjectFromS3Async(string bucketName);
    }
}
