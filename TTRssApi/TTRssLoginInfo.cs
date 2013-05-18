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
using System.Net;

namespace TTRss
{
    namespace Api
    {
        public class TtRssLoginInfo
        {
            public String username
            {
                get;
                set;
            }

            public String password
            {
                get;
                set;
            }

            public string rootUri
            {
                get { return rootUri; }
                set { apiUri = new Uri(value + "/api/"); }
            }

            public Uri apiUri;

            public String sessionid
            {
                get;
                set;
            }

        }
    }
}