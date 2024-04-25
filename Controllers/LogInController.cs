using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPN.Property_Rental_Management_Final_Project.Models;

namespace ASPN.Property_Rental_Management_Final_Project.Controllers
{
    public class LogInController : Controller
    {
        // GET: LogIn
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(LogInModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new PropertyRentalDBFourthVersionEntities())
                {
                    
                    var isValidCredentials = context.Users.Any(u => u.Username == model.Username && u.Password == model.Password);

                    if (isValidCredentials)
                    {
                        var user = context.Users.Single(u => u.Username == model.Username);

                        if (user != null)
                        {
                            Session["UserID"] = user.UserID.ToString();
                            Session["Username"] = user.Username.ToString();
                            Session["Role"] = user.Role.ToString();

                            switch (user.Role)
                            {
                                case "Administrator":
                                    return RedirectToAction("Index", "Employees");
                                case "Property Manager":
                                    return RedirectToAction("PropertyManager", "Employees");
                                case "Tenant":
                                    return RedirectToAction("Index", "Tenants");
                                default:
                                   
                                    return RedirectToAction("Index", "Home"); // In case there's no match. 
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Wrong credentials.");
                    }
                }
            }
            else
            {
                
                ModelState.AddModelError("", "Please enter valid information.");
            }

            return View(model);
        }
    

    public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}