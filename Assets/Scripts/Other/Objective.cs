using System;

namespace DungeonEscape.PlayerController
{
    [Serializable]
    public class Objective
    {
        private readonly string message = "none";
        public string Message => message;

        public Objective(string message)
        {
            this.message = message;
        }
    }
}
