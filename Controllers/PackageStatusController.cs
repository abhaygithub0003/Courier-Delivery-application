using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Courier_Tracking_and_Delivery_System.Data;
using Courier_Tracking_and_Delivery_System.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Courier_Tracking_and_Delivery_System.Controllers
{
    public class PackageStatusController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public PackageStatusController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public IActionResult Index(string name)
        {
            IQueryable<PackageStatus> packageStatuses = _context.PackageStatuses;

            // If a name is provided in the query string, filter package statuses by name
            if (!string.IsNullOrEmpty(name))
            {
                packageStatuses = packageStatuses.Where(p => p.Name.Contains(name));
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
        public async Task<IActionResult> Create(PackageStatus packageStatus)
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
                // Get the current user
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    // Get the user's email
                    string userEmail = user.Email;

                    // Send email using the user's email
                    await _emailSender.SendEmailAsync(userEmail, "New Package Created",
                        "A new package has been created with the following details:<br/>" +
                        "Package ID: " + packageStatus.Id + "<br/>" +
                        "Status: " + packageStatus.Status + "<br/>" +
                        "Status Date: " + packageStatus.StatusDate + "<br/>" +
                        "Remarks: " + packageStatus.Remarks);

                    ViewBag.Message = "Package created successfully and email sent!";
                }
                else
                {
                    ViewBag.Error = "User not found or email address is null.";
                }
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
