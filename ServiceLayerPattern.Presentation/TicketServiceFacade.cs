using ServiceLayerPattern.Contracts;
using ServiceLayerPattern.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayerPattern.Presentation
{
    public class TicketServiceFacade
    {
        private ITicketService _ticketService;

        public TicketServiceFacade(ITicketService ticketService) 
        {
            _ticketService = ticketService;
        }

        public TicketReservationPresentation ReserveTicketsFor(string eventId, int countOfTickets) 
        {
            TicketReservationPresentation reservationPresentation = new TicketReservationPresentation();
            ReservateTicketRequest request = new ReservateTicketRequest();
            request.EventId = eventId;
            request.TicketQuantity = countOfTickets;
            ReservateTicketResponse response = _ticketService.ReservateTicket(request);
            if (response.Success)
            {
                reservationPresentation.TicketWasSuccessfullyReserved = true;
                reservationPresentation.ReservationId = response.ReservationId;
                reservationPresentation.ExpiryDate = response.ExpirationDate;
                reservationPresentation.EventId = response.EventId;
                reservationPresentation.Description = String.Format("{0} ticket(s) reserved for {1}.<br/><small>This reservation will expire on {2} at {3}.</small>", 
                    response.CountOfTickets, response.EventName, response.ExpirationDate.ToLongDateString(), 
                    response.ExpirationDate.ToLongTimeString());
            }
            else 
            {
                reservationPresentation.TicketWasSuccessfullyReserved = false;
                reservationPresentation.Description = response.Message;
            }

            return reservationPresentation;
        }

        public TicketPresentation PurchaseReservedTicket(string EventId, string ReservationId)
        {
            TicketPresentation ticket = new TicketPresentation();
            PurchaseTicketResponse response = new PurchaseTicketResponse();
            PurchaseTicketRequest request = new PurchaseTicketRequest();
            request.ReservationId = ReservationId;
            request.EventId = EventId;
            request.CorrelationId = ReservationId; // In this instance we can use the ReservationId

            response = _ticketService.PurchaseTicket(request);
            if (response.Success)
            {
                ticket.Description = String.Format("{0} ticket(s) purchased for {1}.<br/><small>Your e-ticket id is {2}.</small>", response.CountOfTickets, response.EventName, response.TicketId);
                ticket.EventId = response.EventId;
                ticket.TicketId = response.TicketId;
                ticket.WasAbleToPurchaseTicket = true;
            }
            else
            {
                ticket.WasAbleToPurchaseTicket = false;
                ticket.Description = response.Message;
            }

            return ticket;
        }
    }
}
