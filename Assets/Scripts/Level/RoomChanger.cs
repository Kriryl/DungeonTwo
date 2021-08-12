using UnityEngine;
using System.Collections;

namespace DungeonEscape
{
    public class RoomChanger : MonoBehaviour
    {
        public static float roomCloseDelay = 0.344f;
        public bool active = true;
        public Vector2 desiredLocation = new Vector2();
        public bool hasPassed = false;
        public Room connectedRoom;
        public Room currentRoom;

        private Camera cam;
        private Vector2 currentPos;

        private void Start()
        {
            cam = Camera.main;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (!hasPassed) // Use connectedRoom
                {
                    currentPos = cam.transform.position;
                    CameraWarp.Create(desiredLocation, cam);
                    connectedRoom.OnRoomEnter();
                    _ = StartCoroutine(connectedRoom.CloseAllDoors(roomCloseDelay));
                    hasPassed = true;
                    desiredLocation = currentPos;
                }
                else // Use currentRoom
                {
                    currentPos = cam.transform.position;
                    CameraWarp.Create(desiredLocation, cam);
                    currentRoom.OnRoomEnter();
                    _ = StartCoroutine(currentRoom.CloseAllDoors(roomCloseDelay));
                    hasPassed = false;
                    desiredLocation = currentPos;
                }
            }
        }
    }
}
