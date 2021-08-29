﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRFDControl.FDEntryTypes
{
    public class FDClimbEntry : FDEntry
    {
        public bool IsPositiveX
        {
            get
            {
                return ((Setup.SubFunction & (byte)FDClimbDirection.PositiveX) > 0);
            }
            set
            {
                if (value)
                {
                    Setup.SubFunction |= (byte)FDClimbDirection.PositiveX;
                }
                else if (IsPositiveX)
                {
                    Setup.SubFunction = (byte)(Setup.SubFunction & ~(byte)FDClimbDirection.PositiveX);
                }
            }
        }

        public bool IsPositiveZ
        {
            get
            {
                return ((Setup.SubFunction & (byte)FDClimbDirection.PositiveZ) > 0);
            }
            set
            {
                if (value)
                {
                    Setup.SubFunction |= (byte)FDClimbDirection.PositiveZ;
                }
                else if (IsPositiveZ)
                {
                    Setup.SubFunction = (byte)(Setup.SubFunction & ~(byte)FDClimbDirection.PositiveZ);
                }
            }
        }

        public bool IsNegativeX
        {
            get
            {
                return ((Setup.SubFunction & (byte)FDClimbDirection.NegativeX) > 0);
            }
            set
            {
                if (value)
                {
                    Setup.SubFunction |= (byte)FDClimbDirection.NegativeX;
                }
                else if (IsNegativeX)
                {
                    Setup.SubFunction = (byte)(Setup.SubFunction & ~(byte)FDClimbDirection.NegativeX);
                }
            }
        }

        public bool IsNegativeZ
        {
            get
            {
                return ((Setup.SubFunction & (byte)FDClimbDirection.NegativeZ) > 0);
            }
            set
            {
                if (value)
                {
                    Setup.SubFunction |= (byte)FDClimbDirection.NegativeZ;
                }
                else if (IsNegativeZ)
                {
                    Setup.SubFunction = (byte)(Setup.SubFunction & ~(byte)FDClimbDirection.NegativeZ);
                }
            }
        }
    }
}
