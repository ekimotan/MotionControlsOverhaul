using HarmonyLib;

using TaleWorlds.Core;

using TaleWorlds.MountAndBlade;



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