using System.Runtime.Serialization;

namespace StyleCopNewRule.Tests.Entities
{
    [DataContract]
    public class EntityWithoutId
    {
        public string Name { get; set; }
    }
}
