using HarmonyLib;
using MotionControlsOverhaul.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade.View.Screens;

namespace MotionControlsOverhaul.Behaviours
{
    [HarmonyPatch(typeof(MissionScreen))]
    [HarmonyPatch("HandleUserInput")]
    internal class GyroPatch
    {
        public static float gyroSensitivity;
        public static bool touchDisable;
        public static bool enableDeadzone;
        public static float gyroDeadzone;
        
        
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            //if ((Int32)TaleWorlds.InputSystem.Input.ControllerType < 2) yield break;
            var found = false;
            foreach (var instruction in instructions)
            {

                
                yield return instruction;
                if (instruction.opcode == OpCodes.Stloc_S && instruction.operand is LocalBuilder { LocalIndex: 9})
                {   
                    found = true;
                }
                if (found == true && instruction.opcode == OpCodes.Ldloc_S && instruction.operand is LocalBuilder { LocalIndex: 5 })
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GyroPatch), nameof(GyroPatch.GyroXToBeAdded)));
                    yield return new CodeInstruction(OpCodes.Add, null);
                }
                if (found == true && instruction.opcode == OpCodes.Ldloc_S && instruction.operand is LocalBuilder { LocalIndex: 6 })
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GyroPatch), nameof(GyroPatch.GyroYToBeAdded)));
                    yield return new CodeInstruction(OpCodes.Add, null);
                }
                
            }
            if (found is false)
                throw new ArgumentException("Cannot find <Stdfld someField> in MissionScreen.HandleUserInput");
        }

        static float GyroXToBeAdded()
        {
            if (touchDisable && TaleWorlds.InputSystem.Input.IsAnyTouchActive) return 0f;
            float gyroX = TaleWorlds.InputSystem.Input.GetGyroY() * 12f * -1f * gyroSensitivity;
            return enableDeadzone && Math.Abs(gyroX) < gyroDeadzone ? 0f : gyroX;
        }

        static float GyroYToBeAdded()
        {
            if (touchDisable && TaleWorlds.InputSystem.Input.IsAnyTouchActive) return 0f;
            float gyroY = TaleWorlds.InputSystem.Input.GetGyroX() * 12f * gyroSensitivity * -1f;
            return enableDeadzone && Math.Abs(gyroY) < gyroDeadzone ? 0f : gyroY;
        }
    }
}
