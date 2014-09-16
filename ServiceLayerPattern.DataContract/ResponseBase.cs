using System.Runtime.Serialization;

namespace ServiceLayerPattern.DataContract
{
    [DataContract]
    public abstract class ResponseBase
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}
