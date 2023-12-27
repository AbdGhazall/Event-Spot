using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestGp.Models
{
    public class Booking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Required]
        public int BookingID { get; set; }
        [Required]
        public string First_Name { get; set; }
        [Required]
        public string Last_Name { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
       
        [DataType(DataType.EmailAddress)]
        public string EmailAdress { get; set; }
        [Required]
        public int NumberOfTickets {  get; set; }   
    
    }
}
