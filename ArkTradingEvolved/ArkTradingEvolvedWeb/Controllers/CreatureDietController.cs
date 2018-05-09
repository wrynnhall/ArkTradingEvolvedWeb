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
    public class CreatureDietController : Controller
    {
		private CreatureDietManager _creatureDietManager = new CreatureDietManager();

        // GET: CreatureDiet
        public ActionResult Index()
        {
			try
			{
				return View(_creatureDietManager.RetrieveCreatureDietList());
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // GET: CreatureDiet/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			try
			{
				CreatureDiet diet = _creatureDietManager.RetreiveCreatureDietByID(id);
				CreatureDietViewModel creatureDietViewModel = new CreatureDietViewModel
				{
					DisplayCreatureDietID = diet.CreatureDietID,
					Active = diet.Active
				};


				return View(creatureDietViewModel);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
			
        }

        // GET: CreatureDiet/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CreatureDiet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatureDietViewModel creatureDietViewModel)
        {
            if (ModelState.IsValid)
            {
				try
				{
					_creatureDietManager.AddCreatureDiet(creatureDietViewModel);
				}
				catch (Exception ex)
				{

					return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
				}
                return RedirectToAction("Index");
            }

            return View(creatureDietViewModel);
        }

        // GET: CreatureDiet/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			try
			{
				CreatureDiet diet = _creatureDietManager.RetreiveCreatureDietByID(id);
				CreatureDietViewModel creatureDietViewModel = new CreatureDietViewModel
				{
					DisplayCreatureDietID = diet.CreatureDietID,
					Active = diet.Active
				};
				System.Web.HttpContext.Current.Session["editCreatureDiet"] = diet;
				return View(creatureDietViewModel);
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
			
        }

        // POST: CreatureDiet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreatureDietViewModel creatureDietViewModel)
        {
            if (ModelState.IsValid)
            {
				try
				{
					var oldDiet = (CreatureDiet)(System.Web.HttpContext.Current.Session["editCreatureDiet"]);
					_creatureDietManager.UpdateCreatureDiet(oldDiet, creatureDietViewModel);
				}
				catch (Exception ex)
				{

					return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
				}
                return RedirectToAction("Index");
            }
            return View(creatureDietViewModel);
        }

        // GET: CreatureDiet/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			try
			{
				CreatureDiet diet = _creatureDietManager.RetreiveCreatureDietByID(id);
				CreatureDietViewModel creatureDietViewModel = new CreatureDietViewModel
				{
					DisplayCreatureDietID = diet.CreatureDietID,
					Active = diet.Active
				};
				return View(creatureDietViewModel);
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
			
        }

        // POST: CreatureDiet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
			try
			{
				_creatureDietManager.UpdateCreatureDietActive(id, false);
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
				_creatureDietManager.UpdateCreatureDietActive(id, true);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
			return RedirectToAction("Index");
		}
	}
}
