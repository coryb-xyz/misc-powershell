using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace MyCmdlet
{
    [Cmdlet(VerbsLifecycle.Invoke, "SqlQuery")]
    public class InvokeSqlQuery : PSCmdlet
    {
        [Parameter(Position = 1)]
        public string Server { get; set; }

        [Parameter(Position = 2)]
        public string Database { get; set; }

        [Parameter(Position = 3, Mandatory = true)]
        public string Query { get; set; }

        [Parameter(Position = 4)]
        public string Username { get; set; }

        [Parameter(Position = 5)]
        public string Password { get; set; }
        
        private SqlConnection _connection;
        private const string IntegratedAuth = "IntegratedAuth";
        private const string SqlAuth = "SqlAuth";

        protected override void BeginProcessing()
        {
            ValidateParameters();
            WriteVerbose(this.ParameterSetName);

            string connectionString;
            connectionString = this.ParameterSetName == IntegratedAuth ? $@"Data Source={Server};Initial Catalog={Database};
                      Integrated Security=SSPI;Persist Security Info=true" : $@"Data Source={Server};Initial CataLog={Database};User ID={Username};Password={Password}";

            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        protected override void EndProcessing()
        {
            _connection?.Dispose();
        }

        protected override void StopProcessing()
        {
            _connection?.Dispose();
        }

        protected override void ProcessRecord()
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = Query;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var record = new PSObject();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            record.Properties.Add(new PSVariableProperty(new PSVariable(reader.GetName(i), reader[i])));
                        }
                        WriteObject(record);
                    }
                }
            }
        }

        private void ValidateParameters()
        {
            if (string.IsNullOrEmpty(Server))
            {
                ThrowParameterError("Server");
            }

            if (string.IsNullOrEmpty(Database))
            {
                ThrowParameterError("Database");
            }
        }

        private void ThrowParameterError(string parameterName)
        {
            ThrowTerminatingError(
                new ErrorRecord(
                    new ArgumentNullException($"Must specify '{parameterName}'"),
                    Guid.NewGuid().ToString(),
                    ErrorCategory.InvalidArgument,
                    null
                    )
                );
        }
    }
}