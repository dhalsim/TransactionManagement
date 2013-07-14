using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransactionManagement.Enums;
using TransactionManagement.Library;
using Assert = NUnit.Framework.Assert;

namespace TransactionManagement.UnitTests
{
    [TestClass]
    public class TransactionTestMethods
    {
        [TestMethod]
        public void ShouldAddTransactionLeg()
        {
            var transaction = new Transaction("TestMethod5");
            var transactionLeg = new BasicTransactionLeg("leg5_1", leg => { }, leg => { });

            transaction.Add(transactionLeg);
        }

        [TestMethod]
        public void ShouldCreateInstance()
        {
            Assert.IsNotNull(new Transaction("TestMethod1"));
        }

        [TestMethod]
        public void ShouldExecute()
        {
            var transaction = new Transaction("TestMethod2");
            transaction.Add(new BasicTransactionLeg("leg2_1", leg => { }, leg => { }));
            Assert.AreEqual(ExecutionResult.SUCCEEDED, transaction.Execute());
        }

        [TestMethod]
        public void ShouldRollback()
        {
            var transaction = new Transaction("TestMethod4");
            var basicTransactionLeg = new BasicTransactionLeg("leg4_1", leg => { }, leg => { });
            transaction.Add(basicTransactionLeg);
            basicTransactionLeg.Result = ExecutionResult.SUCCEEDED;
            Assert.AreEqual(ExecutionResult.SUCCEEDED, transaction.Rollback());
        }

        [TestMethod]
        public void ShouldThrowExceptionIfNoLegs()
        {
            var transaction = new Transaction("TestMethod3");
            Assert.Throws<Exception>(() => transaction.Execute());
        }
    }
}