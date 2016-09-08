using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HDS.SDT.Framework.Entities
{
    public class GitHubUserRepoOwner
    {
        [Display(Name = "Login")]
        public string login { get; set; }
        [Display(Name = "Avatar")]
        public string avatar_url { get; set; }
        [Display(Name = "GitHub Home")]
        public string html_url { get; set; }
        [Display(Name = "Owner Type")]
        public string type { get; set; }

        public virtual GitHubUserRepos gitHubUserRepo { get; set; }
    }
}