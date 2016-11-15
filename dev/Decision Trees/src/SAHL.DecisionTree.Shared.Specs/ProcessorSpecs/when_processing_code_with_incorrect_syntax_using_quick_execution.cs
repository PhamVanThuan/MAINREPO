﻿using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Tools.ObjectFromJsonGenerator.Lib;
using SAHL.DecisionTree.Shared.Interfaces;
using SAHL.DecisionTree.Shared.Helpers;
using SAHL.DecisionTree.Shared.Core;

namespace SAHL.DecisionTree.Shared.Specs
{
    public class when_processing_code_with_incorrect_syntax_using_quick_execution : WithFakes 
    {
        private static bool result;
        private static string code;
        private static ProcessingMode mode;
        private static ProcessingEngine processingEngine;
 
        private Establish context = () =>
        {
            mode = ProcessingMode.Execution;
            processingEngine = new ProcessingEngine(mode);
            processingEngine.InitializeEngine();
            code = "a = 1///1"; //incorrect syntax
        };

        private Because of = () =>
        {
            result = processingEngine.Execute(code);
        };

        private It should_return_false = () =>
        {
            result.ShouldEqual(false);           
        };

    }
}