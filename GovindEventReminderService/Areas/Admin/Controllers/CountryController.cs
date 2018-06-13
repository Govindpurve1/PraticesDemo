using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.Script.Serialization;
using GovindEventReminderService.Areas.Admin.Models;
using GovindEventReminderService.App_Code.Comman;
namespace GovindEventReminderService.Areas.Admin
{
    public class CountryController : Controller
    {
        // GET: Admin/Country
        EventDBEntities3 db = new EventDBEntities3();
        public ActionResult Index()
        {
            if (Session["AdminUser"] == null)
                return RedirectToAction("Login", "Home");
            return View(db.tblCountries.ToList());

        }

        #region Crud Operation By Entity in mvc
        public ActionResult Create()
        {
            if (Session["AdminUser"] == null)
                return RedirectToAction("Login", "Home");
            return View();
        }
        [HttpPost]
        public ActionResult Create(tblCountry model)
        {
            if (Session["AdminUser"] == null)
                return RedirectToAction("Login", "Home");
            db.tblCountries.Add(model);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Edit(int? id)
        {
            if (Session["AdminUser"] == null)
                return RedirectToAction("Login", "Home");
            tblCountry model = db.tblCountries.Find(id);
            return View(model);

        }
        [HttpPost]
        public ActionResult Edit(tblCountry model)
        {
            if (Session["AdminUser"] == null)
                return RedirectToAction("Login", "Home");
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult Details(int? id)
        {
            if (Session["AdminUser"] == null)
                return RedirectToAction("Login", "Home");
            tblCountry model = db.tblCountries.Find(id);
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            if (Session["AdminUser"] == null)
                return RedirectToAction("Login", "Home");
            tblCountry model = db.tblCountries.Find(id);
            db.tblCountries.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
       
        public  ActionResult GetCountryList()
        {
            if (Session["AdminUser"] == null)
                return RedirectToAction("Login", "Home");
            List<tblCountry> countryList = new List<tblCountry>();
            foreach(tblCountry country in db.tblCountries.ToList())
            {
                countryList.Add(new tblCountry()
                {
                    CountryId=country.CountryId,CountryName=country.CountryName,IsActive=country.IsActive
                });
              
            }
            return Json(countryList, JsonRequestBehavior.AllowGet);
            

        }

        #endregion


        #region All functionalities done by jquery
   [HttpPost]
        public int InsertCountryDetails(string countryName, string isActive)
        {
            tblCountry country = new tblCountry()
            {
                CountryName = countryName,
                IsActive = Convert.ToBoolean(isActive)
            };
            db.tblCountries.Add(country);
           return db.SaveChanges();
           
        }
        public int DeleteCountryDetails(string countryId)
        {
            tblCountry Country = db.tblCountries.Find(int.Parse(countryId));
            db.tblCountries.Remove(Country);
            return db.SaveChanges();
        }
        public int UpdateCountryDetails(string countryId,string countryName,string IsActive)
        {
            tblCountry country = db.tblCountries.Find(int.Parse(countryId));
            country.CountryName = countryName;
            country.IsActive = Convert.ToBoolean(IsActive);
            return db.SaveChanges();
        }
       
        #endregion
    }
}