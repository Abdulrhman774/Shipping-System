// UI/Controllers/ContactController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UI.Controllers
{
    [AllowAnonymous]
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ContactViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // TODO: إرسال البريد الإلكتروني أو حفظ الرسالة في قاعدة البيانات
            TempData["SuccessMessage"] = "Your message has been sent successfully! We will get back to you soon.";

            return RedirectToAction("Index");
        }
    }

    public class ContactViewModel
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        [StringLength(100)]
        public string? Company { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Message must be between 10 and 1000 characters")]
        public string? Message { get; set; }
    }
}