using BL.Contract;
using BL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Ui.Helpers;
using System.Threading.Tasks;
namespace Ui.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class ShippingTypesController : Controller
    {
        private readonly IShippingType _IShippingTypes;
        public ShippingTypesController(IShippingType shipingTypes)
        {
            _IShippingTypes=shipingTypes;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _IShippingTypes.GetAll();
            return View(data);
        }

        public async Task<IActionResult> Edit(Guid? Id)
        {
            TempData["MessageType"] = null;
            var data=new BL.Dtos.ShippingTypeDto(); 
            if (Id != null)
            {
                data = await _IShippingTypes.GetById((Guid)Id);
            }
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ShippingTypeDto data)
        {
            TempData["MessageType"] = null;
            if (!ModelState.IsValid)
                return View("Edit", data);
            try
            {
                if (data.Id == Guid.Empty)
                    await _IShippingTypes.Add(data);
                else
                    await _IShippingTypes.Update(data);
                TempData["MessageType"] = MessageTypes.SaveSucess;
            }
            catch (Exception ex)
            {
                TempData["MessageType"] = MessageTypes.SaveFailed;
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid Id)
        {
            TempData["MessageType"] = null;
            try
            {
                await _IShippingTypes.ChangeStatus(Id, 0);
                TempData["MessageType"] = MessageTypes.DeleteSuccess;
            }
            catch (Exception ex)
            {
                TempData["MessageType"] = MessageTypes.DeleteFailed;
            }

            return RedirectToAction("Index");
        }
    }
}
