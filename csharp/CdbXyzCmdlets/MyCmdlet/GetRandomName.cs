using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Threading;

namespace MyCmdlet
{
    [Cmdlet(VerbsCommon.Get, "RandomName")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class GetRandomName : Cmdlet
    {
        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; }

        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            WriteVerbose(Name);
            var nameChars = Name.ToCharArray();
            Array.Reverse(nameChars);
            WriteObject(new
            {
                ReversedName= new string(nameChars),
                NameLength = Name.Length

            });
        }

        protected override void EndProcessing()
        {
        }

        protected override void StopProcessing()
        {
        }
    }
}