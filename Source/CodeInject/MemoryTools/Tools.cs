using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.MemoryTools
{
    internal unsafe  class Tools
    {
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
    }
}
