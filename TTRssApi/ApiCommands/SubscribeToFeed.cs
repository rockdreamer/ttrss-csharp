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
    public class SubscribeToFeedRequest : TTRssApiRequest
    {

        [JsonProperty("feed_url")]
        public string feed_url { get; set; }

        [JsonProperty("category_id")]
        public int category_id { get; set; }

        [JsonProperty("login")]
        public string login { get; set; }

        [JsonProperty("password")]
        public string password { get; set; }

        public SubscribeToFeedRequest(int req)
        {
            this.sequence = req;
            this.operation = "subscribeToFeed";
        }

        public SubscribeToFeedResponse response { get; set; }
    }

    public class SubscribeToFeedResponseStatus
    {

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class SubscribeToFeedResponseContent
    {
        [JsonProperty("error")]
        public string error { get; set; }

        [JsonProperty("status")]
        public SubscribeToFeedResponseStatus Status { get; set; }
    }

    public class SubscribeToFeedResponse
    {

        [JsonProperty("seq")]
        public int Seq { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("content")]
        public SubscribeToFeedResponseContent Content { get; set; }
    }

}
