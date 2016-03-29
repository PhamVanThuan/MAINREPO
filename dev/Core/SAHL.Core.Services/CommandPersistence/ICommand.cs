using System;

namespace SAHL.Core.Services.CommandPersistence
{
    public interface ICommand
    {
        string CommandJson { get; set; }

        DateTime CommandDate { get; set; }
    }
}