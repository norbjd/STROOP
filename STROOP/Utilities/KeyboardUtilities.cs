using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Input;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace STROOP.Utilities
{
    public static class KeyboardUtilities
    {
        public static bool IsKeyDown(System.Windows.Input.Key k)
        {
            // return k.ToString() == Console.ReadKey().Key.ToString();
            return false; // TODO_norbjd
        }

        public static bool IsKeyDown(OpenTK.Input.Key k)
        {
            return false; // TODO_norbjd
        }

        public static bool GetState(System.Windows.Input.Key k)
        {
            // return k.ToString() == Console.ReadKey().Key.ToString();
            return false; // TODO_norbjd
        }

        public static int? GetCurrentlyInputtedNumber()
        {
            var key = Console.ReadKey().Key;
            if (key.Equals(System.Windows.Input.Key.D1)) return 1;
            if (key.Equals(System.Windows.Input.Key.D2)) return 2;
            if (key.Equals(System.Windows.Input.Key.D3)) return 3;
            if (key.Equals(System.Windows.Input.Key.D4)) return 4;
            if (key.Equals(System.Windows.Input.Key.D5)) return 5;
            if (key.Equals(System.Windows.Input.Key.D6)) return 6;
            if (key.Equals(System.Windows.Input.Key.D7)) return 7;
            if (key.Equals(System.Windows.Input.Key.D8)) return 8;
            if (key.Equals(System.Windows.Input.Key.D9)) return 9;
            if (key.Equals(System.Windows.Input.Key.D0)) return 0;
            return null;
        }

        public static bool IsNumberHeld()
        {
            return GetCurrentlyInputtedNumber() != null;
        }

        public static bool IsCtrlHeld()
        {
            return Console.ReadKey().Modifiers.Equals(ConsoleModifiers.Control);
        }

        public static bool IsShiftHeld()
        {
            return Console.ReadKey().Modifiers.Equals(ConsoleModifiers.Shift);
        }

        public static bool IsAltHeld()
        {
            return Console.ReadKey().Modifiers.Equals(ConsoleModifiers.Alt);
        }

        public static bool IsDeletishKeyHeld()
        {
            var keyChar = Console.ReadKey().Key;
            return keyChar.Equals(System.Windows.Input.Key.Delete) ||
                keyChar.Equals(System.Windows.Input.Key.Back) ||
                keyChar.Equals(System.Windows.Input.Key.Escape);
        }
    }
}
