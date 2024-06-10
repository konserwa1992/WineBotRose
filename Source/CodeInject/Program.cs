using CodeInject;
using CodeInject.Actors;
using CodeInject.MemoryTools;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TEST;

namespace ISpace
{

    public class IClass
    {
        public unsafe static int IMain(string args)
        {
            cBot cBot = new cBot();
             cBot.ShowDialog();
            /* AllocConsole();
           

            var skills = new DataFetcher().GetNPCs();

            Console.WriteLine($"player name:{(skills[0] as Player).Name}");


            foreach (var skill in skills)
            {
                Console.WriteLine($"{skill.ID} {skill.ObjectPointer.ToString("X")}");
            }
            
            
          

            new GameActions().MoveToPoint(new System.Numerics.Vector2(5068,4437));

           /* skills = GameHackFunc.Game.ClientData.GetPlayerSkills();

            foreach (var skill in skills)
            {
                Console.WriteLine($"{skill}");
            }*/
            // Console.WriteLine(MemoryTools.GetVariableAddres("83 f8 07 0f 8f ?? ?? ?? ?? 48 63 0f 48 8b 05 ?? ?? ?? ??").ToInt64().ToString("X"));

            return 0;
        }

        [DllImport("kernel32")]
        static extern bool AllocConsole();
    }
}
