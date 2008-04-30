﻿using System;
using System.Web.Mvc;
using System.Web.UI;
using System.IO;
using Suteki.Shop.ViewData;
using Suteki.Shop.Controllers;
using Suteki.Shop.Extensions;
using System.Security.Principal;
using System.Linq.Expressions;

namespace Suteki.Shop.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        public static string LoginStatus(this HtmlHelper htmlHelper)
        {
            HtmlTextWriter writer = new HtmlTextWriter(new StringWriter());

            writer.AddAttribute("id", "loginStatus");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.Write(htmlHelper.CurrentUser().PublicIdentity);
            
            writer.RenderEndTag();
            
            return writer.InnerWriter.ToString();
        }

        public static string LoginLink(this HtmlHelper htmlHelper)
        {
            if (htmlHelper.CurrentUser().CanLogin)
            {
                return htmlHelper.ActionLink<LoginController>(c => c.Logout(), "Logout");
            }
            else
            {
                return htmlHelper.ActionLink<LoginController>(c => c.Index(), "Login");
            }
        }

        public static User CurrentUser(this HtmlHelper htmlHelper)
        {
            User user = htmlHelper.ViewContext.HttpContext.User as User;
            if (user == null) throw new ApplicationException("Current context user cannot be cast to Suteki.Shop.User");
            return user;
        }

        /// <summary>
        /// Render an error box 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string ErrorBox(this HtmlHelper htmlHelper, IErrorViewData errorViewData)
        {
            if (errorViewData.ErrorMessage == null) return string.Empty;

            HtmlTextWriter writer = new HtmlTextWriter(new StringWriter());

            writer.AddAttribute("class", "error");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write(errorViewData.ErrorMessage);
            writer.RenderEndTag();
            return writer.InnerWriter.ToString();
        }

        public static string MessageBox(this HtmlHelper htmlHelper, IMessageViewData messageViewData)
        {
            if (messageViewData.Message == null) return string.Empty;

            HtmlTextWriter writer = new HtmlTextWriter(new StringWriter());

            writer.AddAttribute("class", "message");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write(messageViewData.Message);
            writer.RenderEndTag();
            return writer.InnerWriter.ToString();
        }

        public static string Tick(this HtmlHelper htmlHelper, bool ticked)
        {
            HtmlTextWriter writer = new HtmlTextWriter(new StringWriter());

            if (ticked)
            {
                writer.AddAttribute("class", "tick");
            }
            else
            {
                writer.AddAttribute("class", "cross");
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write("&nbsp;");
            writer.RenderEndTag();
            return writer.InnerWriter.ToString();
        }

        public static string WriteCategories(this HtmlHelper htmlHelper, Category rootCategory)
        {
            CategoryWriter categoryWriter = new CategoryWriter(rootCategory, htmlHelper);
            return categoryWriter.Write();
        }

        public static string UpArrowLink<T>(this HtmlHelper htmlHelper, Expression<Action<T>> action) where T : Controller
        {
            return "<a href=\"{0}\" class=\"arrowlink\">{1}</a>".With(
                htmlHelper.BuildUrlFromExpression<T>(action), 
                htmlHelper.Image("~/Content/Images/Up.png"));
        }

        public static string DownArrowLink<T>(this HtmlHelper htmlHelper, Expression<Action<T>> action) where T : Controller
        {
            return "<a href=\"{0}\" class=\"arrowlink\">{1}</a>".With(
                htmlHelper.BuildUrlFromExpression<T>(action),
                htmlHelper.Image("~/Content/Images/Down.png"));
        }
    }
}