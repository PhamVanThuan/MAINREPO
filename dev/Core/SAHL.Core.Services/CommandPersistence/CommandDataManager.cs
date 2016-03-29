using SAHL.Core.Data;
using SAHL.Core.Data.Models.Cuttlefish;
using SAHL.Core.Data.Models.Cuttlefish.SqlStatements.CommandPersistance;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Services.CommandPersistence
{
    public class CommandDataManager : ICommandDataManager
    {
        private readonly IDbFactory dbFactory;

        public CommandDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public int SaveCommand(ICommand command, string serviceName, string contextValues)
        {
            //persists to command store
            CommandDataModel commandDataModel = new CommandDataModel(command.CommandJson, command.CommandDate, Environment.MachineName, serviceName, false, false, contextValues, false, false);
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(commandDataModel);
                db.Complete();
            }
            return commandDataModel.CommandKey;
        }

        public void SetCommandFailed(int commandKey)
        {
            var sql = new SetPersistedCommandFailStatement(commandKey);
            HandleUpdateStatement(sql);
        }

        public void SetCommandCompleted(int commandKey)
        {
            var sql = new SetPersistedCommandCompleteStatement(commandKey);
            HandleUpdateStatement(sql);
        }

        public ICommand GetCommandByCommandKey(int commandKey)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                CommandDataModel commandData = db.SelectOneWhere<CommandDataModel>(" CommandKey = @CommandKey", new { CommandKey = @commandKey });
                db.Complete();
                return CreateCommand(commandData.Data, commandData.CommandInsertDate);
            }
        }

        public ICommand CreateCommand(string commandJson, DateTime? commandInsertDate = null)
        {
            return new Command() { CommandDate = commandInsertDate ?? DateTime.Now, CommandJson = commandJson };
        }

        public IEnumerable<int> GetAllPendingCommands(string serviceName)
        {
            GetCommandsThatArePendingStatement query = new GetCommandsThatArePendingStatement(Environment.MachineName, serviceName);
            List<CommandKeyModel> CommandKeys;

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                CommandKeys = db.Select<CommandKeyModel>(query).ToList();
                db.Complete();
            }

            return CommandKeys.Select(x => x.CommandKey);
        }

        public IEnumerable<int> GetAllFailedCommands(string serviceName)
        {
            GetCommandsThatHaveFailedStatement query = new GetCommandsThatHaveFailedStatement(Environment.MachineName, serviceName);
            List<CommandKeyModel> CommandKeys;

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                CommandKeys = db.Select<CommandKeyModel>(query).ToList();
                db.Complete();
            }

            return CommandKeys.Select(model => model.CommandKey);
        }

        public IContextDetails GetHostContextForCommandKey(int commandKey, Func<string, IContextDetails> deserialiseFunc)
        {
            var query = new GetCommandHostContextDetailsForCommandKeyStatement(commandKey);
            string commandContextValuesSerialised;
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                commandContextValuesSerialised = db.SelectOne(query).ContextValues ?? "";
                db.Complete();
            }

            return string.IsNullOrEmpty(commandContextValuesSerialised) ? BuildDefaultContext() : deserialiseFunc(commandContextValuesSerialised);
        }

        public void MarkCommandUnAuthenticated(int commandKey)
        {
            var statement = new SetPersistedCommandUnAuthenticatedStatement(commandKey);
            HandleUpdateStatement(statement);
        }

        public void MarkCommandUnAuthorized(int commandKey)
        {
            var statement = new SetPersistedCommandUnAuthorizedStatement(commandKey);
            HandleUpdateStatement(statement);
        }

        public void SetCommandCompleteAndAuthorised(int commandKey)
        {
            var statement = new SetPersistedCommandCompleteAndAuthorisedStatement(commandKey);
            HandleUpdateStatement(statement);
        }

        private ContextDetails BuildDefaultContext()
        {
            return new ContextDetails();
        }

        private void HandleUpdateStatement(ISqlStatement<CommandDataModel> sql)
        {
            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Update(sql);
                db.Complete();
            }
        }
    }
}