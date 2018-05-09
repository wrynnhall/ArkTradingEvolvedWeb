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
    public class CollectionEntryController : Controller
    {
        CollectionManager _collectionManager = new CollectionManager();


        // GET: CollectionEntry/Create
        public ActionResult Create(int id)
        {
			try
			{
				
				ViewBag.CollectionID = id;
				ViewBag.Creatures = _collectionManager.RetrieveCreatures();
				return View();
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // POST: CollectionEntry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CollectionEntryModel model)
        {
            
            if (ModelState.IsValid)
            {
                
                try
                {
					
                    model.Active = true;
                    _collectionManager.AddCollectionEntry(model);
                    
                    return RedirectToAction("Index", "Profile");
                }
                catch (Exception ex)
                {

					return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
				}
            }
			try
			{
				ViewBag.CollectionID = model.CollectionID;
				ViewBag.Creatures = _collectionManager.RetrieveCreatures();
				return View(model);
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // GET: CollectionEntry/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			try
			{
				CollectionEntry collectionEntry = _collectionManager.RetreiveCollectionEntryByID(id);
				CollectionEntryModel model = new CollectionEntryModel
				{
					CollectionID = collectionEntry.CollectionID,
					CreatureID = collectionEntry.CreatureID,
					Name = collectionEntry.Name,
					Level = collectionEntry.Level,
					Health = collectionEntry.Health,
					Stamina = collectionEntry.Stamina,
					Oxygen = collectionEntry.Oxygen,
					Food = collectionEntry.Food,
					Weight = collectionEntry.Weight,
					BaseDamage = collectionEntry.BaseDamage,
					MovementSpeed = collectionEntry.MovementSpeed,
					Torpor = collectionEntry.Torpor,
					Imprint = collectionEntry.Imprint,
					Active = collectionEntry.Active
				};

				ViewBag.Creatures = _collectionManager.RetrieveCreatures();
				System.Web.HttpContext.Current.Session["editCollectionEntry"] = collectionEntry;

				return View(model);
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // POST: CollectionEntry/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CollectionEntryModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var oldCollectionEntry = (CollectionEntry)(System.Web.HttpContext.Current.Session["editCollectionEntry"]);
                    _collectionManager.EditCollectionEntry(model, oldCollectionEntry);
                    System.Web.HttpContext.Current.Session.Remove("editCollectionEntry");
                }
                catch (Exception ex)
                {

					return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
				}
                
                return RedirectToAction("Index", "Profile");
            }
			try
			{
				ViewBag.Creatures = _collectionManager.RetrieveCreatures();
				return View(model);
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // GET: CollectionEntry/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

			try
			{
				CollectionEntry collectionEntry = _collectionManager.RetreiveCollectionEntryByID(id);
				CollectionEntryModel model = new CollectionEntryModel
				{
					CollectionID = collectionEntry.CollectionID,
					CreatureID = collectionEntry.CreatureID,
					Name = collectionEntry.Name,
					Level = collectionEntry.Level,
					Health = collectionEntry.Health,
					Stamina = collectionEntry.Stamina,
					Oxygen = collectionEntry.Oxygen,
					Food = collectionEntry.Food,
					Weight = collectionEntry.Weight,
					BaseDamage = collectionEntry.BaseDamage,
					MovementSpeed = collectionEntry.MovementSpeed,
					Torpor = collectionEntry.Torpor,
					Imprint = collectionEntry.Imprint,
					Active = collectionEntry.Active
				};
				ViewBag.Creatures = _collectionManager.RetrieveCreatures();
				return View(model);
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
           
        }

        // POST: CollectionEntry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				_collectionManager.DeactivateCollectionEntry(id);
				return RedirectToAction("Index", "Profile");
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        
    }
}
