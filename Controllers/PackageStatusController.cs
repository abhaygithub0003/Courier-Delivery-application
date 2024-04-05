using Microsoft.AspNetCore.Mvc;
using Courier_Tracking_and_Delivery_System.Data;
using Courier_Tracking_and_Delivery_System.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Net;

namespace Courier_Tracking_and_Delivery_System.Controllers
{
    public class PackageStatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PackageStatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int packageId)
        {
            IQueryable<PackageStatus> packageStatuses = _context.PackageStatuses;

            if (packageId != 0)
            {
                packageStatuses = packageStatuses.Where(p => p.PackageId == packageId);
            }
            var packageStatusList = packageStatuses.ToList();

            return View(packageStatusList);
        }


        public IActionResult Create(int packageId)
        {
            var packageStatus = new PackageStatus { PackageId = packageId };
            return View(packageStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PackageStatus packageStatus)
        {
            packageStatus.StatusDate = DateTime.Now;
            var existingPackageStatus = _context.PackageStatuses.FirstOrDefault(p => p.PackageId == packageStatus.PackageId);

            if (existingPackageStatus != null)
            {
               
                existingPackageStatus.Status = packageStatus.Status;
                existingPackageStatus.StatusDate = packageStatus.StatusDate;
                existingPackageStatus.Remarks = packageStatus.Remarks;
            }
            else
            {
                
                _context.PackageStatuses.Add(packageStatus);
            }
            try
            {
                string smtpServer = "smtp-mail.outlook.com";
                int smtpPort = 587;
                string smtpUsername = "abhaypunia123@outlook.com";
                string smtpPassword = "Aa@123456789";

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(smtpUsername);
                mail.To.Add("abhaypunia17@gmail.com");
                mail.Subject = "New Package Created";
                mail.Body = "A new package has been created with the following details:\n" +
                            "Package ID: " + packageStatus.Id + "\n" +
                            "Status: " + packageStatus.Status + "\n" +
                            "Status Date: " + packageStatus.StatusDate + "\n" +
                            "Remarks: " + packageStatus.Remarks;
                           
 
                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;
                smtpClient.Send(mail);

                ViewBag.Message = "Package created successfully and email sent!";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while sending email: " + ex.Message;
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
