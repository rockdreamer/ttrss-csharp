// Copyright 2013 Claudio Bantaloukas
//
// This file is part of the C# api wrapper for tt-rss.
// The C# api wrapper for tt-rss is free software: you can redistribute it and/or modify it under the terms of 
// the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, 
// or (at your option) any later version.
//
// The C# api wrapper for tt-rss is distributed in the hope that it will be useful, 
// but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with the C# api wrapper for tt-rss. 
// If not, see http://www.gnu.org/licenses/.


using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TTRss.Api.Commands
{

    public class GetHeadlinesRequest : TTRssApiRequest
    {
        [JsonProperty("feed_id")]
        public int feed_id { get; set; }
        
        [JsonProperty("limit")]
        public int limit { get; set; }
        
        [JsonProperty("skip")]
        public int skip { get; set; }
        
        [JsonProperty("filter")]
        public String filter { get; set; }
        
        [JsonProperty("is_cat")]
        public bool is_cat { get; set; }

        [JsonProperty("show_excerpt")]
        public bool show_excerpt { get; set; }

        [JsonProperty("show_content")]
        public bool show_content { get; set; }

        [JsonProperty("view_mode")]
        public String view_mode { get; set; }
        
        [JsonProperty("include_attachments")]
        public bool include_attachments { get; set; }
        
        [JsonProperty("since_id")]
        public int since_id { get; set; }

        [JsonProperty("include_nested")]
        public bool include_nested { get; set; }

        [JsonProperty("sanitize")]
        public bool sanitize { get; set; }

        [JsonProperty("order_by")]
        public String order_by { get; set; }
        
        public GetHeadlinesRequest(int req)
        {
            this.sequence = req;
            this.operation = "getHeadlines";
        }

        public GetHeadlinesResponse response { get; set; }
    }

    public class GetHeadlinesResponseContent
    {
        [JsonProperty("error")]
        public string error { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("unread")]
        public bool Unread { get; set; }

        [JsonProperty("marked")]
        public bool Marked { get; set; }

        [JsonProperty("published")]
        public bool Published { get; set; }

        [JsonProperty("updated")]
        public int Updated { get; set; }

        [JsonProperty("is_updated")]
        public bool IsUpdated { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("feed_id")]
        public string FeedId { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("attachments")]
        public object[] Attachments { get; set; }

        [JsonProperty("excerpt")]
        public string Excerpt { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("labels")]
        public object[] Labels { get; set; }

        [JsonProperty("feed_title")]
        public string FeedTitle { get; set; }

        [JsonProperty("comments_count")]
        public int CommentsCount { get; set; }

        [JsonProperty("comments_link")]
        public string CommentsLink { get; set; }

        [JsonProperty("always_display_attachments")]
        public bool AlwaysDisplayAttachments { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }
    }

    public class GetHeadlinesResponse
    {

        [JsonProperty("seq")]
        public int Seq { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("content")]
        public GetHeadlinesResponseContent[] Content { get; set; }
    }

}
