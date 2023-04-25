using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainReservation.Models
{
    public class RequestModel
    {
        public TrainModel Tren { get; set; }

        public int RezervasyonYapilacakKisiSayisi { get; set; }

        public bool KisilerFarkliVagonlaraYerlestirilebilir { get; set; }
    }
}