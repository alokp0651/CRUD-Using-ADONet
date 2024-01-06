using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDWithADONet.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        [DisplayName("Date Of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime DateofBirth { get; set; }
        
        [DisplayName("Email")]
        [Required]
        public string Email { get; set; }

        [DisplayName("Salary")]
        [Required]
        public double Salary { get; set; }

        [NotMapped]  // because feilds are not mapped and it will give the model validation error
        public string FullName
        {
            get { return FirstName  + " " + LastName; }
        }

    }
}
