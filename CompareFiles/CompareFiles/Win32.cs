using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace CompareFiles
{
    public static class Win32
    {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int memcmp(byte[] b1, byte[] b2, long count);

        public static bool ByteArrayCompare(byte[] b1, byte[] b2)
        {
            // Validate buffers are the same length.
            // This also ensures that the count does not exceed the length of either buffer.  
            return b1.Length == b2.Length && memcmp(b1, b2, b1.Length) == 0;
        }

        //Declaration of structures
        //SYSTEM_CACHE_INFORMATION
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct SYSTEM_CACHE_INFORMATION
        {
            public uint CurrentSize;
            public uint PeakSize;
            public uint PageFaultCount;
            public uint MinimumWorkingSet;
            public uint MaximumWorkingSet;
            public uint Unused1;
            public uint Unused2;
            public uint Unused3;
            public uint Unused4;
        }

        //SYSTEM_CACHE_INFORMATION_64_BIT
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct SYSTEM_CACHE_INFORMATION_64_BIT
        {
            public long CurrentSize;
            public long PeakSize;
            public long PageFaultCount;
            public long MinimumWorkingSet;
            public long MaximumWorkingSet;
            public long Unused1;
            public long Unused2;
            public long Unused3;
            public long Unused4;
        }

        //TokPriv1Luid
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
        }
            //Declaration of constants
            const int SE_PRIVILEGE_ENABLED = 2;
            const string SE_INCREASE_QUOTA_NAME = "SeIncreaseQuotaPrivilege";
            const string SE_PROFILE_SINGLE_PROCESS_NAME = "SeProfileSingleProcessPrivilege";
            const int SystemFileCacheInformation = 0x0015;
            const int SystemMemoryListInformation = 0x0050;
            const int MemoryPurgeStandbyList = 4;
            const int MemoryEmptyWorkingSets = 2;

            //Import of DLL's (API) and the necessary functions 
            [DllImport("advapi32.dll", SetLastError = true)]
            internal static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

            [DllImport("advapi32.dll", SetLastError = true)]
            internal static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall, ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

            [DllImport("ntdll.dll")]
            public static extern UInt32 NtSetSystemInformation(int InfoClass, IntPtr Info, int Length);

            [DllImport("psapi.dll")]
            static extern int EmptyWorkingSet(IntPtr hwProc);

            //Function to clear working set of all processes
            public static void EmptyWorkingSetFunction()
            {
                //Declaration of variables
                string ProcessName = string.Empty;
                Process[] allProcesses = Process.GetProcesses();
                List<string> successProcesses = new List<string>();
                List<string> failProcesses = new List<string>();

                //Cycle through all processes
                for (int i = 0; i < allProcesses.Length; i++)
                {
                    Process p = new Process();
                    p = allProcesses[i];
                    //Try to empty the working set of the process, if succesfull add to successProcesses, if failed add to failProcesses with error message
                    try
                    {
                        ProcessName = p.ProcessName;
                        EmptyWorkingSet(p.Handle);
                        successProcesses.Add(ProcessName);
                    }
                    catch (Exception ex)
                    {
                        failProcesses.Add(ProcessName + ": " + ex.Message);
                    }
                }

                //Print the lists with successful and failed processes
                Console.WriteLine("SUCCESSFULLY CLEARED PROCESSES: " + successProcesses.Count);
                Console.WriteLine("-------------------------------");
                for (int i = 0; i < successProcesses.Count; i++)
                {
                    Console.WriteLine(successProcesses[i]);
                }
                Console.WriteLine();

                Console.WriteLine("FAILED CLEARED PROCESSES: " + failProcesses.Count);
                Console.WriteLine("-------------------------------");
                for (int i = 0; i < failProcesses.Count; i++)
                {
                    Console.WriteLine(failProcesses[i]);
                }
                Console.WriteLine();
            }

            //Function to check if OS is 64-bit or not, returns boolean
            public static bool Is64BitMode()
            {
                return Marshal.SizeOf(typeof(IntPtr)) == 8;
            }

            //Function used to clear file system cache, returns boolean
            public static void ClearFileSystemCache(bool ClearStandbyCache)
            {
                try
                {
                    //Check if privilege can be increased
                    if (SetIncreasePrivilege(SE_INCREASE_QUOTA_NAME))
                    {
                        uint num1;
                        int SystemInfoLength;
                        GCHandle gcHandle;
                        //First check which version is running, then fill structure with cache information. Throw error is cache information cannot be read.
                        if (!Is64BitMode())
                        {
                            SYSTEM_CACHE_INFORMATION cacheInformation = new SYSTEM_CACHE_INFORMATION();
                            cacheInformation.MinimumWorkingSet = uint.MaxValue;
                            cacheInformation.MaximumWorkingSet = uint.MaxValue;
                            SystemInfoLength = Marshal.SizeOf(cacheInformation);
                            gcHandle = GCHandle.Alloc(cacheInformation, GCHandleType.Pinned);
                            num1 = NtSetSystemInformation(SystemFileCacheInformation, gcHandle.AddrOfPinnedObject(), SystemInfoLength);
                            gcHandle.Free();
                        }
                        else
                        {
                            SYSTEM_CACHE_INFORMATION_64_BIT information64Bit = new SYSTEM_CACHE_INFORMATION_64_BIT();
                            information64Bit.MinimumWorkingSet = -1L;
                            information64Bit.MaximumWorkingSet = -1L;
                            SystemInfoLength = Marshal.SizeOf(information64Bit);
                            gcHandle = GCHandle.Alloc(information64Bit, GCHandleType.Pinned);
                            num1 = NtSetSystemInformation(SystemFileCacheInformation, gcHandle.AddrOfPinnedObject(), SystemInfoLength);
                            gcHandle.Free();
                        }
                        if (num1 != 0)
                            throw new Exception("NtSetSystemInformation(SYSTEMCACHEINFORMATION) error: ", new Win32Exception(Marshal.GetLastWin32Error()));
                    }

                    //If passes paramater is 'true' and the privilege can be increased, then clear standby lists through MemoryPurgeStandbyList
                    if (ClearStandbyCache && SetIncreasePrivilege(SE_PROFILE_SINGLE_PROCESS_NAME))
                    {
                        int SystemInfoLength = Marshal.SizeOf(MemoryPurgeStandbyList);
                        GCHandle gcHandle = GCHandle.Alloc(MemoryPurgeStandbyList, GCHandleType.Pinned);
                        uint num2 = NtSetSystemInformation(SystemMemoryListInformation, gcHandle.AddrOfPinnedObject(), SystemInfoLength);
                        gcHandle.Free();
                        if (num2 != 0)
                            throw new Exception("NtSetSystemInformation(SYSTEMMEMORYLISTINFORMATION) error: ", new Win32Exception(Marshal.GetLastWin32Error()));
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
            }

            //Function to increase Privilege, returns boolean
            private static bool SetIncreasePrivilege(string privilegeName)
            {
                using (WindowsIdentity current = WindowsIdentity.GetCurrent(TokenAccessLevels.Query | TokenAccessLevels.AdjustPrivileges))
                {
                    TokPriv1Luid newst;
                    newst.Count = 1;
                    newst.Luid = 0L;
                    newst.Attr = SE_PRIVILEGE_ENABLED;

                    //Retrieves the LUID used on a specified system to locally represent the specified privilege name
                    if (!LookupPrivilegeValue(null, privilegeName, ref newst.Luid))
                        throw new Exception("Error in LookupPrivilegeValue: ", new Win32Exception(Marshal.GetLastWin32Error()));

                    //Enables or disables privileges in a specified access token
                    int num = AdjustTokenPrivileges(current.Token, false, ref newst, 0, IntPtr.Zero, IntPtr.Zero) ? 1 : 0;
                    if (num == 0)
                        throw new Exception("Error in AdjustTokenPrivileges: ", new Win32Exception(Marshal.GetLastWin32Error()));
                    return num != 0;
                }
            }
        }
    }