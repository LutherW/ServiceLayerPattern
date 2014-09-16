using System.Runtime.Serialization;

namespace ServiceLayerPattern.DataContract
{
    [DataContract]
    public class PurchaseTicketResponse : ResponseBase
    {
        [DataMember]
        public string TicketId { get; set; }

        [DataMember]
        public string EventId { get; set; }

        [DataMember]
        public string EventName { get; set; }

        [DataMember]
        public int CountOfTickets { get; set; }
    }
}
