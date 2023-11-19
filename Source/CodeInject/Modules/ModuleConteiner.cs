using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject.Modules
{
    public class ModuleConteiner
    {
        public List<IModule> ModuleList { get; set; } = new List<IModule>();


        public IModule AddModule(IModule module)
        {
            IModule ifExistModule = GetModule<IModule>(module.Name);
            if (ifExistModule != null)
            {
                return ifExistModule;
            }
            else
            {
                ModuleList.Add(module);
                return module;
            }
        }
        public void RemoveModule(IModule module)
        {
            ModuleList.Remove(module);
        }
        public void RemoveModule(string moduleName)
        {
            IModule module = ModuleList.FirstOrDefault(m => m.Name == moduleName);
            if (module != null)
                ModuleList.Remove(module);
        }
        public T GetModule<T>(string moduleName)
        {
            var module = ModuleList.FirstOrDefault(m => m.Name == moduleName);
            return module != null ? (T)module : default;
            // return (T)ModuleList.FirstOrDefault(m => m.Name == moduleName);
        }
        public void ModuleExecute()
        {
            foreach (IModule module in ModuleList)
            {
                module.Update();
            }
        }
    }
}
