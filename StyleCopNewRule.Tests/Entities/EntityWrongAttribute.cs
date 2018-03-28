using System.Web.Mvc;

namespace StyleCopNewRule.Tests.Entities
{
    [Authorize]
    public class EntityWrongAttribute
    {
        public string Id { get; }

        public string Name { get; set; }
    }
}
