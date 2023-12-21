using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBlogClient.ApiModels
{
    internal class CreateBlogPost
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
