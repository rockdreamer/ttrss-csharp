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

    public class ShareToPublishedRequest : TTRssApiRequest
    {

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("url")]
        public string url { get; set; }

        [JsonProperty("content")]
        public string content { get; set; }

        public ShareToPublishedRequest(int req)
        {
            this.sequence = req;
            this.operation = "shareToPublished";
        }

        public ShareToPublishedResponse response { get; set; }
    }

    public class ShareToPublishedResponseContent
    {
        [JsonProperty("error")]
        public string error { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class ShareToPublishedResponse
    {

        [JsonProperty("seq")]
        public int Seq { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("content")]
        public ShareToPublishedResponseContent Content { get; set; }
    }

}
