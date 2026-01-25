using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game.Components;
using VRage.Game.ModAPI.Network;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Sync;

namespace NoOpenMedicals
{
    internal class BaseRespawnBlock : MyGameLogicComponent
    {
        protected MyCubeBlock block;

        private MySync<bool, SyncDirection.BothWays> triggerChange;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            block = Entity as MyCubeBlock;
            triggerChange.SetLocalValue(false);

            NeedsUpdate = MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }

        public override void UpdateOnceBeforeFrame()
        {
            if (block?.CubeGrid?.Physics == null) // ignore projected and other non-physical grids
                return;
            if (MyAPIGateway.Session.IsServer)
            {
                triggerChange.ValueChanged += TriggerChange_ValueChanged;
            }
            if (!MyAPIGateway.Utilities.IsDedicated)
            {
                NeedsUpdate = MyEntityUpdateEnum.EACH_100TH_FRAME;
            }
        }

        private void TriggerChange_ValueChanged(MySync<bool, SyncDirection.BothWays> obj)
        {
            Log.Msg($"Changing ShareMode to Faction on '{block.CubeGrid?.DisplayName}'");
            block.ChangeBlockOwnerRequest(block.OwnerId, VRage.Game.MyOwnershipShareModeEnum.Faction);
        }

        public override void UpdateAfterSimulation100() // only on client
        {
            if (block.IDModule?.ShareMode == VRage.Game.MyOwnershipShareModeEnum.All) // no ownersip probably a refil station
            {
                Log.Msg("Found ShareMode=All");
                triggerChange.Value = !triggerChange.Value;
            }
        }

        public override void Close()
        {
            base.Close();
            if (MyAPIGateway.Session.IsServer)
            {
                triggerChange.ValueChanged -= TriggerChange_ValueChanged;
            }
        }
    }
}