using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainService2;
using SAHL.X2.Common;

namespace WorkflowMaps.Specs.Common
{
    public class TestDomainServiceLoader : IDomainServiceLoader
    {
        public Dictionary<Type, IX2WorkflowService> mocks;

        public TestDomainServiceLoader()
        {
            this.mocks = new Dictionary<Type, IX2WorkflowService>();
        }

        public IDomainServiceIOC DomainServiceIOC
        {
            get;
            set;
        }

        public T Get<T>() where T : IX2WorkflowService
        {
            return (T)mocks[typeof(T)];
        }

        public void RegisterMockForType<T>(T mock) where T : IX2WorkflowService
        {
            mocks.Add(typeof(T), mock);
        }

        
    }

}