using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PostSharp.Aspects;

namespace SAHL.Common.Logging.Attributes
{
    /// <summary>
    /// Log Attribute
    /// </summary>
    [Serializable]
    public class SAHLExceptionAspectAttribute : OnExceptionAspect
    {
        /// <summary>
        /// On Exception
        /// </summary>
        /// <param name="args"></param>
        public override void OnException(MethodExecutionArgs args)
        {
            StringBuilder customMessageBuilder = new StringBuilder();
            customMessageBuilder.AppendLine();

            //Add the method's full name that was called
            //This beign made up of the declaring type's fullname and the method's name
            customMessageBuilder.AppendLine(String.Format("Method Name : {0}.{1}", args.Method.DeclaringType.FullName, args.Method.Name));

            ////Add the parameter's name, type and value
            Dictionary<string, object> parameters = new Dictionary<string,object>();
            parameters.Add(Logger.METHODPARAMSFROMASPECT, ProcessParameters(args.Method.GetParameters(), args.Arguments));

            LogPlugin.Logger.LogErrorMessageWithException(args.Method.Name, customMessageBuilder.ToString(), args.Exception, parameters);

            base.OnException(args);
        }

        Dictionary<string, object> ProcessParameters(ParameterInfo[] reflectionParams, Arguments args)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            for (int i=0;i<args.Count;i++)
            {
                ParameterInfo pi = reflectionParams[i];
                object parameterValue = args[i];

                parameters.Add(pi.Name, args[i]);
            }
            return parameters;
        }
    }
}