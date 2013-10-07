using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

using Fidelity.Allocation.External.App;
using Fidelity.Allocation.External.DomainLayer;

using Fidelity.Framework.DomainLayer;
using Fidelity.Framework.PersistencyLayer;

namespace Fidelity.Allocation.Csl
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            var container = new UnityContainer();
            section.Configure(container);
            ISessionManager sessionManager = container.Resolve<ISessionManager>();

            ISession session = sessionManager.StartSession("allocationext", new DefaultInterceptor("Allocation.Allocation.Csl"));

            //IPayBankPaymentRequestFileRepository repo = container.Resolve<IPayBankPaymentRequestFileRepository>();
            IPayBankPaymentRequestFileServices repoSrv = container.Resolve<IPayBankPaymentRequestFileServices>();

            TimeSpan ts = new TimeSpan(-32, 0, 0, 0);
            IList<PayBankPaymentRequestFile> payBank = repoSrv.GetByDirectDebitDateRange(ts);

            Console.WriteLine(payBank.Count);
            Console.ReadLine();
            sessionManager.EndTransientSession(session);
        }
    }
}
