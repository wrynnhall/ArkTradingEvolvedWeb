using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ArkTradingEvolvedWeb.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using DataTransferObjects;
using LogicLayer;
using ArkTradingEvolvedWeb.ActionFilters;

namespace ArkTradingEvolvedWeb.Controllers
{
	[CurrentUserFilter]
    public class ProfileController : Controller
    {
        private LogicLayer.CollectionManager _collectionmanager = new LogicLayer.CollectionManager();
        private LogicLayer.UserManager _userManager = new LogicLayer.UserManager();
		private MarketEntryManager _marketEntryManager = new MarketEntryManager();
		private ResourceManager _resourceManager = new ResourceManager();
		

		private UserViewModel vm;
		
		private void GetUserViewModel()
		{

			var user = _userManager.RetreiveUserByID((int)System.Web.HttpContext.Current.Session["userID"]);
			List<Collection> collections = _collectionmanager.RetrieveCollectionList(user.UserID);
			List<MarketEntryDetails> purchases = _marketEntryManager.RetreiveMarketEntryPurchaseDetailsByUserID(user.UserID);
			List<MarketEntryDetails> myMarketEntries = _marketEntryManager.RetreiveMarketEntryDetailsByUserID(user.UserID);
			vm = new UserViewModel()
			{
				User = user,
				Collections = collections,
				CollectionEntries = null,
				Purchases = purchases,
				MarketEntries = myMarketEntries
			};
		}


        // GET: Profile
        [Authorize]
        public ActionResult Index()
        {
			try
			{
				GetUserViewModel();

				return View(vm);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
			
        }

        [Authorize]
        public ActionResult CollectionEntries(int id)
		{
			try
			{
				GetUserViewModel();
				List<CollectionEntryDetails> entries = _collectionmanager.RetrieveCollectionEntryDetails(id);
				vm.CollectionEntries = entries;
				vm.SelectedCollection = id;
				return View("Index", vm);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
			
        }


		[Authorize]
		public ActionResult PurchaseConfirmation(int id)
		{
			
			try
			{
				GetUserViewModel();
				var purchaseDetails = _marketEntryManager.RetreivePurchaseDetailByID(id);
				PurchaseCompleteViewModel pvm = new PurchaseCompleteViewModel
				{
					Collections = vm.Collections,
					PurchaseDetails = purchaseDetails
				};

				return View(pvm);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
			
		}

		[Authorize]
		[HttpPost]
		public ActionResult PurchaseConfirmation(PurchaseCompleteViewModel pvm)
		{
			
			try
			{
				GetUserViewModel();
				if (ModelState.IsValid)
				{
					var purchaseDetails = _marketEntryManager.RetreivePurchaseDetailByID(pvm.PurchaseDetails.MarketEntryDetails.MarketEntry.MarketEntryID);
					_marketEntryManager.PerformMarketEntryPurchaseComplete(pvm.CollectionToAddTo, purchaseDetails);
					return RedirectToAction("Index", "Profile" , null);
				}
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}

			return RedirectToAction("Index");
		}

		[Authorize]
		public ActionResult EditMarketEntry(int id)
		{
			try
			{
				var resources = _resourceManager.RetrieveResources();
				var marketEnry = _marketEntryManager.RetreiveMarketEntryDetailByID(id);
				var uvm = new UserMarketEntryViewModel
				{
					Resources = resources,
					Resource = _resourceManager.RetreiveResourceByID(marketEnry.MarketEntry.ResourceID),
					Units = marketEnry.MarketEntry.ResourceAmount,
					CollectionEntry = _collectionmanager.RetreiveCollectionEntryByID(marketEnry.MarketEntry.CollectionEntryID),
					MarketEntryID = marketEnry.MarketEntry.MarketEntryID
				};
				System.Web.HttpContext.Current.Session["editMarketEntry"] = marketEnry;
				return View(uvm);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}

			
		}

		[Authorize]
		[HttpPost]
		public ActionResult EditMarketEntry(UserMarketEntryViewModel uvm)
		{
			
			try
			{
				var oldMarketEntry = ((MarketEntryDetails)(System.Web.HttpContext.Current.Session["editMarketEntry"])).MarketEntry;
				var updateMarketEntry = new MarketEntry
				{
					MarketEntryID = oldMarketEntry.MarketEntryID,
					ResourceID = uvm.Resource.ResourceID,
					ResourceAmount = uvm.Units.HasValue ? (int)uvm.Units : 0
				};
				_marketEntryManager.UpdateMarketEntry(updateMarketEntry, oldMarketEntry);
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
			
			
		}

		[Authorize]
		public ActionResult RemoveMarketEntry(int id)
		{
			try
			{
				var marketEnry = _marketEntryManager.RetreiveMarketEntryDetailByID(id);
				var uvm = new UserMarketEntryViewModel
				{

					Resource = _resourceManager.RetreiveResourceByID(marketEnry.MarketEntry.ResourceID),
					Units = marketEnry.MarketEntry.ResourceAmount,
					CollectionEntry = _collectionmanager.RetreiveCollectionEntryByID(marketEnry.MarketEntry.CollectionEntryID),
					MarketEntryID = marketEnry.MarketEntry.MarketEntryID
				};
				return View(uvm);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
			
		}

		[Authorize]
		public ActionResult RemoveMarketEntryConfirmed(int id)
		{
			try
			{
				var marketEntry = _marketEntryManager.RetreiveMarketEntryDetailByID(id);
				_marketEntryManager.UpdateMarketEntryStatus(marketEntry.MarketEntry, "Closed");
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
			
		}

		[Authorize]
		public ActionResult AddMarketEntry()
		{
			
			try
			{
				GetUserViewModel();
				var resources = _resourceManager.RetrieveResources();
				var entries = _collectionmanager.RetreiveCollectionEntryListByUserID(vm.User.UserID);
				var uvm = new UserMarketEntryViewModel
				{
					Resources = resources,
					CollectionEntries = entries
				};
				return View(uvm);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
		}

		[Authorize]
		[HttpPost]
		public ActionResult AddMarketEntry(UserMarketEntryViewModel uvm)
		{
			if (ModelState.IsValid)
			{
				try
				{
					GetUserViewModel();
					var createEntry = new MarketEntry
					{
						ResourceID = uvm.Resource.ResourceID,
						ResourceAmount = uvm.Units.HasValue ? (int)uvm.Units : 0,
						CollectionEntryID = uvm.CollectionEntry.CollectionEntryID
					};
					_marketEntryManager.AddMarketEntry(createEntry);
					return RedirectToAction("Index");
				}
				catch (Exception ex)
				{

					return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
				}
			}
			GetUserViewModel();
			var resources = _resourceManager.RetrieveResources();
			var entries = _collectionmanager.RetreiveCollectionEntryListByUserID(vm.User.UserID);
			uvm.Resources = resources;
			uvm.CollectionEntries = entries;
			return View(uvm);



		}

	}
}