using System.Runtime.Serialization;

namespace StyleCopNewRule.Tests.Entities
{
    [DataContract]
    internal class NonPublicEntity
    {
        public string Id { get; }

        public string Name { get; set; }
    }
}
