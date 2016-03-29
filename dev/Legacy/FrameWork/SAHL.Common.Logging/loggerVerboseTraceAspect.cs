using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Aspects;
using System.Reflection;

namespace SAHL.Common.Logging
{
    [Serializable]
    [LoggerVerboseTraceAspect(AttributeExclude=true)]
    public class LoggerVerboseTraceAspect : OnMethodBoundaryAspect
    {
        private string methodName;
        /// <summary>
        /// Method executed at build time. Initializes the aspect instance. After the execution
        /// of <see cref="CompileTimeInitialize"/>, the aspect is serialized as a managed 
        /// resource inside the transformed assembly, and deserialized at runtime.
        /// </summary>
        /// <param name="method">Method to which the current aspect instance 
        /// has been applied.</param>
        /// <param name="aspectInfo">Unused.</param>
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            this.methodName = method.DeclaringType.FullName + "." + method.Name;
        }

        /// <summary>
        /// Method invoked before the execution of the method to which the current
        /// aspect is applied.
        /// </summary>
        /// <param name="args">Unused.</param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            LogPlugin.Logger.LogOnEnterMethod(this.methodName);
        }

        /// <summary>
        /// Method invoked after successfull execution of the method to which the current
        /// aspect is applied, usually just before returning from a method call.
        /// </summary>
        /// <param name="args">Unused.</param>
        public override void OnSuccess(MethodExecutionArgs args)
        {
            LogPlugin.Logger.LogOnMethodSuccess(this.methodName);
        }

        /// <summary>
        /// Method invoked when exiting a method, regardless of whether it has been successful or resulted in an exception.
        /// </summary>
        /// <param name="args">Unused.</param>
        public override void OnExit(MethodExecutionArgs args)
        {
            LogPlugin.Logger.LogOnExitMethod(this.methodName);
        }
        /// <summary>
        /// Method invoked after failure of the method to which the current
        /// aspect is applied.
        /// </summary>
        /// <param name="args">Unused.</param>
        public override void OnException(MethodExecutionArgs args)
        {
            LogPlugin.Logger.LogOnMethodException(this.methodName, args.Exception);
        }
    }
}
