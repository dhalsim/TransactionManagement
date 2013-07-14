using NHibernate;
using TransactionManagement.Enums;
using TransactionManagement.Helpers;

namespace TransactionManagement.Library
{
    public class NhibernateTransactionOpenLeg : BasicTransactionLeg
    {
        public ITransaction Transaction { get; set; }
        public ISession Session { get; set; }

        public NhibernateTransactionOpenLeg(string legName) : base(legName)
        {
            
        }

        public override ExecutionResult Execute()
        {
            Session = NhibernateHelper.OpenSession();
            Transaction = Session.BeginTransaction();

            return ExecutionResult.SUCCEEDED;
        }

        public override ExecutionResult Rollback()
        {
            Transaction.Rollback();
            Session.Close();
            return ExecutionResult.SUCCEEDED;
        }
    }
}