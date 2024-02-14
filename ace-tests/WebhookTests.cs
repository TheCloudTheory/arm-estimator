// using System.Net;
// using System.Text.Json;
// using ACE;

// namespace arm_estimator_tests;

// [Ignore("Under rework")]
// internal class WebhookTests
// {
//     private HttpListener _listener;

//     [SetUp]
//     public void Setup()
//     {
//         _listener = new HttpListener();
//         _listener.Prefixes.Add("http://localhost:12345/");
//         _listener.Start();
//     }

//     [Test]
//     [Parallelizable(ParallelScope.Self)]
//     public void WhenWebhookUrlIsProvided_ACEShouldSendEstimationResultsThere()
//     {
//         var result = _listener.BeginGetContext(new AsyncCallback(ProcessMessage), _listener);

//         var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
//         _ = Task.Run(async () =>
//         {
//             var exitCode = await Program.Main(new[] {
//                 "templates/bicep/securestring.bicep",
//                 "cf70b558-b930-45e4-9048-ebcefb926adf",
//                 "arm-estimator-tests-rg",
//                 "--generateJsonOutput",
//                 "--jsonOutputFilename",
//                 outputFilename,
//                 "--inline",
//                 "adminPassword=verysecretpassword123",
//                 "--inline",
//                 $"dbName=db{DateTime.Now.Ticks}",
//                 "--inline",
//                 $"serverName=svr{DateTime.Now.Ticks}",
//                 "--webhook-url",
//                 "http://localhost:12345/"
//             });

//             Assert.That(exitCode, Is.EqualTo(0));

//             var outputFile = File.ReadAllText($"{outputFilename}.json");
//             var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
//             {
//                 PropertyNameCaseInsensitive = true
//             });

//             Assert.That(output, Is.Not.Null);
//             Assert.That(output.Resources.Count(), Is.EqualTo(2));
//         });

//         result.AsyncWaitHandle.WaitOne();

//         var context = _listener.EndGetContext(result);
//         var request = context.Request;

//         using (var sr = new StreamReader(request.InputStream))
//         {
//             var body = sr.ReadToEnd();
//             var webhook = JsonSerializer.Deserialize<EstimationOutput>(body, new JsonSerializerOptions()
//             {
//                 PropertyNameCaseInsensitive = true
//             });

//             Assert.That(webhook, Is.Not.Null);
//             Assert.That(webhook.Resources.Count(), Is.EqualTo(2));
//         }

//         var response = context.Response;
//         var responseString = "<HTML><BODY>Hello world!</BODY></HTML>";
//         var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
//         response.ContentLength64 = buffer.Length;

//         var output = response.OutputStream;
//         output.Write(buffer, 0, buffer.Length);
//         output.Close();
//     }

//     [Test]
//     [Parallelizable(ParallelScope.Self)]
//     public void WhenWebhookUrlIsProvidedWithAuthorizationHeader_ACEShouldSendEstimationResultsThere()
//     {
//         var result = _listener.BeginGetContext(new AsyncCallback(ProcessMessage), _listener);

//         var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
//         _ = Task.Run(async () =>
//         {
//             var exitCode = await Program.Main(new[] {
//                 "templates/bicep/securestring.bicep",
//                 "cf70b558-b930-45e4-9048-ebcefb926adf",
//                 "arm-estimator-tests-rg",
//                 "--generateJsonOutput",
//                 "--jsonOutputFilename",
//                 outputFilename,
//                 "--inline",
//                 "adminPassword=verysecretpassword123",
//                 "--inline",
//                 $"dbName=db{DateTime.Now.Ticks}",
//                 "--inline",
//                 $"serverName=svr{DateTime.Now.Ticks}",
//                 "--webhook-url",
//                 "http://localhost:12345/",
//                 "--webhook-authorization",
//                 "VERY_SECRET_TOKEN"
//             });

//             Assert.That(exitCode, Is.EqualTo(0));

//             var outputFile = File.ReadAllText($"{outputFilename}.json");
//             var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
//             {
//                 PropertyNameCaseInsensitive = true
//             });

//             Assert.That(output, Is.Not.Null);
//             Assert.That(output.Resources.Count(), Is.EqualTo(2));
//         });

//         result.AsyncWaitHandle.WaitOne();

//         var context = _listener.EndGetContext(result);
//         var request = context.Request;

//         Assert.That(request.Headers["Authorization"], Is.Not.Null);
//         Assert.That(request.Headers["Authorization"], Is.EqualTo("VERY_SECRET_TOKEN"));

//         using (var sr = new StreamReader(request.InputStream))
//         {
//             var body = sr.ReadToEnd();
//             var webhook = JsonSerializer.Deserialize<EstimationOutput>(body, new JsonSerializerOptions()
//             {
//                 PropertyNameCaseInsensitive = true
//             });

//             Assert.That(webhook, Is.Not.Null);
//             Assert.That(webhook.Resources.Count(), Is.EqualTo(2));
//         }

//         var response = context.Response;
//         var responseString = "<HTML><BODY>Hello world!</BODY></HTML>";
//         var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
//         response.ContentLength64 = buffer.Length;

//         var output = response.OutputStream;
//         output.Write(buffer, 0, buffer.Length);
//         output.Close();
//     }

//     private void ProcessMessage(IAsyncResult result)
//     {
//     }

//     [TearDown]
//     public void Teardown()
//     {
//         _listener.Close();
//     }
// }