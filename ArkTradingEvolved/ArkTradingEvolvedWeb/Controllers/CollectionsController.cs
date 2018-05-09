using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArkTradingEvolvedWeb.Models;
using DataTransferObjects;
using LogicLayer;
using ArkTradingEvolvedWeb.ActionFilters;

namespace ArkTradingEvolvedWeb.Controllers
{
	[CurrentUserFilter]
    [Authorize(Roles = "General, Admin")]
    public class CollectionsController : Controller
    {

        CollectionManager _collectionManager = new CollectionManager();
        
        // GET: Collections/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Collections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CollectionModels collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    collection.UserID = (int)(System.Web.HttpContext.Current.Session["userID"]);
                    _collectionManager.AddCollection(collection);
                    return RedirectToAction("Index", "Profile");
                }
                catch (Exception ex)
                {
					return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
				}
            }
            return View(collection);
            
        }

        // GET: Collections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			try
			{
				Collection collection = _collectionManager.RetreiveCollectionByID(id);
				CollectionModels model = new CollectionModels
				{
					Name = collection.Name,
					CollectionID = collection.CollectionID,
					UserID = collection.UserID,
					Active = collection.Active
				};
				System.Web.HttpContext.Current.Session["editCollection"] = collection;


				return View(model);
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
           
        }

        // POST: Collections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CollectionModels collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var oldCollection = (Collection)(System.Web.HttpContext.Current.Session["editCollection"]);
                    _collectionManager.EditCollection(oldCollection, collection);
                    System.Web.HttpContext.Current.Session.Remove("editCollection");
                }
                catch (Exception ex)
                {
					return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
				}
               
                return RedirectToAction("Index", "Profile");
            }
            return View(collection);
        }

        // GET: Collections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			try
			{
				Collection collection = _collectionManager.RetreiveCollectionByID(id);
				CollectionModels model = new CollectionModels
				{
					Name = collection.Name,
					CollectionID = collection.CollectionID,
					UserID = collection.UserID,
					Active = collection.Active
				};
				return View(model);
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // POST: Collections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				_collectionManager.DeactivateCollection(id);
				return RedirectToAction("Index", "Profile"); ;
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        
    }
}
