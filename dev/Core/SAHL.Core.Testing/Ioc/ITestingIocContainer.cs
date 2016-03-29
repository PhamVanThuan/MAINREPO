using SAHL.Core.Testing.FileConventions;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Collections.Generic;
namespace SAHL.Core.Testing.Ioc
{
    public interface ITestingIocContainer<T, T2> : ITestingIocContainer
        where T : IFileConvention
        where T2 : IRegistrationConvention, new() {}
    
    public interface ITestingIocContainer : IContainer
    {
        void Initialise();
        IEnumerable<Type> GetRegisteredTypes();
    }
    
}
