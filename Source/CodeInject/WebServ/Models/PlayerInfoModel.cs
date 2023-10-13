
namespace CodeInject.WebServ.Models
{
    public class PlayerInfoModel
    {
        public int Hp = 0;
        public int MaxHp = 0;
        public int Mp = 0;
        public int MaxMp = 0;
        public float X = 0;
        public float Y = 0;
        public float Z = 0;
        public short BuffCount = 0;
        public string Name;

        public string getPrecMP()
        {
            return $"{((float)Mp) * 100.0f / ((float)MaxMp)}".Replace(",", ".");
        }
        public string getPrecHP()
        {
            return $"{((float)Hp) * 100.0f / ((float)MaxHp)}".Replace(",", ".");
        }
    }
}
