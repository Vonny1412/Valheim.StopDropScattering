using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace StopDropScattering.Patches
{
    [HarmonyPatch(typeof(MineRock5), "DamageArea")]
    internal static class MineRock5_DamageArea_Patch
    {

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo insideUnitSphere = AccessTools.PropertyGetter(typeof(UnityEngine.Random), nameof(UnityEngine.Random.insideUnitSphere));
            MethodInfo vectorMultiply = AccessTools.Method(typeof(Vector3), "op_Multiply", new[] { typeof(Vector3), typeof(float) });
            MethodInfo vectorAddition = AccessTools.Method(typeof(Vector3), "op_Addition", new[] { typeof(Vector3), typeof(Vector3) });
            MethodInfo getDropPosition = AccessTools.Method(typeof(MineRock5_DamageArea_Patch), nameof(GetDropPosition));

            var matcher = new CodeMatcher(instructions);

            matcher.MatchForward(
                false,
                new CodeMatch(ci => ci.Calls(insideUnitSphere)),
                new CodeMatch(ci => ci.opcode == OpCodes.Ldc_R4 && ci.operand is float value && Math.Abs(value - 0.3f) < 0.00001f),
                new CodeMatch(ci => ci.Calls(vectorMultiply)),
                new CodeMatch(ci => ci.Calls(vectorAddition))
            );

            if (!matcher.IsValid)
            {
                throw new InvalidOperationException("MineRock5.DamageArea: Drop position calculation not found.");
            }

            matcher.Advance(4);
            matcher.Insert(
                new CodeInstruction(OpCodes.Ldarg_2),
                new CodeInstruction(OpCodes.Call, getDropPosition)
            );
            return matcher.InstructionEnumeration();
        }

        private static Vector3 GetDropPosition(Vector3 position, HitData hit)
        {
            if (position.y <= 4500f)
            {
                return position;
            }

            Player player = hit.GetAttacker() as Player;
            if (player == null)
            {
                Plugin.Log.LogWarning($"No player found to catch drop at {position}");
                return position;
            }

            Plugin.Log.LogInfo($"{player.GetPlayerName()} catches a drop");
            return player.transform.position + (Vector3.up * 0.5f);
        }

    }
}
