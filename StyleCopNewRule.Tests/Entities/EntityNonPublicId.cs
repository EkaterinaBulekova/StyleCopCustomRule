using System.Runtime.Serialization;

namespace StyleCopNewRule.Tests.Entities
{
    [DataContract]
    public class EntityNonPublicId
    {
        private string Id { get; }

        public string Name { get; set; }
    }
}
