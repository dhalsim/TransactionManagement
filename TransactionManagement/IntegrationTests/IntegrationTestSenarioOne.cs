using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransactionManagement.Entities;
using TransactionManagement.Enums;
using TransactionManagement.Library;
using Assert = NUnit.Framework.Assert;

namespace TransactionManagement.IntegrationTests
{
    [TestClass]
    public class IntegrationTestSenarioOne
    {
        [TestMethod]
        public void TestMethodContinueLeg()
        {
            var transaction = new Transaction("TestMethod");

            var continuable = new ContinueOneLeg("ContinueLeg", leg =>
                                                                 {
                                                                     leg.Toplam += 5;
                                                                     throw new Exception("This exception threw for a reason");
                                                                 });
            var sumLeg = new SumLeg("SumLeg");
            transaction.Add(continuable);
            transaction.Add(sumLeg);

            Assert.AreEqual(ExecutionResult.CONTINUED, transaction.Execute());
            Assert.AreEqual(ExecutionResult.CONTINUED, transaction.Result);
            Assert.AreEqual(ExecutionResult.NOTRUN, transaction.RollbackResult);

            Assert.AreEqual(ExecutionResult.CONTINUED, continuable.Result);
            Assert.AreEqual(ExecutionResult.NOTRUN, continuable.RollbackResult);
            Assert.AreEqual(5, continuable.Toplam);

            Assert.AreEqual(ExecutionResult.SUCCEEDED, sumLeg.Result);
            Assert.AreEqual(ExecutionResult.NOTRUN, sumLeg.RollbackResult);
            Assert.AreEqual(4, sumLeg.Toplam);
        }

        [TestMethod]
        public void TestMethodRollbackContinuableLeg()
        {
            var transaction = new Transaction("TestMethod3");

            var sumLeg = new SumLeg("SumLeg");
            var continuable = new ContinueOneLeg("ContinueLeg", leg =>
                                                                 {
                                                                     leg.Toplam += 5;
                                                                     throw new Exception(
                                                                         "This exception threw for a reason");
                                                                 });
            var rollbackLeg = new RollbackLeg("Leg3");
            transaction.Add(sumLeg);
            transaction.Add(continuable);
            transaction.Add(rollbackLeg);

            Assert.AreEqual(ExecutionResult.BROKEN, transaction.Execute());
            Assert.AreEqual(ExecutionResult.BROKEN, transaction.Result);
            Assert.AreEqual(ExecutionResult.CONTINUED, transaction.RollbackResult);

            Assert.AreEqual(ExecutionResult.SUCCEEDED, sumLeg.Result);
            Assert.AreEqual(ExecutionResult.SUCCEEDED, sumLeg.RollbackResult);
            Assert.AreEqual(0, sumLeg.Toplam);

            Assert.AreEqual(ExecutionResult.CONTINUED, continuable.Result);
            Assert.AreEqual(ExecutionResult.NOTRUN, continuable.RollbackResult);
            Assert.AreEqual(5, continuable.Toplam);

            Assert.AreEqual(ExecutionResult.BROKEN, rollbackLeg.Result);
            Assert.AreEqual(ExecutionResult.SUCCEEDED, rollbackLeg.RollbackResult);
            Assert.AreEqual(0, rollbackLeg.Toplam);

            Assert.DoesNotThrow(() => transaction.ToJson());
        }

        [TestMethod]
        public void TestMethodRollbackLeg()
        {
            var transaction = new Transaction("TestMethod7");

            var rollbackLeg = new RollbackLeg("Leg1");
            transaction.Add(rollbackLeg);

            Assert.AreEqual(ExecutionResult.BROKEN, transaction.Execute());
            Assert.AreEqual(ExecutionResult.BROKEN, transaction.Result);
            Assert.AreEqual(ExecutionResult.SUCCEEDED, transaction.RollbackResult);

            Assert.AreEqual(ExecutionResult.BROKEN, rollbackLeg.Result);
            Assert.AreEqual(ExecutionResult.SUCCEEDED, rollbackLeg.RollbackResult);
            Assert.AreEqual(0, rollbackLeg.Toplam);
        }

        [TestMethod]
        public void TestMethodRollbackLegs()
        {
            var transaction = new Transaction("TestMethod2");

            var sumLeg = new SumLeg("SumLeg");
            var rollbackLeg = new RollbackLeg("Leg2");
            transaction.Add(sumLeg);
            transaction.Add(rollbackLeg);

            Assert.AreEqual(ExecutionResult.BROKEN, transaction.Execute());
            Assert.AreEqual(ExecutionResult.BROKEN, transaction.Result);
            Assert.AreEqual(ExecutionResult.SUCCEEDED, transaction.RollbackResult);

            Assert.AreEqual(ExecutionResult.SUCCEEDED, sumLeg.Result);
            Assert.AreEqual(ExecutionResult.SUCCEEDED, sumLeg.RollbackResult);
            Assert.AreEqual(0, sumLeg.Toplam);

            Assert.AreEqual(ExecutionResult.BROKEN, rollbackLeg.Result);
            Assert.AreEqual(ExecutionResult.SUCCEEDED, rollbackLeg.RollbackResult);
            Assert.AreEqual(0, rollbackLeg.Toplam);
        }

        [TestMethod]
        public void TestMethodSumLeg()
        {
            var transaction = new Transaction("TestMethod6");

            var vposLegTransaction = new SumLeg("Leg6");
            transaction.Add(vposLegTransaction);

            Assert.AreEqual(ExecutionResult.SUCCEEDED, transaction.Execute());
            Assert.AreEqual(ExecutionResult.SUCCEEDED, transaction.Result);
            Assert.AreEqual(ExecutionResult.NOTRUN, transaction.RollbackResult);

            Assert.AreEqual(4, vposLegTransaction.Toplam);
        }

