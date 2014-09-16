using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayerPattern.Model
{
    public class Event
    {
        public Event() 
        {
            TicketPurchases = new List<TicketPurchase>();
            TicketReservations = new List<TicketReservation>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Allocation { get; set; }
        public List<TicketPurchase> TicketPurchases { get; set; }
        public List<TicketReservation> TicketReservations { get; set; }

        public int AvailableAllocation() 
        {
            int salesAndReservaations = 0;
            TicketPurchases.ForEach(t => salesAndReservaations += t.TicketQuantity);
            TicketReservations.FindAll(r => r.StillActive()).ForEach(t => salesAndReservaations += t.TicketQuantity);

            return Allocation - salesAndReservaations;
        }

        public bool CanPurchaseTicketWith(Guid reservationId) 
        {
            if (HasReservationWith(reservationId))
            {
                GetTicketReservationWith(reservationId).StillActive();
            }

            return false;
        }

        public TicketReservation GetTicketReservationWith(Guid reservationId) 
        {
            if (!HasReservationWith(reservationId))
            {
                throw new ApplicationException("没有找到可匹配的预定票");
            }

            return TicketReservations.FirstOrDefault(t => t.Id == reservationId);
        }

        private bool HasReservationWith(Guid reservationId) 
        {
            return TicketReservations.Exists(r => r.Id == reservationId);
        }

        public TicketPurchase PurchaseTicketWith(Guid reservationId) 
        {
            if (!CanPurchaseTicketWith(reservationId))
            {
                throw new ApplicationException("此预定票不能被购买");
            }
            TicketReservation ticketReservation = GetTicketReservationWith(reservationId);
            TicketPurchase ticketPurchase = TicketPurchaseFactory.CreateTicket(this, ticketReservation.TicketQuantity);
            ticketReservation.HasBeenRedeemed = true;
            TicketPurchases.Add(ticketPurchase);

            return ticketPurchase;
        }

        public bool CanReservateTicket(int ticketQuantity) 
        {
            return AvailableAllocation() >= ticketQuantity;
        }

        public TicketReservation ReservateTicket(int ticketQuantity) 
        {
            if (!CanReservateTicket(ticketQuantity))
            {
                throw new ApplicationException("此票不能被预定");
            }

            TicketReservation reservation = TicketReservationFactory.CreateTicket(this, ticketQuantity);
            TicketReservations.Add(reservation);

            return reservation;
        }

        public string DetermineWhyATicketCannotbePurchasedWith(Guid reservationId)
        {
            string reservationIssue = "";
            if (HasReservationWith(reservationId))
            {
                TicketReservation reservation = GetTicketReservationWith(reservationId);
                if (reservation.HasExpired())
                    reservationIssue = String.Format("Ticket reservation '{0}' has expired", reservationId.ToString());
                else if (reservation.HasBeenRedeemed)
                    reservationIssue = String.Format("Ticket reservation '{0}' has already been redeemed", reservationId.ToString());
            }
            else
                reservationIssue = String.Format("There is no ticket reservation with the Id '{0}'", reservationId.ToString());

            return reservationIssue;
        }

        private void ThrowExceptionWithDetailsOnWhyTicketsCannotBeReserved()
        {
            throw new ApplicationException("There are no tickets available to reserve.");
        }
    }
}
