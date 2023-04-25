using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainReservation.Models
{
    public class ResponseModel
    {
        public bool RezervasyonYapilabilir { get; set; }

        public List<PlacementDetailModel> YerlesimAyrinti { get; set; }

        //public string HataMesaj { get; set; }

    }
}