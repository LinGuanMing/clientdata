﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace clientdata.Models
{
    public class contactmanagerController : Controller
    {
        private clientEntities db = new clientEntities();

        // GET: contactmanager
        public ActionResult Index(string keyword, string 職稱)
        {
            var data = from x in db.客戶聯絡人.Where(x => x.IsDeleted != true)
                       select x.職稱;
            var list = new List<object>();
            list.Add(new { value = "", text = "全部" });
            foreach (var item in data.Distinct())
            {
                list.Add(new { value = item, text = item });
            }
            ViewData["職稱"] = new SelectList(list, "value", "text", 0);

            var view = db.客戶聯絡人.Where(x => x.姓名.Contains(keyword) && x.職稱.Contains(職稱) && x.IsDeleted != true);

            var 客戶聯絡人 = db.客戶聯絡人.Where(x => x.IsDeleted != true).Include(客 => 客.客戶資料);

            if (!string.IsNullOrEmpty(keyword) || 職稱 != null)
            {
                return View("index", view);
            }
            else
            {
                return View(客戶聯絡人.ToList());
            }
        }

        // GET: contactmanager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: contactmanager/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(db.客戶資料.Where(x => x.IsDeleted != true), "Id", "客戶名稱");
            return View();
        }

        // POST: contactmanager/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            var data = db.客戶聯絡人.Where(x => x.IsDeleted != true && x.Email == 客戶聯絡人.Email).Count();
            if (ModelState.IsValid && data == 0)
            {
                db.客戶聯絡人.Add(客戶聯絡人);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (data != 0)
            {
                ModelState.AddModelError("Email", "聯絡人資料已有相同Email！");
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料.Where(x => x.IsDeleted != true), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: contactmanager/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料.Where(x => x.IsDeleted != true), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: contactmanager/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            var data = db.客戶聯絡人.Where(x => x.IsDeleted != true && x.Email == 客戶聯絡人.Email);
            if (ModelState.IsValid && data.Count() == 0)
            {
                db.Entry(客戶聯絡人).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (data.Count() != 0)
            {
                if (data.Where(x => x.Id == 客戶聯絡人.Id).Select(x => x.Email).FirstOrDefault() == 客戶聯絡人.Email)
                {
                    db.Entry(客戶聯絡人).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Email", "聯絡人資料已有相同Email！");
                }
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料.Where(x => x.IsDeleted != true), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: contactmanager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: contactmanager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            db.客戶聯絡人.Where(x => x.Id == id).ToList().ForEach(x => x.IsDeleted = true);
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
