
using System.Runtime.Serialization;
using System.Security.AccessControl;

namespace StyleCopNewRule.Tests.Controllers
{
    using System.Web.Mvc;

    public class WrongAttributeMetodController : Controller
    {
        [AllowAnonymous]
        public void DoSomthing()
        {
        }
    }
}
