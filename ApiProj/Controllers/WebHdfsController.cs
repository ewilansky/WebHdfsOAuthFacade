using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ApiProj.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class WebHdfsController : Controller
    {
        private readonly Uri webHdfsUriBase =
            new Uri("https://hdserver.local:44305/webhdfs/v1/");

        private HttpClient client = new HttpClient();

        [HttpGet]
        public async Task<IActionResult> GetFolderList()
        {
            client.BaseAddress = webHdfsUriBase;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            string path = "movie-and-ratings?op=LISTSTATUS";

            var response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var status = await response.Content.ReadAsAsync<Rootobject>();
                return Ok(new JsonResult(from f in status.FileStatuses.FileStatus select new {
                    f.Type,
                    f.PathSuffix,
                    f.AccessTime,
                    f.FileId,
                    f.Owner,
                    f.Group
                }));
            }
            else
            {
                return BadRequest("Sorry, no data returned");
            }
        }

        public class Rootobject
        {
            public FileStatuses FileStatuses { get; set; }
        }

        public class FileStatuses
        {
            public Filestatus[] FileStatus { get; set; }
        }

        public class Filestatus
        {
            public long AccessTime { get; set; }
            public int BlockSize { get; set; }
            public int ChildrenNum { get; set; }
            public int FileId { get; set; }
            public string Group { get; set; }
            public int Length { get; set; }
            public long ModificationTime { get; set; }
            public string Owner { get; set; }
            public string PathSuffix { get; set; }
            public string Permission { get; set; }
            public int Replication { get; set; }
            public int StoragePolicy { get; set; }
            public string Type { get; set; }
        }
    }
}
