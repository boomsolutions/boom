using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApplication4.Models
{
    public class LogVisitas //: System.Data.Entity
    {
        [Key]
        public int IdLogVisita { get; set; }

        public DateTime? Fecha { get; set; }

        [Required]
        public string IP { get; set; } //?????????

        public int? IdCampaign { get; set; }

    }
}