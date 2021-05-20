using System;
using System.Collections.Generic;
using DeskBooker.Core.Domain;

namespace DeskBooker.Core.DataInterface
{
    public interface IDeskRespository
    {
        IEnumerable<Desk> GetAvailableDesks(DateTime date);
    }
}
