using ServiceLayerPattern.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ServiceLayerPattern.Contracts
{
    [ServiceContract(Namespace = "ServiceLayerPattern/")]
    public interface ITicketService
    {
        [OperationContract()]
        ReservateTicketResponse ReservateTicket(ReservateTicketRequest request);

        [OperationContract()]
        PurchaseTicketResponse PurchaseTicket(PurchaseTicketRequest request);
    }
}
