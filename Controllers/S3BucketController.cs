using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using S3TestWebApi.Service;

namespace S3TestWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/S3Bucket")]
    public class S3BucketController : Controller
    {

        private readonly IS3Service _service;

        public S3BucketController(IS3Service service) {

            _service = service;
        }

        [HttpPost("{bucketName}")]
        public async Task<IActionResult> CreateBucket([FromRoute] string bucketName)
        {
            await _service.CreateBucketAsync(bucketName);

            return Ok();

        }


        [HttpPost]
        [Route("AddFile/{bucketName}")]
        public async Task<IActionResult> AddFile([FromRoute] string bucketName)
        {
             await _service.CreateBucketAsync(bucketName);

            return Ok();
        }

        [HttpGet]
        [Route("GetFile/{bucketName}")]
        public async Task<IActionResult> GetObjectFromS3Async([FromRoute] string bucketName)
        {
            await _service.GetObjectFromS3Async(bucketName);

            return Ok();
        }
    }
}