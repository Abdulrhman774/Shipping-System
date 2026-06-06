using BL.Contract;
using BL.Dtos;
using DAL.Contracts;
using Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using Ui.Models;

namespace Ui.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //void TestShipment()
        //{
        //    var testShipment = new ShipmentDto
        //    {
        //        Id = Guid.NewGuid(),
        //        ShipingDate = DateTime.UtcNow,
        //        DelivryDate = DateTime.UtcNow.AddDays(3),

        //        SenderId = Guid.Empty,
        //        UserSender = new UserSenderDto
        //        {
        //            Id = Guid.NewGuid(),
        //            UserId = Guid.NewGuid(),
        //            SenderName = "Ali Sender",
        //            Email = "sender@example.com",
        //            Phone = "01012345678",
        //            PostalCode = "12345",
        //            Contact = "Ali Sender Contact",
        //            OtherAddress = "Apartment 5B, Sender Tower",
        //            IsDefault = true,
        //            CityId = Guid.Parse("ebfd4e1f-1126-4f75-9c36-a25cf22efd41"),
        //            Address = "123 Sender Street"
        //        },

        //        ReceiverId = Guid.Empty,
        //        UserReceiver = new UserReceiverDto
        //        {
        //            Id = Guid.NewGuid(),
        //            UserId = Guid.NewGuid(),
        //            ReceiverName = "Omar Receiver",
        //            Email = "receiver@example.com",
        //            Phone = "01087654321",
        //            PostalCode = "54321",
        //            Contact = "Omar Receiver Contact",
        //            OtherAddress = "Floor 2, Receiver Building",
        //            CityId = Guid.Parse("ebfd4e1f-1126-4f75-9c36-a25cf22efd41"),
        //            Address = "456 Receiver Road"
        //        },

        //        ShippingTypeId = Guid.Parse("459afb92-3374-4b02-ac67-086590009462"),
        //        ShipingPackgingId = null, // optional
        //        Width = 25.0,
        //        Height = 15.0,
        //        Weight = 5.5,
        //        Length = 30.0,
        //        PackageValue = 1000m,
        //        ShippingRate = 75.0m,
        //        PaymentMethodId = null,
        //        UserSubscriptionId = null,
        //        TrackingNumber = 10000001,
        //        ReferenceId = Guid.NewGuid()
        //    };
        //    _IShipment.Create(testShipment);
        //}

        public IActionResult Index()
        {
            //TestShipment();
            return View();
        }

        public IActionResult Payment()
        {
            //TestShipment();
            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
