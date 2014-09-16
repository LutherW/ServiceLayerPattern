using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayerPattern.Model
{
    public class TicketPurchaseFactory
    {
        public static TicketPurchase CreateTicket(Event pEvent, int ticketQuantity) 
        {
            TicketPurchase tiketPurchase = new TicketPurchase();
            tiketPurchase.Id = Guid.NewGuid();
            tiketPurchase.Event = pEvent;
            tiketPurchase.TicketQuantity = ticketQuantity;

            return tiketPurchase;
        }
    }
}
