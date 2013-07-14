using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransactionManagement.Library;
using Assert = NUnit.Framework.Assert;

namespace TransactionManagement.UnitTests
{
    [TestClass]
    public class TransactionManagerTestMethods
    {
        [TestMethod]
        public void ShouldCreateInstance()
        {
            Assert.IsNotNull(new TransactionManager(new Transaction("TestMethod1")));
        }
    }
}