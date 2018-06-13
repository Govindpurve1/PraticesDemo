using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Serialization;
using GovindEventReminderService.Areas.Admin.Models;
using GovindEventReminderService.App_Code.Comman;


namespace GovindEventReminderService.Areas.Admin
{
    public class StateController : Controller
    {
        // GET: Admin/Country
        EventDBEntities3 db = new EventDBEntities3();
        public ActionResult Index()
        {
            if (Session["AdminUser"] == null)
                return RedirectToAction("Login", "Home");
            return View(db.tblStates.ToList());

        }

        //public ActionResult Create()
        //{
        //    if (Session["AdminUser"] == null)
        //        return RedirectToAction("Login", "Home");
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Create(tblState model)
        //{
        //    if (Session["AdminUser"] == null)
        //        return RedirectToAction("Login", "Home");
        //    db.tblStates.Add(model);
        //    db.SaveChanges();

        //    return RedirectToAction("Index");
        //}
        public ActionResult Create()
        {
            if (Session["AdminUser"] == null)
                return RedirectToAction("Login", "Home");


            ViewBag.CountryId = new SelectList(db.tblCountries, "CountryId", "CountryName");
            return View();
        }
        [HttpPost]
        public ActionResult Create(tblState tblState)
        {
            if (Session["AdminUser"] == null)
                return RedirectToAction("Login", "Home");

            if (ModelState.IsValid)
            {
                db.tblStates.Add(tblState);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.tblCountries, "CountryId", "CountryName", tblState.CountryId);
            return View(tblState);
        }

        [HttpPost]
        public ActionResult Edit(tblState model)
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
            tblState model = db.tblStates.Find(id);
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            if (Session["AdminUser"] == null)
                return RedirectToAction("Login", "Home");
            tblState model = db.tblStates.Find(id);
            db.tblStates.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //[WebMethod]
        //public void GetCountry()
        //{
        //    string cs = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
        //    List<tblCountry> Country = new List<tblCountry>();
        //    using(SqlConnection con=new SqlConnection(cs))
        //    {
        //        SqlCommand cmd = new SqlCommand("select*from tblCountry", con);
        //        cmd.CommandType = CommandType.Text;
        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            tblCountry tblcountry = new tblCountry();
        //            tblcountry.CountryId = Convert.ToInt32(dr["CountryId"]);
        //            tblcountry.CountryName = dr["CountryName"].ToString();
        //            Country.Add(tblcountry);
        //        }
        //        JavaScriptSerializer js = new JavaScriptSerializer();

        //        //Context.Response.Write(js.Serialize(Country));

        //    }

        //}
    }
}