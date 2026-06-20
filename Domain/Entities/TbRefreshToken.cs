using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TbRefreshToken : BaseEntity
    {
        public string Token { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public DateTime Expires { get; set; }
        public DateTime? RevokedDate { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
