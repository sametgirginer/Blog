using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blog.DAL;
using Blog.Filters;

namespace Blog.Controllers
{
    [CustomLoginFilter]
    [RoutePrefix("admin")]
    public class KonuYonetimiController : Controller
    {
        private BlogEntities db = new BlogEntities();

        [Route("konu")]
        public ActionResult Index()
        {
            var konu = db.Konu.Include(k => k.Kategori).Include(k => k.Uye);
            return View(konu.ToList());
        }

        [Route("konu/detay/{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Konu konu = db.Konu.Find(id);
            if (konu == null)
            {
                return HttpNotFound();
            }
            return View(konu);
        }

        [Route("konu/yeni")]
        public ActionResult Create()
        {
            ViewBag.KategoriID = new SelectList(db.Kategori, "ID", "Ad");
            ViewBag.UyeID = new SelectList(db.Uye, "ID", "EPosta");
            return View();
        }

        [Route("konu/yeni")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Baslik,Icerik,Tarih,OneCikar,UyeID,KategoriID,Resim")] Konu konu)
        {
            if (ModelState.IsValid)
            {
                konu.UyeID = ((Blog.DAL.Uye)Session["giris"]).ID;
                konu.Tarih = DateTime.Now;
                db.Konu.Add(konu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KategoriID = new SelectList(db.Kategori, "ID", "Ad", konu.KategoriID);
            ViewBag.UyeID = new SelectList(db.Uye, "ID", "EPosta", konu.UyeID);
            return View(konu);
        }

        [Route("konu/duzenle/{id}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Konu konu = db.Konu.Find(id);
            if (konu == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriID = new SelectList(db.Kategori, "ID", "Ad", konu.KategoriID);
            ViewBag.UyeID = new SelectList(db.Uye, "ID", "EPosta", konu.UyeID);
            return View(konu);
        }

        [Route("konu/duzenle/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Baslik,Icerik,Tarih,OneCikar,UyeID,KategoriID,Resim")] Konu konu)
        {
            if (ModelState.IsValid)
            {
                var yeni = db.Konu.Find(konu.ID);
                db.Konu.Attach(yeni);

                yeni.Baslik = konu.Baslik;
                yeni.Icerik = konu.Icerik;
                yeni.KategoriID = konu.KategoriID;
                yeni.Resim = konu.Resim;
                yeni.OneCikar = konu.OneCikar;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KategoriID = new SelectList(db.Kategori, "ID", "Ad", konu.KategoriID);
            ViewBag.UyeID = new SelectList(db.Uye, "ID", "EPosta", konu.UyeID);
            return View(konu);
        }

        [Route("konu/sil/{id}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Konu konu = db.Konu.Find(id);
            if (konu == null)
            {
                return HttpNotFound();
            }
            return View(konu);
        }

        [Route("konu/sil/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Konu konu = db.Konu.Find(id);
            db.Konu.Remove(konu);
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
    }
}
