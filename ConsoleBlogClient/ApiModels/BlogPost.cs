using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsoleBlogClient.ApiModels
{
    internal class BlogPost
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("comments")]
        public Comment[] Comments { get; set; }
    }
}
