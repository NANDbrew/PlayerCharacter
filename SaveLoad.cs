using HarmonyLib;
using System.Collections.Generic;

namespace PlayerChar
{
#if !BREAD
    [HarmonyPatch(typeof(SaveLoadManager))]
    internal static class SaveLoadPatches
    {
        [HarmonyPatch("SaveModData")]
        [HarmonyPostfix]
        internal static void SavePatch()
        {
            if (Plugin.perSave.Value)
            {
                KeyValuePair<string, string> entry = new KeyValuePair<string, string>(Plugin.PLUGIN_ID, Plugin.avatar.Value.ToString());
                if (GameState.modData.ContainsKey(entry.Key)) GameState.modData[entry.Key] = entry.Value;
                else GameState.modData.Add(entry.Key, entry.Value);
            }
        }
        [HarmonyPatch("LoadModData")]
        [HarmonyPostfix]
        internal static void LoadPatch()
        {
            if (Plugin.perSave.Value && GameState.modData.TryGetValue(Plugin.PLUGIN_ID, out string value))
            {
                Plugin.avatar.Value = int.Parse(value);
            }
        }
    }
#endif
}
