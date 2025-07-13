using HarmonyLib;
using PsychoticLab;
using UnityEngine;

namespace PlayerChar
{
    internal static class PlayerCharPatch
    {
        public static GameObject avatar;
        static Vector3 charOffset = new Vector3(0f, -0.55f, 0f);
        static bool ran = false;

        public static void AddBread()
        {
            Transform player = Refs.observerMirror.transform;

            var bread = new GameObject("bread_avatar");
            var rend = bread.AddComponent<MeshRenderer>();
            var filt = bread.AddComponent<MeshFilter>();
            filt.mesh = PrefabsDirectory.instance.directory[43].GetComponent<MeshFilter>().mesh;
            rend.material = PrefabsDirectory.instance.directory[43].GetComponent<MeshRenderer>().material;

            bread.transform.parent = Refs.observerMirror.transform;
            bread.transform.localPosition = new Vector3(0, 0.3f, -0.2f);
            bread.transform.localScale = new Vector3(2.5f, 2.25f, 2.25f);
            bread.transform.localEulerAngles = new Vector3(0, 90, 90);
            avatar = bread;
            BreadToggler controller = player.GetComponent<BreadToggler>() ?? player.gameObject.AddComponent<BreadToggler>();
            controller.RegisterAvatar(avatar.gameObject);
        }
        internal static void AddChar()
        {
            int index = Plugin.avatar.Value;
            Transform player = Refs.observerMirror.transform;
            PlayerAvatarController controller = player.GetComponent<PlayerAvatarController>() ?? player.gameObject.AddComponent<PlayerAvatarController>();
            if (avatar != null)
            {
                GameObject.Destroy(avatar);
            }
            if (index < 0 || index >= Port.ports.Length || Port.ports[index] == null)
            {
                controller.enabled = false;
                Debug.Log("PlayerPatch: port is null");
                return;
            }
            GameObject thing = Port.ports[Plugin.avatar.Value].GetDude().GetComponentInChildren<CharacterCustomizer>().gameObject;//GameObject.FindObjectOfType<CharacterCustomizer>().gameObject;

            avatar = UnityEngine.Object.Instantiate(thing, player.transform, false).gameObject;
            avatar.transform.localPosition = charOffset;
            avatar.transform.localRotation = Quaternion.identity;
            avatar.transform.localScale = Vector3.one;

            controller.enabled = true;
            controller.RegisterAvatar(avatar.gameObject);
        }

        [HarmonyPatch(typeof(BoatCamera), "SwitchOn")]
        internal static class BoatCameraPatch
        {
            public static void Postfix()
            {
                if (!ran)
                {
#if BREAD
                    AddBread();
#else
                    PlayerCharPatch.AddChar();
#endif
                }
                ran = true;
            }
        }
    }
}
