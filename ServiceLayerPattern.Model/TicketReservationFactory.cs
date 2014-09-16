using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayerPattern.Model
{
    public class TicketReservationFactory
    {
        public static TicketReservation CreateTicket(Event pEvent, int ticketQuantity)
        {
            TicketReservation ticketReservation = new TicketReservation();
            ticketReservation.Id = Guid.NewGuid();
            ticketReservation.Event = pEvent;
            ticketReservation.ExpiryTime = DateTime.Now.AddMinutes(1);
            ticketReservation.TicketQuantity = ticketQuantity;

            return ticketReservation;
        }
    }
}
