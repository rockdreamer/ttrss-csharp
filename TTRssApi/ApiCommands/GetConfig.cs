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

    public class GetConfigRequest : TTRssApiRequest
    {

        public GetConfigRequest(int req)
        {
            this.sequence = req;
            this.operation = "getConfig";
        }

        public GetConfigResponse response { get; set; }
    }

    public class GetConfigResponseContent
    {
        [JsonProperty("error")]
        public string error { get; set; }

        [JsonProperty("icons_dir")]
        public string IconsDir { get; set; }

        [JsonProperty("icons_url")]
        public string IconsUrl { get; set; }

        [JsonProperty("daemon_is_running")]
        public bool DaemonIsRunning { get; set; }

        [JsonProperty("num_feeds")]
        public int NumFeeds { get; set; }
    }

    public class GetConfigResponse
    {

        [JsonProperty("seq")]
        public int Seq { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("content")]
        public GetConfigResponseContent Content { get; set; }
    }

}
