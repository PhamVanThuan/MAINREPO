﻿using SAHL.Core.IoC;
using StructureMap.Graph;
using System;

namespace SAHL.Core.Testing.Ioc.Registration
{
    public class ServiceDataManagerConvention : IRegistrationConvention
    {
        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            if (type.Name.EndsWith("DataManager") && type.IsClass && !type.Assembly.GetName().Name.Contains("Domain")
             && type.Assembly.GetName().Name.Contains("Services"))
            {
                registry.For(type).Use(type);
            }
        }
    }
}