using HarmonyLib;
using UnityEngine;

namespace PlayerChar
{
    internal class PlayerAvatarController : MonoBehaviour
    {
        Transform neck;
        Transform camera;
        Transform head;
        Quaternion headInitialRot;
        float headLookAngle = 70;
        float min = 0;
        float max = 45;
        Transform avatar;
        GoPointer pointer;
        Traverse clickedButton;

        private void Start()
        {
            camera = avatar.transform.parent.Find("OVRCameraRig");
            pointer = GetComponentInChildren<GoPointer>(false);
            clickedButton = Traverse.Create(pointer).Field("stickyClickedButton");
        }

        public void RegisterAvatar(GameObject newAvatar)
        {
            var anim = newAvatar.GetComponent<NPCAnimations>();
            avatar = anim.transform;
            head = anim.head;//neck.GetChild(0); headInitialRot = head.localRotation;
            neck = head.parent;//avatar.transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Neck");
            anim.enabled = false;
            if (!GameState.playing || !BoatCamera.on)
            {
                avatar.gameObject.SetActive(false);
            }
        }

        public void Update()
        {
            if (!avatar) return;

            if (BoatCamera.on && GameState.playing)
            {
                avatar.gameObject.SetActive(true);
            }
            else
            {
                avatar.gameObject.SetActive(false);
            }

            if (clickedButton.GetValue() is GoPointerButton target)
            {
                //avatar.rotation = Quaternion.LookRotation(target.transform.position - avatar.position, Vector3.up);
                avatar.LookAt(new Vector3(target.transform.position.x, avatar.position.y, target.transform.position.z));
            }
            else
            {
                avatar.localRotation = Quaternion.identity;
            }
            if (camera.localEulerAngles.x / 2 < max)
            {
                neck.transform.localEulerAngles = new Vector3(0, 0, Mathf.Clamp(camera.localEulerAngles.x / 2, 0, max));

            }
            else if (camera.localEulerAngles.x > min)
            {
                neck.transform.localEulerAngles = new Vector3(0, 0, Mathf.Clamp(camera.localEulerAngles.x + ((360 - camera.localEulerAngles.x) / 2), 315, 359.99f));
            }
            else
            {
                neck.transform.localEulerAngles = Vector3.zero;
            }
            /*Quaternion b = Quaternion.LookRotation(base.transform.position - camera.forward + camera.position, Vector3.up);
            float c = Quaternion.Angle(base.transform.rotation, b);
            if (c > 180f - headLookAngle)
            {
                //head.rotation = Quaternion.Slerp(head.rotation, Quaternion.LookRotation(camera.forward + camera.position - head.position, Vector3.up), Time.deltaTime * 3f);
                head.rotation = Quaternion.LookRotation(camera.forward + camera.position - head.position, Vector3.up);
            }
            else
            {
                head.localRotation = Quaternion.Slerp(head.localRotation, headInitialRot, 180 / c);
            }*/
            head.rotation = Quaternion.LookRotation(camera.forward + camera.position - head.position, Vector3.up);

        }
        float ClosestIfBetween(float val, float low, float high)
        {
            if (val > low && val < high)
            {
                float mid = (high - low) / 2 + low;
                return val < mid ? low : high;
            }
            return val;
        }
    }
}
