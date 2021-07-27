using HikingApp_RSWEB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HikingApp_RSWEB.ViewModel
{
    public class FiltrirajVodichi
    {
        public IList<Vodich> Vodichi { get; set; }

        public SelectList Pozicii { get; set; }
        public string SearchPozicija { get; set; }
    }
}
