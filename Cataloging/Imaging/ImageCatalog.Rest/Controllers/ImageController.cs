using ImageCatalog.Rest.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ImageCatalog.Rest.Controllers
{
    [RoutePrefix("api/v1/images")]
    public class ImageController : ApiController
    {
        private IObserver<PendingImage> Observer { get; set; }
        public ImageController(IObserver<PendingImage> observer)
        {
            Observer = observer;
        }
        [Route()]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<string>))]
        public IHttpActionResult GetImages()
        {
            IEnumerable<string> files = null;
            string directory = HttpContext.Current.Server.MapPath("~/pending");
            if(Directory.Exists(directory))
            {
                files = Directory.GetFiles(directory);
            }

            return Ok(files);
        }
        [Route()]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadImage()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string root = HttpContext.Current.Server.MapPath("~/pending");
            if(!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (MultipartFileData file in provider.FileData)
                {
                    Observer.OnNext(new PendingImage { SavedFile = file.LocalFileName, FileName = file.Headers.ContentDisposition.FileName });
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }

            /*var files = HttpContext.Current.Request.Files;
            foreach(HttpPostedFile file in files)
            {
                file.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath("~/pending"), file.FileName));
            }*/

        }
    }
}
