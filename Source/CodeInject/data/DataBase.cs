using CodeInject.MemoryTools;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Linq;


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
        public override string ToString() { return ID+" " + Name; }
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
    public class HeadItemsInfo : IBasicInfo
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


    public class MountItemsInfo : IBasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public override string ToString() { return Name; }
    }

    public class AccesoriesItemsInfo : IBasicInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public override string ToString() { return Name; }
    }

    public class GemItemsInfo : IBasicInfo
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
        public List<AccesoriesItemsInfo> AccesoriesItemsDatabase = new List<AccesoriesItemsInfo>();
        public List<FootItemsInfo> FootItemsDatabase = new List<FootItemsInfo>();
        public List<ShieldItemsInfo> SheildItemsDatabase = new List<ShieldItemsInfo>();
        public List<MaterialItemsInfo> MaterialItemsDatabase = new List<MaterialItemsInfo>();
        public List<HeadItemsInfo> HeadItemsDatabase = new List<HeadItemsInfo>();
        public List<MountItemsInfo> MountItemsDatabase = new List<MountItemsInfo>();
        public List<GemItemsInfo> GemItemsDatabase = new List<GemItemsInfo>();
        public static string DataPath;



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
            if (typeof(T) == typeof(HeadItemsInfo))
                return (HeadItemsDatabase as List<T>);
            if (typeof(T) == typeof(MountItemsInfo))
                return (MountItemsDatabase as List<T>);
            if (typeof(T) == typeof(AccesoriesItemsInfo))
                return (AccesoriesItemsDatabase as List<T>);
            if (typeof(T) == typeof(GemItemsInfo))
                return (GemItemsDatabase as List<T>);

            return null;
        }


        private DataBase()
        {
            DataPath = MemoryTools.MemoryTools.GetModulePath("clrbootstrap").Substring(0, MemoryTools.MemoryTools.GetModulePath("clrbootstrap").LastIndexOf("\\")) + "\\data\\";
            LoadMonsterDataBase();
            LoadSkillDataBase();
            LoadUsableItemDataBase();
            LoadWeaponDataBase();
            LoadBodyDataBase();
            LoadArmDataBase();
            LoadFootDataBase();
            LoadShieldItemDataBase();
            LoadMaterialItemDataBase();
            LoadHeadDataBase();
            LoadMountDataBase();
            LoadAccesoriesDataBase();
            LoadGemDataBase();
        }

        private void LoadGemDataBase()
        {
            if (!File.Exists(DataPath + "GemItemList.json")) GameHackFunc.Actions.Logger($"Missing file: GemItemList.json");
            StreamReader dataRead = new StreamReader(DataPath + "GemItemList.json");
            GemItemsDatabase = JsonConvert.DeserializeObject<List<GemItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }


        private void LoadMountDataBase()
        {
            if (!File.Exists(DataPath + "MountItemList.json")) GameHackFunc.Actions.Logger($"Missing file: MaterialItemList.json");
            StreamReader dataRead = new StreamReader(DataPath + "MountItemList.json");
            MountItemsDatabase = JsonConvert.DeserializeObject<List<MountItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }

        private void LoadMaterialItemDataBase()
        {
            if (!File.Exists(DataPath+"MaterialItemList.json")) GameHackFunc.Actions.Logger($"Missing file: MaterialItemList.json");
            StreamReader dataRead = new StreamReader(DataPath+"MaterialItemList.json");
            MaterialItemsDatabase = JsonConvert.DeserializeObject<List<MaterialItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }

        private void LoadAccesoriesDataBase()
        {
            if (!File.Exists(DataPath + "AccesoriesItemList.json")) GameHackFunc.Actions.Logger($"Missing file: AccesoriesItemList.json");
            StreamReader dataRead = new StreamReader(DataPath + "AccesoriesItemList.json");
            AccesoriesItemsDatabase = JsonConvert.DeserializeObject<List<AccesoriesItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }

        private void LoadHeadDataBase()
        {
            if (!File.Exists(DataPath + "HeadItemList.json")) GameHackFunc.Actions.Logger($"Missing file: HeadItemList.json");
            StreamReader dataRead = new StreamReader(DataPath + "HeadItemList.json");
            HeadItemsDatabase = JsonConvert.DeserializeObject<List<HeadItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }

        private void LoadBodyDataBase()
        {
            if (!File.Exists(DataPath + "BodyItemList.json")) GameHackFunc.Actions.Logger($"Missing file: BodyItemList.json");
            StreamReader dataRead = new StreamReader(DataPath + "BodyItemList.json");
            BodyItemsDatabase = JsonConvert.DeserializeObject<List<BodyItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }
        private void LoadArmDataBase()
        {
            if (!File.Exists(DataPath + "ArmItemList.json")) GameHackFunc.Actions.Logger($"Missing file: ArmItemList.json");
            StreamReader dataRead = new StreamReader(DataPath + "ArmItemList.json");
            ArmItemsDatabase = JsonConvert.DeserializeObject<List<ArmItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }
        private void LoadFootDataBase()
        {
            if (!File.Exists(DataPath + "FootItemList.json")) GameHackFunc.Actions.Logger($"Missing file: FootItemList.json");
            StreamReader dataRead = new StreamReader(DataPath + "FootItemList.json");
            FootItemsDatabase = JsonConvert.DeserializeObject<List<FootItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }
        private void LoadWeaponDataBase()
        {
            if (!File.Exists(DataPath + "WeaponItemList.json")) GameHackFunc.Actions.Logger($"Missing file: WeaponItemList.json");
            StreamReader dataRead = new StreamReader(DataPath + "WeaponItemList.json");
            WeaponItemsDatabase = JsonConvert.DeserializeObject<List<WeaponItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }
        private void LoadMonsterDataBase()
        {
            if (!File.Exists(DataPath + "MonsterList.json")) GameHackFunc.Actions.Logger($"Missing file: MonsterList.json");
            StreamReader dataRead = new StreamReader(DataPath + "MonsterList.json");
            MonsterDatabase = JsonConvert.DeserializeObject<List<MobInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }
        private void LoadSkillDataBase()
        {
            if (!File.Exists(DataPath + "SkillList.json")) GameHackFunc.Actions.Logger($"Missing file: SkillList.json");
            StreamReader dataRead = new StreamReader(DataPath + "SkillList.json");
            SkillDatabase = JsonConvert.DeserializeObject<List<SkillInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }
        private void LoadUsableItemDataBase()
        {
            if (!File.Exists(DataPath + "UseItemList.json")) GameHackFunc.Actions.Logger($"Missing file: UseItemList.json");
            StreamReader dataRead = new StreamReader(DataPath + "UseItemList.json");
            UsableItemsDatabase = JsonConvert.DeserializeObject<List<UsableItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }
        private void LoadShieldItemDataBase()
        {
            if (!File.Exists(DataPath + "ShieldItemList.json")) GameHackFunc.Actions.Logger($"Missing file: ShieldItemList.json");
            StreamReader dataRead = new StreamReader(DataPath + "ShieldItemList.json");

            SheildItemsDatabase = JsonConvert.DeserializeObject<List<ShieldItemsInfo>>(dataRead.ReadToEnd());
            dataRead.Close();
        }
    }
}
