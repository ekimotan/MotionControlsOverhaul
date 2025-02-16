using HarmonyLib;
using SandBox.Missions.MissionLogics;
using SandBox.View.Map;
using System.Configuration;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.MountAndBlade.GauntletUI.Mission;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.GameKeys;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.FormationMarker;
using TaleWorlds.MountAndBlade.View.Screens;
using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Base;
using MCM.Abstractions.Base.Global;
using System.Xml.Serialization;
using MotionControlsOverhaul.Config;


namespace MotionControlsOverhaul{
   

    public class SubModule : MBSubModuleBase
    {

        public static readonly string ModuleFolderName = "MotionControlsOverhaul";
        public static readonly string ModName = "MotionControlsOverhaul";

        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
           
                 

        }
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            Harmony harmony = new Harmony("MotionController");
            harmony.PatchAll();
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

        }

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);
            mission.AddMissionBehavior(new MotionControllerBehavior());
        }


    }
    
    

    public class MotionControllerBehavior : MissionLogic
    {
        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);
        

            
            //AddGyroToLookDirection();


        }

        /*private void AddGyroToLookDirection()
        {
            
            
            if(Input.IsKeyPressed(TaleWorlds.InputSystem.InputKey.Q))
            {
                InformationManager.DisplayMessage(new InformationMessage("haha"));
            }
            

            if (TaleWorlds.InputSystem.Input.ControllerType == TaleWorlds.InputSystem.Input.ControllerTypes.PlayStationDualShock || TaleWorlds.InputSystem.Input.ControllerType == TaleWorlds.InputSystem.Input.ControllerTypes.PlayStationDualSense)
            {
                
                float axisX = TaleWorlds.InputSystem.Input.GetGyroY() * 12f * -1f;
                float axisY = TaleWorlds.InputSystem.Input.GetGyroX() * 12f;
                




                if (Agent.Main != null)
                {
                    //CameraBearing
                    //GameAxisKey

                    //Mission.MainAgent.AddController()
                    //Agent.Main.LookDirection.RotateAboutZ;
                    //Agent.Main.LookDirection.RotationX + gyroZ;
                    //Agent.Main.LookRotation.RotateAboutSide();
                }

            }
         
        }
        */
    }
}