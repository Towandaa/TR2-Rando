﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRLevelReader.Model
{
    public class TRAnimDispatch
    {
        public short Low { get; set; }

        public short High { get; set; }

        public short NextAnimation { get; set; }

        public short NextFrame { get; set; }
    }
}
