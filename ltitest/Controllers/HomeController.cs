using System;
using System.Web.Mvc;
using LtiLibrary.Core.Common;
using LtiLibrary.Core.Lti1;
using ltitest.Models;
using Newtonsoft.Json;

namespace ltitest.Controllers
{
    public class HomeController : Controller
    {
        const string consumerKey = "adfe-8d2435fbf9b.admission.org";
        const string consumerSecret = "1uJCUiZQXCwNFHsv";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetLtiResource(string resourceUrl, string resourceId)
        {
            var resource = getLtiResource(resourceUrl, resourceId);
            return Content(JsonConvert.SerializeObject(resource), "application/json");
        }

        LtiResource getLtiResource(string resourceUrl, string resourceId)
        {
            Uri launchUri;
            var ltiRequest = new LtiRequest(LtiConstants.BasicLaunchLtiMessageType)
            {
                ConsumerKey = consumerKey,
                ResourceLinkId = resourceId,
                Url = Uri.TryCreate(resourceUrl, UriKind.Absolute, out launchUri) ? launchUri : null,

                // Tool
                ToolConsumerInfoProductFamilyCode = "LtiLibrary",
                ToolConsumerInfoVersion = "1.2",

                // Context - populate with the Topic that we are launching from
                ContextId = "topic1",
                ContextTitle = "Topic 1",
                ContextType = LisContextType.Group,

                // Instance
                ToolConsumerInstanceGuid = "4e09-9e52-926ae4e24fa.admission.org",
                ToolConsumerInstanceName = "SSATB Default Instance",
                //ResourceLinkTitle = "Launch",
                //ResourceLinkDescription = "Perform a basic LTI 1.2 launch",

                // User - replace this with the current user information
                LisPersonEmailPrimary = "jdoe@andyfmiller.com",
                LisPersonNameFamily = "Doe",
                LisPersonNameGiven = "Joan",
                UserId = "1"
            };
            ltiRequest.SetRoles(new[] { Role.Learner });

            return ltiRequest.AsLtiResource(consumerSecret);
        }
    }
}