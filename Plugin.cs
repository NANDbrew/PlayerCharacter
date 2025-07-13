using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.Reflection;

namespace PlayerChar
{
    [BepInPlugin(PLUGIN_ID, PLUGIN_NAME, PLUGIN_VERSION)]
    //[BepInDependency("com.app24.sailwindmoddinghelper", "2.0.3")]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_ID = "com.nandbrew.playerchar";
#if BREAD
        public const string PLUGIN_NAME = "Player Character: Bread Edition";
#else
        public const string PLUGIN_NAME = "Player Character";
#endif
        public const string PLUGIN_VERSION = "0.2.0";

        //--settings--
        internal static ConfigEntry<int> avatar;
        internal static ConfigEntry<bool> perSave;

        private void Awake()
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_ID);
#if !BREAD
            avatar = Config.Bind("Stuff", "player character", 2, new ConfigDescription("Port index from which to copy character. Set to -1 to disable"));
            perSave = Config.Bind("Stuff", "per-save character", false, new ConfigDescription("save character selection in the save file"));
            avatar.SettingChanged += (sender, args) => PlayerCharPatch.AddChar();
#endif
        }
    }
}
