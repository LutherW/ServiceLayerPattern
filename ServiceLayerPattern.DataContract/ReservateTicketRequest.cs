using System.Runtime.Serialization;

namespace ServiceLayerPattern.DataContract
{
    [DataContract]
    public class ReservateTicketRequest
    {
        [DataMember]
        public string EventId { get; set; }

        [DataMember]
        public int TicketQuantity { get; set; }
    }
}
