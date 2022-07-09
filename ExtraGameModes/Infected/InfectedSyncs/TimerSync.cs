using UnityEngine;
using UnityEngine.Events;
using VTNetworking;
using VTNetworking.FlatBuffers;

namespace ExtraGameModes.Infected.InfectedSyncs
{
    public class TimerSync : VTNetSyncRPCOnly
    {
        protected override void OnNetInitialized()
        {
            if (netEntity == null)
                Debug.Log("netEntity is null");

            if (base.isMine)
            {
                Infected.InfectedTimer.AddListener(new UnityAction<bool, float, float>(InfectedTimer));
            }
        }

        private void InfectedTimer(bool running, float timer, float baseTimer)
        {
            if (base.isMine)
            {
                SendRPC("RPC_InfectedTimerSync", new object[] { running, timer, baseTimer });
            }
        }

        [VTRPC]
        public void RPC_InfectedTimerSync(bool running, float timer, float baseTimer)
        {
            if (running)
            {
                
                Infected.InfectTimer = timer;
                
            }
            else if (timer == baseTimer)
            {
                Infected.Running = false;
                Infected.InfectTimer = baseTimer;
            }
        }
    }
}