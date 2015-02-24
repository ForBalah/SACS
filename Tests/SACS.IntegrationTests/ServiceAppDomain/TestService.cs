using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Implementation;

namespace SACS.IntegrationTests.ServiceAppDomain
{
    public class TestService : ServiceAppBase
    {
        public override void Initialize()
        {
            string appSetting = ConfigurationManager.AppSettings["Test"];

            // exception will happen if this fails.
            appSetting += ": Success!";
        }

        public override void Execute()
        {
            // do nothing
        }

        public override void CleanUp()
        {
        }
    }
}
