using System;

namespace STROOP.Utilities
{
    public class PlatformSpecific
    {
        // using System.Runtime.InteropServices.RuntimeInformation is better, but needs .NET Framework 4.7.1, see :
        // https://stackoverflow.com/questions/5116977/how-to-check-the-os-version-at-runtime-e-g-on-windows-or-linux-without-using/47390306#47390306
        // as of now, we can only use https://www.mono-project.com/docs/faq/technical/#how-to-detect-the-execution-platform
        public static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }
    }
}