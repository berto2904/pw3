﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace TrabajoPracticoPw3.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/validacion.js",
                        "~/Scripts/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/Site").Include(
                "~/Content/Site.css",
                "~/Content/bootstrap.min.css",
                "~/Content/templateEmpanada/css/half-slider.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/fastSearchSelect").Include(
                       "~/Content/fastsearch-master/dist/fastsearch.min.js",
                       "~/Content/fastselect-master/dist/fastselect.js"));

            bundles.Add(new StyleBundle("~/Content/fastselect").Include(
                "~/Content/fastselect-master/dist/fastselect.css"
                ));

            bundles.Add(new StyleBundle("~/Content/font-awesome").Include(
                 "~/Content/font-awesome.min.css"
                ));


               
           


               //bundles.Add(new ScriptBundle("~/bundles/Site").Include(
               //          "~/Scripts/bootstrap.js",
               //          "~/Scripts/respond.js"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}