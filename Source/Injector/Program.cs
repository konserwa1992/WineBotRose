using Reloaded.Injector;
using System.Diagnostics;

Process process = Process.GetProcessesByName("trose").FirstOrDefault();


if (process != null)
{
    Injector injector = new Injector(process);
    Console.WriteLine(process.ProcessName +" "+injector.Inject("\\ClrBootstrap.dll"));
 
}else
{
    Console.WriteLine("Process not found.");
}