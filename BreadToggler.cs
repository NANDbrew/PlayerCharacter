using UnityEngine;

namespace PlayerChar
{
    internal class BreadToggler : MonoBehaviour
    {
        public GameObject avatar;

        public void RegisterAvatar(GameObject newAvatar)
        {
            avatar = newAvatar;
            if (!GameState.playing || !BoatCamera.on)
            {
                avatar.gameObject.SetActive(false);
            }
        }

        public void Update()
        {
            if (BoatCamera.on && GameState.playing)
            {
                avatar.gameObject.SetActive(true);
            }
            else
            {
                avatar.gameObject.SetActive(false);
            }
        }
    }
}
