using HarmonyLib;
using MotionControlsOverhaul.Config;
using System.Runtime.InteropServices;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.Screens;




namespace MotionControlsOverhaul
{


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
        
        
        


        

        private Agent _mainAgent => Mission.MainAgent;
        private GameKeyContext _gameAxisKey = HotKeyManager.GetCategory("Generic");
        private GameKeyContext _keyContextGeneric = HotKeyManager.GetCategory("Generic");

        private GameKeyContext _keyContext = HotKeyManager.GetCategory("CombatHotKeyCategory");
        private static bool _rStickBinded = true;
        private static InputKey _controllerRStick;
        private IInputContext _inputContext => Mission.InputManager;


        public override void EarlyStart()
        {
            base.EarlyStart();
            _keyContext.GetGameKey(25).ControllerKey.ChangeKey(InputKey.Invalid);
        }
        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);

            AlternateFighting();
            
            //AddGyroToLookDirection();

            
        }
        public override void OnMissionStateFinalized()
        {
            base.OnMissionStateFinalized();
            BindCameraRStick();
        }
      
        public void UnbindCameraRStick()
        {
            //_keyContext.GetGameKey(9).ControllerKey.ChangeKey(InputKey.Invalid); //unbind Attack
            //_keyContext.GetGameKey(10).ControllerKey.ChangeKey(InputKey.Invalid); //unbind Defend
           
            foreach (var key in  _gameAxisKey.RegisteredGameAxisKeys)
            {
                if(key.AxisKey.IsControllerInput && ((key.Id == "CameraAxisX") || (key.Id == "CameraAxisY")))
                {
                    _controllerRStick = key.AxisKey.InputKey;

                    key.AxisKey.ChangeKey(InputKey.Invalid);
                }
                    
            }
            
            _rStickBinded = false;
        }

        public void BindCameraRStick()
        {
            
            //_keyContext.GetGameKey(9).ControllerKey.ChangeKey(InputKey.Invalid); //bind Attack
            //_keyContext.GetGameKey(10).ControllerKey.ChangeKey(InputKey.Invalid); //bind Defend

            
            foreach (var key in _gameAxisKey.RegisteredGameAxisKeys)
            {
                if (((key.Id == "CameraAxisX") || (key.Id == "CameraAxisY")) && (key.AxisKey.InputKey == InputKey.Invalid)) key.AxisKey.ChangeKey(_controllerRStick);
            }
            _rStickBinded = true;
            
        }

        public void AlternateFighting()
        {
            if (!MCOSettings.Instance.AlternateFighting) 
            { 
                if (!_rStickBinded) BindCameraRStick();
                return; 
            }
            if (_rStickBinded) UnbindCameraRStick();


            if (Input.IsKeyDown(InputKey.ControllerLLeft))
            {
               
            }
            if (Input.IsKeyReleased(InputKey.ControllerLLeft))
            {

                

            }



            if (Input.GetKeyState(InputKey.ControllerRTrigger).x>0.99f)
            {
                
            }
            if (Input.IsKeyReleased(InputKey.ControllerRTrigger))
            {

                

            }
            //if (_inputContext.GetKeyState(InputKey.ControllerRTrigger).x > 0.2f)
            //{
            //    InformationManager.DisplayMessage(new InformationMessage("Rtrigger"));
            //    switch (Input.GetFirstKeyDownInRange(236))
            //    {

            //        case 236:
            //            _mainAgent.MovementFlags |= Agent.MovementControlFlag.AttackUp; 
            //            break;
            //        case 237:
            //            _mainAgent.MovementFlags |= Agent.MovementControlFlag.AttackDown;
            //            break;
            //        case 238:
            //            _mainAgent.MovementFlags |= Agent.MovementControlFlag.AttackLeft;

            //            break;
            //        case 239:
            //            _mainAgent.MovementFlags |= Agent.MovementControlFlag.AttackRight;
            //            break;
            //    }
            //}

            //if (_inputContext.GetKeyState(InputKey.ControllerLTrigger).x > 0.2f)
            //{
            //    switch (Input.GetFirstKeyDownInRange(236))
            //    {

            //        case 236:
            //            _mainAgent.MovementFlags |= Agent.MovementControlFlag.DefendUp;
            //            break;
            //        case 237:
            //            _mainAgent.MovementFlags |= Agent.MovementControlFlag.DefendDown;
            //            break;
            //        case 238:
            //            _mainAgent.MovementFlags |= Agent.MovementControlFlag.DefendLeft;
            //            break;
            //        case 239:
            //            _mainAgent.MovementFlags |= Agent.MovementControlFlag.DefendRight;
            //            break;
            //    }
            //    Game.Current?.EventManager.TriggerEvent(new MissionPlayerMovementFlagsChangeEvent(_mainAgent.MovementFlags));
            //}


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