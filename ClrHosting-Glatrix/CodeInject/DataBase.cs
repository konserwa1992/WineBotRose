using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject
{

    public class MobInfo
    {
        public int ID;
        public string Name;
    }

    public class SkillInfo
    {
        public int ID;
        public string Name = " ";
    }

    public class DataBase
    {
        public static DataBase GameDataBase { get; private set; } = new DataBase();

        public List<MobInfo> MonsterDatabase = new List<MobInfo>();
        public List<SkillInfo> SkillDatabase = new List<SkillInfo>();


        private DataBase()
        {
            LoadMonsterDataBase();
            LoadSkillDataBase();

        }

        private void LoadMonsterDataBase()
        {
            StreamReader dataReade = new StreamReader("MonsterList.json");
            MonsterDatabase = JsonConvert.DeserializeObject<List<MobInfo>>(dataReade.ReadToEnd());
        }

        private void LoadSkillDataBase()
        {
            StreamReader dataReade = new StreamReader("SkillList.json");

            SkillDatabase = JsonConvert.DeserializeObject<List<SkillInfo>>(dataReade.ReadToEnd());
        }

    }
}
