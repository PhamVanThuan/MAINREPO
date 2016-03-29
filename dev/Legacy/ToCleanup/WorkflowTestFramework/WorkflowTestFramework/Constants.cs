using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Threading;

namespace WorkflowTestFramework
{
    public class Constants
    {
        // WorkFlow Name Constants
        public class WorkFlowName
        {
            public const string WTFSimpleTest = "WTFSimpleTest";
            public const string X2EngineTest = "X2EngineTest";
            public const string X2EngineTest2 = "X2EngineTest2";
            public const string X2EngineTest3 = "X2EngineTest3";
        }
        // WorkFlow Process Name Constants
        public class WorkFlowProcessName
        {
            public const string WTFSimpleTest = "WTFSimpleTest";
            public const string X2EngineTest = "X2EngineTest";
        }
        // WorkFlow ActivityName
        public class WorkFlowActivityName
        {
            public const string Create = "CreateCase";
            public const string CreateInstanceAction = "CreateInstanceAction";
        }
        // WorkFlow DataTables
        public class WorkFlowDataTables
        {
            public const string WTFSimpleTest = "WTFSimpleTest";
            public const string X2EngineTest = "X2EngineTest";
            public const string X2EngineTest2 = "X2EngineTest2";
            public const string X2EngineTest3 = "X2EngineTest3";
        }
    }

    public enum StateTypes : int
    {
	    User = 1,
	    System = 2,
	    SystemDecision = 3,
	    StartingPoint = 4,
	    Archive = 5,
	    Hold = 6
    }
}
