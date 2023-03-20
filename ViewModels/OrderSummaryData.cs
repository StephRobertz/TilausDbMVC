

namespace TilausDbMVC.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using TilausDbMVC.Models;

    public class OrderSummaryData
    {
        public int TilausID { get; set; }
        public int AsiakasID { get; set; }
        public int TuoteID { get; set; }
        public float Ahinta { get; set; }
        public string Toimitusosoite { get; set; }
        public Nullable<System.DateTime>Tilauspvm { get; set; }
        public Nullable<System.DateTime> Toimituspvm { get; set; }
        public string Postinumero { get; set; }
        public string Postitoimipaikka { get; set; }
        public string Nimi { get; set; }    
        public int Maara { get; set; }
        public int TilausriviID { get; set; }

    }
}