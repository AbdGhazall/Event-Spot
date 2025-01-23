using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestGp.Models
{
    public class Booking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int BookingID { get; set; }

        [Required]
        public string First_Name { get; set; }

        [Required]
        public string Last_Name { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public int PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAdress { get; set; }

        [Required]
        public int NumberOfTickets { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Event_Title { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public int Tickets { get; set; }
    }
}