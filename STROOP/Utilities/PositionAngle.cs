﻿using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public class PositionAngle
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;
        public readonly double? Angle;

        public PositionAngle(double x, double y, double z, double? angle = null)
        {
            X = x;
            Y = y;
            Z = z;
            Angle = angle;
        }

        public static PositionAngle Custom()
        {
            return new PositionAngle(SpecialConfig.CustomX, SpecialConfig.CustomY, SpecialConfig.CustomZ, SpecialConfig.CustomAngle);
        }

        public static PositionAngle Mario()
        {
            return new PositionAngle(DataModels.Mario.X, DataModels.Mario.Y, DataModels.Mario.Z, DataModels.Mario.FacingYaw);
        }

        public static PositionAngle Holp()
        {
            return new PositionAngle(DataModels.Mario.HolpX, DataModels.Mario.HolpY, DataModels.Mario.HolpZ);
        }

        public static PositionAngle Camera()
        {
            return new PositionAngle(DataModels.Camera.X, DataModels.Camera.Y, DataModels.Camera.Z, DataModels.Camera.FacingYaw);
        }

        public static PositionAngle Object(uint address)
        {
            return new PositionAngle(
                Config.Stream.GetSingle(address + ObjectConfig.XOffset),
                Config.Stream.GetSingle(address + ObjectConfig.YOffset),
                Config.Stream.GetSingle(address + ObjectConfig.ZOffset),
                Config.Stream.GetUInt16(address + ObjectConfig.YawFacingOffset));
        }

        public static PositionAngle ObjectHome(uint address)
        {
            return new PositionAngle(
                Config.Stream.GetSingle(address + ObjectConfig.HomeXOffset),
                Config.Stream.GetSingle(address + ObjectConfig.HomeYOffset),
                Config.Stream.GetSingle(address + ObjectConfig.HomeZOffset));
        }

        public static PositionAngle Tri(uint address, int vertex)
        {
            TriangleDataModel tri = new TriangleDataModel(address);
            switch (vertex)
            {
                case 1:
                    return new PositionAngle(tri.X1, tri.Y1, tri.Z1);
                case 2:
                    return new PositionAngle(tri.X2, tri.Y2, tri.Z2);
                case 3:
                    return new PositionAngle(tri.X3, tri.Y3, tri.Z3);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static PositionAngle FromId(PositionAngleId posAngleId)
        {
            switch (posAngleId.PosAngleType)
            {
                case PositionAngleTypeEnum.Custom:
                    return Custom();
                case PositionAngleTypeEnum.Mario:
                    return Mario();
                case PositionAngleTypeEnum.Holp:
                    return Holp();
                case PositionAngleTypeEnum.Camera:
                    return Camera();
                case PositionAngleTypeEnum.Object:
                    return Object(posAngleId.Address.Value);
                case PositionAngleTypeEnum.ObjectHome:
                    return ObjectHome(posAngleId.Address.Value);
                case PositionAngleTypeEnum.Tri:
                    return Tri(posAngleId.Address.Value, posAngleId.TriVertex.Value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
