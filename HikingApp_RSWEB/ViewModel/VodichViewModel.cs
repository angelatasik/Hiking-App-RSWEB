using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HikingApp_RSWEB.ViewModel
{
    public class VodichViewModel
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

        [Display(Name = "Слика")]

        public IFormFile? Picture { get; set; }

    }
}
