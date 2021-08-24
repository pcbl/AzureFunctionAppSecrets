using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace SecretsFunctionApp
{
    /// <summary>
    /// In order to support Dependency Injection,
    /// The Function APP should NOT be static
    /// </summary>
    public class Function1
    {
        //We inject the logget and Configuration
        private ILogger<Function1> log;
        private IConfiguration config;
        public Function1(ILogger<Function1> log, IConfiguration config)
        {
            this.log = log;
            this.config = config;
        }

        [FunctionName("Function1")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req
            )
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            //We read the secret as a "normal" Configuration provided over IConfiguration!!!
            string secret = config["the-secret"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? $"{secret}: This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"{secret}: Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
