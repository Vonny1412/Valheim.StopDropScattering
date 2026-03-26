using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Jotunn.Utils;
using System.Reflection;

namespace StopDropScattering
{
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    public sealed partial class Plugin : BaseUnityPlugin
    {

        internal static ManualLogSource Log { get; private set; }

        private void Awake()
        {
            Log = this.Logger;
            Plugin.Configs.Initialize(Config);
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }

    }
}
