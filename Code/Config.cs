using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;


namespace NoOpenMedicals
{
    public partial class Config
    {
        internal const string configFilename = "Config-NOM.xml";
        const string VariableId = nameof(NOMconfig);


        public bool Debug = false;
        public List<string> ExcludeFactionTags = new List<string>();

        public Config()
        {
        }

        private static void LoadToVariariable()
        {
            string configContents;
            if (MyAPIGateway.Utilities.FileExistsInWorldStorage(configFilename, typeof(Config)) == true)
            {
                try
                {
                    var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage(configFilename, typeof(Config));
                    configContents = reader.ReadToEnd();
                    MyAPIGateway.Utilities.SetVariable<string>(VariableId, Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes(configContents)));

                    Log.Msg($"Loaded Existing Settings From {configFilename}");
                    return;
                }
                catch (Exception exc)
                {
                    Log.Msg(exc.ToString());
                    Log.Msg($"ERROR: Could Not Load Settings From {configFilename}. Using Empty Configuration.");
                    configContents = MyAPIGateway.Utilities.SerializeToXML<Config>(new Config());
                    MyAPIGateway.Utilities.SetVariable<string>(VariableId, Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes(configContents)));
                    return;
                }

            }

            Log.Msg($"{configFilename} Doesn't Exist. Creating Default Configuration. ");
            Config config = new Config();
            configContents = MyAPIGateway.Utilities.SerializeToXML<Config>(config);
            MyAPIGateway.Utilities.SetVariable<string>(VariableId, Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes(configContents)));

            try
            {
                using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage(configFilename, typeof(Config)))
                {
                    writer.Write(configContents);
                }
            }
            catch (Exception exc)
            {
                Log.Msg(exc.ToString());
                Log.Msg($"ERROR: Could Not Create {configFilename}. Default Settings Will Be Used.");
            }

            return;
        }

        public static Config Load()
        {
            if (MyAPIGateway.Session.IsServer)
                LoadToVariariable();

            Config config = new Config();
            string configContents;
            try
            {
                string saveText;
                if (!MyAPIGateway.Utilities.GetVariable<string>(VariableId, out saveText))
                    throw new Exception($"Variable {VariableId} not found in game save!");
                configContents = Encoding.UTF8.GetString(Convert.FromBase64String(saveText));
                config = MyAPIGateway.Utilities.SerializeFromXML<Config>(configContents);
                Log.Debug = config.Debug;
                Log.Msg($"Loaded Config Debug={config.Debug}");
                if (Log.Debug)
                {
                    Log.Msg($"Config.ExcludeFactionTags.Count={config.ExcludeFactionTags?.Count.ToString() ?? "null"}");
                }
            }
            catch (Exception e)
            {
                Log.Msg($"Error config\n {e}");
                config = new Config();
            }
            return config;
        }
    }
}
