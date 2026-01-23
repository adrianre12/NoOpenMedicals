using Sandbox.Common.ObjectBuilders;
using VRage.Game.Components;
using VRage.ObjectBuilders;

namespace NoOpenMedicals
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_MedicalRoom), false)]
    internal class MedicalRoomBlock : BaseRespawnBlock
    {
        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            base.Init(objectBuilder);
            Log.Msg("MedicalRoom Init...");
        }
    }
}