using System;
using TransactionManagement.Enums;

namespace TransactionManagement.Library
{
    public class BasicTransactionLeg : TransactionLeg
    {
        private Action<TransactionLeg> ExecutionAction { get; set; }
        private Action<TransactionLeg> RollbackAction { get; set; }

        protected BasicTransactionLeg(string name) : base(name)
        {
            
        }
        
        public BasicTransactionLeg(string name, Action<TransactionLeg> execution, Action<TransactionLeg> rollback)
            : base(name)
        {
            ExecutionAction = execution;
            RollbackAction = rollback;
        }

        public override ExecutionResult Execute()
        {
            ExecutionAction(this);
            return ExecutionResult.SUCCEEDED;
        }

        public override ExecutionResult Rollback()
        {
            RollbackAction(this);
            return ExecutionResult.SUCCEEDED;
        }
    }
}