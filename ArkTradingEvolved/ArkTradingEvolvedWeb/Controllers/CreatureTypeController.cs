using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArkTradingEvolvedWeb.Models;
using LogicLayer;
using DataTransferObjects;
using ArkTradingEvolvedWeb.ActionFilters;

namespace ArkTradingEvolvedWeb.Controllers
{
	[CurrentUserFilter]
    public class CreatureTypeController : Controller
    {
		CreatureTypeManager _creatureTypeManager = new CreatureTypeManager();

		// GET: CreatureType
		public ActionResult Index()
        {
			try
			{
				return View(_creatureTypeManager.RetrieveCreatureTypeList());
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // GET: CreatureType/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			try
			{
				CreatureType creatureType = _creatureTypeManager.RetreiveCreatureTypeByID(id);
				CreatureTypeViewModel creatureTypeViewModel = new CreatureTypeViewModel
				{
					DisplayCreatureTypeID = creatureType.CreatureTypeID,
					Active = creatureType.Active
				};

				return View(creatureTypeViewModel);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
			
        }

        // GET: CreatureType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CreatureType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatureTypeViewModel creatureTypeViewModel)
        {
            if (ModelState.IsValid)
            {
				try
				{
					_creatureTypeManager.AddCreatureType(creatureTypeViewModel);
					return RedirectToAction("Index");
				}
				catch (Exception ex)
				{
					return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
				}
                
            }

            return View(creatureTypeViewModel);
        }

        // GET: CreatureType/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			try
			{
				CreatureType creatureType = _creatureTypeManager.RetreiveCreatureTypeByID(id);
				CreatureTypeViewModel creatureTypeViewModel = new CreatureTypeViewModel
				{
					DisplayCreatureTypeID = creatureType.CreatureTypeID,
					Active = creatureType.Active
				};

				System.Web.HttpContext.Current.Session["editCreatureType"] = creatureType;
				return View(creatureTypeViewModel);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
			
        }

        // POST: CreatureType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreatureTypeViewModel creatureTypeViewModel)
        {
            if (ModelState.IsValid)
            {
				try
				{
					var oldType = (CreatureType)(System.Web.HttpContext.Current.Session["editCreatureType"]);
					_creatureTypeManager.UpdateCreatureType(oldType, creatureTypeViewModel);
					return RedirectToAction("Index");
				}
				catch (Exception ex)
				{

					return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
				}
                
            }
            return View(creatureTypeViewModel);
        }

        // GET: CreatureType/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			try
			{
				CreatureType creatureType = _creatureTypeManager.RetreiveCreatureTypeByID(id);
				CreatureTypeViewModel creatureTypeViewModel = new CreatureTypeViewModel
				{
					DisplayCreatureTypeID = creatureType.CreatureTypeID,
					Active = creatureType.Active
				};
				return View(creatureTypeViewModel);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
			
        }

        // POST: CreatureType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
			try
			{
				_creatureTypeManager.UpdateCreatureTypeActive(id, false);
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
           
        }

		[ActionName("Activate")]
		public ActionResult Activate(string id)
		{
			try
			{
				_creatureTypeManager.UpdateCreatureTypeActive(id, true);
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
			
		}

	}
}
