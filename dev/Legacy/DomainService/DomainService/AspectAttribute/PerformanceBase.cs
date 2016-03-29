namespace AspectAttribute
{
    ///// <summary>
    ///// 08 July Paul C
    ///// Wont Compile due to bug in postsharp see the following
    ///// http://www.postsharp.org/tracker/view.php?id=335
    ///// http://www.postsharp.org/tracker/view.php?id=362
    ///// </summary>
    //[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Class)]
    //[MulticastAttributeUsage(MulticastTargets.Method,
    //   TargetMemberAttributes =
    //    MulticastAttributes.Public | MulticastAttributes.Protected |
    //    MulticastAttributes.Instance,
    //    Inheritance = MulticastInheritance.None)]
    //[Serializable]
    //public class PerformanceBase : OnMethodBoundaryAspect
    //{
    //    int ThreadId;
    //    long TicksStart;
    //    public PerformanceBase()
    //        : base()
    //    {
    //        ThreadId = Thread.CurrentThread.GetHashCode();
    //    }

    //    public override void OnSuccess(MethodExecutionArgs eventArgs)
    //    {
    //        base.OnSuccess(eventArgs);
    //    }

    //    public override void OnEntry(MethodExecutionArgs eventArgs)
    //    {
    //        //Console.WriteLine("OnEnterMethod - {0}", eventArgs.Method.Name);
    //        TicksStart = DateTime.Now.Ticks;
    //        PerfObj p = PerfCache.Get(ThreadId);
    //        if (null == p)
    //        {
    //            p = new PerfObj(eventArgs.Method.Name, eventArgs.Method.GetHashCode(), eventArgs.Method.DeclaringType.FullName);
    //            p.Start();
    //            PerfCache.Add(ThreadId, p);
    //        }
    //    }

    //    public override void OnExit(MethodExecutionArgs eventArgs)
    //    {
    //        PerfObj p = PerfCache.Get(ThreadId);
    //        if (null != p)
    //        {
    //            if (p.IsInitialMethod(eventArgs.Method.Name, eventArgs.Method.GetHashCode()))
    //            {
    //                p.Complete();
    //                WriteToDB(p);
    //                PerfCache.Remove(ThreadId);
    //            }
    //            else
    //            {
    //                long Ticks = DateTime.Now.Ticks - TicksStart;
    //                TimeSpan ts = new TimeSpan(Ticks);
    //                p.AddMethodCall(eventArgs.Method.Name, ts.TotalSeconds);
    //            }
    //        }
    //    }

    //    public override void OnException(MethodExecutionArgs eventArgs)
    //    {
    //        Console.WriteLine("Death Awaits you all with nasty big pointed teeth - {0}{1}{2}",
    //            eventArgs.Method.Name, Environment.NewLine, eventArgs.Exception.ToString());
    //        Debug.WriteLine(string.Format("Death Awaits you all with nasty big pointed teeth - {0}{1}{2}",
    //            eventArgs.Method.Name, Environment.NewLine, eventArgs.Exception.ToString()));
    //    }

    //    public virtual void WriteToDB(PerfObj p)
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        sb.AppendFormat("insert into MethodProfiler values (getdate(), '{0}', '{1}', {2}, '{3}', {4}, '{5}')",
    //            p.MethodName, p.ClassName, p.TotalSeconds, p.InnerMethod, p.HashCode, Assembly.GetEntryAssembly().GetName().Name);
    //        PerfDBWriter.WriteToDB(sb.ToString());
    //    }
    //}
}