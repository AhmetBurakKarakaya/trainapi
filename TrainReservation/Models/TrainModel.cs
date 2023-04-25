using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainReservation.Models
{
    public class TrainModel
    {
        public int Id { get; set; }

        public string Ad { get; set; }

        public List<WagonModel> Vagonlar { get; set; }

    }
}