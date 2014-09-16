using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ServiceLayerPattern.Contracts;

namespace ServiceLayerPattern.ServiceProxy
{
    public class TicketServiceClientProxy : ClientBase<ITicketService>, ITicketService
    {

        public DataContract.ReservateTicketResponse ReservateTicket(DataContract.ReservateTicketRequest request)
        {
            return base.Channel.ReservateTicket(request);
        }

        public DataContract.PurchaseTicketResponse PurchaseTicket(DataContract.PurchaseTicketRequest request)
        {
            return base.Channel.PurchaseTicket(request);
        }
    }
}
