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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TTRss.Api.Commands;

namespace TTRss
{
    namespace Api
    {
        /// <summary>
        /// This class allows calling all tt-rss JSON methods via a C# based interface
        /// The JSON API is documented here
        /// http://tt-rss.org/redmine/projects/tt-rss/wiki/JsonApiReference
        /// </summary>
        public class Caller
        {

            /// <summary>
            /// Constructor with login parameters
            /// </summary>
            /// <param name="loginInfo">an object containing the url of the tt-rss installation and, optionally, username and password pair</param>
            public Caller(TtRssLoginInfo loginInfo)
            {
                this.loginInfo = loginInfo;
                // needed to avoid getting errors due to bad proxies
                System.Net.ServicePointManager.Expect100Continue = false;
                hasSessionId = false;
                currentRequestNum = 0;
            }

            #region Public TT-RSS API

            /// <summary>
            /// Logs in via the username and password provided in the loginInfo object passed to the constructor
            /// If login is successful, the session id is memorised and other api methods can be used
            /// The session id can be discarded by calling Logout()
            /// </summary>
            /// <seealso cref="Logout()" />
            /// <returns>a LoginResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<LoginResponse> Login()
            {
                LoginResponse ret = await Login(loginInfo.username, loginInfo.password);
                return ret;
            }

            /// <summary>
            /// Logs in via the provided username and password to the tt-rss instance passed in the loginInfo object to the constructor
            /// If login is successful, the session id is memorised and other api methods can be used
            /// The session id can be discarded by calling Logout()
            /// </summary>
            /// <param name="username">the username</param>
            /// <param name="password">the password</param>
            /// <returns>a LoginResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<LoginResponse> Login(String username, String password)
            {
                ++currentRequestNum;
                LoginRequest req = new LoginRequest(currentRequestNum);
                req.username = loginInfo.username;
                req.password = loginInfo.password;
                req.sid = loginInfo.sessionid;
                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    LoginResponse resp = new LoginResponse();
                    resp.Status = 1;
                    resp.Content.error = status.error;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<LoginResponse>(status.response);
                if (response.Status == 0)
                {
                    loginInfo.sessionid = response.Content.SessionId;
                    hasSessionId = true;
                }
                return response;
            }

            /// <summary>
            /// Closes the current session
            /// </summary>
            /// <returns>a LogoutResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<LogoutResponse> Logout()
            {
                if (!hasSessionId)
                {
                    LogoutResponse resp = new LogoutResponse();
                    resp.Status = 0;
                    resp.Content.Status = "already logged out";
                    return resp;
                }
                ++currentRequestNum;
                LogoutRequest req = new LogoutRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    LogoutResponse resp = new LogoutResponse();
                    resp.Status = 1;
                    resp.Content.error = status.error;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<LogoutResponse>(status.response);
                hasSessionId = false;
                return response;
            }

            /// <summary>
            /// Return an abstracted integer API version level, increased with each API functionality change. This is the proper way to detect host API functionality, instead of using getVersion()
            /// </summary>
            /// <returns>a GetApiLevelResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<GetApiLevelResponse> GetApiLevel()
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                GetApiLevelRequest req = new GetApiLevelRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    GetApiLevelResponse resp = new GetApiLevelResponse();
                    resp.Status = 1;
                    resp.Content.error = status.error;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<GetApiLevelResponse>(status.response);
                return response;
            }

            /// <summary>
            /// Returns tt-rss version. As of, 1.5.8 it is not recommended to use this to detect API functionality, please use getApiLevel() instead.
            /// </summary>
            /// <returns>a GetVersionResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<GetVersionResponse> GetVersion()
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                GetVersionRequest req = new GetVersionRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    GetVersionResponse resp = new GetVersionResponse();
                    resp.Status = 1;
                    resp.Content.error = status.error;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<GetVersionResponse>(status.response);
                return response;
            }

