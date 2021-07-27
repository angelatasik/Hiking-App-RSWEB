using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HikingApp_RSWEB.Models
{
    public class Vodich
    {
        public int Id { get; set; }

        [Display(Name = "Име")]
        public string Ime { get; set; }

        [Display(Name = "Презиме")]
        public string Prezime { get; set; }

        [Display(Name = "Позиција")]
        public string Pozicija { get; set; }

        [Display(Name = "Возраст")]
        public int Vozrast { get; set; }

        // [DataType(DataType.PhoneNumber)]
        //[Phone]
        //public string MobilePhone { get; set; }
        /*https://www.twilio.com/blog/validating-phone-numbers-effectively-with-c-and-the-net-frameworks
        */
        [Display(Name = "Слика")]
        public string ProfilePicture { get; set; }

        [Display(Name = "Тура 1")]
        public ICollection<Tura> Tura1 { get; set; }

        [Display(Name = "Тура 2")]
        public ICollection<Tura> Tura2 { get; set; }

        public string FullName
        {
            get { return String.Format("{0} {1}", Ime, Prezime); }
        
        }


    }
}
