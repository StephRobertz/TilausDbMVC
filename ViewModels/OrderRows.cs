using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TilausDbMVC.Models;

namespace TilausDbMVC.ViewModels
{
   

    public class OrderRows
    {

        public int TilausID { get; set; }
        public int TuoteID { get; set; }
        public float Ahinta { get; set; }
        public string Postitoimipaikka { get; set; }
        public string Nimi { get; set; }
        public int Maara { get; set; }
        public int TilausriviID { get; set; }

    }
}