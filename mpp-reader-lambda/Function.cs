using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;
using System.IO;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Serialization.SystemTextJson;
using System.ComponentModel;
using System.Text.Json;
using java.nio.file;
using Amazon.Runtime.Internal.Util;

using net.sf.mpxj;
using net.sf.mpxj.reader;
using net.sf.mpxj.writer;
using net.sf.mpxj.mpp;
using net.sf.mpxj.MpxjUtilities;
using net.sf.mpxj.sample;

[assembly: Amazon.Lambda.Core.LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace mpp_reader_lambda;

[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyRequest))]
[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyResponse))]


public class Function
{
    public APIGatewayHttpApiV2ProxyResponse FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        string message;

        if (request.QueryStringParameters == null || !request.QueryStringParameters.ContainsKey("mppurl"))
        {
            message = "mppurl parameter is required.";
        } else
        {
            try
            {
                string url = request.QueryStringParameters["mppurl"];

                string filePath = AsyncHelpers.RunSync<string>(() => GetFile.DownloadFile(
                    url
                ));

                string pathToSave = System.IO.Path.GetTempPath();
                pathToSave = System.IO.Path.Combine(pathToSave, "temp.json");

                Console.WriteLine(pathToSave);

                new MpxjConvert().Process(filePath, pathToSave);

                using (StreamReader file = File.OpenText(pathToSave))
                {
                    message = file.ReadToEnd();
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = message,
            //Body = JsonSerializer.Serialize(new Dictionary<string, string>
            //    {
            //        {"karakara", "love you"}
            //    }),
            Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"},
                    {"Access-Control-Allow-Origin", "*"},
                    {"Access-Control-Allow-Credentials", "true"}
                }
        };
    }
}
