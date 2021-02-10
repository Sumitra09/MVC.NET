using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;
namespace NPBank.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundle(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Content/jquery/jquery-{version}.js",
                "~/Content/jquery/jquery.validate*"
            ));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                "~/Content/kendo2020/kendo.all.min.js",
                "~/Content/kendo2020/jszip.min.js",
                "~/Content/kendo2020/kendo.aspnetmvc.min.js",
                 "~/Content/kendo2020/kendo.timezones.min.js",
                "~/Content/kendo2020/modernizr-2.6.2.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/script").Include(
                "~/Content/jquery/jquery.form.js",
               "~/Content/bootstrap/bootstrap.js",
               "~/Content/bootstrap/bootbox.js",
               "~/Content/toastr/toastr.js",
               "~/Content/toastr/customalert.js"
            ));
            bundles.Add(new StyleBundle("~/bundles/style")
                .Include("~/Content/kendo2020/kendo.common-material.min.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/kendo2020/kendo.material.min.css", new CssRewriteUrlTransformWrapper())
                 .Include("~/Content/kendo2020/kendo.common.min.css", new CssRewriteUrlTransformWrapper())
                 .Include("~/Content/kendo2020/kendo.bootstrap.min.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/bootstrap/bootstrap.min.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/toastr/toastr.min.css", new CssRewriteUrlTransformWrapper())
            );
        }
        public class CssRewriteUrlTransformWrapper : IItemTransform
        {
            private static string RebaseUrlToAbsolute(string baseUrl, string url, string prefix, string suffix)
            {
                if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(baseUrl) || url.StartsWith("/", StringComparison.OrdinalIgnoreCase) || url.StartsWith("http://") || url.StartsWith("https://"))
                {
                    return url;
                }
                if (url.StartsWith("data:"))
                {
                    return prefix + url + suffix;
                }
                if (!baseUrl.EndsWith("/", System.StringComparison.OrdinalIgnoreCase))
                {
                    baseUrl += "/";
                }
                return VirtualPathUtility.ToAbsolute(baseUrl + url);
            }
            private static string ConvertUrlsToAbsolute(string baseUrl, string content)
            {
                if (string.IsNullOrWhiteSpace(content))
                {
                    return content;
                }

                var regex = new Regex("url\\((?<prefix>['\"]?)(?<url>[^)]+?)(?<suffix>['\"]?)\\)");

                return regex.Replace(content, (Match match) => "url(" + CssRewriteUrlTransformWrapper.RebaseUrlToAbsolute(baseUrl, match.Groups["url"].Value, match.Groups["prefix"].Value, match.Groups["suffix"].Value) + ")");
            }
            public string Process(string includedVirtualPath, string input)
            {
                if (includedVirtualPath == null)
                {
                    throw new ArgumentNullException("includedVirtualPath");
                }
                if (includedVirtualPath.Length < 1 || includedVirtualPath[0] != '~')
                {
                    throw new ArgumentException("includedVirtualPath must be valid ( i.e. have a length and start with ~ )");
                }

                var directory = VirtualPathUtility.GetDirectory(includedVirtualPath);

                return CssRewriteUrlTransformWrapper.ConvertUrlsToAbsolute(directory, input);
            }
        }
       
    }
}