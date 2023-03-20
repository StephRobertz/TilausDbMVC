using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TilausDbMVC.Models;
using TilausDbMVC.ViewModels;

namespace TilausDbMVC.Controllers
{
    public class TilauksetController : Controller
    {
        private TilausDbEntities db = new TilausDbEntities();

        // GET: Tilaukset
        public ActionResult Index()
        {
            var tilaukset = db.Tilaukset.Include(t => t.Asiakkaat).Include(t => t.Postitoimipaikat);
            return View(tilaukset.ToList());
        }

        // GET: Tilaukset/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null)
            {
                return HttpNotFound();
            }
            return View(tilaukset);
        }

        // GET: Tilaukset/Create
        public ActionResult Create()
        {
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi");
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka");
            return View();
        }

        // POST: Tilaukset/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TilausID,AsiakasID,Toimitusosoite,Postinumero,Tilauspvm,Toimituspvm")] Tilaukset tilaukset)
        {
            if (ModelState.IsValid)
            {
                db.Tilaukset.Add(tilaukset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaukset.AsiakasID);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", tilaukset.Postinumero);
            return View(tilaukset);
        }

        // GET: Tilaukset/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null)
            {
                return HttpNotFound();
            }
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaukset.AsiakasID);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", tilaukset.Postinumero);
            return View(tilaukset);
        }

        // POST: Tilaukset/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TilausID,AsiakasID,Toimitusosoite,Postinumero,Tilauspvm,Toimituspvm")] Tilaukset tilaukset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tilaukset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaukset.AsiakasID);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", tilaukset.Postinumero);
            return View(tilaukset);
        }

        // GET: Tilaukset/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null)
            {
                return HttpNotFound();
            }
            return View(tilaukset);
        }

        // POST: Tilaukset/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            db.Tilaukset.Remove(tilaukset);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Ordersummary()
        {
            var orderSummary = from t in db.Tilaukset
                               join td in db.Tilausrivit on t.TilausID equals td.TilausID
                               join p in db.Tuotteet on td.TuoteID equals p.TuoteID
                               select new OrderSummaryData
                               {
                                   TilausID = t.TilausID,
                                   AsiakasID = (int)t.AsiakasID,
                                   TuoteID = p.TuoteID,
                                   Ahinta = (float)p.Ahinta,
                                   Toimitusosoite = t.Toimitusosoite,
                                   Postinumero = t.Postinumero,
                                   Tilauspvm = t.Tilauspvm,
                                   Toimituspvm = t.Toimituspvm,
                                   //Postitoimipaikka = t.Postitoimipaikka,
                                   Nimi = p.Nimi,
                                   Maara = (int)td.Maara,
                                   TilausriviID = (int)td.TilausriviID

                               };


            return View(orderSummary);
        }


        public ActionResult TilausOtsikot()
        {
            var tilaukset = db.Tilaukset.Include(t => t.Asiakkaat).Include(t => t.Postitoimipaikat);
            return View(tilaukset.ToList());
        }


        public ActionResult _TilausRivit(int? tilausid)
        {
            if ((tilausid == null) || (tilausid == 0))
            {
                return HttpNotFound();
            }
            else
            {
                var orderRowsList = from td in db.Tilausrivit
                                    join p in db.Tuotteet on td.TuoteID equals p.TuoteID
                                    where td.TilausID == tilausid

                                    select new OrderRows
                                    {
                                        TilausID = (int)td.TilausID,
                                        TuoteID = p.TuoteID,
                                        Ahinta = (float)p.Ahinta,
                                        //Postitoimipaikka = t.Postitoimipaikka,
                                        Nimi = p.Nimi,
                                        Maara = (int)td.Maara,
                                        TilausriviID = (int)td.TilausriviID

                                    };
                return PartialView(orderRowsList);
            }

        }
    }
}
