using CodeInject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ISpace
{
    public unsafe class IClass
    {


        IntPtr? GetSignatureAddreses(string pattern)
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
                        Console.WriteLine("Found: " + ((Int64)modulePointer).ToString("X") + " / " + ((Int64)proc.MainModule.BaseAddress.ToInt64() + proc.MainModule.ModuleMemorySize).ToString("X"));
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


        public static int IMain(string args)
        {
            cBot cBot = new cBot();
            cBot.ShowDialog();
  
            return 0;
        }

        [DllImport("kernel32")]
        static extern bool AllocConsole();
    }
}
