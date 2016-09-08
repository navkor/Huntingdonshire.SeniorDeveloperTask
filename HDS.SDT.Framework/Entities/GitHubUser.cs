using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HDS.SDT.Framework.Entities
{
    public class GitHubUser
    {
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "Login")]
        public string login { get; set; }
        [Display(Name = "Location")]
        public string location { get; set; }
        [Display(Name = "Avatar")]
        public string avatar_url { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string email { get; set; }
        [Display(Name = "Biography")]
        public string bio { get; set; }
        [Display(Name = "Public Repos Count")]
        public int public_repos { get; set; }

        [Display(Name = "Repo List")]
        public virtual ICollection<GitHubUserRepos> GitHubUserRepos { get; set; }
    }
}