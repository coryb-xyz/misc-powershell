using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;
using MyCmdlet;
using NUnit.Framework;

namespace MyCmdletTests
{
    [TestFixture]
    public class GetRandomNamePipelineTest
    {
        [Test]
        public void TestThreeNames()
        {
            var initialState = InitialSessionState.CreateDefault();

            initialState.Commands.Add(
                new SessionStateCmdletEntry(
                    "Get-RandomName",
                    typeof(GetRandomName),
                    null
                )
            );

            using (var runspace = RunspaceFactory.CreateRunspace(initialState))
            {
                runspace.Open();
                using (var powershell = PowerShell.Create())
                {
                    powershell.Runspace = runspace;


                    powershell.Commands.AddCommand("Get-RandomName");

                    var results = powershell.Invoke<string>(new[] {"Cory", "Danny", "Lou"});

                    Assert.AreEqual(results.Count, 3);
                    Assert.AreEqual(results[0].Length, 4);
                    Assert.AreEqual(results[1].Length, 5);
                    Assert.AreEqual(results[2].Length, 3);
                }
            }
        }
    }
}