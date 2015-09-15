﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cicd.domain.context.trigger.abstracts
{
    public interface IBrain
    {
        void Process(IEvent Event);
    }
}
