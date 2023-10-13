using System;
using System.Diagnostics;
using System.Globalization;

namespace CodeInject.MemoryTools
{
    internal unsafe class MemoryTools
    {

        public static string GetModulePath(string moduleName)
        {
            foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
            {
                if(module.FileName.ToUpper().Contains(moduleName.ToUpper()))
                {
                    return module.FileName;
                }
            }
            return "NOT FOUND";
        }


        public static IntPtr? GetSignatureAddreses(string pattern)
        {
            Process proc = Process.GetCurrentProcess();

            string[] bytesArray = pattern.Split(' ');

            byte[] patternBytes = new byte[bytesArray.Length];

            for (int i = 0; i < bytesArray.Length; i++)
            {
                if (bytesArray[i] == "??")
                {
                    patternBytes[i] = 0xFF;
                }
                else
                {
                    patternBytes[i] = byte.Parse(bytesArray[i], NumberStyles.HexNumber);
                }
            }

            byte* modulePointer = (byte*)proc.MainModule.BaseAddress.ToPointer();

            IntPtr? foundAdresses = null;



            byte* maxAddres = (byte*)new IntPtr(proc.MainModule.BaseAddress.ToInt64() + proc.MainModule.ModuleMemorySize).ToPointer();


            while (modulePointer < maxAddres)
            {
                for (int i = 0; i < bytesArray.Length; i++)
                {
                    if (i == bytesArray.Length - 1)
                    {
                        foundAdresses = new IntPtr(modulePointer);
                    }

                    if (bytesArray[i] == "??")
                    {
                        continue;
                    }

                    if (modulePointer[i] != patternBytes[i])
                    {
                        break;
                    }
                }

                if (foundAdresses != null) { break; }
                modulePointer++;
            }

            return foundAdresses;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="pattern">Pattern have to be completed whole asm instruction with function call</param>
        /// <returns></returns>
        public static IntPtr GetCallAddress(string pattern)
        {
          
              IntPtr? Addresses = GetSignatureAddreses(pattern);
              IntPtr nextInstructionAddres = new IntPtr(Addresses.Value.ToInt64() + pattern.Split(' ').Length);
              int* jumpOffset = (int*)new IntPtr(nextInstructionAddres.ToInt64() - 4).ToPointer();
              return new IntPtr((Addresses.Value.ToInt64() + (long)(pattern.Split(' ')).Length) + (long)*jumpOffset);
        }


        public static IntPtr? GetFunctionAddress(string functionBeginPattern)
        {

            IntPtr? Addresses = GetSignatureAddreses(functionBeginPattern);

            return Addresses;
        }


        public static IntPtr GetVariableAddres(string pattern)
        {
            return GetCallAddress(pattern);
        }


        public static long GetInt64(long Adress, short[] offsets)
        {
            long finalAdress = Adress;

            foreach (short offset in offsets)
            {
                finalAdress = (*(long*)(finalAdress)) + (long)offset;
            }

            return finalAdress;
        }
    }
}

