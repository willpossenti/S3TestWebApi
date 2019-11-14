using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using S3TestWebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace S3TestWebApi.Service
{
    public class S3Service : IS3Service
    {

        private readonly IAmazonS3 _client;

        public S3Service(IAmazonS3 client)
        {

            _client = client;
        }

        public async Task<S3Response> CreateBucketAsync(string bucketName)
        {


            try
            {

                if (await AmazonS3Util.DoesS3BucketExistV2Async(_client, bucketName) == false)
                {

                    var putBucketRequest = new PutBucketRequest
                    {

                        BucketName = bucketName,
                        UseClientRegion = true

                    };

                    var response = await _client.PutBucketAsync(putBucketRequest);

                    return new S3Response()
                    {

                        Message = response.ResponseMetadata.RequestId,
                        Status = response.HttpStatusCode

                    };
                }

            }
            catch (AmazonS3Exception ex)
            {

                return new S3Response()
                {

                    Status = ex.StatusCode,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {

                return new S3Response()
                {

                    Status = System.Net.HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }

            return new S3Response()
            {

                Status = System.Net.HttpStatusCode.InternalServerError,
                Message = "Something went wrong"
            };
        }

        private const string FilePath = "C:\\";
        private const string UploadWithKeyName = "UploadWithKeyName";
        private const string FileStreamUpload = "FileStreamUpload";
        private const string AdvancedUpload = "AdvancedUpload";

        public async Task UploadFileAsync(string bucketName)
        {

            try
            {

                var fileTransferUtility = new TransferUtility(_client);

                await fileTransferUtility.UploadAsync(FilePath, bucketName);

                await fileTransferUtility.UploadAsync(FilePath, bucketName, UploadWithKeyName);

                using (var fileTupload = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {

                    await fileTransferUtility.UploadAsync(fileTupload, bucketName, FileStreamUpload);
                }

                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {

                    BucketName = bucketName,
                    FilePath = FilePath,
                    StorageClass = S3StorageClass.Standard,
                    PartSize = 6291456,
                    CannedACL = S3CannedACL.NoACL

                };

                fileTransferUtilityRequest.Metadata.Add("param1","value1");
                fileTransferUtilityRequest.Metadata.Add("param2", "value2");

                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);

            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine("Error encountered on server. Message: '{0}' when writing an object", ex.Message);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error encountered on server. Message: '{0}' when writing an object", ex.Message);

            }

        }


        public async Task GetObjectFromS3Async(string bucketName) {

            const string keyName = "s3TestFile.txt";

            try {

                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName

                };

                string responseBody;

                using (var response = await _client.GetObjectAsync(request))
                using (var responseStream = response.ResponseStream)
                using (var reader = new StreamReader(responseStream)) {

                    var title = response.Metadata["x-amz-meta-title"];
                    var contentType = response.Headers["Content-Type"];

                    Console.WriteLine($"Object meta, Title : {title}");
                    Console.WriteLine($"Content type: {contentType}");

                    responseBody = reader.ReadToEnd();
                }

                var pathAndFileName = $"";

                var createText = responseBody;

                File.WriteAllText(pathAndFileName, createText);

            }
            catch (Exception e) {


                Console.WriteLine(e);
                throw;
            
            }
        
        
        }
    }
}
