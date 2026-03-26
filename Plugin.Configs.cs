using BepInEx.Configuration;

namespace StopDropScattering
{
    public sealed partial class Plugin
    {
        public class Configs
        {
            private const string Section_General = "General";

            public static void Initialize(ConfigFile Config)
            {
                // no config right now
            }
        }
    }
}
