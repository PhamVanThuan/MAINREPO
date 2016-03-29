using SAHL.Core;
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Exceptions;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Factories;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.X2Engine2.Node
{
	public class X2WorkflowRequestHandler : IX2WorkflowRequestHandler
	{
		private ICommandFactory commandFactory;
		private IX2ResponseFactory responseFactory;
		private IIocContainer iocContainer;
		const string ErrorMessage = "The request could not be completed, there are Error Messages";
		public X2WorkflowRequestHandler(ICommandFactory commandFactory, IX2ResponseFactory responseFactory, IIocContainer iocContainer)
		{
			this.commandFactory = commandFactory;
			this.responseFactory = responseFactory;
			this.iocContainer = iocContainer;
		}


		X2Response HandleSystemRequestGroup(IX2Request request, IEnumerable<dynamic> commands, IX2ServiceCommandRouter commandHandler)
		{
			long instanceId = 0;
			SystemMessageCollection messages = new SystemMessageCollection();
            bool successPathFound = false;
            
			foreach (var command in commands)
			{
				HandleSystemRequestBaseCommand handleSystemRequestBaseCommand = command as HandleSystemRequestBaseCommand;
				if (command is ISplittable)
				{
					try
					{
						using (var context = new Db().InWorkflowContext())
						{
							try
							{
								messages.Aggregate(HandleCommand(commandHandler, command, request.ServiceRequestMetadata));
								context.Complete();
							}
							catch (MapReturnedFalseException ex)
							{
								messages.Aggregate(ex.Messages);
							}
						}
						if (!IsErrorResponse(messages, command))
						{
							if (command is IContinueWithCommands)
							{
								commandHandler.ProcessQueuedCommands(request.ServiceRequestMetadata);
							}
						}
						else
						{
							var unlockInstanceCommand = new UnlockInstanceCommand(command.InstanceId);
							messages.Aggregate(HandleCommand(commandHandler, unlockInstanceCommand, request.ServiceRequestMetadata));
						}
					}
					catch (Exception ex)
					{
						return responseFactory.CreateErrorResponse(request, string.Format("{0}-{1}", ex.Message, ex.StackTrace), null, messages);
					}
				}
				else
				{
					try
					{
						using (var context = new Db().InWorkflowContext())
						{
							try
							{
								messages.Aggregate(HandleCommand(commandHandler, command, request.ServiceRequestMetadata));
								context.Complete();
							}
							catch (MapReturnedFalseException ex)
							{
								messages.Aggregate(ex.Messages);
							}
						}
						if (handleSystemRequestBaseCommand.Result)
						{
                            successPathFound = true;
							if (command is IContinueWithCommands)
							{
								commandHandler.ProcessQueuedCommands(request.ServiceRequestMetadata);
							}
							break;
						}
						else
						{
							var unlockInstanceCommand = new UnlockInstanceCommand(command.InstanceId);
							messages.Aggregate(HandleCommand(commandHandler, unlockInstanceCommand, request.ServiceRequestMetadata));
						}
					}
					catch(Exception ex)
					{
                        return responseFactory.CreateErrorResponse(request, string.Format("{0}-{1}", ex.Message, ex.StackTrace), null, messages);
					}
				}
			}

            if (!successPathFound)
            {
                foreach (var commandToBeDeleted in commands)
                {
                    DeleteScheduledActivityCommand deleteScheduledActivitySplitCommand = new DeleteScheduledActivityCommand(commandToBeDeleted.InstanceId, commandToBeDeleted.Activity.ActivityID);
                    messages.Aggregate(commandHandler.HandleCommand<DeleteScheduledActivityCommand>(deleteScheduledActivitySplitCommand, request.ServiceRequestMetadata));
                }
            }

			return responseFactory.CreateSuccessResponse(request, instanceId, messages);
		}

		public X2Response Handle(IX2Request request)
		{
			long instanceId = 0;
			SystemMessageCollection messages = new SystemMessageCollection();
			try
			{
				IEnumerable<dynamic> commands = commandFactory.CreateCommands(request);
				IX2ServiceCommandRouter commandHandler = iocContainer.GetInstance<IX2ServiceCommandRouter>();
				if (request.RequestType == X2RequestType.SystemRequestGroup || request.RequestType == X2RequestType.Timer)
				{
					return HandleSystemRequestGroup(request, commands, commandHandler);
				}
				foreach (var command in commands)
				{
					try
					{
						using (var context = new Db().InWorkflowContext())
						{
							try
							{
								messages.Aggregate(HandleCommand(commandHandler, command, request.ServiceRequestMetadata));
								context.Complete();
							}
							catch (MapReturnedFalseException ex) 
							{
								messages.Aggregate(ex.Messages);
							}
						}
						if (!IsErrorResponse(messages, command))
						{
							if (command is IContinueWithCommands)
							{
								commandHandler.ProcessQueuedCommands(request.ServiceRequestMetadata);
							}
							if (request.RequestType == X2RequestType.UserCreate)
							{
								instanceId = ((UserRequestCreateInstanceCommand)command).NewlyCreatedInstanceId;
							}
                            else if (request.RequestType == X2RequestType.UserCreateWithComplete)
                            {
                                instanceId = ((UserRequestCreateInstanceWithCompleteCommand)command).NewlyCreatedInstanceId;
                            }
							else
							{
								instanceId = request.InstanceId;
							}
						}
						else
						{
							if (command is IContinueWithCommands)
							{
								commandHandler.ProcessQueuedCommands(request.ServiceRequestMetadata);
							}
							return responseFactory.CreateErrorResponse(request, ErrorMessage, instanceId, messages);
						}

					}
					catch (Exception ex)
					{
						return responseFactory.CreateErrorResponse(request, string.Format("{0}-{1}", ex.Message, ex.StackTrace), instanceId, messages);
					}
				}
				IX2RequestForExistingInstance requestForExistingInstance = request as IX2RequestForExistingInstance;
				if (requestForExistingInstance != null)
				{
					instanceId = requestForExistingInstance.InstanceId;
				}
				return responseFactory.CreateSuccessResponse(request, instanceId, messages);
			}
			catch
			{
				return responseFactory.CreateErrorResponse(request, "An error occurred while handling the request.", instanceId, messages);
			}
		}

		protected bool IsErrorResponse(ISystemMessageCollection messages, dynamic command)
		{
			bool ignoreWarnings = false;
			if (command is IIgnoreWarnings)
			{
				ignoreWarnings = ((IIgnoreWarnings)command).IgnoreWarnings;
			}
			if (messages.HasErrors || (messages.HasWarnings && !ignoreWarnings))
				return true;
			return false;
		}

		public ISystemMessageCollection HandleCommand(IX2ServiceCommandRouter commandHandler, dynamic command, IServiceRequestMetadata serviceRequestMetadata)
		{
			ISystemMessageCollection messages = commandHandler.HandleCommand(command, serviceRequestMetadata);

			ISystemMessageCollection newMessages = SystemMessageCollection.Empty();
			newMessages.AddMessages(messages.CopyMessages());

			((ISystemMessageCollection)messages).Dispose();
			return newMessages;

		}
	}
}