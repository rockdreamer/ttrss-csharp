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
    public class GetLabelsRequest : TTRssApiRequest
    {

        [JsonProperty("article_id")]
        public int article_id { get; set; }

        public GetLabelsRequest(int req)
        {
            this.sequence = req;
            this.operation = "getLabels";
        }

        public GetLabelsResponse response { get; set; }
    }

    public class GetLabelsResponseContent
    {
        [JsonProperty("error")]
        public string error { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("fg_color")]
        public string FgColor { get; set; }

        [JsonProperty("bg_color")]
        public string BgColor { get; set; }

        [JsonProperty("checked")]
        public bool Checked { get; set; }
    }

    public class GetLabelsResponse
    {

        [JsonProperty("seq")]
        public int Seq { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("content")]
        public GetLabelsResponseContent[] Content { get; set; }
    }

}
