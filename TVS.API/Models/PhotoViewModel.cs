using System;
using DefinitelyTypedNet;

namespace TVS.API.Models
{
    [TypeScript]
    public class PhotoViewModel
    {
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public long Size { get; set; }

    }
}