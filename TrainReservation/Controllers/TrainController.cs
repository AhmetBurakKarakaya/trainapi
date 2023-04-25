using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrainReservation.Models;
using TrainReservation.Services;

namespace TrainReservation.Controllers
{
    public class TrainController : ApiController
    {

        [HttpPost]
        public ResponseModel Post([FromBody] RequestModel request)
        {
            ResponseModel response = new ResponseModel();

            response = TrainService.VagonaAta(request);

            return response;
        }
       
    }
}