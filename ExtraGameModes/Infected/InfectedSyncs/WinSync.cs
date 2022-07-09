using UnityEngine;
using UnityEngine.Events;
using VTNetworking;
using VTOLVR.Multiplayer;

namespace ExtraGameModes.Infected.InfectedSyncs
{
    public class WinSync : VTNetSyncRPCOnly
    {
        protected override void OnNetInitialized()
        {
            if (netEntity == null)
                Debug.Log("netEntity is null");

            if (base.isMine)
            {
                Infected.TeamWon.AddListener(new UnityAction<int>(InfectedTeamWon));
            }
        }

        private void InfectedTeamWon(int team)
        {
            if (base.isMine)
            {
                SendRPC("RPC_InfectedTeamWon", new object[team]);
            }
        }

        [VTRPC]
        public void RPC_InfectedTeamWon(int team)
        {
            if (team == 0)
            {
                Infected.AlliedWin();
            }

            if (team == 1)
            {
                Infected.EnemyWin();
            }
        }
    }
}