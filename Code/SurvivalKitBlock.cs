using Sandbox.Common.ObjectBuilders;
using VRage.Game.Components;
using VRage.ObjectBuilders;

namespace NoOpenMedicals
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_SurvivalKit), false)]
    internal class SurvivalKitBlock : BaseRespawnBlock
    {
        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            base.Init(objectBuilder);
            Log.Msg("SurvivalKit Init...");
        }
    }
}