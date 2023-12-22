using UnityEngine;

namespace Lib.Network
{
    /// <summary>
    /// Tracks the players connected to a game instance.
    /// The game instance is imaginary; that is, there is no server running things.
    /// </summary>
    public class Lobby : MonoBehaviour
    {
        /// <summary>
        /// Player 1 is always the host.
        /// </summary>
        internal long player1 = -1, player2 = -1;

        public void Init()
        {
        }
    }
}