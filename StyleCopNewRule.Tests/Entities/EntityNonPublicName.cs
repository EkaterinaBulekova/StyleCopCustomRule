using System.Runtime.Serialization;

namespace StyleCopNewRule.Tests.Entities
{
    [DataContract]
    public class EntityNonPublicName
    {
        public string Id { get; }

        private string Name { get; set; }
    }
}
