using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicLayer;
using DataTransferObjects;
using Microsoft.AspNet.Identity;
using ArkTradingEvolvedWeb.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Routing;

namespace ArkTradingEvolvedWeb.Controllers
{
    public class BaseController : Controller
    {
		private UserManager _usrManager = new UserManager();

		public int UserID {
			get
			{
				if (HttpContext.User.Identity.IsAuthenticated && System.Web.HttpContext.Current.Session["userID"] == null)
				{
					var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
					var username = userManager.GetEmail(HttpContext.User.Identity.GetUserId());
					var user = _usrManager.RetreiveUserByUsername(username);
					System.Web.HttpContext.Current.Session["userID"] = user.UserID;
					return user.UserID;

				}
				else if(HttpContext.User.Identity.IsAuthenticated)
				{
					return (int)System.Web.HttpContext.Current.Session["userID"];
				}

				return 0;
			}
		}

		protected override void Initialize(RequestContext requestContext)
		{
			base.Initialize(requestContext);

			int user = this.UserID;//force intialization of UserID
		}

		
	}
}