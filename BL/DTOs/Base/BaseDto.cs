using System;
using System.Collections.Generic;
using System.Text;

namespace BL.DTOs.Base
{
    public class BaseDto
    {
        public Guid Id { get; set; }
        public enEntityState CurrentState { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
