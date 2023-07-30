using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Amazon.S3;

namespace mpp_reader_lambda
{
    internal class GetFile
    {
        public static async Task<bool> DownloadObjectFromBucketAsync(
            IAmazonS3 client,
            string bucketName,
            string objectName,
            string filePath)
        {
            // Create a GetObject request
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = objectName,
            };


            //filePath = System.IO.Path.GetTempPath();

            // Issue request and remember to dispose of the response
            using GetObjectResponse response = await client.GetObjectAsync(request);

            try
            {
                //return true;

                // Save object to local file
                await response.WriteResponseStreamToFileAsync($"{filePath}\\{objectName}", true, CancellationToken.None);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error saving {objectName}: {ex.Message}");
                return false;
            }

            //// Issue request and remember to dispose of the response
            //using (GetObjectResponse response = await client.GetObjectAsync(request))
            //{
            //    using (StreamReader reader = new StreamReader(response.ResponseStream))
            //    {
            //        string contents = reader.ReadToEnd();
            //        Console.WriteLine("Object - " + response.Key);
            //        Console.WriteLine(" Version Id - " + response.VersionId);
            //        Console.WriteLine(" Contents - " + contents);

            //        return true;
            //    }
            //}
        }
    }
}
