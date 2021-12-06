using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LobbyCanvas : MonoBehaviour
    {
        [SerializeField] private InputField roomIDInputField;
        [SerializeField] private Toggle publicToggle;
        
        public void OnClickRandomRoomButton()
        {
            RoomManager.instance.EnterRandomRoom();
        }

        public void OnClickCustomRoomButton()
        {
            RoomManager.instance.EnterCustomRoom(roomIDInputField.text);
        }

        public void OnClickCreateRoomButton()
        {
            RoomManager.instance.CreateRoom(publicToggle.isOn);
        }

        public void OnClickExitRoomButton()
        {
            RoomManager.instance.DisconnectRoom();
        }
    }
}