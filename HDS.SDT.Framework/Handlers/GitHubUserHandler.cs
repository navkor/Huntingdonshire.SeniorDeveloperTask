using HDS.SDT.Framework.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace HDS.SDT.Framework.Handlers
{
    /*
     This class is the central handler for the github search.  This takes the place of the interface/repository system for a MSSQL-Entity data model.  Since we are only
     dealing with a single source with only a few return types, an interface/repository (though it would properly demontrate Inversion of Control, was overly
     complex and wouldn't allow for good DRY development.
         */
    public class GitHubUserHandler
    {
        /*
         these handlers are the public viewable handlers for the internal ones below.  They are a shorthand version of a delegate without the added complexity.
             */
        GitHubAPIHandlers _handlers;

        public GitHubUserHandler()
        {
            _handlers = new GitHubAPIHandlers();
        }
        public IEnumerable<GitHubUser> ListGitHubUsersHandler(string name)
        {
            return _handlers.GitHubAPICallSearch(name);
        }

        public GitHubUser SingleGitHubUserHandler(string name)
        {
            return _handlers.GitHubAPICallSingle(name);
        }

        public IEnumerable<GitHubUserRepos> ListGitHubUserRepos(string name)
        {
            return _handlers.GitHubAPIReposSearch(name);
        }

        public List<string> ListGitHubUserNames(string name)
        {
            var users = _handlers.GitHubAPICallSearch(name);
            var list = new List<string>();

            foreach(var item in users)
            {
                list.Add(item.login);
            }
            return list;
        }
    }


    internal class GitHubAPIHandlers
    {
        /*
         This call fires while the user is typing the name in the search box.  It returnes a list of possible matches.  The user can then select from there.  Similar to the GitHubAPICallSingle
         this calls the GitHubAPICallMaker, but does so as a search rather than a name lookup.  The HTTP header is different.  The Serialized JSON string is then returned as an array of possible
         matches.  This array is deserialized into an anonymous object of items = List of GitHubUser.  Then, a for loop (because foreach doesn't work on anonymous types due to no Enumeration)
         each result is then added ot the githubuserlist.  Which is returned to the controller.
             */
        public IEnumerable<GitHubUser> GitHubAPICallSearch(string name)
        {
            String result = GitHubAPICallMaker(name, true, false);
            var githubuserList = new List<GitHubUser>();
            if (!String.IsNullOrEmpty(result))
            {
                var itemArray = new { items = new List<GitHubUser>() };
                var itemsList = JsonConvert.DeserializeAnonymousType(result, itemArray);
                for (int i = 0; i < itemsList.items.Count(); i++)
                {
                    githubuserList.Add(itemsList.items[i]);
                }
            }
            return githubuserList;
        }

        /*
         Quite possibly the easies of the GitHubAPIHandlers.  It simply sends a call to GitHubAPICallMaker with the name entered into the search box.  This is not the JQuery AutoComplete call, but what fires AFTER that had returned the list, and the user selected that name from the available list.  The returned JSON serialized string is then cast to a GitHubUser and returned as a single object.
             */
        public GitHubUser GitHubAPICallSingle(string name)
        {
            String result = GitHubAPICallMaker(name, false, false);
            var gitHubUser = new GitHubUser();
            if (!String.IsNullOrEmpty(result))
            {
                gitHubUser = JsonConvert.DeserializeObject<GitHubUser>(result);
            }
            return gitHubUser;
        }

        /*
         this method here returns the GitHubUserRepos in an IEnumerable collection.  It calls the GitHubAPICallMaker below passing in the name that the GitHubAPICallSingle returned.  Then, it
         creates an empty JsonArray from NewtonSoft, does an AnonymousType deserialization (also from NewtonSoft), and returns that into the repoDeserialized anonymous object.
         a foreach loop goes through each entry in the repoDeserialized object containing the JsonArray.  Then casts that item into a GitHubUserRepos class, and adds it to the
         gitHubRepoList.  Once the foreach finishes, it returns the gitHubRepoList.
             */
        public IEnumerable<GitHubUserRepos> GitHubAPIReposSearch(string name)
        {
            String result = GitHubAPICallMaker(name, false, true);
            var gitHubRepoList = new List<GitHubUserRepos>();
            if (!String.IsNullOrEmpty(result))
            {
                var repoList = new JArray();
                var repoDeserialized = JsonConvert.DeserializeAnonymousType(result, repoList);

                foreach (var item in repoDeserialized.Children())
                {
                    gitHubRepoList.Add(JsonConvert.DeserializeObject<GitHubUserRepos>(item.ToString()));
                }
            }
            return gitHubRepoList;
        }

        /*
         this method does all the easy lifting.  Once the call had been properly filtered through the refering methods, this one makes the API call and reads the data, returning it
         back to the refering method as a JSON serialized string.  Then, it's up to the method to deserialize and match the content to the entity.  I kept this separate to adhere to
         good DRY programming practices.
             */
        public String GitHubAPICallMaker(string name, bool search, bool repos)
        {
            String result = String.Empty;
            string baseAddress = "";
            if (search)
            {
                baseAddress = $"https://api.github.com/search/users?q={name}";
            }
            else
            {
                baseAddress = repos ? $"https://api.github.com/users/{name}/repos" : $"https://api.github.com/users/{name}";
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(baseAddress);
            request.Method = "GET";
            if (search)
            {
                request.Accept = "application/vnd.github.v3.text-match+json";
            }
            request.ContentType = "application/json";
            request.UserAgent = "HuntingdonShire Senior Development API";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    result = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                }
            } catch
            {
                return "";
            }
            return result;
        }
    }
}