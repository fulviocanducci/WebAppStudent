using System.ComponentModel.DataAnnotations;

namespace WebAppStudent.Models
{
   public class People
   {
      public int Id { get; set; }

      [Required(ErrorMessage = "Digite o nome")]
      [MinLength(2, ErrorMessage = "Digite o nome no minimo com 2 caracteres")]
      public string Name { get; set; }

      public bool Active { get; set; }
   }
}
