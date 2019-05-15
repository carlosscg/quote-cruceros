using Amazon.Lambda.Core;
using System;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System.Threading.Tasks;
using System.Linq;

[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsDotnetCsharp
{
    public class Handler  {
        private AmazonSimpleNotificationServiceClient _snsClient;
        public string _topicName { get; private set; }
        internal string _topicArn { get; private set; }
        public string _subscriptionName { get; private set; }
        private string _subscriptionArn;
      public Handler()
      {
        _snsClient = new AmazonSimpleNotificationServiceClient(Amazon.RegionEndpoint.USEast1);
      }

      public Response Manager(Request request){
        var snsRequest = new PublishRequest
        {
          TopicArn = _topicArn, //"arn:aws:sns:us-east-1:150135223216:SNSCruiseTopic", //"cruiseManager",
          Message = "Test Message"
        };

        SendMessage(_snsClient, snsRequest).Wait();
        return new Response("Go Serverless v1.0! Your function executed successfully!", request);
      }

      public bool TopicExists(string topicName){
        var exists= false;
        var matchString = string.Format(":{0}", topicName);
        var response = _snsClient.ListTopicsAsync().Result;
        var matches = response.Topics.Where(x => x.TopicArn.EndsWith(matchString));
        if(matches.Count() == 1){
          _topicArn = matches.ElementAt(0).TopicArn;
          exists = true;
        }
        return exists;
      }

      static async Task SendMessage(IAmazonSimpleNotificationService snsClient, PublishRequest request)
      {
            await snsClient.PublishAsync(request);
      }
    }

    // public class Handler
    // {
    //   AmazonSimpleNotificationServiceClient _snsService;
    //    public Response Hello(Request request)
    //    {
    //      var SNSRequest = new PublishRequest{
    //        Message = $"Test at {DateTime.UtcNow.ToLongDateString()}",
    //        TargetArn = "cruiseManager"
    //      };
    //      SNSRequest.MessageAttributes.Add("ProviderName",
    //      new MessageAttributeValue(){ 
    //         DataType = "String",StringValue = "Hotelbeds"  
    //      });

    //      Console.WriteLine("Request to be sent " + SNSRequest.Message);
         
    //      Console.WriteLine("Publishing the request");

    //      var r = SendSNS(SNSRequest);



    //        return new Response("Go Serverless v1.0! Your function executed successfully!", request);
    //    }

    //    public async Task<PublishResponse> SendSNS(PublishRequest request) {
    //      var response = await _snsService.PublishAsync(request);
    //      Console.WriteLine("response " + response.MessageId);
    //      return response;
    //   }
    // }
    

    public class Response
    {
      public string Message {get; set;}
      public Request Request {get; set;}

      public Response(string message, Request request){
        Message = message;
        Request = request;
      }
    }

    public class Request
    {
      public string Key1 {get; set;}
      public string Key2 {get; set;}
      public string Key3 {get; set;}

      public Request(string key1, string key2, string key3){
        Key1 = key1;
        Key2 = key2;
        Key3 = key3;
      }
    }
}
