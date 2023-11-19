using CodeInject.Actors;
using CodeInject.BotStates;
using CodeInject.BotStates.States;
using CodeInject.Hunt;
using CodeInject.MemoryTools;
using CodeInject.Modules;
using CodeInject.PickupFilters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace CodeInject
{
    public class BotContext: ModuleConteiner
    {
        public IBotState CurrentBotState { get; set; }
        public Dictionary<string, IBotState> States { private set; get; } = new Dictionary<string, IBotState>();
        public IFilter Filter = new QuickFilter();



        public List<IObject> GetItemsNearby()
        {
            IObject player = GameFunctionsAndObjects.DataFetch.GetPlayer();
            return GameFunctionsAndObjects.DataFetch.GetItemsAroundPlayerV2().Where(x => Filter.CanPickup(x) && x.CalcDistance(player) < 20).OrderBy(x => x.CalcDistance(player)).ToList();
        }


        public BotContext()
        {
            StandbyState standBy = new StandbyState();
            States.Add("STANDBY", standBy);
            States.Add("PICK", new PickUpState());
            States.Add("HUNT",new HuntState(new DefaultHunt()));
            SetState("STANDBY");
        }

        public void Start(HuntState huntState)
        {
            ReplaceState("HUNT", huntState);
            SetState("HUNT");
        }

        public void Stop()
        {
            SetState("STANDBY");
        }
        public void SetState(string stateName)
        {
            if (States.ContainsKey(stateName) && CurrentBotState != States[stateName])
            {
                CurrentBotState = States[stateName];
                GameFunctionsAndObjects.Actions.Logger($"Change state: {stateName}",Color.Azure);
            }
        }
        public void Update()
        {
            CurrentBotState.Work(this);
            base.ModuleExecute();
        }
 
        public void AddState(string key,IBotState state)
        {
            if (!States.ContainsKey(key))
            {
                States.Add(key, state);
            }
        }
        public void ReplaceState(string key, IBotState newState)
        {
            IBotState existState;
            if (States.TryGetValue(key, out existState))
            {
                States.Remove(key);
                States.Add(key, newState);
            }
            else
            {
                AddState(key, newState);
            }
        }

        public T GetState<T>(string key)
        {
            return (T)States[key];
        }
    }
}
