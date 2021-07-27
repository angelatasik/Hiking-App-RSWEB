using HikingApp_RSWEB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HikingApp_RSWEB.ViewModel
{
    public class EditTuraViewModel
    {
        public Tura Tura { get; set; }
        public IEnumerable<int> SelectedPlaninari { get; set; }
        public IEnumerable<SelectListItem> ListaPlaninari { get; set; }
    }
}
