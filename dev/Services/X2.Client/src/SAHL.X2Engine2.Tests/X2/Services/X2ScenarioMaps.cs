using SAHL.X2Engine2.Tests.X2.Models;
using System.Collections.Generic;

namespace SAHL.X2Engine2.Tests.X2.Services
{
    public class X2ScenarioMaps
    {
        public IEnumerable<X2ScenarioMapInfo> GetMapNames()
        {
            return new[]
            {
               //new X2ScenarioMapInfo("Create Instance", "AutoForwardToState", "AutoForwardToState", @"SAHL\HaloUser", false, 2,
               //    new Dictionary<string, string>()
               //    {
               //        { "ApplicationKey", "1"}
               //    }),
               //new X2ScenarioMapInfo("Create Instance", "CloneWakeUpParent", "CloneWakeUpParent", @"SAHL\HaloUser", false, 8,
               //    new Dictionary<string, string>()
               //    {
               //        { "ApplicationKey", "1"}
               //    }),
               //new X2ScenarioMapInfo("Create Instance", "CommonFlagOnMainInstance", "CommonFlagOnMainInstance", @"SAHL\HaloUser", false, 3,
               //    new Dictionary<string, string>()
               //    {
               //        { "ApplicationKey", "1" },
               //        { "Timer2HasFired", "True" },
               //        { "FlagCount", "0" }
               //    }),
               //new X2ScenarioMapInfo("Create Instance", "CreateInstance", "CreateInstance", @"SAHL\HaloUser", false, 0,
               //    new Dictionary<string, string>()
               //    {
               //        { "ApplicationKey", "1"},
               //        { "Created", "True"}
               //    }),
               // new X2ScenarioMapInfo("Create Instance", "MultipleDecisions", "MultipleDecisions", @"SAHL\HaloUser", false, 3,
               //    new Dictionary<string, string>()
               //    {
               //        { "ApplicationKey", "1"}
               //    }),
               //new X2ScenarioMapInfo("Create Instance", "SetDataFieldsTest", "SetDataFieldsTest", @"SAHL\HaloUser", false, 0,
               //    new Dictionary<string, string>()
               //    {
               //        { "ApplicationKey", "1"}
               //    }),
               //new X2ScenarioMapInfo("Create Instance", "SimpleTimer", "SimpleTimer", @"SAHL\HaloUser", false, 8,
               //    new Dictionary<string, string>()
               //    {
               //        { "ApplicationKey", "1"}
               //    }),
               //new X2ScenarioMapInfo("Create Instance", "SourceDestinationReturn", "Source", @"SAHL\HaloUser", false, 6,
               //    new Dictionary<string, string>()
               //    {
               //        { "ApplicationKey", "1"}
               //    }),
               //new X2ScenarioMapInfo("Create Instance", "UserCloneCreated", "UserCloneCreated", @"SAHL\HaloUser", false, 2,
               //    new Dictionary<string, string>()
               //    {
               //        { "ApplicationKey", "1"}
               //    })
               new X2ScenarioMapInfo("Create Instance", "MultipleDecisionsWithFailure", "Map Exception", @"SAHL\HaloUser", false, 6,
                   new Dictionary<string, string>()
                   {
                       { "TestKey", "1"}
                   }),
               new X2ScenarioMapInfo("Create Instance", "MultipleDecisionsWithFailure", "DAO Exception", @"SAHL\HaloUser", false, 6,
                   new Dictionary<string, string>()
                   {
                       { "TestKey", "1"}
                   }),
               new X2ScenarioMapInfo("Create Instance", "MultipleDecisionsWithFailure", "SQL Exception", @"SAHL\HaloUser", false, 6,
                   new Dictionary<string, string>()
                   {
                       { "TestKey", "1"}
                   }),
               new X2ScenarioMapInfo("Create Instance", "MultipleDecisionsWithFailure", "SQL Timeout Exception", @"SAHL\HaloUser", false, 33,
                   new Dictionary<string, string>()
                   {
                       { "TestKey", "1"}
                   }),
               new X2ScenarioMapInfo("Create Instance", "MultipleDecisionsWithFailure", "Domain Validation Exception", @"SAHL\HaloUser", false, 6,
                   new Dictionary<string, string>()
                   {
                       { "TestKey", "1"}
                   }),
               new X2ScenarioMapInfo("Create Instance", "MultipleDecisionsWithFailure", "Domain Message Exception", @"SAHL\HaloUser", false, 6,
                   new Dictionary<string, string>()
                   {
                       { "TestKey", "1"}
                   }),
               new X2ScenarioMapInfo("Create Instance", "MultipleDecisionsWithFailure", "General Exception With Messages", @"SAHL\HaloUser", false, 6,
                   new Dictionary<string, string>()
                   {
                       { "TestKey", "1"}
                   }),
               new X2ScenarioMapInfo("Create Instance", "MultipleDecisionsWithFailure", "General Exception Without Messages", @"SAHL\HaloUser", false, 6,
                   new Dictionary<string, string>()
                   {
                       { "TestKey", "1"}
                   }),
               new X2ScenarioMapInfo("Create Instance", "MultipleDecisionsWithFailure", "SQL API Exception Test Workflow", @"SAHL\HaloUser", false, 6,
                   new Dictionary<string, string>()
                   {
                       { "TestKey", "1"}
                   }),
               new X2ScenarioMapInfo("Create Instance", "CloneWithMultipleDecisions", "CloneWithMultipleDecisions", @"SAHL\HaloUser", false, 6,
                   new Dictionary<string, string>()
                   {
                       { "TestKey", "1"}
                   })
            };
        }
    }
}