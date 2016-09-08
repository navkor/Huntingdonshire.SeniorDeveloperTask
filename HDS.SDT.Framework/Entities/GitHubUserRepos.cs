using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HDS.SDT.Framework.Entities
{
    public class GitHubUserRepos
    {
        [Display(Name = "id")]
        public int id { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "Full Name")]
        public string full_name { get; set; }
        [Display(Name = "Private Repo")]
        [JsonProperty("private")]
        public bool IsPrivate { get; set; }
        [Display(Name = "Repo Link")]
        public string html_url { get; set; }
        [Display(Name = "Repo Description")]
        public string description { get; set; }
        [Display(Name = "Repo Created Date")]
        public DateTime created_at { get; set; }
        [Display(Name = "Repo Updated Date")]
        public DateTime updated_at { get; set; }


        [Display(Name = "repo owner information")]
        public virtual GitHubUserRepoOwner owner { get; set; }

        public virtual GitHubUser GitHubUser { get; set; }
    }
}