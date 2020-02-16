using System.ComponentModel.DataAnnotations;
namespace WebAppStudentWithDapper.Models
{
   [Dapper.Contrib.Extensions.Table("People")]
   public partial class People
   {
      [Dapper.Contrib.Extensions.Key()]
      public int Id { get; set; }

      [Required(ErrorMessage = "Name required")]
      public string Name { get; set; }
      public bool Active { get; set; }
   }
}
