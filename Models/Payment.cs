using System.ComponentModel.DataAnnotations;

namespace TestGp.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Card_Name { get; set; }

        [Required]
        public string Card_Number { get; set; }

        [Required]
        public string Exp_Date { get; set; }

        [Required]
        public string Cvv { get; set; }
    }
}