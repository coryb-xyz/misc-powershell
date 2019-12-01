using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace MyCmdlet
{
    [Cmdlet(VerbsCommon.Get, "RandomName")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class GetRandomName : Cmdlet
    {
        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            Console.WriteLine($"{Name}, len: {Name.Length}");
        }
    }
}