using HikingApp_RSWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HikingApp_RSWEB.ViewModel
{
    public class FiltrirajPlaninari
    {

        public IList<Planinar> Planinari { get; set; }
        public string SearchStringName { get; set; }
        public string SearchStringSurname { get; set; }
    }
}
