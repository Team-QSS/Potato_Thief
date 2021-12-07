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
            Debug.Log(roomIDInputField.text.Length);
            if (roomIDInputField.text.Length != 6)
            {
                // ToDo : 에러 메세지 UI로 표시
                return;
            }
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