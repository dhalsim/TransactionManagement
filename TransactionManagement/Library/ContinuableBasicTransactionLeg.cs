using System;
using System.Collections.Generic;
using TransactionManagement.Enums;
using TransactionManagement.Interfaces;

namespace TransactionManagement.Library
{
    public class ContinuableBasicTransactionLeg : BasicTransactionLeg, IContinuable
    {
        private Action<ContinuableBasicTransactionLeg> ExecutionAction { get; set; }

        protected ContinuableBasicTransactionLeg(string name) : base(name)
        {
            
        }

        protected ContinuableBasicTransactionLeg(string name, Action<ContinuableBasicTransactionLeg> execution)
            : base(name)
        {
            ExecutionAction = execution;
        }

        public override ExecutionResult Execute()
        {
            ExecutionAction(this);
            return ExecutionResult.SUCCEEDED;
        }

        public override ExecutionResult Rollback()
        {
            throw new Exception("Rollback not supported in this Transaction Leg");
        }
    }
}