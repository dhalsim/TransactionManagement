using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using TransactionManagement.Enums;

namespace TransactionManagement.Library
{
    public class NhibernateTransactionCloseLeg : BasicTransactionLeg
    {
        private readonly NhibernateTransactionOpenLeg _openLeg;

        public NhibernateTransactionCloseLeg(string legName, NhibernateTransactionOpenLeg openLeg)
            : base(legName)
        {
            _openLeg = openLeg;
        }

        public override ExecutionResult Execute()
        {
            _openLeg.Transaction.Commit();
            _openLeg.Session.Close();

            return ExecutionResult.SUCCEEDED;
        }

        public override ExecutionResult Rollback()
        {
            return ExecutionResult.SUCCEEDED;
        }
    }
}