using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GovindEventReminderService.App_Code.Comman;

using GovindEventReminderService.Areas.User.Models;
using System.IO;

namespace GovindEventReminderService.Areas.User.Controllers
{
    public class HomeController : Controller
    {
        EventDBEntities14 db = new EventDBEntities14();
        // GET: User/Home
        public ActionResult Index()
        {
            //if (Session["UserId"] == null)
            //{
            //    return RedirectToAction("Login", "Home", new
            //    {
            //        areas = ""
            //    });
            //}
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Remove("UserId");
            Session.Remove("FirstName");
            Session.Remove("EmailId");
            Session.Remove("LastAccessedDateTime");
            return RedirectToAction("Index", "Home");
        }
        public ActionResult MemberCorner()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Home", new
                {
                    area = ""
                });

            return View();
        }
        public ActionResult MyAccount()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Home", new
                {
                    area = ""
                });

            GetUserDetails objUser = BindUserDetails();

            return View(objUser);
        }
        public ActionResult BirthdayRequester()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BirthdayRequester(string toMails, string emailSubject)
        {
            string message = "I ' m organizing my friends and family and family's birthdays." + "<br/><br/>" + "Please do me a favour and enter your birthday by cliking on: " + "<a href='http://localhost:2642/Home/BirthdayDate?id=" + Session["UserId"].ToString() + "&nm=" + Session["FirstName"].ToString() + "' target='_blank'>http://localhost:2642/Home/BirthdayDate?id=" + Session["UserId"].ToString() + "&nm" + Session["FirstName"].ToString() + "</a>" + "br/><br>" + "It only takes a mintes." + "\n\n" + "Thanks! <br/>" + Session["FirstName"].ToString();
          Utility.SendEmail(Session["FirstName"].ToString(),emailSubject,toMails, message, true, null);


             TempData["ToMails"] = toMails;
            return RedirectToAction("BirthdayRequesterStatus");
        }




       
        public ActionResult BirthdayRequesterStatus()
        {
            return View();
        }
       


        [HttpPost]
        public ActionResult MyAccount(string firstName, HttpPostedFileBase profilePhoto, string middlename, string lastName, string nickName, string Gender, string dateOfBirth, string isAgeSecret, string anniversaryDate, string mobileNo, string newPassword, int HintQuestion, string newHintQuestion, string Answer, string Address, int Country, int State, int City, string PinCode)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Home", new
                {
                    area = ""
                });

            GetUserDetails objUser = BindUserDetails();

            string virtualFilePath = objUser.UserPhoto;
            string serverFilepath = string.Empty;
            if (profilePhoto != null)
            {
                string fileType = profilePhoto.ContentType;
                if (fileType == "image/jpg" || fileType == "image/jpeg" || fileType == "image/png" || fileType == "image/gif" || fileType == "image/bmp")
                {
                    int fileSize = profilePhoto.ContentLength;
                    if (fileSize <= 512000)
                    {
                        string serverFolderpath = Server.MapPath("../../Images/UserPhotos\\");
                        if (!Directory.Exists(serverFolderpath))
                            Directory.CreateDirectory(serverFolderpath);
                        string fileName = Path.GetFileName(profilePhoto.FileName);
                        string primaryFileName = Path.GetFileNameWithoutExtension(fileName);
                        string fileExtensionName = Path.GetExtension(fileName);
                        //Generate New fileName

                        Random objRandom = new Random();
                        int num = objRandom.Next(999999999);
                        DateTime dt = DateTime.Now;
                        string newFileName = primaryFileName + num.ToString() + dt.Day.ToString() + dt.Month.ToString() + dt.Year.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString() + dt.Millisecond.ToString() + fileExtensionName;

                        serverFilepath = serverFolderpath + newFileName;
                        virtualFilePath = "../../Images/UserPhotos/" + newFileName;
                    }
                    else
                    {
                        ViewBag.FileUploadError = "Please upload max 500kb size file only";
                        return View(objUser);
                    }

                }
                else
                {
                    ViewBag.FileUploadError = "please upload image file only.";
                    return View(objUser);
                }
            }
            bool ageSecret;
            if (isAgeSecret == "on")
                ageSecret = true;
            else
                ageSecret = false;

            if (!string.IsNullOrEmpty(newPassword)) newPassword = Utility.Encript(newPassword, "ER");
            db.SP_UpdateUserProfile(Convert.ToInt32(Session["UserId"]), newPassword, HintQuestion, newHintQuestion, Answer, firstName, middlename, lastName, nickName, dateOfBirth, anniversaryDate, Gender, ageSecret, mobileNo, virtualFilePath, Address, Country, State, City, PinCode);
            if (profilePhoto != null)
            {
                string oldFilePath = Server.MapPath(objUser.UserPhoto);
                if (System.IO.File.Exists(oldFilePath))
                {

                }
            }
            ViewBag.Status = "User Profile has been updated suceesfully";
            return View(BindUserDetails());



        }
        private GetUserDetails BindUserDetails()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            GetUserDetails objUser = db.SP_GetUserDetails(userId).ToList().FirstOrDefault();

            List<BindHintQuestion> HintQuestionList = db.SP_BindHintQuestion().ToList();
            HintQuestionList.Add(new BindHintQuestion() { HintQuestion = "Other", HintQuestionId = 0, IsActive = true });
            HintQuestionList.Insert(0, new BindHintQuestion() { HintQuestion = "Select", HintQuestionId = 0, IsActive = true });
            ViewBag.HintQuestionList = new SelectList(HintQuestionList, "HintQuestionId", "HintQuestion", objUser.HintQuetionId);

            // Bind Country

            List<BindCountry> CountryList = db.SP_BindCountry().ToList();
            CountryList.Insert(0, new BindCountry()
            {
                CountryName = "Select",
                CountryId = 0,
                IsActive = true
            });

            ViewBag.CountryList = new SelectList(CountryList, "CountryId", "CountryName", objUser.CountryId);

            // Bind State
            List<BindState> StateList = db.SP_BindState(objUser.CountryId).ToList();
            StateList.Insert(0, new BindState()
            {
                StateName = "Select",
                StateId = 0,
                IsActive = true
            });

            ViewBag.StateList = new SelectList(StateList, "StateId", "StateName", objUser.StateId);


            // Bind City
            List<BindCity> CityList = db.SP_BindCity(objUser.StateId).ToList();
            CityList.Insert(0, new BindCity()
            {
                CityName = "Select",
                CityId = 0,
                IsActive = true
            });

            ViewBag.CityList = new SelectList(CityList, "CityId", "CityName", objUser.CityId);

            if (objUser.Gender == "M")
            {
                if (string.IsNullOrEmpty(objUser.UserPhoto))
                    objUser.UserPhoto = "/../../Images/bf.PNG";
            }
            else if (objUser.Gender == "F")
            {
                if (string.IsNullOrEmpty(objUser.UserPhoto))
                    objUser.UserPhoto = "/../../Images/gf.PNG";
            }
            return objUser;
        }
        public JsonResult GetStates(string countryId)
        {
            List<SelectListItem> states = new List<SelectListItem>();
            List<BindState> stateList = db.SP_BindState(int.Parse(countryId)).ToList();
            states.Add(new SelectListItem() { Text = "Select", Value = "0" });
            foreach (BindState State in stateList)
            {
                states.Add(new SelectListItem() { Text = State.StateName, Value = State.StateId.ToString() });
            }
            return Json(new SelectList(states, "Value", "Text"), JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetCities(string stateId)
        {
            List<SelectListItem> cities = new List<SelectListItem>();
            List<BindCity> citiesList = db.SP_BindCity(int.Parse(stateId)).ToList();
            cities.Add(new SelectListItem() { Text = "Select", Value = "0" });
            foreach (BindCity city in citiesList)
            {
                cities.Add(new SelectListItem() { Text = city.CityName, Value = city.CityId.ToString() });
            }
            return Json(new SelectList(cities, "Value", "Text"), JsonRequestBehavior.AllowGet);

        }

        public ActionResult MyReminders()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Home", new { area = "" });
            List<GetBirthdayDetailsByUserId> objBirthdayDetails = db.SP_GetBirthdayDetailsByUserId(Convert.ToInt32(Session["UserId"])).ToList();
            ViewBag.BirthdayEvents = objBirthdayDetails;
            return View();
        }
        public ActionResult AddBirthdays()

        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Home", new { area = "" });

            List<SelectListItem> Gender = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Male",Value="M" },
                new SelectListItem() {Text="Female",Value="F" }

            };
            ViewBag.Gender = Gender;

            List<SelectListItem> Relationships = new List<SelectListItem>();
            List<BindRelationship> RelationshipList = db.SP_BindRelationship().ToList();

            foreach (BindRelationship relationship in RelationshipList)
            {
                Relationships.Add(new SelectListItem() { Text = relationship.RelationshipName, Value = relationship.RelationshipId.ToString() });
            }
            ViewBag.Relationships = Relationships;
            return View();


        }
        [HttpPost]
        public ActionResult AddBirthdays(tblBirthdayDetail model)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Home", new
                {
                    area = ""
                });
            model.UserId = Convert.ToInt32(Session["UserId"]);
            db.tblBirthdayDetails.Add(model);

            db.SaveChanges();
            TempData["Status"] = "Birthday Details has been added sussesfully ! !";
            return RedirectToAction("AddBirthdays");
        }
       
      

        [HttpPost]
        public ActionResult SendBirthdayMail(string toMail,string fromMail ,string subject,string message,HttpPostedFileBase[] fileAttach)
        {
            Utility.SendEmail(Session["Firstname"].ToString(),
                subject, toMail, message, false,fileAttach);
            TempData["MailSucess"] = "your birthday wishes has been sent suceessfully!!!";
            return RedirectToAction("MyReminders");
        }

        public ActionResult BirthdayReminderSetup(int? id)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Home", new
                {
                    area = ""
                });
            TempData["BirthdayDetailsId"] = id;
            GetBirthdayDetailsByUserId birthdayDetails = db.SP_GetBirthdayDetailsByUserId(id).SingleOrDefault();
            ViewBag.Name = birthdayDetails.FirstName;
            ViewBag.DateOfBirth = birthdayDetails.DateOfBirth;
            GetBirthdayReminder birthdayReminder = db.SP_GetBirthdayReminder(id, Convert.ToInt32(Session["UserId"])).SingleOrDefault();
            if (birthdayReminder != null)
            {
                TempData["BirthdayReminderId"] = birthdayReminder.BirthdayReminder;
                ViewBag.Reminder = "Update";
            }
            else
            {
                ViewBag.Reminder = "Add";
                birthdayReminder = new GetBirthdayReminder();

            }
            return View(birthdayReminder);
        }
        [HttpPost]
        public ActionResult BirthdayReminderSetup(string chkDays30, string chkDays14, string chkDays7, string chkDays3, string chkDays0, string SubmitButton)
        {
            bool[] chkDays = new bool[5] { false, false, false, false, false };
            if (chkDays30 == "30")
                chkDays[0] = true;
            if (chkDays14 == "14")
                chkDays[1] = true;
            if (chkDays7 == "7")
                chkDays[2] = true;
            if (chkDays3 == "3")
                chkDays[3] = true;
            if (chkDays0 == "0")
                chkDays[4] = true;
            if (SubmitButton == "Add")
            {
                tblBirthdayReminder objBirthdayReminder = new tblBirthdayReminder()
                {
                    UserId = Convert.ToInt32(Session["UserId"]),
                    BirthdayDetailsId = Convert.ToInt32(TempData["BirthdayDetailsId"]),
                    C30DaysBefore = chkDays[0],
                    C14DaysBefore = chkDays[1],
                    C7DaysBefore = chkDays[2],
                    C3DaysBefore = chkDays[3],
                    DayofEvent = chkDays[4]
                };
                db.tblBirthdayReminders.Add(objBirthdayReminder);
                db.SaveChanges();
            }
            else if (SubmitButton == "Update")
            {
                tblBirthdayReminder objBirthdayReminder = db.tblBirthdayReminders.Find(Convert.ToInt32(TempData["BirthdayReminderId"]));//  check it  id
                objBirthdayReminder.C30DaysBefore = chkDays[0];
                objBirthdayReminder.C14DaysBefore = chkDays[1];
                objBirthdayReminder.C7DaysBefore = chkDays[2];
                objBirthdayReminder.C3DaysBefore = chkDays[3];
                objBirthdayReminder.DayofEvent = chkDays[4];
                db.SaveChanges();
            }
            return RedirectToAction("MyReminders");
        }



    }
}