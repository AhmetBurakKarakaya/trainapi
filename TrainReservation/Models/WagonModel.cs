using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainReservation.Models
{
    public class WagonModel
    {
        public int Id { get; set; }

        public string Ad { get; set; }

        public int Kapasite { get; set; }

        public int DoluKoltukAdet { get; set; }

    }
}