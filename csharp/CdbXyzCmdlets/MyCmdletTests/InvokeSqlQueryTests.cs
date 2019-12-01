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
    public class InvokeSqlQueryTests
    {
        [Test]
        public void SelectStarTest()
        {
            var initialState = InitialSessionState.CreateDefault();

            initialState.Commands.Add(
                new SessionStateCmdletEntry(
                    "Invoke-SqlQuery",
                    typeof(InvokeSqlQuery),
                    null
                )
            );

            using (var runspace = RunspaceFactory.CreateRunspace(initialState))
            {
                runspace.Open();
                using (var powershell = PowerShell.Create())
                {
                    powershell.Runspace = runspace;

                    var sqlQueryCommand = new Command("Invoke-SqlQuery");
                    sqlQueryCommand.Parameters.Add("Server", "localhost");
                    sqlQueryCommand.Parameters.Add("Database", "NTest");
                    sqlQueryCommand.Parameters.Add("Query", "SELECT * FROM People");

                    powershell.Commands.AddCommand(sqlQueryCommand);

                    var results = powershell.Invoke();

                    Assert.AreEqual(results.Count, 4);


                }
            }
        }
    }
}