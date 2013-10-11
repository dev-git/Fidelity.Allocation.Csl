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

using Fidelity.Allocation.App;
using Fidelity.Allocation.DomainLayer;

using Fidelity.Framework.DomainLayer;
using Fidelity.Framework.PersistencyLayer;

namespace Fidelity.Allocation.Csl
{
    public class Program
    {
        private static UnityConfigurationSection section;
        private static UnityContainer container;

        public static void Main(string[] args)
        {
            section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            container = new UnityContainer();
            section.Configure(container);
            ISessionManager sessionManager = container.Resolve<ISessionManager>();
            ISession session = sessionManager.StartSession("allocation", new DefaultInterceptor("External.App"));
            //var transRepo = container.Resolve<Fidelity.Allocation.External.PersistencyLayer.IBankTransactionRepository>();

            //var bankAccountRepo = container.Resolve<Fidelity.Allocation.PersistencyLayer.IBankAccountRepository>();
            //var bankAccount = bankAccountRepo.FindById(new Guid("{8A4CA7CA-777E-4F41-9DBF-8466EC36CCD8}"));



            //var bankStatementRepo = container.Resolve<Fidelity.Allocation.PersistencyLayer.IBankStatementRepository>();

            //var accountEntryRepo = container.Resolve<Fidelity.Allocation.PersistencyLayer.IAccountEntryRepository>();

            //var bs = new BankStatement(bankAccount, BankStatementStatus.Balanced);
            //bs.AllocationDate = DateTime.Today;
            //bs.OpeningBalance = 52.26M;
            //bs.ClosingBalance = 123.45M;
            //bs.TransactionCount = 0;
            //bs.OpeningDate = DateTime.Today;
            //bs.ClosingDate = DateTime.Today;
            ////bs.SetBankTransactionId(bankTransHead.Id);
            //bs.Version = 1234;
            //bs.UpdateAuditStamp("system", "Testing");

            ////var ae = new AccountEntry();
            ////ae.Amount = 50.26M;
            ////ae.CreditAdviserNo = 1;

            ///*var testRepo = container.Resolve<Fidelity.Allocation.PersistencyLayer.ITestRepository>();
            //var ts = new Test();
            //ts.TestNote = "hello";
            //ts.TestDate = DateTime.Today;
            //ts.UpdateAuditStamp("system", "testing");*/

            //using (UnitOfWork uow = new UnitOfWork(bankStatementRepo))
            //{
            //    try
            //    {
            //        //session.SaveOrUpdate(ae);
            //        bankStatementRepo.Add(bs);
                    
            //        uow.Commit();
            //    }
            //    catch (DuplicateKeyException)
            //    {
            //        uow.Rollback();
            //    }
            //}

            //sessionManager.EndTransientSession(session);


            //GetPayBankRequestFileServices();    
            //using (var session = sessionManager.StartSession("allocation"))

            using (var tx = session.BeginTransaction())
            {
                IBankStatementServices bankStateSrv = container.Resolve<IBankStatementServices>();
                bankStateSrv.CheckCommonBanking();
                bankStateSrv.Import(new List<Guid>());
                // IT WILL FAIL HERE...
                tx.Commit();
            }
   
            // //session.Flush();
            ////DoCommonBankingExt();

            //Console.WriteLine("Press enter a key...");
            //Console.ReadLine();
            sessionManager.EndTransientSession(session);
        }

        private static void DoCommonBanking()
        {
            IBankStatementServices bankStateSrv = container.Resolve<IBankStatementServices>();
            bankStateSrv.CheckCommonBanking();
            bankStateSrv.Import(new List<Guid>());
           
        }

        private static void DoCommonBankingExt()
        {
            IBankTransactionServices bankTransSrv = container.Resolve<IBankTransactionServices>();
           // Console.WriteLine(((System.Collections.Generic.List<Fidelity.Allocation.External.DomainLayer.BankTransactionHeader>)
               // (((Fidelity.Allocation.External.App.BankTransactionServices)(bankTransSrv)).bthList)).Count);
            
            bankTransSrv.CheckCommonBanking();
            
            bankTransSrv.Import(new List<Guid>());
            //Console.WriteLine(bankTransSrv.
        }

        private static void GetPayBankRequestFileServices()
        {
            IPayBankPaymentRequestFileServices repoSrv = container.Resolve<IPayBankPaymentRequestFileServices>();

            TimeSpan ts = new TimeSpan(-32, 0, 0, 0);
            IList<PayBankPaymentRequestFile> payBank = repoSrv.GetByDirectDebitDateRange(ts);

            Console.WriteLine(payBank.Count);
        }
    }
}
