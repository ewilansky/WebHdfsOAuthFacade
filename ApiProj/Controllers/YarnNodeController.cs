using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ApiProj.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class YarnNodeController : Controller
    {
        private readonly Uri yarnNodeBase =
            new Uri("https://hdserver.local:44307/ws/v1/");

        private HttpClient client = new HttpClient();

        [HttpGet]
        public async Task<IActionResult> GetNodeInfo()
        {
            client.BaseAddress = yarnNodeBase;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            string path = "node/info";

            var response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var nodeInfoList = new List<Nodeinfo>();
                var info = await response.Content.ReadAsAsync<Rootobject>();
                nodeInfoList.Add(info.NodeInfo);
                
                return new JsonResult(nodeInfoList);
            }
            else
            {
                return BadRequest("Sorry, no data returned");
            }
        }


        public class Rootobject
        {
            public Nodeinfo NodeInfo { get; set; }
        }

        public class Nodeinfo
        {
            public string HealthReport { get; set; }
            public int TotalVmemAllocatedContainersMB { get; set; }
            public int TotalPmemAllocatedContainersMB { get; set; }
            public int TotalVCoresAllocatedContainers { get; set; }
            public bool VmemCheckEnabled { get; set; }
            public bool PpmemCheckEnabled { get; set; }
            public long LastNodeUpdateTime { get; set; }
            public bool NodeHealthy { get; set; }
            public string NodeManagerVersion { get; set; }
            public string NodeManagerBuildVersion { get; set; }
            public string NodeManagerVersionBuiltOn { get; set; }
            public string HadoopVersion { get; set; }
            public string HadoopBuildVersion { get; set; }
            public string HadoopVersionBuiltOn { get; set; }
            public string Id { get; set; }
            public string NodeHostName { get; set; }
            public long NmStartupTime { get; set; }
        }

    }
}