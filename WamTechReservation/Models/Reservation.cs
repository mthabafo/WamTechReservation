using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WamTechReservation.Models
{
    [Table(name: "Reservations")]
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname Required")]
        public string Surname { get; set; }

        [MinLength(10, ErrorMessage = "Telephone must be a 10 digit number")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Reservation Type required")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Start date required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndDate { get; set; }
    }
}
