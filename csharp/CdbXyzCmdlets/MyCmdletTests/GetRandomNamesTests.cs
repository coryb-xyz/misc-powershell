using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCmdlet;
using NUnit.Framework;

namespace MyCmdletTests
{
    [TestFixture]
    public class GetRandomNamesTests
    {
        [Test]
        public void TestReplacingCory()
        {
            var cmdlet = new GetRandomName
            {
                Name = "Cory"
            };

            var results = cmdlet.Invoke<string>().ToList();

            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual(results[0].Length, 4);
        }

    }
}
