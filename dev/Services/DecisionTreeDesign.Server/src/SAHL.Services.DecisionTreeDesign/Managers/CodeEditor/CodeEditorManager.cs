using SAHL.Core.DataStructures;
using SAHL.Services.DecisionTreeDesign.Managers.EnumerationSet;
using SAHL.Services.DecisionTreeDesign.Managers.MessageSet;
using SAHL.Services.DecisionTreeDesign.Managers.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DecisionTreeDesign.Managers.CodeEditor
{
    public class CodeEditorManager : ICodeEditorManager
    {
        IEnumerationSetManager enumerationSetManager;
        IMessageSetManager messageSetManager;
        IVariableManager variableSetManager;
        IEnumerable<string> reservedWords = new List<string>(){
            "alias", "and", "BEGIN", "begin", "break", "case", "class", "def", "defined?", "do", "else",
            "elsif", "END", "end", "ensure", "false", "for", "if", "in", "module", "next", "not", "or",
            "redo", "rescue", "retry", "return", "self", "super", "then", "true", "undef", "unless",
            "until", "when", "while", "yield", "nil", "raise", "throw", "catch", "fail", "loop", "callcc",
            "caller", "lambda", "proc", "public", "protected", "private", "require", "load",
            "require_relative", "extend", "autoload", "__END__", "__FILE__", "__LINE__", "__dir__",
            "Messages.AddInfo()","Messages.AddWarning()","Messages.AddError()","()","{}","\"\"","''","[]", "if then \r\n else \r\n end"
        };

        public CodeEditorManager(IEnumerationSetManager enumerationSetManager, IMessageSetManager messageSetManager, IVariableManager variableSetManager)
        {
            this.enumerationSetManager = enumerationSetManager;
            this.messageSetManager = messageSetManager;
            this.variableSetManager = variableSetManager;
        }

        public Core.DataStructures.NestedTernary GetTSTForCodeEditor()
        {
            reservedWords = reservedWords.Concat(this.messageSetManager.GetLatestMessageSetWords());
            reservedWords = reservedWords.Concat(this.enumerationSetManager.GetLatestEnumerationSetWords());
            reservedWords = reservedWords.Concat(this.variableSetManager.GetLatestVariableSetWords());
            NestedTernary tree = new NestedTernary();
            foreach (string str in reservedWords)
            {
                tree.Add(str);
            }
            return tree;
        }
    }
}
