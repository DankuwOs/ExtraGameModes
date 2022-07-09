using UnityEngine;
using UnityEngine.Events;
using VTNetworking;
using VTOLVR.Multiplayer;

namespace ExtraGameModes.Infected.InfectedSyncs
{
    public class InfectPlayerSync : VTNetSyncRPCOnly
    {
        protected override void OnNetInitialized()
        {
            if (netEntity == null)
                Debug.Log("netEntity is null");

            if (base.isMine)
            {
                Infected.FirstInfectedPlayer.AddListener(new UnityAction<ulong>(InfectPlayer));
            }
        }

        private void InfectPlayer(ulong player)
        {
            if (base.isMine)
            {
                if (player == VTOLMPSceneManager.instance.localPlayer.steamUser.Id)
                    InfectHost();
                else
                    SendDirectedRPC(player, "RPC_InfectPlayer");
                
                if (!Infected.Running)
                    Infected.StartCountdown();
            }
        }

        [VTRPC]
        public void RPC_InfectPlayer()
        {
            if (!Infected.Running)
                Infected.StartCountdown();
            
            Infected.ToEnemyTeamChange();
            Infected.EnemySpawnObjective();
        }
        
        public void InfectHost()
        {
            if (!Infected.Running)
                Infected.StartCountdown();
            
            Infected.EnemySpawnObjective();
            Infected.ToEnemyTeamChange();
            
        }
    }
}