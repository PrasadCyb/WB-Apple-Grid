using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using DeluxeOM.Services;
using DeluxeOM.Models.Account;
using DeluxeOM.Models;
using System.Collections.Generic;
//using DeluxeOM.WebUI.Models;


namespace DeluxeOM.WebUI.Controllers
{
    [DeluxeOMAuthorize(Roles = "SystemAdmin")]
    public class ManageController : BaseController
    {
        private IManageService _service = null;
        private INotificationService _notificationService = null;
        string tmpMsg = "Message";
        public ManageController()
        {
            _service = new ManageService();
            _notificationService = new NotificationService();
        }
        // GET: User
        public ActionResult Users()
        {
            var userList = _service.GetAllUsers();
            //User user = new User();
            //user.Name = Session["Name"].ToString();
            if (TempData[tmpMsg] != null)
            {
              
                ViewBag.Message = (string)TempData[tmpMsg];
            }
            return View(userList);
        }
        //
        // GET: User new
        [AllowAnonymous]
        public ActionResult Users_new(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        // GET: Configure FTP
        [AllowAnonymous]
        public ActionResult ConfigureFtp(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        // GET: Notification Mapping
        [AllowAnonymous]
        public ActionResult NotificationMapping(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            NotificationMgt notificationMgt = new NotificationMgt();
            var notification=_notificationService.GetAllNotification();
            notificationMgt.Notifications = notification;
            return View(notificationMgt);
        }

        public ActionResult UpdateNotification(List<Notification> notifications)
        {
            _notificationService.UpdateNotification(notifications);
            return RedirectToAction("NotificationMapping");
        }

        // GET: Vid
        [AllowAnonymous]
        public ActionResult Vid(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
              // GET: Territory
              [AllowAnonymous]
        public ActionResult Territory(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        // GET: Monitor Import
        [AllowAnonymous]
        public ActionResult MonitorImport(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // GET: VidTerritory
        [AllowAnonymous]
        public ActionResult VidTerritory(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // GET: ViewJobStatus
        [AllowAnonymous]
        public ActionResult ViewJobStatus(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        // GET: Customers
        [AllowAnonymous]
        public ActionResult Customers(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public ActionResult NewUser()
        {
            var user = new DeluxeOM.Models.Account.User()
            {
                Active = true ,
                Roles = _service.GetRoles(),
                SelectedRoleId = 4
            };
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewUser(User model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = _service.GetRoles();
                return View(model);
            }

            SaveResult result = _service.SaveUser(model);
            if (result.Success)
            {
                TempData[tmpMsg] = "Create new User successful";
                return RedirectToAction("Users");
            }
            else
            {
                model.Roles = _service.GetRoles();
                ViewBag.Message = result.Message;
            }

            return View(model);
        }

        // GET: Users/Edit/5
        public ActionResult EditUser(int id)
        {

            var user = _service.GetUserById(id);
            if (user != null)
            {
                user.Roles = _service.GetRoles();
                ViewBag.SelectedRoles = new SelectList(user.Roles, "RoleId", "RoleName", user.Role);
                return View(user);
            }

            ViewBag.Message = "User not found.";
            return View();
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User model)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            ModelState.Clear();
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var saveStatus = _service.UpdateUser(model);
            if (saveStatus.Success)
            {
                TempData[tmpMsg] = "User updated successfully";
                return RedirectToAction("Users");
            }
            else
            {
                ViewBag.Message = "Unable to save user changes.";
                return View(model);
            }
        }

        [HttpPost]
        // GET: Users/Delete/5
        public JsonResult DeleteUser(int id)
        {
            var saveStatus = _service.DeleteUser(id);
            JsonResult result = new JsonResult();
            if (saveStatus.Success)
            {
                result.Data = "true";
                return result;
            }

            ViewBag.Message = "User not found.";
            result.Data = "false";

            return result;
        }

        public ActionResult AssignRoles(int userId)
        {
            User user = _service.GetUserById(userId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignRoles(User model)
        {
            SaveResult result = _service.AssignRoles(model.UserId, model.Roles);
            TempData[tmpMsg] = "Roles assigned successfully";
            return View("Users");
        }

        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            var passReset = new ResetPasswordViewModel();
            passReset.Email = this.CurrentUser.Email;

            return View(passReset);
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            PasswordReset pwdResetModel = new PasswordReset()
            {
                Email = model.Email,
                Password = model.Password
            };
            var result = _service.ChangePassword(pwdResetModel);
            if (result.Success)
            {
                //return RedirectToAction("Edit", new { id = User.userid });
                return RedirectToAction("Users");
            }

            ViewBag.Message = "Error resetting password.  Please try again. " + result.Message;
            return View();
        }

        // GET: /Account/ChangePasswordConfirmation
        public ActionResult ChangePasswordConfirmation()
        {
            return View();
        }

        public JsonResult IsUserEmailAvailable(string email)
        {
            return Json(!_service.EmailExists(email), JsonRequestBehavior.AllowGet);
        }


    }
}