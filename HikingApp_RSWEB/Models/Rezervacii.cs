using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HikingApp_RSWEB.Models
{
    public class Rezervacii
    {
        public int Id { get; set; }

        [Display(Name = "Тура")]
        public int TuraId { get; set; }
        public Tura Tura { get; set; }

        [Display(Name = "Планинар")]
        public int PlaninarId { get; set; }
        public Planinar Planinar { get; set; }
    }
}
