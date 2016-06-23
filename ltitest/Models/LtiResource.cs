using System.Collections.Specialized;
using System.Linq;
using LtiLibrary.Core.Lti1;
using Newtonsoft.Json.Linq;

namespace ltitest.Models
{
    public class LtiResource
    {
        public string action { get; set; }
        public string signature { get; set; }
        public dynamic fields { get; set; }
    }

    public static class LtiResourceExtensions
    {
        public static LtiResource AsLtiResource(this LtiRequest ltiRequest, string consumerSecret)
        {
            // remove empty parameters
            var parameters = new NameValueCollection(ltiRequest.Parameters);
            foreach (var key in parameters.AllKeys.Where(key => string.IsNullOrWhiteSpace(parameters[key])))
                parameters.Remove(key);

            // Perform the custom variable substitutions
            ltiRequest.SubstituteCustomVariables(parameters);

            // Calculate the signature based on the substituted values
            var signature = ltiRequest.GenerateSignature(parameters, consumerSecret);

            var resource = new LtiResource
            {
                action = ltiRequest.Url.ToString(),
                fields = new JObject(),
                signature = signature
            };
            foreach (var key in parameters.AllKeys)
                resource.fields[key] = parameters[key];

            return resource;
        }
    }
}
