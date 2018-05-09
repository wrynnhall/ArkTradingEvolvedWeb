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

namespace ArkTradingEvolvedWeb.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ResourceController : Controller
    {
        ResourceManager _resourceManager = new ResourceManager();

        // GET: Resource
        public ActionResult Index()
        {
            
            try
            {
                return View(_resourceManager.RetrieveResourcesList()); 
            }
            catch (Exception ex)
            {

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // GET: Resource/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			try
			{
				Resource resource = _resourceManager.RetreiveResourceByID(id);
				ResourceViewModel resourceVM = new ResourceViewModel
				{
					DisplayResourceID = resource.ResourceID,
					Active = resource.Active
				};
				return View(resourceVM);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // GET: Resource/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Resource/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ResourceViewModel resourceViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _resourceManager.AddResource(resourceViewModel);
                }
                catch (Exception ex)
                {

					return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
				}
                return RedirectToAction("Index");
            }

            return View(resourceViewModel);
        }

        // GET: Resource/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

			try
			{
				Resource resource = _resourceManager.RetreiveResourceByID(id);
				ResourceViewModel resourceVM = new ResourceViewModel
				{
					DisplayResourceID = resource.ResourceID,
					Active = resource.Active
				};

				System.Web.HttpContext.Current.Session["editResource"] = resource;

				return View(resourceVM);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // POST: Resource/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ResourceViewModel resourceViewModel)
        {
			try
			{
				if (ModelState.IsValid)
				{
					var oldResource = (Resource)(System.Web.HttpContext.Current.Session["editResource"]);
					_resourceManager.UpdateResource(oldResource, resourceViewModel);
					return RedirectToAction("Index");
				}
				return View(resourceViewModel);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // GET: Resource/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			try
			{
				Resource resource = _resourceManager.RetreiveResourceByID(id);
				ResourceViewModel resourceVM = new ResourceViewModel
				{
					DisplayResourceID = resource.ResourceID,
					Active = resource.Active
				};

				return View(resourceVM);
			}
			catch (Exception ex)
			{

				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            
        }

        // POST: Resource/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                _resourceManager.UpdateResourceActive(id, false);
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
                _resourceManager.UpdateResourceActive(id, true);
            }
            catch (Exception ex)
            {
				return RedirectToAction("Index", "Error", new { message = ex.Message, stackTrace = ex.StackTrace });
			}
            return RedirectToAction("Index");
        }


    }
}
