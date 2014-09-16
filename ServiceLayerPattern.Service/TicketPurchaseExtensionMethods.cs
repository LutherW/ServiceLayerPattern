using ServiceLayerPattern.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayerPattern.Model;

namespace ServiceLayerPattern.Service
{
    public static class TicketPurchaseExtensionMethods
    {
        public static PurchaseTicketResponse ConvertToPurchaseTicketResponse(this TicketPurchase ticketPurchase) 
        {
            PurchaseTicketResponse response = new PurchaseTicketResponse();
            response.TicketId = ticketPurchase.ToString();
            response.EventId = ticketPurchase.Event.Id.ToString();
            response.EventName = ticketPurchase.Event.Name;
            response.CountOfTickets = ticketPurchase.TicketQuantity;

            return response;
        }
    }
}
