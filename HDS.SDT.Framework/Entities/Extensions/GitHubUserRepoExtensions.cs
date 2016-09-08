using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace HDS.SDT.Framework.Entities.Extensions
{
    /*
     a list of Method extensions for some cleaner Razor code in the view.  Granted, the exact same thing could have been accomplished in Razor...but then we wouldn't have the awesome
     class, and our Razor code would look a little dirtier.  This follows the MVC separation of concerns methodology perfectly.
         */
    public static class GitHubUserRepoExtensions
    {
        // because TRUE FALSE just looks so cold
        public static string BoolToYes(this bool boolean)
        {
            return boolean ? "Yes" : "No";
        }

        // because GitHub deals in ZULU time...
        public static string DateToZulu(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }

        // because Razor, though capable of creating and working with HTML, should really focus on returning model values.  So, I'm letting the model
        // do the heavy lifting of converting the string to an HTML A tag
        public static string StringToURL(this string s)
        {
            StringWriter stringWriter = new StringWriter();
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Href, s);
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(s);
                writer.RenderEndTag();
            }
            return stringWriter.ToString();
        }

        // does the same as above, only with an Img tag...one small, and one medium.
        public static string StringToSmallIMG(this string s)
        {
            ImageGenerator i = new ImageGenerator();
            return i.imageGenerator(s, "90");
        }

        public static string StringToMediumIMG(this string s)
        {
            ImageGenerator i = new ImageGenerator();
            return i.imageGenerator(s, "120");
        }

    }

    internal class ImageGenerator
    {
        public string imageGenerator(string s, string width)
        {
            StringWriter stringWriter = new StringWriter();
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, s);
                writer.AddAttribute(HtmlTextWriterAttribute.Width, width);
                writer.AddAttribute(HtmlTextWriterAttribute.Alt, "Account Avatar Url");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
            }
            return stringWriter.ToString();
        }
    }
}