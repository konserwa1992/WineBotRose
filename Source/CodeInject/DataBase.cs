using CodeInject.MemoryTools;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.IO;


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
        private static string dataPath;



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
            dataPath = MemoryTools.MemoryTools.GetModulePath("clrbootstrap").Substring(0, MemoryTools.MemoryTools.GetModulePath("clrbootstrap").LastIndexOf("\\")) + "\\data\\";
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
            if (!File.Exists(dataPath+"MaterialItemList.json")) GameFunctionsAndObjects.Actions.Logger($"Missing file: MaterialItemList.json", Color.Red);
            StreamReader dataRead = new StreamReader(dataPath+"MaterialItemList.json");
            MaterialItemsDatabase = JsonConvert.DeserializeObject<List<MaterialItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }

        private void LoadBodyDataBase()
        {
            if (!File.Exists(dataPath + "BodyItemList.json")) GameFunctionsAndObjects.Actions.Logger($"Missing file: BodyItemList.json", Color.Red);
            StreamReader dataRead = new StreamReader(dataPath + "BodyItemList.json");
            BodyItemsDatabase = JsonConvert.DeserializeObject<List<BodyItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }
        private void LoadArmDataBase()
        {
            if (!File.Exists(dataPath + "ArmItemList.json")) GameFunctionsAndObjects.Actions.Logger($"Missing file: ArmItemList.json", Color.Red);
            StreamReader dataRead = new StreamReader(dataPath + "ArmItemList.json");
            ArmItemsDatabase = JsonConvert.DeserializeObject<List<ArmItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }
        private void LoadFootDataBase()
        {
            if (!File.Exists(dataPath + "FootItemList.json")) GameFunctionsAndObjects.Actions.Logger($"Missing file: FootItemList.json", Color.Red);
            StreamReader dataRead = new StreamReader(dataPath + "FootItemList.json");
            FootItemsDatabase = JsonConvert.DeserializeObject<List<FootItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }
        private void LoadWeaponDataBase()
        {
            if (!File.Exists(dataPath + "WeaponItemList.json")) GameFunctionsAndObjects.Actions.Logger($"Missing file: WeaponItemList.json", Color.Red);
            StreamReader dataRead = new StreamReader(dataPath + "WeaponItemList.json");
            WeaponItemsDatabase = JsonConvert.DeserializeObject<List<WeaponItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }
        private void LoadMonsterDataBase()
        {
            if (!File.Exists(dataPath + "MonsterList.json")) GameFunctionsAndObjects.Actions.Logger($"Missing file: MonsterList.json", Color.Red);
            StreamReader dataRead = new StreamReader(dataPath + "MonsterList.json");
            MonsterDatabase = JsonConvert.DeserializeObject<List<MobInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }
        private void LoadSkillDataBase()
        {
            if (!File.Exists(dataPath + "SkillList.json")) GameFunctionsAndObjects.Actions.Logger($"Missing file: SkillList.json", Color.Red);
            StreamReader dataRead = new StreamReader(dataPath + "SkillList.json");
            SkillDatabase = JsonConvert.DeserializeObject<List<SkillInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }
        private void LoadUsableItemDataBase()
        {
            if (!File.Exists(dataPath + "UseItemList.json")) GameFunctionsAndObjects.Actions.Logger($"Missing file: UseItemList.json", Color.Red);
            StreamReader dataRead = new StreamReader(dataPath + "UseItemList.json");
            UsableItemsDatabase = JsonConvert.DeserializeObject<List<UsableItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }
        private void LoadShieldItemDataBase()
        {
            if (!File.Exists(dataPath + "ShieldItemList.json")) GameFunctionsAndObjects.Actions.Logger($"Missing file: ShieldItemList.json", Color.Red);
            StreamReader dataRead = new StreamReader(dataPath + "ShieldItemList.json");

            SheildItemsDatabase = JsonConvert.DeserializeObject<List<ShieldItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }
    }
}
