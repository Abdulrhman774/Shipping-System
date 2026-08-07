using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace UI.Helpers
{
    public static class PermissionMatrix
    {
        // طريقة مساعدة لدمج الأدوار
        public const string Admin = AppRoles.Admin;
        public const string OpManager = AppRoles.OpManager;
        public const string Op = AppRoles.Op;
        public const string Reviewer = AppRoles.Reviewer;

        // قاموس: Controller -> (Action -> الأدوار المسموحة)
        public static readonly Dictionary<string, Dictionary<string, string>> Permissions = new()
        {
            // -------------------------------------------------------
            // 1. Account Controller (الملف الشخصي)
            // -------------------------------------------------------
            ["Account"] = new()
            {
                ["Profile"] = Admin + "," + OpManager + "," + Op + "," + Reviewer,
                ["ChangePassword"] = Admin + "," + OpManager + "," + Op + "," + Reviewer
            },

            // -------------------------------------------------------
            // 2. Dashboard Controller
            // -------------------------------------------------------
            ["Dashboard"] = new()
            {
                ["Index"] = Admin + "," + OpManager + "," + Op + "," + Reviewer, // الجميع يشاهد
                ["Analytics"] = Admin + "," + OpManager + "," + Reviewer // ❌ Op ممنوع من التقارير (كما اختبرت سابقاً)
            },

            // -------------------------------------------------------
            // 3. Shipment Controller (الصلاحيات المعقدة)
            // -------------------------------------------------------
            ["Shipment"] = new()
            {
                ["Index"] = Admin + "," + OpManager + "," + Op + "," + Reviewer,
                ["Details"] = Admin + "," + OpManager + "," + Op + "," + Reviewer,
                ["Create"] = Admin + "," + OpManager + "," + Op,
                ["Edit"] = Admin + "," + OpManager + "," + Op,
                ["Delete"] = Admin,
                ["Approve"] = Admin + "," + OpManager + "," + Reviewer,
                ["MarkReadyForShip"] = Admin + "," + OpManager + "," + Op,
                ["MarkShipped"] = Admin + "," + OpManager
            },

            // -------------------------------------------------------
            // 4. Carrier Controller (و باقي CRUD Lists)
            // -------------------------------------------------------
            ["Carrier"] = new()
            {
                ["Index"] = Admin + "," + OpManager + "," + Op,
                ["Create"] = Admin + "," + OpManager + "," + Op,
                ["Edit"] = Admin + "," + OpManager, // ❌ Op لا يستطيع التعديل
                ["Delete"] = Admin + "," + OpManager
            },

            // -------------------------------------------------------
            // 5. City Controller
            // -------------------------------------------------------
            ["City"] = new()
            {
                ["Index"] = Admin + "," + OpManager + "," + Op,
                ["Create"] = Admin + "," + OpManager + "," + Op,
                ["Edit"] = Admin + "," + OpManager,
                ["Delete"] = Admin + "," + OpManager
            },

            // -------------------------------------------------------
            // 6. Country Controller
            // -------------------------------------------------------
            ["Country"] = new()
            {
                ["Index"] = Admin + "," + OpManager + "," + Op,
                ["Create"] = Admin + "," + OpManager + "," + Op,
                ["Edit"] = Admin + "," + OpManager,
                ["Delete"] = Admin + "," + OpManager
            },

            // -------------------------------------------------------
            // 7. PaymentMethod Controller
            // -------------------------------------------------------
            ["PaymentMethod"] = new()
            {
                ["Index"] = Admin + "," + OpManager + "," + Op,
                ["Create"] = Admin + "," + OpManager + "," + Op,
                ["Edit"] = Admin + "," + OpManager,
                ["Delete"] = Admin + "," + OpManager
            },

            // -------------------------------------------------------
            // 8. ShippingType Controller
            // -------------------------------------------------------
            ["ShippingType"] = new()
            {
                ["Index"] = Admin + "," + OpManager + "," + Op,
                ["Create"] = Admin + "," + OpManager + "," + Op,
                ["Edit"] = Admin + "," + OpManager,
                ["Delete"] = Admin + "," + OpManager
            },

            // -------------------------------------------------------
            // 9. SubscriptionPackage Controller
            // -------------------------------------------------------
            ["SubscriptionPackage"] = new()
            {
                ["Index"] = Admin + "," + OpManager + "," + Op,
                ["Create"] = Admin + "," + OpManager + "," + Op,
                ["Edit"] = Admin + "," + OpManager,
                ["Delete"] = Admin + "," + OpManager
            },

            // -------------------------------------------------------
            // 10. UserSender Controller
            // -------------------------------------------------------
            ["UserSender"] = new()
            {
                ["Index"] = Admin + "," + OpManager + "," + Op + "," + Reviewer,
                ["Details"] = Admin + "," + OpManager + "," + Op + "," + Reviewer,
                ["Create"] = Admin + "," + OpManager + "," + Op,
                ["Edit"] = Admin + "," + OpManager + "," + Op, // Op يقدر يعدل بتاعه بس (اللوجيك هيتحقق في الكونترولر)
                ["Delete"] = Admin + "," + OpManager
            },

            // -------------------------------------------------------
            // 11. UserReceiver Controller
            // -------------------------------------------------------
            ["UserReceiver"] = new()
            {
                ["Index"] = Admin + "," + OpManager + "," + Op + "," + Reviewer,
                ["Details"] = Admin + "," + OpManager + "," + Op + "," + Reviewer,
                ["Create"] = Admin + "," + OpManager + "," + Op,
                ["Edit"] = Admin + "," + OpManager + "," + Op,
                ["Delete"] = Admin + "," + OpManager
            },

            // -------------------------------------------------------
            // 12. UserSubscription Controller
            // -------------------------------------------------------
            ["UserSubscription"] = new()
            {
                ["Index"] = Admin + "," + OpManager + "," + Reviewer, // ❌ Op لا يراها
                ["Create"] = Admin + "," + OpManager,
                ["Edit"] = Admin + "," + OpManager,
                ["Delete"] = Admin + "," + OpManager
            },

            // -------------------------------------------------------
            // 13. Setting Controller (Admin فقط)
            // -------------------------------------------------------
            ["Setting"] = new()
            {
                ["Edit"] = Admin // ❌ الجميع ممنوع إلا الأدمن
            }
        };

        /// <summary>الحصول على الأدوار المسموحة لـ Controller و Action معينين</summary>
        public static string? GetRoles(string controller, string action)
        {
            if (Permissions.TryGetValue(controller, out var actions))
                if (actions.TryGetValue(action, out var roles))
                    return roles;
            return null;
        }
    }
}