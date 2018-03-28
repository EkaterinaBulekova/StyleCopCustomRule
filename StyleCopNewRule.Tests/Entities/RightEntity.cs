using System.Runtime.Serialization;

namespace StyleCopNewRule.Tests.Entities
{
    [DataContract]
    public class RightEntity
    {
        public string Id { get; }

        public string Name { get; set; }
    }
}
