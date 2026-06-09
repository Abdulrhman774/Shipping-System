using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace DAL.Contracts;

public interface IViewRepository<TView> where TView : class
{
    Task<IEnumerable<TView>> GetAllAsync();  
}
