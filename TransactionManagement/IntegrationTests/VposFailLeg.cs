using TransactionManagement.Enums;
using TransactionManagement.Library;

namespace TransactionManagement.IntegrationTests
{
    public class VposFailLeg : TransactionLeg
    {
        public VposFailLeg(string name)
            : base(name)
        {
        }

        public override ExecutionResult Execute()
        {
            return ExecutionResult.FAILED;
        }

        public override ExecutionResult Rollback()
        {
            return ExecutionResult.SUCCEEDED;
        }
    }
}