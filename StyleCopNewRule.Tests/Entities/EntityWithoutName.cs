using System.Runtime.Serialization;

namespace StyleCopNewRule.Tests.Entities
{
    [DataContract]
    public class EntityWithoutName
    {
        public string Id { get; set; }
    }
}
