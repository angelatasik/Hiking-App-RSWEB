using HikingApp_RSWEB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HikingApp_RSWEB.ViewModel
{
    public class FiltrirajTuri
    {
        public IList<Tura> Turi { get; set; }
        public IList<Vodich>  Vodichi { get; set; }
        //public IList<Vodich> Vodichi { get; set; }
        public SelectList Tezini { get; set; }

        //public SelectList Datumi { get; set; }

        //public DateTime Datum { get; set;}
        //public SelectList Vodicii { get; set; }

        public string SearchStringName { get; set; }
        public string Tezina { get; set; }
        //public string Vodichii { get; set; }
    }
}
