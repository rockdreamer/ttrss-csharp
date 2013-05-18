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

    public class GetCountersRequest : TTRssApiRequest
    {
        [JsonProperty("mode")]
        public string mode { get; set; }

        public GetCountersRequest(int req)
        {
            this.sequence = req;
            this.operation = "getCounters";
        }

        public GetCountersResponse response { get; set; }
    }

    public class GetCountersResponseContent
    {

        [JsonProperty("id")]
        public object Id { get; set; }

        [JsonProperty("counter")]
        public object Counter { get; set; }

        [JsonProperty("updated")]
        public string Updated { get; set; }

        [JsonProperty("has_img")]
        public int? HasImg { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("kind")]
        public string Kind { get; set; }
    }

    public class GetCountersResponse
    {

        [JsonProperty("seq")]
        public int Seq { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("content")]
        public GetCountersResponseContent[] Content { get; set; }
    }

}
