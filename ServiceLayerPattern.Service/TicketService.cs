using ServiceLayerPattern.Contracts;
using ServiceLayerPattern.DataContract;
using ServiceLayerPattern.Model;
using ServiceLayerPattern.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayerPattern.Service
{
    public class TicketService : ITicketService
    {
        private IEventRepository _eventRepository;
        private static MessageResponseHistory<PurchaseTicketResponse> _messageResponse;

        public TicketService(IEventRepository eventRepository) 
        {
            _eventRepository = eventRepository;
        }

        public TicketService()
            :this(new EventRepository())
        {

        }

        public ReservateTicketResponse ReservateTicket(ReservateTicketRequest request)
        {
            ReservateTicketResponse response = new ReservateTicketResponse();
            try
            {
                Event eventEntity = _eventRepository.FindBy(new Guid(request.EventId));
                TicketReservation reservation;
                if (eventEntity.CanReservateTicket(request.TicketQuantity))
                {
                    reservation = eventEntity.ReservateTicket(request.TicketQuantity);
                    _eventRepository.Save(eventEntity);
                    response = reservation.ConvertToReservateTicketResponse();
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.Message = string.Format("只有{0}数量的票是可以预定的", eventEntity.AvailableAllocation());
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorLog.GenerateErrorRefMessageAndLog(ex);
            }

            return response;
        }

        public PurchaseTicketResponse PurchaseTicket(PurchaseTicketRequest request)
        {
            PurchaseTicketResponse response = new PurchaseTicketResponse();
            try
            {
                if (_messageResponse.IsUniqueRequest(request.CorrelationId))
                {
                    TicketPurchase ticketPurchase;
                    Event eventEntity = _eventRepository.FindBy(new Guid(request.ReservationId));
                    if (eventEntity.CanPurchaseTicketWith(new Guid(request.ReservationId)))
                    {
                        ticketPurchase = eventEntity.PurchaseTicketWith(new Guid(request.ReservationId));
                        _eventRepository.Save(eventEntity);
                        response = ticketPurchase.ConvertToPurchaseTicketResponse();
                        response.Success = true;
                    }
                    else 
                    {
                        response.Message = eventEntity.DetermineWhyATicketCannotbePurchasedWith(new Guid(request.ReservationId));
                        response.Success = false;
                    }
                    _messageResponse.LogResponse(request.CorrelationId, response);
                }
                else
                {
                    response = _messageResponse.RetrievePreviousResponseFor(request.CorrelationId);
                }
            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = ErrorLog.GenerateErrorRefMessageAndLog(ex);
            }

            return response;
        }
    }
}
