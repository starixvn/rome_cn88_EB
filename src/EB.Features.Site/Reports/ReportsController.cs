using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EB.Features.Site.Reports
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult HouseWinLoss()
        {
            return View("~/Reports/Views/HouseWinLoss.cshtml");
        }

        public ActionResult MatchWinLoss()
        {
            return View("~/Reports/Views/MatchWinLoss.cshtml");
        }

        // GET: Reports/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Reports/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reports/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reports/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reports/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reports/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reports/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
