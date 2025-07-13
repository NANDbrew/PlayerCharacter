using UnityEngine;

namespace PlayerChar
{
    public class CharCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform centerEye;

        [SerializeField]
        private Transform mouseGoPointer;

        [SerializeField]
        private GameObject crosshair;

        [SerializeField]
        private float zoomSpeed = 10f;

        [SerializeField]
        private float camHeight = 2f;

        [SerializeField]
        private MouseLook[] playerLooks;

        [SerializeField]
        private MouseLook[] boatLooks;

        private Transform initialCamParent;

        public static CharCamera instance;

        public static bool on;

        private float zoomLevel;

        private Vector3 currentPosOffset;

        private float initialLineIntensity;

        private float initialFillAmount;

        private void Start()
        {
            initialCamParent = centerEye.parent;
            instance = this;
        }

        public void SwitchOn()
        {
            on = true;
            mouseGoPointer.transform.parent = Refs.observerMirror.transform;
            MouseLook[] array = playerLooks;
            for (int i = 0; i < array.Length; i++)
            {
                array[i].enabled = false;
            }

            array = boatLooks;
            for (int i = 0; i < array.Length; i++)
            {
                array[i].enabled = true;
            }

            centerEye.parent = base.transform.GetChild(0);
            centerEye.localRotation = Quaternion.identity;
            UISoundPlayer.instance.PlayUISound(UISounds.buttonClick, 1f, 1.2f);
            crosshair.SetActive(value: false);
            currentPosOffset = Vector3.zero;
        }


        public void SwitchOff()
        {
            on = false;
            MouseLook[] array = playerLooks;
            for (int i = 0; i < array.Length; i++)
            {
                array[i].enabled = true;
            }

            array = boatLooks;
            for (int i = 0; i < array.Length; i++)
            {
                array[i].enabled = false;
            }

            centerEye.parent = initialCamParent;
            centerEye.localPosition = Vector3.zero;
            centerEye.localRotation = Quaternion.identity;
            UISoundPlayer.instance.PlayUISound(UISounds.buttonClick, 1f, 1.4f);
            crosshair.SetActive(value: true);
            mouseGoPointer.transform.parent = centerEye;
            mouseGoPointer.transform.localPosition = Vector3.zero;
            mouseGoPointer.transform.localRotation = Quaternion.identity;
        }

        private void Update()
        {
            if (GameInput.GetKeyDown(InputName.CameraMode) && !GameState.currentShipyard)
            {
                if (on)
                {
                    SwitchOff();
                }
                else
                {
                    SwitchOn();
                }
            }

            if (!on)
            {
                return;
            }

            if (GameState.currentBoat != null)
            {
                SwitchOff();
            }
            else
            {
                base.transform.position = GameState.currentBoat.position + Vector3.up * 0f + currentPosOffset;
                UpdateZoom();
                centerEye.localPosition = new Vector3(0f, camHeight, zoomLevel);
            }
        }

        private void UpdateZoom()
        {
            zoomLevel += GameInput.GetScrollAxis() * zoomSpeed;
            if (zoomLevel > -8f)
            {
                zoomLevel = -8f;
            }

            if (zoomLevel < -40f)
            {
                zoomLevel = -40f;
            }
        }
    }
}
