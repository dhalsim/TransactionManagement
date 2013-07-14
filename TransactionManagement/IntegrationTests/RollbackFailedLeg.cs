using System;
using TransactionManagement.Enums;
using TransactionManagement.Library;

namespace TransactionManagement.IntegrationTests
{
    public class RollbackFailedLeg : TransactionLeg
    {
        public int Toplam { get; set; }

        public RollbackFailedLeg(string name) : base(name)
        {
        }

        public override ExecutionResult Execute()
        {
            Toplam += 1;
            throw new Exception("Intensionally threw exception on execute()");
        }

        public override ExecutionResult Rollback()
        {
            throw new Exception("Intensionally threw exception on rollback");
        }
    }
}