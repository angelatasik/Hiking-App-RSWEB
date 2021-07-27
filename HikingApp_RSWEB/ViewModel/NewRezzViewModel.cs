using HikingApp_RSWEB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HikingApp_RSWEB.ViewModel
{
    public class NewRezzViewModel
    {
        public Rezervacii Rezervacii { get; set; }
        public virtual Tura Tura { get; set; }
        public int TuraId { get; set; }


        [Display(Name = "Планинари")]
        public IEnumerable<int> SelectedPlaninari { get; set; }
        public SelectList Turi { get; set;}
        
        public SelectList Planinari { get; set; }
    }
}
