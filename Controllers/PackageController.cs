using Microsoft.AspNetCore.Mvc;
using Courier_Tracking_and_Delivery_System.Models;
using System.Linq;
using Courier_Tracking_and_Delivery_System.Data;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Courier_Tracking_and_Delivery_System.Controllers
{
    [Authorize(Roles=SD.Role_Admin)]
    public class PackageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PackageController(ApplicationDbContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Package package)
        {
            if (ModelState.IsValid)
            {
                _context.Packages.Add(package);
                _context.SaveChanges();

                // Email sending functionality is here
                try
                {
                    string smtpServer = "smtp-mail.outlook.com";
                    int smtpPort = 587;
                    string smtpUsername = "abhaypunia123@outlook.com";
                    string smtpPassword = "Aa@123456789";

                    // I have Created the email message here
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(smtpUsername);
                    mail.To.Add("abhaypunia17@gmail.com");
                    mail.Subject = "New Package Created";
                    mail.Body = "A new package has been created with the following details:\n" +
                                "Package ID: " + package.Id + "\n" +
                                "Pickup Address: " + package.PickupAddress + "\n" +
                                "Delivery Address: " + package.DeliveryAddress + "\n" +
                                "Package Type: " + package.PackageType + "\n" +
                                "Preferred Delivery Time: " + package.PreferredDeliveryTime + "\n" +
                                "Delivery By: " + package.DeliveryBy;

                    SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;

                    //it is  used to Send the email
                    smtpClient.Send(mail);

                    ViewBag.Message = "Package created successfully and email sent!";
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "An error occurred while sending email: " + ex.Message;
                }

                return RedirectToAction("ConfirmOrder");
            }
            return View(package);
        }
    

  
    public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = _context.Packages.Find(id);
            if (package == null)
            {
                return NotFound();
            }
            return View(package);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Package package)
        {
            if (id != package.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(package);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(package);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = _context.Packages.Find(id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

     
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var package = _context.Packages.Find(id);
            _context.Packages.Remove(package);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
 
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = _context.Packages.Find(id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }
     
        public IActionResult Index()
        {
            var packages = _context.Packages.ToList();
            return View(packages);
        }
        [AllowAnonymous]
            public IActionResult ConfirmOrder()
            {
                return View();
            }

    }

}
