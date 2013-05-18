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

    public class GetArticleRequest : TTRssApiRequest
    {

        [JsonProperty("article_id")]
        public string article_ids { get; set; }

        public GetArticleRequest(int req)
        {
            this.sequence = req;
            this.operation = "getArticle";
        }

        public GetArticleResponse response { get; set; }
    }

    public class GetArticleResponseAttachment
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("content_url")]
        public string ContentUrl { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("post_id")]
        public string PostId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("duration")]
        public string Duration { get; set; }
    }

    public class GetArticleResponseContent
    {

        [JsonProperty("error")]
        public string error { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("labels")]
        public string[][] Labels { get; set; }

        [JsonProperty("unread")]
        public bool Unread { get; set; }

        [JsonProperty("marked")]
        public bool Marked { get; set; }

        [JsonProperty("published")]
        public bool Published { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("updated")]
        public int Updated { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("feed_id")]
        public string FeedId { get; set; }

        [JsonProperty("attachments")]
        public GetArticleResponseAttachment[] Attachments { get; set; }
    }

    public class GetArticleResponse
    {

        [JsonProperty("seq")]
        public int Seq { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("content")]
        public GetArticleResponseContent[] Content { get; set; }
    }

}
