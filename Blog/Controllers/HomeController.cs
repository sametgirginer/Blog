using Blog.DAL;
using Blog.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private BlogEntities db = new BlogEntities();

        public ActionResult Index()
        {
            return View();
        }

        [Route("konu")]
        public ActionResult Konu(int? id)
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

            ViewBag.Title = konu.Baslik;

            return View(konu);
        }

        [Route("kategori")]
        public ActionResult KategoriKonuListele(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Kategori kategori = db.Kategori.Find(id);

            if (kategori == null)
            {
                return HttpNotFound();
            }

            var konular = db.Konu.Where(k => k.KategoriID == kategori.ID).ToList();

            ViewBag.Title = kategori.Ad;
            ViewBag.Konu = konular;

            return View();
        }

        [Route("arsiv")]
        public ActionResult TarihKonuListele(int yil, int ay)
        {
            var konular = db.Konu.Where(h => h.Tarih.Year == yil && h.Tarih.Month == ay).ToList();

            switch (ay)
            {
                case 1:
                    ViewBag.Ay = "Ocak";
                    break;
                case 2:
                    ViewBag.Ay = "Şubat";
                    break;
                case 3:
                    ViewBag.Ay = "Mart";
                    break;
                case 4:
                    ViewBag.Ay = "Nisan";
                    break;
                case 5:
                    ViewBag.Ay = "Mayıs";
                    break;
                case 6:
                    ViewBag.Ay = "Haziran";
                    break;
                case 7:
                    ViewBag.Ay = "Temmuz";
                    break;
                case 8:
                    ViewBag.Ay = "Ağustos";
                    break;
                case 9:
                    ViewBag.Ay = "Eylül";
                    break;
                case 10:
                    ViewBag.Ay = "Ekim";
                    break;
                case 11:
                    ViewBag.Ay = "Kasım";
                    break;
                case 12:
                    ViewBag.Ay = "Aralık";
                    break;
            }

            ViewBag.Konu = konular;

            return View();
        }

        [Route("giris")]
        public ActionResult Giris()
        {
            return View();
        }

        [Route("giris")]
        [HttpPost]
        [ActionName("Giris")]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Uye uye)
        {

            try
            {
                using (BlogEntities db = new BlogEntities())
                {
                    var giris = db.Uye.Where(h => h.EPosta == uye.EPosta && h.Sifre == uye.Sifre).FirstOrDefault();

                    if (giris == null)
                    {
                        ViewBag.girisHata = "Girmiş olduğunuz bilgiler yanlış ya da eksik.";
                        return View();
                    }
                    else
                    {
                        Session["giris"] = giris;
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Hata = "HATA: " + ex.Message + ex.InnerException;
                return View(uye);
            }

        }

        [Route("kayitol")]
        public ActionResult Kayit()
        {
            return View();
        }

        [Route("kayitol")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kayit([Bind(Include = "ID,EPosta,Sifre,Ad,Soyad,Yetki")] Uye uye)
        {
            try
            {
                var epostaKontrol = db.Uye.Where(m => m.EPosta == uye.EPosta).SingleOrDefault();
                if (ModelState.IsValid)
                {
                    if (epostaKontrol == null)
                    {
                        db.Uye.Add(uye);
                        db.SaveChanges();
                        ViewBag.Kayit = "Başarıyla kayıt oldunuz. Artık giriş yapabilirsiniz.";
                        return View(uye);
                    }
                    else
                    {
                        ViewBag.Hata = "E-Posta adresi şu anda kullanımda!";
                        return View(uye);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Hata = "HATA: " + ex.Message + ex.InnerException;
                return View(uye);
            }

            return View(uye);
        }

    }

}