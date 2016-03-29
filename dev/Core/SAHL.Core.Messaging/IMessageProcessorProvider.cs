﻿namespace SAHL.Core.Messaging
 {
     public interface IMessageProcessorProvider
     {
         dynamic GetMessageProcessor(object message);
     }
 }