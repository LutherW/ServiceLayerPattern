using System.Runtime.Serialization;
using System;

namespace ServiceLayerPattern.DataContract
{
    [DataContract]
    public class ReservateTicketResponse : ResponseBase
    {
        [DataMember]
        public string ReservationId { get; set; }

        [DataMember]
        public DateTime ExpirationDate { get; set; }

        [DataMember]
        public string EventId { get; set; }

        [DataMember]
        public string EventName { get; set; }

        [DataMember]
        public int CountOfTickets { get; set; }
    }
}
