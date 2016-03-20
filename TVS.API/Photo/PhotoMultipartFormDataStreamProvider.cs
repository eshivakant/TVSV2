using System;
using System.Linq;
using System.Net.Http;
using System.Web.UI;

namespace TVS.API.Photo
{
    public class PhotoMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
    
        public PhotoMultipartFormDataStreamProvider(string path) : base(path)    
        {
        }
 
        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            //Make the file name URL safe and then use it & is the only disallowed url character allowed in a windows filename
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";

            var guid = Guid.NewGuid();
            return guid.ToString() +"."+ name.Split('.').Last();

        }
    }
}