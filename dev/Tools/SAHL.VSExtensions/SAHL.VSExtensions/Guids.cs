// Guids.cs
// MUST match guids.h
using System;

namespace SAHomeloans.SAHL_VSExtensions
{
    internal static class GuidList
    {
        public const string guidSAHL_VSExtensionsPkgString = "c94aea5a-0e41-46de-b176-df751edeaa14";
        public const string guidSAHL_VSExtensionsCmdSetString = "11ccc5e0-3bad-4ac3-8d56-4b6b74103b3f";

        public static readonly Guid guidSAHL_VSExtensionsCmdSet = new Guid(guidSAHL_VSExtensionsCmdSetString);
    };
}