            /// <summary>
            /// Is true when the Caller object has successfully established a session
            /// There is no need to check as all methods implicitly try to establish a session
            /// but it must be checked in case of error.
            /// </summary>
            public bool hasSessionId { get; set; }

            /// <summary>
            /// Returns a status message with boolean value showing whether your current session id is active.
            /// </summary>
            /// <returns>a IsLoggedInResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<IsLoggedInResponse> IsLoggedIn()
            {
                if (!hasSessionId)
                {
                    IsLoggedInResponse resp = new IsLoggedInResponse();
                    resp.Status = 1;
                    resp.Content.Status = false;
                    return resp;
                }
                ++currentRequestNum;
                IsLoggedInRequest req = new IsLoggedInRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    IsLoggedInResponse resp = new IsLoggedInResponse();
                    resp.Status = 1;
                    resp.Content.error = status.error;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<IsLoggedInResponse>(status.response);
                return response;
            }

            /// <summary>
            /// Returns an integer value of currently unread articles.
            /// </summary>
            /// <returns>a GetUnreadResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<GetUnreadResponse> GetUnread()
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                GetUnreadRequest req = new GetUnreadRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    GetUnreadResponse resp = new GetUnreadResponse();
                    resp.Status = 1;
                    resp.Content.error = status.error;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<GetUnreadResponse>(status.response);
                return response;
            }

            /// <summary>
            /// Returns Counter information
            /// </summary>
            /// <param name="getFeeds">Get feed counters</param>
            /// <param name="getLabels">Get Labels counters</param>
            /// <param name="getCategories">Get Categories counters</param>
            /// <param name="getTags">Get Tags counters</param>
            /// <returns>a GetCountersResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<GetCountersResponse> GetCounters(bool getFeeds = true, bool getLabels = true, bool getCategories = true, bool getTags = false)
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                String mode = "";
                if (getFeeds)
                    mode += "f";
                if (getLabels)
                    mode += "l";
                if (getCategories)
                    mode += "c";
                if (getTags)
                    mode += "t";
                ++currentRequestNum;
                GetCountersRequest req = new GetCountersRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                req.mode = mode;
                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    GetCountersResponse resp = new GetCountersResponse();
                    resp.Status = 1;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<GetCountersResponse>(status.response);
                return response;
            }



            /// <summary>
            /// Returns a list of feeds. The list includes category id, title, feed url, etc.
            /// </summary>
            /// <param name="cat_id">return feeds under category cat_id
            /// The folowing cat_ids are special:
            ///  0 - uncategorized
            /// -1 - Special (Starred, Published, Archived)
            /// -2 - Labels
            /// -3 - All Feeds, excluding virtual feeds
            /// -4 - All Feeds, incuding virtual feeds
            /// </param>
            /// <param name="unread_only">only return feeds which have unread articles</param>
            /// <param name="limit">limit amount of feeds returned to this value. if different from 0, do not set unread_only</param>
            /// <param name="offset">skip this amount of feeds first. if different from 0, do not set unread_only</param>
            /// <param name="include_nested"> include child categories (as Feed objects with is_cat set)</param>
            /// <returns>a GetCountersResponse object with Status==0 if successful. Further data is available in the Content member.</returns>
            public async Task<GetFeedsResponse> GetFeeds(int cat_id, bool unread_only, int limit, int offset, bool include_nested)
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                GetFeedsRequest req = new GetFeedsRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                req.cat_id = cat_id;
                req.unread_only = unread_only;
                req.limit = limit;
                req.offset = offset;
                req.include_nested = include_nested;
                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    GetFeedsResponse resp = new GetFeedsResponse();
                    resp.Status = 1;
                    return resp;
                }
                if (((limit != 0) || (offset != 0)) && unread_only)
                {
                    GetFeedsResponse resp = new GetFeedsResponse();
                    resp.Status = 1;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<GetFeedsResponse>(status.response);
                return response;
            }

            /// <summary>
            /// returns a list of categories with unread counts.
            /// </summary>
            /// <param name="unread_only">if true, only return categories which have unread articles</param>
            /// <param name="enable_nested">if true, switch to nested mode, only returns topmost categories. 
            /// Nested mode in this case means that a flat list of only topmost categories is returned and unread 
            /// counters include counters for child categories. 
            /// This should be used as a starting point, to display a root list of all 
            /// (for backwards compatibility) or topmost categories, use getFeeds to traverse deeper.</param>
            /// <param name="include_empty">if true, include empty categories</param>
            /// <returns>a GetCategoriesResponse object with Status==0 if successful. Further data is available in the Content member.</returns>
            public async Task<GetCategoriesResponse> GetCategories(bool unread_only, bool enable_nested, bool include_empty)
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                GetCategoriesRequest req = new GetCategoriesRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                req.unread_only = unread_only;
                req.enable_nested = enable_nested;
                req.include_empty = include_empty;
                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    GetCategoriesResponse resp = new GetCategoriesResponse();
                    resp.Status = 1;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<GetCategoriesResponse>(status.response);
                return response;
            }

            /// <summary>
            /// View Mode for GetHeadlines
            /// </summary>
            public enum GetHeadlinesViewMode { AllArticles, UnreadOnly, Adaptive, MarkedOnly, UpdatedOnly };

            /// <summary>
            /// Sort Order for GetHeadlines
            /// </summary>
            public enum GetHeadlinesSortOrder { NoSort, SortByFeedDateOldestFirst, SortByFeedDateNewestFirst };

            /// <summary>
            /// Get a list of headlines 
            /// </summary>
            /// <param name="feed_id">only output articles for this feed.
            /// Special feed IDs are as follows:
            /// -1 - starred
            /// -2 - published
            /// -3 - fresh
            /// -4 - all articles
            /// 0 - archived
            /// IDs lower than -10 - labels</param>
            /// <param name="limit">limits the amount of returned articles. 
            /// Before API level 6 maximum amount of returned headlines is capped at 60, API 6 and above sets it to 200.</param>
            /// <param name="skip">skip this amount of feeds first</param>
            /// <param name="is_cat">requested feed_id is a category</param>
            /// <param name="show_excerpt">include article excerpt in the output</param>
            /// <param name="show_content">include full article text in the output</param>
            /// <param name="viewMode">limit amount of headlines to display</param>
            /// <param name="include_attachments">include article attachments (e.g. enclosures)</param>
            /// <param name="since_id">only return articles with id greater than since_id</param>
            /// <param name="include_nested">include articles from child categories</param>
            /// <param name="sanitize">sanitize content or not</param>
            /// <param name="sort_order">(optional) override default sort order</param>
            /// <returns>a GetHeadlinesResponse object with Status==0 if successful. Further data is available in the Content member.</returns>
            public async Task<GetHeadlinesResponse> GetHeadlines(int feed_id, int limit, int skip, bool is_cat, bool show_excerpt, bool show_content, GetHeadlinesViewMode viewMode, bool include_attachments, int since_id, bool include_nested, bool sanitize, GetHeadlinesSortOrder sort_order = GetHeadlinesSortOrder.NoSort)
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                GetHeadlinesRequest req = new GetHeadlinesRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                req.feed_id = feed_id;
                req.limit = limit;
                req.skip = skip;
                req.filter = "";
                req.is_cat = is_cat;
                req.show_excerpt = show_excerpt;
                req.show_content = show_content;
                req.sanitize = sanitize;
                req.include_attachments = include_attachments;
                req.since_id = since_id;
                req.include_nested = include_nested;
                switch (viewMode)
                {
                    case GetHeadlinesViewMode.Adaptive:
                        req.view_mode = "adaptive";
                        break;
                    case GetHeadlinesViewMode.AllArticles:
                        req.view_mode = "all_articles";
                        break;
                    case GetHeadlinesViewMode.MarkedOnly:
                        req.view_mode = "marked";
                        break;
                    case GetHeadlinesViewMode.UnreadOnly:
                        req.view_mode = "unread";
                        break;
                    case GetHeadlinesViewMode.UpdatedOnly:
                        req.view_mode = "updated";
                        break;
                }
                switch (sort_order)
                {
                    case GetHeadlinesSortOrder.NoSort:
                        req.order_by = "";
                        break;
                    case GetHeadlinesSortOrder.SortByFeedDateNewestFirst:
                        req.order_by = "feed_dates";
                        break;
                    case GetHeadlinesSortOrder.SortByFeedDateOldestFirst:
                        req.order_by = "date_reverse";
                        break;
                }

                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    GetHeadlinesResponse resp = new GetHeadlinesResponse();
                    resp.Status = 1;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<GetHeadlinesResponse>(status.response);
                return response;
            }

            /// <summary>
            /// Set Mode for UpdateArticle
            /// </summary>
            public enum UpdateArticleMode { SetFalse=0, SetTrue=1, Toggle=2 };

            /// <summary>
            /// Field To update for UpdateArticle
            /// </summary>
            public enum UpdateArticleField { Starred=0, Published=1, Unread=2, Note=3 };

            /// <summary>
            /// Update information on specified articles.
            /// </summary>
            /// <param name="article_ids">List of article ids</param>
            /// <param name="mode">type of operation to perform</param>
            /// <param name="field">field to operate on</param>
            /// <param name="data">optional data parameter when setting note field</param>
            /// <returns>a UpdateArticleResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<UpdateArticleResponse> UpdateArticle(List<int> article_ids, UpdateArticleMode mode, UpdateArticleField field, String data="")
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                UpdateArticleRequest req = new UpdateArticleRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                String articles = String.Join(",", article_ids);
                req.article_ids = articles;
                req.mode = (int)mode;
                req.field = (int)field;
                req.data = data;

                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    UpdateArticleResponse resp = new UpdateArticleResponse();
                    resp.Status = 1;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<UpdateArticleResponse>(status.response);
                return response;
            }

            /// <summary>
            /// "star" a list of articles
            /// </summary>
            /// <param name="article_ids">a list of article ids</param>
            /// <returns>a UpdateArticleResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<UpdateArticleResponse> starArticles(List<int> article_ids)
            {
                return await UpdateArticle(article_ids, UpdateArticleMode.SetTrue, UpdateArticleField.Starred, "");
            }

            /// <summary>
            /// "unstar" a list of articles
            /// </summary>
            /// <param name="article_ids">a list of article ids</param>
            /// <returns>a UpdateArticleResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<UpdateArticleResponse> UnstarArticles(List<int> article_ids)
            {
                return await UpdateArticle(article_ids, UpdateArticleMode.SetFalse, UpdateArticleField.Starred, "");
            }

            /// <summary>
            /// toggle star "star" on a list of articles
            /// </summary>
            /// <param name="article_ids">a list of article ids</param>
            /// <returns>a UpdateArticleResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<UpdateArticleResponse> ToggleStarOnArticles(List<int> article_ids)
            {
                return await UpdateArticle(article_ids, UpdateArticleMode.Toggle, UpdateArticleField.Starred, "");
            }

            /// <summary>
            /// publish a list of articles
            /// </summary>
            /// <param name="article_ids">a list of article ids</param>
            /// <returns>a UpdateArticleResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<UpdateArticleResponse> PublishArticles(List<int> article_ids)
            {
                return await UpdateArticle(article_ids, UpdateArticleMode.SetTrue, UpdateArticleField.Published, "");
            }

            /// <summary>
            /// unpublish a list of articles
            /// </summary>
            /// <param name="article_ids">a list of article ids</param>
            /// <returns>a UpdateArticleResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<UpdateArticleResponse> UnpublishArticles(List<int> article_ids)
            {
                return await UpdateArticle(article_ids, UpdateArticleMode.SetFalse, UpdateArticleField.Published, "");
            }

            /// <summary>
            /// toggle publication on a list of articles
            /// </summary>
            /// <param name="article_ids">a list of article ids</param>
            /// <returns>a UpdateArticleResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<UpdateArticleResponse> TogglePublishedOnArticles(List<int> article_ids)
            {
                return await UpdateArticle(article_ids, UpdateArticleMode.Toggle, UpdateArticleField.Published, "");
            }

            /// <summary>
            /// set a list of articles as unread
            /// </summary>
            /// <param name="article_ids">a list of article ids</param>
            /// <returns>a UpdateArticleResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<UpdateArticleResponse> SetArticlesAsUnread(List<int> article_ids)
            {
                return await UpdateArticle(article_ids, UpdateArticleMode.SetTrue, UpdateArticleField.Unread, "");
            }

            /// <summary>
            /// set a list of articles as read
            /// </summary>
            /// <param name="article_ids">a list of article ids</param>
            /// <returns>a UpdateArticleResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<UpdateArticleResponse> SetArticlesAsRread(List<int> article_ids)
            {
                return await UpdateArticle(article_ids, UpdateArticleMode.SetFalse, UpdateArticleField.Unread, "");
            }

            /// <summary>
            /// toggle read status on a list of articles
            /// </summary>
            /// <param name="article_ids">a list of article ids</param>
            /// <returns>a UpdateArticleResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<UpdateArticleResponse> ToggleReadOnArticles(List<int> article_ids)
            {
                return await UpdateArticle(article_ids, UpdateArticleMode.Toggle, UpdateArticleField.Unread, "");
            }

            /// <summary>
            /// set note on a list of articles
            /// </summary>
            /// <param name="article_ids">a list of article ids</param>
            /// <param name="note">the note text</param>
            /// <returns>a UpdateArticleResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<UpdateArticleResponse> SetNoteOnArticles(List<int> article_ids, String note)
            {
                return await UpdateArticle(article_ids, UpdateArticleMode.SetTrue, UpdateArticleField.Note, note);
            }

            /// <summary>
            /// clear notes on a list of articles
            /// </summary>
            /// <param name="article_ids">a list of article ids</param>
            /// <returns>a UpdateArticleResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<UpdateArticleResponse> ClearNoteOnArticles(List<int> article_ids)
            {
                return await UpdateArticle(article_ids, UpdateArticleMode.SetFalse, UpdateArticleField.Note, "");
            }

            /// <summary>
            /// Obtain all information on an article, including attachments
            /// </summary>
            /// <param name="article_ids">a list of article ids</param>
            /// <returns>a GetArticleResponse object with Status==0 if successful. Further data is available in the Content member.</returns>
            public async Task<GetArticleResponse> GetArticles(List<int> article_ids)
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                GetArticleRequest req = new GetArticleRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                String articles = String.Join(",", article_ids);
                req.article_ids = articles;

                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    GetArticleResponse resp = new GetArticleResponse();
                    resp.Status = 1;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<GetArticleResponse>(status.response);
                return response;
            }

            /// <summary>
            /// Returns the following tt-rss configuration parameters:
            /// icons_dir - path to icons on the server filesystem
            /// icons_url - path to icons when requesting them over http
            /// daemon_is_running - whether update daemon is running
            /// num_feeds - amount of subscribed feeds (this can be used to refresh feedlist when this amount changes)
            /// </summary>
            /// <returns>a GetConfigResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<GetConfigResponse> GetConfig()
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                GetConfigRequest req = new GetConfigRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;

                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    GetConfigResponse resp = new GetConfigResponse();
                    resp.Status = 1;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<GetConfigResponse>(status.response);
                return response;
            }

            /// <summary>
            /// Tries to update specified feed. This operation is not performed in the background, 
            /// so it might take considerable time and, potentially, be aborted by the HTTP server.
            /// </summary>
            /// <param name="feed_id">ID of feed to update</param>
            /// <returns>a UpdateFeedResponse object with Status==0 and status-message if the operation has been completed. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<UpdateFeedResponse> UpdateFeed(int feed_id)
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                UpdateFeedRequest req = new UpdateFeedRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                req.feed_id = feed_id;

                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    UpdateFeedResponse resp = new UpdateFeedResponse();
                    resp.Status = 1;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<UpdateFeedResponse>(status.response);
                return response;
            }

            /// <summary>
            /// Returns preference value of specified key
            /// </summary>
            /// <param name="pref_name">preference key to return value of</param>
            /// <returns>a GetPrefResponse object with Status==0 if successful. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<GetPrefResponse> GetPref(String pref_name)
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                GetPrefRequest req = new GetPrefRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                req.pref_name = pref_name;

                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    GetPrefResponse resp = new GetPrefResponse();
                    resp.Status = 1;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<GetPrefResponse>(status.response);
                return response;
            }

            /// <summary>
            /// Tries to catchup (i.e. mark as read) the specified feed.
            /// </summary>
            /// <param name="feed_id">ID of feed to update</param>
            /// <param name="is_cat">true if the specified feed_id is a category</param>
            /// <returns>a CatchupFeedResponse object with Status==0 and status-message if the operation has been completed. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<CatchupFeedResponse> CatchupFeed(int feed_id, bool is_cat)
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                CatchupFeedRequest req = new CatchupFeedRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                req.feed_id = feed_id;
                req.is_cat = is_cat;

                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    CatchupFeedResponse resp = new CatchupFeedResponse();
                    resp.Status = 1;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<CatchupFeedResponse>(status.response);
                return response;
            }

            /// <summary>
            /// Returns list of configured labels
            /// </summary>
            /// <param name="article_id">if set, set "checked" to true if specified article id has returned label</param>
            /// <returns>a GetArticleResponse object with Status==0 if successful. Further data is available in the Content member.</returns>
            public async Task<GetLabelsResponse> GetLabels(int article_id = 0)
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                GetLabelsRequest req = new GetLabelsRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                req.article_id = article_id;

                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    GetLabelsResponse resp = new GetLabelsResponse();
                    resp.Status = 1;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<GetLabelsResponse>(status.response);
                return response;
            }

            /// <summary>
            /// Assigns article_ids to specified label.
            /// </summary>
            /// <param name="article_ids">list of article ids</param>
            /// <param name="label_id">id of label, as returned by getLabels</param>
            /// <param name="assign">assign or remove label</param>
            /// <returns>a SetArticleLabelResponse object with Status==0 if the operation has been completed. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<SetArticleLabelResponse> SetArticleLabel(List<int> article_ids, int label_id, bool assign)
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                SetArticleLabelRequest req = new SetArticleLabelRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                String articles = String.Join(",", article_ids);
                req.article_ids = articles;
                req.label_id = label_id;
                req.assign = assign;

                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    SetArticleLabelResponse resp = new SetArticleLabelResponse();
                    resp.Status = 1;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<SetArticleLabelResponse>(status.response);
                return response;
            }

            /// <summary>
            /// Creates an article with specified data in the Published feed.
            /// </summary>
            /// <param name="title">Article title</param>
            /// <param name="url">Article url</param>
            /// <param name="content">Article content</param>
            /// <returns>a ShareToPublishedResponse object with Status==0 if the operation has been completed. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<ShareToPublishedResponse> ShareToPublished(String title, String url, String content)
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                ShareToPublishedRequest req = new ShareToPublishedRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                req.title = title;
                req.url = url;
                req.content = content;

                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    ShareToPublishedResponse resp = new ShareToPublishedResponse();
                    resp.Status = 1;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<ShareToPublishedResponse>(status.response);
                return response;
            }

            /// <summary>
            /// Subscribes to specified feed, returns a status code.
            /// </summary>
            /// <param name="feed_url">Feed url</param>
            /// <param name="category_id">Category to place feed into (0=uncategorized)</param>
            /// <param name="login">login info for feed</param>
            /// <param name="password">password for feed</param>
            /// <returns>a SubscribeToFeedResponse object with Status==0 if the operation has been completed. 
            /// Further data is available in the Content member. 
            /// Errors are available in the Content.error member
            /// Status codes meaning:
            /// 0 - OK, Feed already exists
            /// 1 - OK, Feed added
            /// 2 - Invalid URL
            /// 3 - URL content is HTML, no feeds available
            /// 4 - URL content is HTML which contains multiple feeds.
            /// 5 - Couldn't download the URL content.
            /// 6 - Content is an invalid XML.
            /// </returns>
            public async Task<SubscribeToFeedResponse> SubscribeToFeed(String feed_url, int category_id, String login, String password)
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                SubscribeToFeedRequest req = new SubscribeToFeedRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                req.feed_url = feed_url;
                req.category_id = category_id;
                req.login = login;
                req.password = password;

                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    SubscribeToFeedResponse resp = new SubscribeToFeedResponse();
                    resp.Status = 1;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<SubscribeToFeedResponse>(status.response);
                return response;
            }

            /// <summary>
            /// Unsubscribes specified feed.
            /// </summary>
            /// <param name="feed_id">Feed id to unsubscribe from</param>
            /// <returns>a UnsubscribeFeedResponse object with Status==0 if the operation has been completed. Further data is available in the Content member. Errors are available in the Content.error member</returns>
            public async Task<UnsubscribeFeedResponse> UnsubscribeFeed(int feed_id)
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                UnsubscribeFeedRequest req = new UnsubscribeFeedRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                req.feed_id = feed_id;

                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    UnsubscribeFeedResponse resp = new UnsubscribeFeedResponse();
                    resp.Status = 1;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<UnsubscribeFeedResponse>(status.response);
                return response;
            }

            /// <summary>
            /// Returns full tree of categories and feeds.
            /// </summary>
            /// <param name="include_empty"></param>
            /// <returns>a GetFeedTreeResponse object with Status==0 if the operation has been completed. 
            /// Further data is available in the Content member.</returns>
            public async Task<GetFeedTreeResponse> GetFeedTree(bool include_empty)
            {
                if (!hasSessionId)
                {
                    await Login();
                }
                ++currentRequestNum;
                GetFeedTreeRequest req = new GetFeedTreeRequest(currentRequestNum);
                req.sid = loginInfo.sessionid;
                req.include_empty = include_empty;

                string post = JsonConvert.SerializeObject(req);
                ResponseStatus status = await requestToServer(post);
                if (!status.ok)
                {
                    GetFeedTreeResponse resp = new GetFeedTreeResponse();
                    resp.Status = 1;
                    return resp;
                }
                var response = JsonConvert.DeserializeObject<GetFeedTreeResponse>(status.response);
                return response;
            }
            #endregion

            #region Private Methods and attributes

            private TtRssLoginInfo loginInfo;
            private int currentRequestNum;

            class ResponseStatus
            {
                public String response;
                public bool ok;
                public String error;
            }

            private async Task<ResponseStatus> requestToServer(String postRequest)
            {
                try
                {
                    var client = new WebClient();
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string HtmlResult = await client.UploadStringTaskAsync(loginInfo.apiUri, postRequest);
                    ResponseStatus resp = new ResponseStatus();
                    resp.ok = true;
                    resp.response = HtmlResult;
                    return resp;
                }
                catch (ArgumentNullException e1)
                {
                    ResponseStatus resp = new ResponseStatus();
                    resp.ok = false;
                    resp.error = e1.ToString();
                    return resp;
                }
                catch (WebException e2)
                {
                    ResponseStatus resp = new ResponseStatus();
                    resp.ok = false;
                    resp.error = e2.ToString();
                    return resp;
                }

            }
            #endregion

        }
    }
}