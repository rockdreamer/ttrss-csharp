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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TTRss.Api;
using TTRss.Api.Commands;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TTRss
{
    namespace Api
    {
        namespace Test
        {

            [TestClass]
            public class ApiTests
            {
                TtRssLoginInfo info;
                String unsubscribeFeed = "http://tt-rss.org/forum/feed.php?f=10&t=2030";

                public ApiTests()
                {
                    info = new TtRssLoginInfo();
                    info.username = "set-yours";
                    info.password = "set-yours";
                    info.rootUri = "set-yours";
                }

                [TestMethod]
                public async Task TestLogin()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    await api.Login();
                    Assert.IsTrue(api.hasSessionId);
                }

                [TestMethod]
                public async Task TestLogout()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    await api.Login();
                    Assert.IsTrue(api.hasSessionId);
                    var logout = await api.Logout();
                    Assert.IsFalse(api.hasSessionId);
                    Assert.AreEqual(logout.Status, 0);
                }

                [TestMethod]
                public async Task TestGetApiLevel()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var level = await api.GetApiLevel();
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(level.Status, 0);
                    Assert.IsTrue(level.Content.Level >= 5);
                }

                [TestMethod]
                public async Task TestGetVersion()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var level = await api.GetVersion();
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(level.Status, 0);
                    Assert.IsTrue(level.Content.Version.Length != 0);
                }

                [TestMethod]
                public async Task TestGetUnread()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var level = await api.GetUnread();
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(level.Status, 0);
                    Assert.IsTrue(level.Content.Unread.Length != 0);
                    int num = Convert.ToInt32(level.Content.Unread);
                    Assert.IsTrue(num > 0);
                }

                [TestMethod]
                public async Task TestGetCounters()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var counters = await api.GetCounters();
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(counters.Status, 0);
                }

                [TestMethod]
                public async Task TestGetFeeds1()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var feeds = await api.GetFeeds(-1, false, 10, 0, true);
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(feeds.Status, 0);
                }

                [TestMethod]
                public async Task TestGetCategories1()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var cats = await api.GetCategories(false, true, true);
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(cats.Status, 0);
                }

                [TestMethod]
                public async Task TestGetHeadlines1()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var heads = await api.GetHeadlines(-4, 10, 0, true, true, true, Caller.GetHeadlinesViewMode.AllArticles, true, 0, true, true);
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(heads.Status, 0);
                }

                [TestMethod]
                public async Task TestGetArticle()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var heads = await api.GetArticles(new List<int>(new int[] { 100, 200, 130 }));
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(heads.Status, 0);
                    Assert.AreEqual(heads.Content.Length, 3);
                }

                [TestMethod]
                public async Task TestUpdateArticle()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var heads = await api.UpdateArticle(new List<int>(new int[] { 100, 200, 130 }), Caller.UpdateArticleMode.Toggle, Caller.UpdateArticleField.Starred);
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(heads.Status, 0);
                    Assert.AreEqual(heads.Content.Updated, 3);
                    heads = await api.UpdateArticle(new List<int>(new int[] { 100, 200, 130 }), Caller.UpdateArticleMode.Toggle, Caller.UpdateArticleField.Starred);
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(heads.Status, 0);
                    Assert.AreEqual(heads.Content.Updated, 3);
                    heads = await api.UpdateArticle(new List<int>(new int[] { 100 }), Caller.UpdateArticleMode.SetFalse, Caller.UpdateArticleField.Note);
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(heads.Status, 0);
                    heads = await api.UpdateArticle(new List<int>(new int[] { 100 }), Caller.UpdateArticleMode.SetTrue, Caller.UpdateArticleField.Note, "test-added note");
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(heads.Status, 0);
                    Assert.AreEqual(heads.Content.Updated, 1);
                    var article = await api.GetArticles(new List<int>(new int[] { 100 }));
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(article.Status, 0);
                    Assert.AreEqual(article.Content.Length, 1);
                }

                [TestMethod]
                public async Task TestGetConfig()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var heads = await api.GetConfig();
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(heads.Status, 0);
                }

                [TestMethod]
                public async Task TestUpdateFeed()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var heads = await api.UpdateFeed(-1);
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(heads.Status, 0);
                }

                [TestMethod]
                public async Task TestGetPref()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var heads = await api.GetPref("ENABLE_FEED_CATS");
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(heads.Status, 0);
                }

                [TestMethod]
                public async Task TestCatchupFeed()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var heads = await api.CatchupFeed(1, false);
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(heads.Status, 0);
                }

                [TestMethod]
                public async Task TestGetLabels()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var heads = await api.GetLabels(100);
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(heads.Status, 0);
                }

                [TestMethod]
                public async Task TestSetArticleLabel()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var heads = await api.SetArticleLabel(new List<int>(new int[] { 100, 200, 130 }), 1, false);
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(heads.Status, 0);
                }

                [TestMethod]
                public async Task UnsubscribeFeed()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var trysubscription = await api.SubscribeToFeed(unsubscribeFeed, 0, "", "");
                    var feeds = await api.GetFeeds(0, false, 200, 0, true);
                    bool found = false;
                    int feed_id = 0;
                    foreach (GetFeedsResponseContent c in feeds.Content)
                    {
                        if (c.FeedUrl.Equals(unsubscribeFeed))
                        {
                            found = true;
                            feed_id = c.Id;
                            break;
                        }
                    }
                    Assert.IsTrue(found);
                    var heads = await api.UnsubscribeFeed(feed_id);
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(heads.Status, 0);
                    feeds = await api.GetFeeds(0, false, 200, 0, true);
                    found = false;
                    foreach (GetFeedsResponseContent c in feeds.Content)
                    {
                        if (c.FeedUrl.Equals(unsubscribeFeed))
                        {
                            found = true;
                            break;
                        }
                    }
                    Assert.IsFalse(found);
                }

                [TestMethod]
                public async Task TestSubscribeToFeed()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var heads = await api.SubscribeToFeed("http://tt-rss.org", 0, "", "");
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(heads.Status, 0);
                }

                [TestMethod]
                public async Task TestShareToPublished()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var heads = await api.ShareToPublished("test", "http://tt-rss.org", "test");
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(heads.Status, 0);
                }

                [TestMethod]
                public async Task TestGetFeedTree()
                {
                    Caller api = new Caller(info);
                    Assert.IsNotNull(api);
                    Assert.IsFalse(api.hasSessionId);
                    var heads = await api.GetFeedTree(true);
                    Assert.IsTrue(api.hasSessionId);
                    Assert.AreEqual(heads.Status, 0);
                }

            }
        }
    }
}