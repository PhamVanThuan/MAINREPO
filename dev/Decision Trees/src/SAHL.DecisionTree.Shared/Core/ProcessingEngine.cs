using IronRuby;
using IronRuby.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using SAHL.DecisionTree.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DecisionTree.Shared.Core
{
    public class ProcessingEngine : IProcessingEngine
    {
        private ScriptEngine engine;
        private ProcessingMode mode;

        public ProcessingEngine(ProcessingMode mode)
        {
            this.mode = mode;
            InitializeEngine();
        }

        public void InitializeEngine()
        {
            ScriptRuntimeSetup runtimeSetup = new ScriptRuntimeSetup();
            
            LanguageSetup ls = Ruby.CreateRubySetup();

            if (mode == ProcessingMode.Debug)
            {
                ls.ExceptionDetail = true;
                runtimeSetup.DebugMode = true;
            }
            else if (mode == ProcessingMode.Execution)
            {
                ls.ExceptionDetail = false;
                runtimeSetup.DebugMode = false;
            }

            runtimeSetup.LanguageSetups.Add(ls);
            ScriptRuntime runtime = new ScriptRuntime(runtimeSetup);

            this.engine = runtime.CreateScope("Ruby").Engine;
        }

        public bool Execute(string code)
        {
            bool result = false;

            try
            {
                engine.Execute(code);
                result = true;
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }

        public bool Process(string code)
        {
            bool result = false;

            try
            {
                ScriptSource source = engine.CreateScriptSourceFromString(code, "source.rb", Microsoft.Scripting.SourceCodeKind.AutoDetect);

                source.Execute();

                result = true;//GetVariable("Variables").outputs.NodeResult;
            }
            catch (Exception ex)
            {
                int line = 0;
                ExceptionType etype;
                if (ex is SyntaxErrorException)
                {
                    line = (ex as SyntaxErrorException).Line;
                    etype = ExceptionType.Syntax;
                }
                else
                {
                    RubyExceptionData red = RubyExceptionData.GetInstance(ex);
                    if (red.Backtrace != null)
                    {
                        foreach (IronRuby.Builtins.MutableString msrc in red.Backtrace)
                        {
                            string src = Convert.ToString(msrc);
                            if (src.Contains("source.rb"))
                            {
                                int.TryParse(src.Split(':')[1], out line);
                            }
                        }
                    }
                    etype = ExceptionType.Runtime;
                }
                string ErrorCode = "";
                if (line != 0)
                {
                    string[] codeLines = code.Split(Environment.NewLine.ToCharArray());
                    ErrorCode = codeLines[line - 1];
                }
                
                OnCodeExecutionExceptionRaised(new NodeExceptionEventsArgs(line, ErrorCode, etype, ex));
                //dynamic messages = GetVariable("Messages");
                //messages.AddError(ex.Message);
            }

            return result;
        }

        public void SetVariable(string name, dynamic obj)
        {
            engine.Runtime.Globals.SetVariable(name, obj);
        }

        public dynamic GetVariable(string name)
        {
            return engine.Runtime.Globals.GetVariable(name);
        }

        public event EventHandler<NodeExceptionEventsArgs> CodeExecutionExceptionRaised;

        protected virtual void OnCodeExecutionExceptionRaised(NodeExceptionEventsArgs e)
        {
            EventHandler<NodeExceptionEventsArgs> handler = CodeExecutionExceptionRaised;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public enum ProcessingMode
    {
        Execution,
        Debug
    }
}
