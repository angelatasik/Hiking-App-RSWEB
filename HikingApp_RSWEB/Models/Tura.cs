using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HikingApp_RSWEB.Models
{
    public class Tura
    {
        public int Id { get; set; }

        [Display(Name = "Место")]
        public string Mesto { get; set; }

        [Display(Name = "Датум-почеток")]
        public DateTime DatumPocetok { get; set; }

        [Display(Name = "Датум-крај")]
        public DateTime DatumKraj { get; set; }

        [Display(Name = "Тежина")]
        public string Tezina { get; set; }

        [Display(Name = "Времетраење")]
        public string Vremetraenje { get; set; }

        [Display(Name = "Водич1")]
        public int? FirstVodichId { get; set; }
        [Display(Name = "Водич1")]
        public Vodich FirstVodich { get; set; }

        [Display(Name = "Водич2")]
        public int? SecoundVodichId { get; set; }
        [Display(Name = "Водич2")]
        public Vodich SecoundVodich { get; set; }

        public ICollection<Rezervacii> Planinari { get; set; }
    }
}
