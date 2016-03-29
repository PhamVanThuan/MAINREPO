namespace SAHL.Tools.DecisionTree.TestRunner.Lib
{
    public delegate void DebugLocationChanged(object sender, DebugLocationChangedArgs e);

    public delegate void DecisionTreeExecutionStarted(object sender, DecisionTreeExecutionStartedArgs e);

    public delegate void DecisionTreeExecutionEnded(object sender, DecisionTreeExecutionEndedArgs e);
}