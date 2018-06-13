using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Services;
using GovindEventReminderService.Areas.Admin.Models;
using GovindEventReminderService.App_Code.Comman;
using GovindEventReminderService.Areas.Admin.Controllers;
namespace GovindEventReminderService.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        EventDBEntities3 db = new EventDBEntities3();
      
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

       
        [HttpPost]
        public ActionResult Login(tblAdminAccount model)
        {
            List<tblAdminAccount> objAdminaccountList =db.sp_CheckAdminLogin(model.UserName,Utility.Encript(model.Password,"ER")).ToList();
            if (objAdminaccountList.Count > 0)
            {
                Session["AdminUser"] = objAdminaccountList[0].UserName;
                string lastAccessDateTime = objAdminaccountList[0].LastAccessedDateTime.ToString();
                DateTime CurrentAccessDateTime = DateTime.Now;
                if (string.IsNullOrEmpty(lastAccessDateTime))
                {
                    Session["lastAccessDateTime"] = CurrentAccessDateTime.ToString();
                }
                else
                {
                    Session["lastAccessDateTime"] = lastAccessDateTime;
                }
                tblAdminAccount objAdminAccount =db.tblAdminAccounts.Find(objAdminaccountList[0].UserName);
                objAdminAccount.LastAccessedDateTime = CurrentAccessDateTime;

                db.SaveChanges();



                ViewBag.Error = string.Empty;

                return RedirectToAction("ControlPanel");
            }
            else
            {
                ViewBag.Error = "Invalid UserName Or Password";
            }

            return View();
        }

        public ActionResult ControlPanel()
        {
            if (Session["AdminUser"] == null)
                return RedirectToAction("Login");
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Remove("AdminUser");
            Session.Remove("LastAcessDateTime");
            return RedirectToAction("Login");
        }
        public ActionResult ChangePassword()
        {
            if (Session["AdminUser"] == null)
                return RedirectToAction("Login");
            return View();
        }
       
        [HttpPost]
        public ActionResult ChangePassword(Models.Admin model)
        {
            if (Session["AdminUser"] == null)
                return RedirectToAction("Login");
            if (ModelState.IsValid)
            {
                tblAdminAccount objAdminAccount = db.tblAdminAccounts.Find(Session["AdminUser"].ToString());
                objAdminAccount.Password = Utility.Encript(model.NewPassword, "ER");

                if (db.SaveChanges() > 0)
                    ViewBag.Status = "Password Sucessfully Changed";
                else

                    ViewBag.Status = "Password changed failed";
            }
            return View();


        }
        public JsonResult CheckOldPassword(string oldpwd, string Username)
        {
            return Json(db.sp_CheckAdminOldPassword(Username, Utility.Encript(oldpwd, "ER")),
               JsonRequestBehavior.AllowGet);
        }
       

    }
}