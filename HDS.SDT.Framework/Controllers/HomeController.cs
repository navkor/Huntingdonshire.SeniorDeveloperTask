using HDS.SDT.Framework.Entities;
using HDS.SDT.Framework.Handlers;
using HDS.SDT.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HDS.SDT.Framework.Controllers
{
    public class HomeController : Controller
    {
        private readonly GitHubUserHandler _userHandler;

        public HomeController()
        {
            _userHandler = new GitHubUserHandler();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new GitHubUserSearchModel();
            ViewData["Step"] = "Search";
            return View(model);
        }

        public ActionResult Users(GitHubUser user = null)
        {
            ViewData["Step"] = "View";
            return View(user);
        }

        // we're using HttpPost here for the JQuery Autocomplete function so it doesn't fire if someone just passes a query string with term in it
        [HttpPost]
        public JsonResult Index(string term)
        {
            return Json(_userHandler.ListGitHubUserNames(term));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Search(GitHubUserSearchModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userHandler.SingleGitHubUserHandler(model.login);
                // let's check to make sure something actually came back
                if (!String.IsNullOrEmpty(user.login))
                {
                    user.GitHubUserRepos = _userHandler.ListGitHubUserRepos(model.login).ToList();
                    return View("Users", user);
                } else
                {
                    // it came back empty, so let's send them back to the form and try the search again
                    ModelState.AddModelError("", "That account doesn't exist.  Please try again.");
                    return View("Index", model);
                }
            }
            return View("Index", model);
        }
    }
}