        [TestMethod]
        public void TestMethodRollbackFailed()
        {
            var transaction = new Transaction("RollbackFailed Transaction");

            var leg1 = new SumLeg("SumLeg 1");
            var leg2 = new RollbackFailedLeg("Rollback Failed Leg");
            var leg3 = new SumLeg("SumLeg 2");
            transaction.Add(leg1);
            transaction.Add(leg2);
            transaction.Add(leg3);

            Assert.AreEqual(ExecutionResult.BROKEN, transaction.Execute());
            Assert.AreEqual(ExecutionResult.BROKEN, transaction.Result);
            Assert.AreEqual(ExecutionResult.BROKEN, transaction.RollbackResult);

            Assert.AreEqual(ExecutionResult.NOTRUN, leg3.Result);
            Assert.AreEqual(ExecutionResult.NOTRUN, leg3.RollbackResult);
            Assert.AreEqual(0, leg3.Toplam);

            Assert.AreEqual(ExecutionResult.BROKEN, leg2.Result);
            Assert.AreEqual(ExecutionResult.BROKEN, leg2.RollbackResult);
            Assert.AreEqual(1, leg2.Toplam);

            Assert.AreEqual(ExecutionResult.SUCCEEDED, leg1.Result);
            Assert.AreEqual(ExecutionResult.SUCCEEDED, leg1.RollbackResult);
            Assert.AreEqual(0, leg1.Toplam);

            var exceptions = transaction.GetExceptions();
            Assert.AreEqual(2, exceptions.Count);

            Assert.AreEqual(exceptions[0].ExceptionType, ExceptionType.ON_EXECUTE);
            Assert.AreEqual(exceptions[1].ExceptionType, ExceptionType.ON_ROLLBACK);

            Assert.AreEqual(exceptions[0].LegName, "Rollback Failed Leg");
            Assert.AreEqual(exceptions[1].LegName, "Rollback Failed Leg");
        }

        [TestMethod]
        public void TestExecuteFailOnPurpose()
        {
            var transaction = new Transaction("Rollback Success");

            var leg1 = new VposFailLeg("VposLeg");
            var leg2 = new SumLeg("SumLeg 2");
            transaction.Add(leg1);
            transaction.Add(leg2);

            Assert.AreEqual(ExecutionResult.FAILED, transaction.Execute());
            Assert.AreEqual(ExecutionResult.FAILED, transaction.Result);
            Assert.AreEqual(ExecutionResult.SUCCEEDED, transaction.RollbackResult);

            Assert.AreEqual(ExecutionResult.FAILED, leg1.Result);
            Assert.AreEqual(ExecutionResult.SUCCEEDED, leg1.RollbackResult);

            Assert.AreEqual(ExecutionResult.NOTRUN, leg2.Result);
            Assert.AreEqual(ExecutionResult.NOTRUN, leg2.RollbackResult);
            Assert.AreEqual(0, leg2.Toplam);
        }

        [TestMethod]
        public void TestNhibernateTransactionSuccess()
        {
            var transaction = new Transaction("Nhibernate transaction success");

            var leg1 = new NhibernateTransactionOpenLeg("transaction_open");
            var leg2 = new BasicTransactionLeg("insert_zone", leg =>
                                                                  {
                                                                      var zone = new Zone {CityId = 123};
                                                                      leg1.Session.Save(zone);
                                                                  }, null);
            var leg3 = new NhibernateTransactionCloseLeg("transaction_close", leg1);

            transaction.Add(leg1);
            transaction.Add(leg2);
            transaction.Add(leg3);

            Assert.AreEqual(ExecutionResult.SUCCEEDED, transaction.Execute());
            Assert.IsFalse(leg1.Transaction.WasRolledBack);
            Assert.IsFalse(leg1.Session.IsOpen);

            var transactionDelete = new Transaction("Nhibernate delete inserted zone");

            var delLeg1 = new NhibernateTransactionOpenLeg("transaction_open");
            var delLeg2 = new BasicTransactionLeg("delete_zone", leg =>
                                    {
                                        Zone delZone = delLeg1.Session.QueryOver<Zone>()
                                                        .Where(z => z.CityId == 123)
                                                        .SingleOrDefault();
                                        delLeg1.Session.Delete(delZone);
                                    }, null);
            var delLeg3 = new NhibernateTransactionCloseLeg("transaction_close", delLeg1);

            transactionDelete.Add(delLeg1);
            transactionDelete.Add(delLeg2);
            transactionDelete.Add(delLeg3);

            Assert.AreEqual(ExecutionResult.SUCCEEDED, transactionDelete.Execute());
            Assert.IsFalse(delLeg1.Transaction.WasRolledBack);
            Assert.IsFalse(delLeg1.Session.IsOpen);
        }

        [TestMethod]
        public void TestNhibernateTransactionFailRollback()
        {
            var transaction = new Transaction("Nhibernate transaction rollback");

            var leg1 = new NhibernateTransactionOpenLeg("transaction_open");
            var leg2 = new BasicTransactionLeg("insert_zone", leg =>
                                                                  {
                                                                      var zone = new Zone {CityId = 222};
                                                                      leg1.Session.Save(zone);
                                                                      throw new Exception("Intensionally threw exception");
                                                                  }, null);
            var leg3 = new NhibernateTransactionCloseLeg("transaction_close", leg1);

            transaction.Add(leg1);
            transaction.Add(leg2);
            transaction.Add(leg3);

            Assert.AreEqual(ExecutionResult.BROKEN, transaction.Execute());
            Assert.IsTrue(leg1.Transaction.WasRolledBack);
            Assert.IsFalse(leg1.Session.IsOpen);
        }
    }
}