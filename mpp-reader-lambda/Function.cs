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

        string url = request.QueryStringParameters["mppurl"];

        string filePath = AsyncHelpers.RunSync<string>(() => GetFile.DownloadFile(
            url
        ));

        string pathToSave = System.IO.Path.GetTempPath();
        pathToSave = System.IO.Path.Combine(pathToSave, "temp.json");

        new MpxjConvert().Process(filePath, pathToSave);

        string json = "";

        using (StreamReader file = File.OpenText(pathToSave))
        {
            json = file.ReadToEnd();
        };


        //return result.ToUpper();

        //var response = new APIGatewayHttpApiV2ProxyResponse
        //{
        //    StatusCode = (int)HttpStatusCode.OK,
        //    Body = JsonSerializer.Serialize(new Dictionary<string, string>
        //        {
        //            {"karakara", "love you"}
        //        }),
        //    Headers = new Dictionary<string, string>
        //        {
        //            {"Content-Type", "application/json"},
        //            {"Access-Control-Allow-Origin", "*"},
        //            {"Access-Control-Allow-Credentials", "true"}
        //        }
        //};

        //return response;

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = json,
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
