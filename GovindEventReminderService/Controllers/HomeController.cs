using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GovindEventReminderService.Models;
using GovindEventReminderService.App_Code.Comman;
namespace GovindEventReminderService.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        EventDBEntities16 db1 = new EventDBEntities16();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string email, string password)
        {
            TempData["Email"] = email;
            TempData["Password"] = password;
            return RedirectToAction("Join");
        }
        public JsonResult CheckEmailAvailablity(string email)
        {
            return Json(db1.SP_CheckEmailAvailability(email), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Join()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Join(string email, string firstName, string middleName, string lastName, string dateOfBirth, string isAgeSecret, string gender, string mobileNo, string password)
        {


            if (db1.SP_UserRegisteration(email, Utility.Encript(password, "ER"), DateTime.Now, false, false, firstName, middleName, lastName, dateOfBirth, gender, isAgeSecret == null ? false : true, mobileNo) > 0)
            {
                TempData["Email"] = email;

                ActivationMail();
                ViewBag.staus = 1;
            }
            else {
                ViewBag.Status = 0;

            }


            return View();
        }
        public ActionResult Activation()
        {
            if (Request.QueryString["eml"] == null)
                return RedirectToAction("Login");
            string email = Request.QueryString["eml"];
            if (db1.SP_UserApproval(email) > 0)
            {
                ViewBag.Status = 1;
            }
            else
            {
                ViewBag.Status = 0;
            }
            return View();
        }
        public ViewResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            List<tblUserAccountDetail> objUserAccount = db1.SP_CheckUserLogin(email, Utility.Encript(password, "ER")).ToList();
            if (objUserAccount.Count > 0)
            {
                if (db1.SP_CheckUserAccountStatus(email, 1).FirstOrDefault().Value > 0)
                {
                    ViewBag.Status = "please check your mail to activate the account!!!";
                    ViewBag.Activation = 1;
                    TempData["Email"] = email;
                    return View();
                }
                if (db1.SP_CheckUserAccountStatus(email, 2).FirstOrDefault().Value > 0)
                {
                    ViewBag.Status = "your account has been cancelled please contact to administrator!!!";
                    return View();
                }
                Session["UserId"] = objUserAccount[0].UserId;
                Session["FirstName"] = objUserAccount[0].tblUPersonalDetails.FirstOrDefault().FirstName;

                Session["EmailId"] = objUserAccount[0].Email;
                string lastAccessedDateTime = objUserAccount[0].LastAccessDateTime.ToString();

                DateTime currentAccessedDateTime = DateTime.Now;
                if (string.IsNullOrEmpty(lastAccessedDateTime))
                {
                    Session["LastAcessedDateTime"] = currentAccessedDateTime.ToString();
                }
                else
                {
                    Session["LastAcessedDateTime"] = lastAccessedDateTime;
                }
                tblUserAccountDetail objUserAccountDetail = db1.tblUserAccountDetails.Find(objUserAccount[0].UserId);
                objUserAccountDetail.LastAccessDateTime = currentAccessedDateTime;
                db1.SaveChanges();
                return RedirectToAction("Index","Home",new {
                    area="User"
                });
            }
            else
            {
                ViewBag.Status = "Invalid Email Address or Password";
            }
            return View();
           
        }

    

        public ViewResult AboutUs()
        {
            return View();
        }
        public ViewResult Registration()
        {
           return View();
        }
        [HttpPost]
        public ActionResult ActivationMail()
        {
            string email = TempData["email"].ToString();
            string message = string.Empty;
            message += "<table style='width:700px;border:10px outset blue;border-radius:30px'>";
            message += "<tr><td><Image src='~/Images/event.jpg' width='700' alt='Event-reminder Logo'/></td>";
            message  += "<tr style='background-color:yellow;color:green'><td><b>Dear Member," + "<br/><br/>Tank You for registering with us.<br/><br/>pleasr click following:<br/><br/></b>";
            message += "<a href='http://localhost:2642/Home/Activation?eml=" + email + "'target='_blank'>http://localhost:2642/Home/Activation?eml=" + email + "</a><br/><br/>";
            message += "<b> Thanks & best regards</b><br/>Event-Reminder<br/>Management Team<br/>Hyderabad India</td></tr></table>";
            Utility.SendMail("SendReminder:Activation mail", email, message, true);

            TempData["SendActivationMail"] = 1;
            return RedirectToAction("Login");
              
        }

        public ActionResult SendForgotPassword(string email, string answer)
        {
            int userCount = db1.SP_CheckUserByEmailAnswer(email, answer).SingleOrDefault().Value;
            int Status = 0;
            if (userCount > 0)
            {
                //Generate New Random Password
                Random objRandom = new Random();
                int num = objRandom.Next(999999999);
                DateTime dt = DateTime.Now;
                string password = num.ToString() + dt.Day.ToString() + dt.Month.ToString() + dt.Year.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString() + dt.Millisecond.ToString();
                //update Generted Pasword into Database
                db1.SP_CheckUserByEmailAnswer(email, Utility.Encript(password, "ER"));
                //Send Generted Password By email
                string message = string.Empty;
                message += "<table style='width:700px;border:10px outset blue;border-radius:30px'>";
                message += "<tr><td><Image src='~/Images/event.jpg' width='700' alt='Event-reminder Logo'/></td>";
                message += "<tr style='background-color:yellow;color:green'><td><b>Dear Member," + "<br/><br/>";
                message += "Your New Password Is:" + password + "<br/><br/>";
                message += "<b>Thanks & Best Reagrds,</b><br/>Event-Reminder<br/>Management Team<br/>hyderabad India</td></tr></table>";

                Utility.SendMail("Event-Reminder:Forget Password", email, message, true);
                Status = 1;
            }
            else
            {
                Status = 0;
            }
            return Json(Status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHintQuestion(string email)
        {
            SP_GetHintQuestion_Result objHintQuestionResult = db1.SP_GetHintQuestion(email).FirstOrDefault();



            string hintquestion = string.Empty;
            if (objHintQuestionResult != null)
            {
                if (!string.IsNullOrEmpty(objHintQuestionResult.HintQuestion))
                {
                    hintquestion = objHintQuestionResult.HintQuestion;
                }
                else if (!string.IsNullOrEmpty(objHintQuestionResult.NewHintQuestion))
                {
                    hintquestion = objHintQuestionResult.NewHintQuestion;
                }
            }
            return Json(hintquestion, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Birthdaydate()
        {
            List<SelectListItem> Gender = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Male",Value="M" },
                new SelectListItem() {Text="Female",Value="F" }
            };
            ViewBag.Gender = Gender;
            List<SelectListItem> Relationships = new List<SelectListItem>();
            List<SP_BindRelationship> RelationshipList = db1.SP_BindRelationship().ToList();

            foreach (SP_BindRelationship relationship in RelationshipList)
            {
                Relationships.Add(new SelectListItem() { Text = relationship.RelationshipName, Value = relationship.RelationshipId.ToString() });
            }
            ViewBag.Relationships = Relationships;
            return View();
        }

        [HttpPost]
        public ActionResult Birthdaydate(tblBirthdayDetail1 model)
        {
            model.UserId = int.Parse(Request.QueryString["UserId"]);
            db1.tblBirthdayDetail1.Add(model);
            db1.SaveChanges();
            TempData["Status"] = "Birthday Details has been added successfully!!!";
            return RedirectToAction("Birthdaydate");
        }

    }
}