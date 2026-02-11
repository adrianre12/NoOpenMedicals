using System;
using VRage.Game.Components;

namespace NoOpenMedicals
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    class NOMconfig : MySessionComponentBase
    {
        public static NOMconfig Instance;

        private Config config;

        public string DefaultCustomData;

        public override void LoadData()
        {
            Instance = this;
            config = Config.Load();
        }

        protected override void UnloadData()
        {
            try
            {
                Instance = null;
            }
            catch (Exception e)
            {
                Log.Msg($"Error in UnloadData\n{e.ToString()}");
            }
        }

        public bool ExcludeFactionTag(string tag)
        {
            return config.ExcludeFactionTags.Contains(tag);
        }
    }
}
