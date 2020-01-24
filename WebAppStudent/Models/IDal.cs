using System.Collections.Generic;
using System.Data.SqlClient;

namespace WebAppStudent.Models
{
   public interface IDal<T>
   {
      SqlConnection SqlConnection { get; }
      T Add(T model);
      bool Edit(T model);
      bool Delete(int id);
      IEnumerable<T> All();
   }
}
