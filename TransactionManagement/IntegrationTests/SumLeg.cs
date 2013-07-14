using TransactionManagement.Enums;
using TransactionManagement.Library;

namespace TransactionManagement.IntegrationTests
{
    public class SumLeg : TransactionLeg
    {
        public SumLeg(string name) : base(name)
        {
        }

        public int Toplam { get; set; }

        public override ExecutionResult Execute()
        {
            Toplam += 4;
            return ExecutionResult.SUCCEEDED;
        }

        public override ExecutionResult Rollback()
        {
            Toplam -= 4;
            return ExecutionResult.SUCCEEDED;
        }
    }
}