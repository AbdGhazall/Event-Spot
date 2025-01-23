using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestGp.Models
{
    public enum Types
    { Consert, experience, Trip, Workshop }

    public enum OPlace
    { Yes, No }

    public enum EvPlace
    { Amman, Aqaba, Deadsea, Petra }

    public class Event
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int ID { get; set; }

        [Required]
        public string EventTitle { get; set; }

        [Required]
        public string Organizer { get; set; }

        [Required]
        public string Tags { get; set; }

        [Required]
        public Types Type { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateAndTime { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string Link { get; set; }

        [Required]
        public OPlace Typ { get; set; }

        [Required]
        public EvPlace Place { get; set; }

        [Required]
        public int Tickects { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public string Event_Image { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}