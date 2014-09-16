using System.Runtime.Serialization;

namespace ServiceLayerPattern.DataContract
{
    [DataContract]
    public class PurchaseTicketRequest
    {
        [DataMember]
        public string CorrelationId { get; set; }

        [DataMember]
        public string EventId { get; set; }

        [DataMember]
        public string ReservationId { get; set; }
    }
}
