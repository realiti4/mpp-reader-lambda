using Amazon.Lambda.Core;
using Amazon.S3.Model;
using Amazon.S3;

using net.sf.mpxj.reader;
using Task = net.sf.mpxj.Task;
using Amazon.Runtime.Internal.Util;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace mpp_reader_lambda;

public class Function
{
    public string FunctionHandler(string input, ILambdaContext context)
    {
        AmazonS3Client client = new AmazonS3Client();

        string objectName = "home_backup/flex.mpp";
        // string filePath = @"C:\sil";
        string filePath = System.IO.Path.GetTempPath();

        bool result = AsyncHelpers.RunSync<bool>(() => GetFile.DownloadObjectFromBucketAsync(
            client,
            "realitifiles",
            objectName,
            filePath
        ));

        string testName = string.Empty;

        if (result)
        {
            var project = new UniversalProjectReader().read($"{filePath}\\{objectName}");

            foreach (Task task in project.Tasks)
            {
                testName += task.Name.ToString();
                break;
            }
        }
        else
        {
            testName = "result is false";
        }


        Console.WriteLine(result);

        return testName;
    }
}
