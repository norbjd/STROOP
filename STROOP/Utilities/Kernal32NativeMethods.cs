using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public static class Kernal32NativeMethods
    {
        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [Flags]
        public enum ProcessAccess : int
        {
            VM_OPERATION                        = 0x0008,
            VM_READ                             = 0x0010,
            VM_WRITE                            = 0x0020,
            PROCESS_QUERY_LIMITED_INFORMATION   = 0x1000,
            SUSPEND_RESUME                      = 0x0800,
        }

        [Flags]
        public enum MemoryType : uint
        {
            MEM_IMAGE       = 0x1000000,
            MEM_MAPPED      = 0x40000,
            MEM_PRIVATE     = 0x20000
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MemoryBasicInformation
        {
            public UIntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public IntPtr RegionSize;
            public uint State;
            public uint Protect;
            public MemoryType Type;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PsapiWorkingSetExInformation
        {
            public IntPtr VirtualAddress;
            public ulong VirtualAttributes;
        }

        #region DLL Import
        static IntPtr OpenThread(Process process, ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId) {
            foreach (ProcessThread pT in process.Threads)
            {
                if (dwThreadId == pT.Id) {
                    return pT.StartAddress;
                }
            }
            return IntPtr.Zero;
        }

        static uint SuspendThread(IntPtr hThread) {
            return (uint)hThread.ToInt32();
        }
        
        static int ResumeThread(IntPtr hThread) {
            return hThread.ToInt32();
        }

        static bool CloseHandle(IntPtr hObject) {
            return true;
        }

        static IntPtr OpenProcess(ProcessAccess dwDesiredAccess, bool bInheritHandle, int dwProcessId) {
            return System.Diagnostics.Process
                .GetProcessById(dwProcessId)
                .Handle;
        }

        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess,
            UIntPtr lpBaseAddress, byte[] lpBuffer, IntPtr dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress,
            byte[] lpBuffer, IntPtr dwSize, ref int lpNumberOfBytesWritten);

        // used only for Dolphin
        [DllImport("kernel32.dll")]
        static extern IntPtr VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MemoryBasicInformation lpBuffer, IntPtr dwLength);

        // used only for Dolphin
        [DllImport("psapi", SetLastError = true)]
        static extern bool QueryWorkingSetEx(IntPtr hProcess, out PsapiWorkingSetExInformation pv, uint cb);
        #endregion

        public static IntPtr ProcessGetHandleFromId(ProcessAccess dwDesiredAccess, bool bInheritHandle, int dwProcessId)
        {
            return OpenProcess(dwDesiredAccess, bInheritHandle, dwProcessId);
        }

        public static bool CloseProcess(IntPtr processHandle)
        {
            return CloseHandle(processHandle);
        }
        public static bool ProcessReadMemory(IntPtr hProcess,
            UIntPtr lpBaseAddress, byte[] lpBuffer)
        {
            int numOfBytes = 0;
            return ReadProcessMemory(hProcess, lpBaseAddress, lpBuffer, (IntPtr)lpBuffer.Length, ref numOfBytes);
        }

        public static bool ProcessWriteMemory(IntPtr hProcess, UIntPtr lpBaseAddress,
            byte[] lpBuffer)
        {
            int numOfBytes = 0;
            return WriteProcessMemory(hProcess, lpBaseAddress, lpBuffer, (IntPtr)lpBuffer.Length, ref numOfBytes);
        }

        public static void ResumeProcess(Process process)
        {
            // Resume all threads
            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(process, ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                    continue;

                int suspendCount = 0;
                do
                {
                    suspendCount = ResumeThread(pOpenThread);
                } while (suspendCount > 0);

                CloseHandle(pOpenThread);
            }
        }

        public static void SuspendProcess(Process process)
        {
            // Pause all threads
            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(process, ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                    continue;

                SuspendThread(pOpenThread);
                CloseHandle(pOpenThread);
            }
        }

        public static IntPtr VQueryEx(IntPtr hProcess, IntPtr lpAddress, out MemoryBasicInformation lpBuffer, IntPtr dwLength)
        {
            return VirtualQueryEx(hProcess, lpAddress, out lpBuffer, dwLength);
        }

        public static bool QWorkingSetEx(IntPtr hProcess, out PsapiWorkingSetExInformation pv, uint cb)
        {
            return QueryWorkingSetEx(hProcess, out pv, cb);
        }
    }
}
