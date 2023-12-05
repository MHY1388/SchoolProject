using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitesLayer.Utilities.ali
{
    public class CloudTool
    {

            private readonly IAmazonS3 _s3Client;
            private const string Access_Key = "442fa1d4-548b-44e9-b57f-8a448408c669";
            private const string Secret_Key = "35179269fe7308da5fc390184d99cfe19d95b07cdc2a2f8890d7735a3a82e8d8";
            private const string EndPoint = "https://s3.ir-thr-at1.arvanstorage.ir";
            public CloudTool()
            {
                var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(Access_Key, Secret_Key);
                var config = new AmazonS3Config { ServiceURL = EndPoint };
                _s3Client = new AmazonS3Client(awsCredentials, config);
            }

            public async Task<List<S3Object>> ListingObjectsAsync(IAmazonS3 client, string bucketName, int size)
            {
                try
                {
                    ListObjectsRequest request = new()
                    {
                        BucketName = bucketName,
                        MaxKeys = size,
                    };

                    do
                    {
                        ListObjectsResponse response = await client.ListObjectsAsync(request);

                        if (response.IsTruncated)
                        {
                            request.Marker = response.NextMarker;
                        }
                        else
                        {
                            request = null;
                        }
                        return response.S3Objects;
                    } while (request != null);
                }
                catch
                {
                    return default;
                }
            }
            public async Task DeleteObjectHelper(string BUCKET_NAME, string OBJECT_NAME)
            {
                try
                {
                    DeleteObjectsRequest request = new()
                    {
                        BucketName = BUCKET_NAME,
                        Objects = new List<KeyVersion> { new KeyVersion() { Key = OBJECT_NAME, VersionId = null } }
                    };

                    DeleteObjectsResponse response = await _s3Client.DeleteObjectsAsync(request);

                }
                catch
                {

                }
            }
            public string GeneratePreSignedUrl(string OBJECT_NAME, string BUCKET_NAME, DateTime? time = null)
            {
                try
                {
                    if (time == null)
                    {
                        time = DateTime.Now.AddDays(3);
                    }
                    var getPreSignedUrlRequest = new GetPreSignedUrlRequest
                    {
                        BucketName = BUCKET_NAME,
                        Key = OBJECT_NAME,
                        Expires = (DateTime)time,
                        Verb = HttpVerb.GET
                    };

                    string url = _s3Client.GetPreSignedURL(getPreSignedUrlRequest);
                    return url;
                }
                catch
                {
                    return string.Empty;
                }
            }
            public async Task AddItem(IFormFile file, string Name, string BUCKET_NAME, DateTime? time)
            {

                using (Stream fileToUpload = file.OpenReadStream())
                {
                    var putObjectRequest = new PutObjectRequest();
                    putObjectRequest.BucketName = BUCKET_NAME;
                    putObjectRequest.Key = Name;
                    putObjectRequest.InputStream = fileToUpload;
                    putObjectRequest.ContentType = file.ContentType;
                    putObjectRequest.Headers.Expires = DateTime.Now.AddYears(3);
                    var response = await _s3Client.PutObjectAsync(putObjectRequest);
                }
            }
    }
}
