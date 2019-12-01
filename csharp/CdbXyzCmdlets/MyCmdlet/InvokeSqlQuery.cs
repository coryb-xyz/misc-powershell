using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace MyCmdlet
{
    [Cmdlet(VerbsLifecycle.Invoke, "SqlQuery", DefaultParameterSetName = IntegratedAuth)]
    public class InvokeSqlQuery : PSCmdlet
    {
        [Parameter(Position = 1, ParameterSetName = IntegratedAuth)]
        [Parameter(Position = 1, ParameterSetName = SqlAuth)]
        public string Server { get; set; }

        [Parameter(Position = 2, ParameterSetName = IntegratedAuth)]
        [Parameter(Position = 2, ParameterSetName = SqlAuth)]
        public string Database { get; set; }

        [Parameter(Position = 3, Mandatory = true, ParameterSetName = IntegratedAuth)]
        [Parameter(Position = 3, Mandatory = true, ParameterSetName = SqlAuth)]
        public string Query { get; set; }

        [Parameter(Position = 4, Mandatory = true, ParameterSetName = SqlAuth)]
        public string Username { get; set; }

        [Parameter(Position = 5, Mandatory = true, ParameterSetName = SqlAuth)]
        public string Password { get; set; }

        private SqlConnection _connection;
        private const string IntegratedAuth = "IntegratedAuth";
        private const string SqlAuth = "SqlAuth";

        protected override void BeginProcessing()
        {
            ValidateParameters();
            WriteVerbose(this.ParameterSetName);

            var connectionString = this.ParameterSetName == IntegratedAuth
                ? $@"Data Source={Server};Initial Catalog={Database};
                      Integrated Security=SSPI;Persist Security Info=true"
                : $@"Data Source={Server};Initial CataLog={Database};User ID={Username};Password={Password}";

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
            const string serverVariable = "InvokeSqlQueryServer";
            const string databaseVariable = "InvokeSqlQueryDatabase";

            if (!string.IsNullOrEmpty(Server))
            {
                SessionState.PSVariable.Set(serverVariable, Server);
            }
            else
            {
                Server = SessionState.PSVariable.GetValue(serverVariable, string.Empty).ToString();
                if (string.IsNullOrEmpty(Server))
                {
                    ThrowParameterError("Server");
                }
            }

            if (!string.IsNullOrEmpty(Database))
            {
                SessionState.PSVariable.Set(databaseVariable, Database);
            }
            else
            {
                Database = SessionState.PSVariable.GetValue(databaseVariable, string.Empty).ToString();
                if (string.IsNullOrEmpty(Database))
                {
                    ThrowParameterError("Database");
                }
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