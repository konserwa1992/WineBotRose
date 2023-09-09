using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CodeInject
{
    interface IBasicInfo
    {
         int ID { get; set; }
         string Name { get; set; }

         string ToString();
    }

    public class MobInfo: IBasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";

        public override string ToString()
        {
            return Name;
        }
    }

    public class SkillInfo: IBasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } = " ";
        public override string ToString() { return Name; }
    }

    public class WeaponItemsInfo: IBasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";


        public override string ToString() { return Name; }
    }

    public class UsableItemsInfo: IBasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } ="";
        public string DisplayName = "";
        public override string ToString() { return DisplayName; }

    }

    public class BodyItemsInfo : IBasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public override string ToString() { return Name; }
    }
    public class FootItemsInfo : IBasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public override string ToString() { return Name; }
    }

    public class ArmItemsInfo : IBasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public override string ToString() { return Name; }
    }
    public class ShieldItemsInfo : IBasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public override string ToString() { return Name; }
    }

    public class MaterialItemsInfo : IBasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public override string ToString() { return Name; }
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
        public List<ShieldItemsInfo> SheildItemsDatabase = new List<ShieldItemsInfo>();
        public List<MaterialItemsInfo> MaterialItemsDatabase = new List<MaterialItemsInfo>();




        public List<T> GetList<T>() where T : class
        {
            if(typeof(T) == typeof(MobInfo))
                return (MonsterDatabase as List<T>);
            if (typeof(T) == typeof(SkillInfo))
                return (SkillDatabase as List<T>);
            if (typeof(T) == typeof(UsableItemsInfo))
                return (UsableItemsDatabase as List<T>);
            if (typeof(T) == typeof(WeaponItemsInfo))
                return (WeaponItemsDatabase as List<T>);
            if (typeof(T) == typeof(BodyItemsInfo))
                return (BodyItemsDatabase as List<T>);
            if (typeof(T) == typeof(ArmItemsInfo))
                return (ArmItemsDatabase as List<T>);
            if (typeof(T) == typeof(FootItemsInfo))
                return (FootItemsDatabase as List<T>);
            if (typeof(T) == typeof(ShieldItemsInfo))
                return (SheildItemsDatabase as List<T>);
            if (typeof(T) == typeof(MaterialItemsInfo))
                return (MaterialItemsDatabase as List<T>);

            return null;
        }


        private DataBase()
        {
            LoadMonsterDataBase();
            LoadSkillDataBase();
            LoadUsableItemDataBase();
            LoadWeaponDataBase();
            LoadBodyDataBase();
            LoadArmDataBase();
            LoadFootDataBase();
            LoadShieldItemDataBase();
            LoadMaterialItemDataBase();

        }


        private void LoadMaterialItemDataBase()
        {
            StreamReader dataReade = new StreamReader("MaterialItemList.json");

            MaterialItemsDatabase = JsonConvert.DeserializeObject<List<MaterialItemsInfo>>(dataReade.ReadToEnd());
            dataReade.Close();
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
            StreamReader dataReade = new StreamReader("WeaponItemList.json");

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
        private void LoadShieldItemDataBase()
        {
            StreamReader dataReade = new StreamReader("ShieldItemList.json");

            SheildItemsDatabase = JsonConvert.DeserializeObject<List<ShieldItemsInfo>>(dataReade.ReadToEnd());
            dataReade.Close();
        }
    }
}
