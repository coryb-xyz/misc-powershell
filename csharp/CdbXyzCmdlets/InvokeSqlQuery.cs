using System;
using System.Data.SqlClient;
using System.Management.Automation;

namespace MyCmdlets
{
    [Cmdlet(VerbsLifecycle.Invoke, "SqlQuery", DefaultParameterSetName=IntegratedAuth)]
    public class InvokeSqlQuery : PSCmdlet
    {
        private const string IntegratedAuth = "IntegratedAuth";
        private const string SqlAuth = "SqlAuth";

        [Parameter(Position = 1, ParameterSetName = IntegratedAuth)]
        [Parameter(Position = 1, ParameterSetName = SqlAuth)]
        public string Server { get; set; }

        [Parameter(Position = 2, ParameterSetName = IntegratedAuth)]
        [Parameter(Position = 2, ParameterSetName = SqlAuth)]
        public string Database { get; set; }

        [Parameter(Position = 3, Mandatory = true, ParameterSetName = IntegratedAuth)]
        [Parameter(Position = 3, Mandatory = true, ParameterSetName = SqlAuth)]
        public string Query { get; set; }

        [Parameter(Position = 4, ParameterSetName = SqlAuth, Mandatory=true)]
        public string Username { get; set; }

        [Parameter(Position = 5, ParameterSetName = SqlAuth, Mandatory = true)]
        public string Password { get; set; }

        private SqlConnection _connection;

        protected override void BeginProcessing()
        {
            ValidateParameters();

            WriteVerbose(this.ParameterSetName);

            string connectionString;
            if (this.ParameterSetName == IntegratedAuth)
            {
                connectionString = String.Format(
                    @"Data Source={0};Initial Catalog={1};
                      Integrated Security=SSPI;Persist Security Info=true",
                    Server, Database);
            }
            else
            {
                connectionString = String.Format(
                    @"Data Source={0};Initial CataLog={1};User ID={2};Password={3}",
                    Server, Database, Username, Password);
            }


            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        protected override void EndProcessing()
        {
            if (_connection != null)
                _connection.Dispose();
        }

        protected override void StopProcessing()
        {
            if (_connection != null)
                _connection.Dispose();
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
                            record.Properties.Add(
                                new PSVariableProperty(
                                    new PSVariable(reader.GetName(i), reader[i])));
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

            if (!String.IsNullOrEmpty(Server))
            {
                SessionState.PSVariable.Set(serverVariable, Server);
            }
            else
            {
                Server = SessionState.PSVariable.GetValue(serverVariable, String.Empty).ToString();
                if (String.IsNullOrEmpty(Server))
                {
                    ThrowParameterError("Server");
                }
            }

            if (!String.IsNullOrEmpty(Database))
            {
                SessionState.PSVariable.Set(databaseVariable, Database);
            }
            else
            {
                Database = SessionState.PSVariable.GetValue(databaseVariable, String.Empty).ToString();

                if (String.IsNullOrEmpty(Database))
                {
                    ThrowParameterError("Database");
                }
            }
        }

        private void ThrowParameterError(string parameterName)
        {
            ThrowTerminatingError(
                new ErrorRecord(
                    new ArgumentException(String.Format(
                        "Must specify '{0}'", parameterName)),
                    Guid.NewGuid().ToString(),
                    ErrorCategory.InvalidArgument,
                    null));
        }

    }
}
