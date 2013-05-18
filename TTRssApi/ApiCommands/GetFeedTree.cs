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

    public class GetFeedTreeRequest : TTRssApiRequest
    {

        public GetFeedTreeRequest(int req)
        {
            this.sequence = req;
            this.operation = "getFeedTree";
        }

        [JsonProperty("include_empty")]
        public bool include_empty { get; set; }

        public GetFeedTreeResponse response { get; set; }
    }

    public class GetFeedTreeResponseContentFeed
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("unread")]
        public int Unread { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("updated")]
        public string Updated { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("bare_id")]
        public int BareId { get; set; }

        [JsonProperty("fg_color")]
        public string FgColor { get; set; }

        [JsonProperty("bg_color")]
        public string BgColor { get; set; }

        [JsonProperty("checkbox")]
        public bool? Checkbox { get; set; }

        [JsonProperty("param")]
        public string Param { get; set; }
    }

    public class GetFeedTreeResponseContentCategory
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("items")]
        public GetFeedTreeResponseContentFeed[] Items { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("unread")]
        public int Unread { get; set; }

        [JsonProperty("bare_id")]
        public int BareId { get; set; }

        [JsonProperty("checkbox")]
        public bool? Checkbox { get; set; }

        [JsonProperty("child_unread")]
        public int? ChildUnread { get; set; }

        [JsonProperty("param")]
        public string Param { get; set; }
    }

    public class GetFeedTreeResponseCategoriesContainer
    {

        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("items")]
        public GetFeedTreeResponseContentCategory[] Items { get; set; }
    }

    public class GetFeedTreeResponseContent
    {

        [JsonProperty("error")]
        public string error { get; set; }

        [JsonProperty("categories")]
        public GetFeedTreeResponseCategoriesContainer Categories { get; set; }
    }

    public class GetFeedTreeResponse
    {

        [JsonProperty("seq")]
        public int Seq { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("content")]
        public GetFeedTreeResponseContent Content { get; set; }
    }

}
