namespace Bot_Menu.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SkillsListsModel
    {
      public List<Skill> UnUsedSkillList { get; set; }= new List<Skill>();
        public List<Skill> SkillInUseList { get; set; } = new List<Skill>();
    }
}
