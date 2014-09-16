using ServiceLayerPattern.DataContract;
using ServiceLayerPattern.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayerPattern.Service
{
    public static class TicketReservationExtensionMethods
    {
        public static ReservateTicketResponse ConvertToReservateTicketResponse(this TicketReservation ticketReservation) 
        {
            ReservateTicketResponse response = new ReservateTicketResponse();
            response.ReservationId = ticketReservation.ToString();
            response.ExpirationDate = ticketReservation.ExpiryTime;
            response.EventId = ticketReservation.Event.Id.ToString();
            response.EventName = ticketReservation.Event.Name;
            response.CountOfTickets = ticketReservation.TicketQuantity;

            return response;
        }
    }
}
