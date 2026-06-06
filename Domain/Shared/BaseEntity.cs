using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shared
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        public Guid? UpdatedBy { get; set; }

        public enEntityState CurrentState { get; set; }

        public DateTime CreatedDate { get; set; } 

        public Guid CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
