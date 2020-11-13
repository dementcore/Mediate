﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Core.Abstractions
{
    public interface IHandlerProvider : IEventHandlerProvider, IQueryHandlerProvider
    {
    }
}
