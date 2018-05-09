using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using ArkTradingEvolvedWeb.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using LogicLayer;

namespace ArkTradingEvolvedWeb.ActionFilters
{
	public class CurrentUserFilter : ActionFilterAttribute
	{
		private UserManager _usrManager = new UserManager();
		

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (HttpContext.Current.User.Identity.IsAuthenticated && System.Web.HttpContext.Current.Session["userID"] == null)
			{
				try
				{
					var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
					var username = userManager.GetEmail(HttpContext.Current.User.Identity.GetUserId());
					var user = _usrManager.RetreiveUserByUsername(username);
					System.Web.HttpContext.Current.Session["userID"] = user.UserID;
				}
				catch (Exception ex)
				{

					var context = new RequestContext(
									new HttpContextWrapper(System.Web.HttpContext.Current),
									new RouteData());
					var urlHelper = new UrlHelper(context);
					var url = urlHelper.Action("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
					System.Web.HttpContext.Current.Response.Redirect(url);
				}
				
			}
		}
	}
}