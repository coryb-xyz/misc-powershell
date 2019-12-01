using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Reflection;
using System.Threading;

namespace MyCmdlet
{
    [Cmdlet(VerbsCommon.Get, "RandomName")]
    public class GetRandomName : Cmdlet
    {
        private string[] _names;

        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; }

        protected override void BeginProcessing()
        {
            WriteVerbose("Loading files");
            _names = File.ReadAllLines(
                Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                    throw new InvalidOperationException(),
                    "GetRandomName.txt"
                )
            );
        }

        protected override void ProcessRecord()
        {
            WriteVerbose(Name);
            WriteObject(
                _names.Where(n => n.Length == Name.Length)
                    .OrderBy(n => Guid.NewGuid())
                    .FirstOrDefault()
            );
        }

        protected override void EndProcessing() { }

        protected override void StopProcessing() { }
    }
}