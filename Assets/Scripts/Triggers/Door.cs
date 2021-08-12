using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonEscape.Trigger
{
    /// <summary>
	/// Script for controlling door states.
	/// </summary>
    public class Door : MonoBehaviour
    {
        public GameObject doorOpen;

        public void Open()
        {
            doorOpen.SetActive(false);
        }

        public void Close()
        {
            doorOpen.SetActive(true);
        }
    }
}
