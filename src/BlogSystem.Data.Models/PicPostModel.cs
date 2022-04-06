using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Data.Models
{

    using BlogSystem.Data.Models;
    using System.Collections.Generic;
    using System.Web;

    public class PicPostModel 
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Id { get; set; }
        public int Ordinal { get; set; }
        public IEnumerable<HttpPostedFileBase> files { get; set; }
    }
}
