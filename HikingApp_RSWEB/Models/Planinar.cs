using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HikingApp_RSWEB.Models
{
    public class Planinar
    {
        public int Id { get; set; }

        [Display(Name = "Име")]
        public string Ime { get; set; }

        [Display(Name = "Презиме")]
        public string Prezime { get; set; }

        [Display(Name = "Возраст")]
        public int Vozrast { get; set; }

        [Display(Name = "Слика")]
        public string ProfilePicture { get; set; }

        [Display(Name = "Планинар")]
        public string FullName
        {
            get { return String.Format("{0} {1}", Ime, Prezime); }
        }

        public ICollection<Rezervacii> Turi { get; set; }
    }
}
