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
    public class CreatureController : Controller
    {
        CreatureManager _creatureManager = new CreatureManager();
        CreatureDietManager _creatureDietManager = new CreatureDietManager();
        CreatureTypeManager _creatureTypeManager = new CreatureTypeManager();

        // GET: Creature
        public ActionResult Index()
        {
            try
            {
                return View(_creatureManager.RetrieveCreatureList());
            }
            catch (Exception ex)
            {

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // GET: Creature/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			try
			{
				Creature creature = _creatureManager.RetreiveCreatureByID(id);
				CreatureViewModel creatureViewModel = new CreatureViewModel
				{
					DisplayCreatureID = creature.CreatureID,
					DisplayCreatureDietID = creature.CreatureDietID,
					DisplayCreatureTypeID = creature.CreatureTypeID,
					Active = creature.Active
				};

				return View(creatureViewModel);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // GET: Creature/Create
        public ActionResult Create()
        {
			try
			{
				ViewBag.CreatureDiets = _creatureDietManager.RetrieveCreatureDietList();
				ViewBag.CreatureTypes = _creatureTypeManager.RetrieveCreatureTypeList();
				return View();
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // POST: Creature/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatureViewModel creatureViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _creatureManager.AddCreature(creatureViewModel);
                }
                catch (Exception ex)
                {

					return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
				}
                return RedirectToAction("Index");
            }
			try
			{
				ViewBag.CreatureDiets = _creatureDietManager.RetrieveCreatureDietList();
				ViewBag.CreatureTypes = _creatureTypeManager.RetrieveCreatureTypeList();
				return View(creatureViewModel);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
           
        }

        // GET: Creature/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			try
			{
				Creature creature = _creatureManager.RetreiveCreatureByID(id);
				CreatureViewModel creatureViewModel = new CreatureViewModel
				{
					DisplayCreatureID = creature.CreatureID,
					DisplayCreatureDietID = creature.CreatureDietID,
					DisplayCreatureTypeID = creature.CreatureTypeID,
					Active = creature.Active
				};
				System.Web.HttpContext.Current.Session["editCreature"] = creature;
				ViewBag.CreatureDiets = _creatureDietManager.RetrieveCreatureDietList();
				ViewBag.CreatureTypes = _creatureTypeManager.RetrieveCreatureTypeList();
				return View(creatureViewModel);
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // POST: Creature/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreatureViewModel creatureViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var oldCreature = (Creature)(System.Web.HttpContext.Current.Session["editCreature"]);
                    _creatureManager.UpdateCreature(oldCreature, creatureViewModel);
                }
                catch (Exception ex)
                {
					return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
				}
                
                return RedirectToAction("Index");
            }

			try
			{
				ViewBag.CreatureDiets = _creatureDietManager.RetrieveCreatureDietList();
				ViewBag.CreatureTypes = _creatureTypeManager.RetrieveCreatureTypeList();
				return View(creatureViewModel);
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // GET: Creature/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			try
			{
				Creature creature = _creatureManager.RetreiveCreatureByID(id);
				CreatureViewModel creatureViewModel = new CreatureViewModel
				{
					DisplayCreatureID = creature.CreatureID,
					DisplayCreatureDietID = creature.CreatureDietID,
					DisplayCreatureTypeID = creature.CreatureTypeID,
					Active = creature.Active
				};
				return View(creatureViewModel);
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // POST: Creature/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                _creatureManager.UpdateCreatureActive(id, false);
            }
            catch (Exception ex)
            {

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            return RedirectToAction("Index");
        }

        [ActionName("Activate")]
        public ActionResult Activate(string id)
        {
            try
            {
                _creatureManager.UpdateCreatureActive(id, true);
            }
            catch (Exception ex)
            {
				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            return RedirectToAction("Index");
        }

    }
}
