
using HikingApp_RSWEB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HikingApp_RSWEB.ViewModel
{
    public class RezervaciiPoTura
    {
        public IList<Rezervacii> Rezervacii { get; set; }

        public SelectList TuraMesto { get; set; }
        public string Tura { get; set; }
    }
}
