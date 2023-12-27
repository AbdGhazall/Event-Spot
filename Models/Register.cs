using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestGp.Models
{
    public class Register
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Required]
        public int ID { get; set; }
       [Required]
        public string First_Name {  get; set; }
        [Required]
        public string Last_Name { get; set;}
        [Required]
        public string Username {  get; set; }
        [Required]
        public DateTime Birth_Date { get; set; }
        [Required]
   
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set;}
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set;}
     
    }
}
