using Amazon.S3;
using Amazon.S3.Model;

namespace FileManager.S3
{
    public class OperationGetAllFiles
    {
        public static async Task<List<string>> GetAllUrlsAsync(string bucketName, string folder)
        {
            var client = new AmazonS3Client("your access key", "your secret key");
            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = bucketName,
                    MaxKeys = 5,
                    
                };

                Console.WriteLine("--------------------------------------");
                Console.WriteLine($"Listing the contents of {bucketName}:");
                Console.WriteLine("--------------------------------------");

                ListObjectsV2Response response;
                var urls = new List<string>();

                do
                {
                    response = await client.ListObjectsV2Async(request);

                    response.S3Objects
                        .ForEach(obj =>
                        {
                            Console.WriteLine($"{obj.Key,-35}{obj.LastModified.ToShortDateString(),10}{obj.Size,10}");
                            urls.Add(obj.Key);
                        });

                    // If the response is truncated, set the request ContinuationToken
                    // from the NextContinuationToken property of the response.
                    request.ContinuationToken = response.NextContinuationToken;
                }
                while (response.IsTruncated);

                return urls;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error encountered on server. Message:'{ex.Message}' getting list of objects.");
                return new List<string>();
            }
        }
    }
}
