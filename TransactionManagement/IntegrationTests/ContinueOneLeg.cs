using System;
using TransactionManagement.Enums;
using TransactionManagement.Library;

namespace TransactionManagement.IntegrationTests
{
    public class ContinueOneLeg : ContinuableBasicTransactionLeg
    {
        private Action<ContinueOneLeg> ExecutionAction { get; set; }

        public ContinueOneLeg(string name, Action<ContinueOneLeg> execution)
            : base(name)
        {
            ExecutionAction = execution;
        }

        public int Toplam { get; set; }

        public override ExecutionResult Execute()
        {
            ExecutionAction(this);
            return ExecutionResult.SUCCEEDED;
        }
    }
}