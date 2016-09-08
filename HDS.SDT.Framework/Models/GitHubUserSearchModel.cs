using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HDS.SDT.Framework.Models
{
    /*
     * The purpose for this class is to give me a basic model I could use to search GitHub and have form validation on the search form
     * rather than using the whole user model.  This was a cleaner solution that provided exactly what I'm looking for.
     *
     */
    public class GitHubUserSearchModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Account Login")]
        public string login { get; set; }
    }
}