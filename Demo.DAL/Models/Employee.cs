using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Enter Your Name")]
        [MaxLength(50,ErrorMessage ="Max Length 50 Chars") ]
        [MinLength(5, ErrorMessage = "Min Length 5 Chars")]
        public string Name { get; set; }

        [Range(22,35,ErrorMessage ="Your Age Must be in Range From 22 To 35")]
        public int? Age { get; set; }

        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{5,10}-[a-zA-Z]{5,10}$",
            ErrorMessage ="Your Address Must Like 123-street-City-Country")]
        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal  Salary { get; set; }

        public bool IsActive { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        public DateTime HiringDate { get; set; }
        public DateTime CreationDate { get; set; }=DateTime.Now;
        [ForeignKey("Departments")]
        public int? DeptID { get; set; }
        [InverseProperty("Employees")]
        public Department Departments { get; set; }
    }
}
