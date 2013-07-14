using System;
using System.Collections.Generic;
using TransactionManagement.Enums;
using TransactionManagement.Library;

namespace TransactionManagement.IntegrationTests
{
    public class RollbackLeg : TransactionLeg
    {
        public RollbackLeg(string name) : base(name)
        {
        }

        public int Toplam { get; set; }

        public override ExecutionResult Execute()
        {
            Toplam += 4;
            throw new Exception("Intentionally threw exception");
        }

        public override ExecutionResult Rollback()
        {
            Toplam -= 4;
            return ExecutionResult.SUCCEEDED;
        }
    }
}