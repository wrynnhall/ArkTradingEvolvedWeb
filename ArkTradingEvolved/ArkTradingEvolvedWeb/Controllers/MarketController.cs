using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTransferObjects;
using LogicLayer;
using ArkTradingEvolvedWeb.Models;
using ArkTradingEvolvedWeb.ActionFilters;

namespace ArkTradingEvolvedWeb.Controllers
{
	[CurrentUserFilter]
	[Authorize]
    public class MarketController : Controller
    {
		private MarketEntryManager _marketEntryManager = new MarketEntryManager();
        // GET: Market
        public ActionResult Index()
        {
			try
			{
				var entries = _marketEntryManager.RetreiveMarketEntryDetailsByAvailable();
				var vm = new MarketViewModel
				{
					MarketEntries = entries
				};
				ViewBag.UserID = (int)System.Web.HttpContext.Current.Session["userID"];
				return View(vm);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
				
			}
			
        }

		public ActionResult Purchase(int id)
		{
			try
			{
				var vm = new MarketEntryPurchaseViewModel
				{
					MarketEntryDetails = _marketEntryManager.RetreiveMarketEntryDetailByID(id)
					
				};

				return View(vm);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
			
		}

		[HttpPost]
		public ActionResult Purchase(MarketEntryPurchaseViewModel pvm)
		{

			try
			{
				var userId = (int)System.Web.HttpContext.Current.Session["userID"];
				var user = new User
				{
					UserID = userId
				};
				var marketEntry = new MarketEntry
				{
					MarketEntryID = pvm.MarketEntryDetails.MarketEntry.MarketEntryID
				};
				_marketEntryManager.AddMarketEntryPurchase(user, marketEntry);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}


			return RedirectToAction("Index");
		}


    }
}