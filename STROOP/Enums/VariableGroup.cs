﻿using STROOP.Managers;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public enum VariableGroup
    {
        Basic,
        Intermediate,
        Advanced,

        ObjectSpecific,
        Scheduler,
        Snow,

        NoGroup,
        Custom,

        ProcessGroup,
        Collision,
        Movement,
        Transformation,
        Coordinate,

        HolpMario,
        HolpPoint,
        Trajectory,
        TAS,
        Point,
        Coin,
        Hacks,
        Rng,
        Self,
    };
}
