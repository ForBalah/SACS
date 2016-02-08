using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Implementation;
using SACS.Implementation.Execution;

namespace SACS.IntegrationTests.ServiceAppDomain
{
    public class TestService : ServiceAppBase
    {
        protected override void Initialize()
        {
            // do nothing
        }

        public override void Execute(ref ServiceAppContext context)
        {
            // do nothing
        }
    }
}