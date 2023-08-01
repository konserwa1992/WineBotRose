using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject
{
    interface BasicInfo
    {
         int ID { get; set; }
         string Name { get; set; }
    }

    public class MobInfo: BasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class SkillInfo: BasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } = " ";
    }

    public class WeaponItemsInfo: BasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
    }

    public class UsableItemsInfo: BasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } ="";
        public string DisplayName = "";
    }

    public class BodyItemsInfo : BasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
    }
    public class FootItemsInfo : BasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
    }
    public class ArmItemsInfo : BasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
    }



    public class DataBase
    {
        public static DataBase GameDataBase { get; private set; } = new DataBase();

        public List<MobInfo> MonsterDatabase = new List<MobInfo>();
        public List<SkillInfo> SkillDatabase = new List<SkillInfo>();
        public List<UsableItemsInfo> UsableItemsDatabase = new List<UsableItemsInfo>();
        public List<WeaponItemsInfo> WeaponItemsDatabase = new List<WeaponItemsInfo>();
        public List<BodyItemsInfo> BodyItemsDatabase = new List<BodyItemsInfo>();
        public List<ArmItemsInfo> ArmItemsDatabase = new List<ArmItemsInfo>();
        public List<FootItemsInfo> FootItemsDatabase = new List<FootItemsInfo>();

        private DataBase()
        {
            LoadMonsterDataBase();
            LoadSkillDataBase();
            LoadUsableItemDataBase();
            LoadWeaponDataBase();
            LoadBodyDataBase();
            LoadArmDataBase();
            LoadFootDataBase();
        }
        private void LoadBodyDataBase()
        {
            StreamReader dataReade = new StreamReader("BodyItemList.json");

            BodyItemsDatabase = JsonConvert.DeserializeObject<List<BodyItemsInfo>>(dataReade.ReadToEnd());
            dataReade.Close();
        }
        private void LoadArmDataBase()
        {
            StreamReader dataReade = new StreamReader("ArmItemList.json");

            ArmItemsDatabase = JsonConvert.DeserializeObject<List<ArmItemsInfo>>(dataReade.ReadToEnd());
            dataReade.Close();
        }
        private void LoadFootDataBase()
        {
            StreamReader dataReade = new StreamReader("FootItemList.json");

            FootItemsDatabase = JsonConvert.DeserializeObject<List<FootItemsInfo>>(dataReade.ReadToEnd());
            dataReade.Close();
        }
        private void LoadWeaponDataBase()
        {
            StreamReader dataReade = new StreamReader("WeaponList.json");

            WeaponItemsDatabase = JsonConvert.DeserializeObject<List<WeaponItemsInfo>>(dataReade.ReadToEnd());
            dataReade.Close();
        }
        private void LoadMonsterDataBase()
        {
            StreamReader dataReade = new StreamReader("MonsterList.json");
            MonsterDatabase = JsonConvert.DeserializeObject<List<MobInfo>>(dataReade.ReadToEnd());
            dataReade.Close();
        }
        private void LoadSkillDataBase()
        {
            StreamReader dataReade = new StreamReader("SkillList.json");

            SkillDatabase = JsonConvert.DeserializeObject<List<SkillInfo>>(dataReade.ReadToEnd());
            dataReade.Close();
        }
        private void LoadUsableItemDataBase()
        {
            StreamReader dataReade = new StreamReader("UseItemList.json");

            UsableItemsDatabase = JsonConvert.DeserializeObject<List<UsableItemsInfo>>(dataReade.ReadToEnd());
            dataReade.Close();
        }
    }
}
