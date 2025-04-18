﻿using CorpseLib;
using CorpseLib.DataNotation;

namespace OBSPlugin
{
    public class Settings
    {
        public class DataSerializer : ADataSerializer<Settings>
        {
            protected override OperationResult<Settings> Deserialize(DataObject reader)
            {
                Settings settings = new()
                {
                    m_Password = reader.GetOrDefault("password", string.Empty),
                    m_Host = reader.GetOrDefault("host", string.Empty),
                    m_Port = reader.GetOrDefault("port", 4455)
                };
                settings.m_Radios.AddRange(reader.GetList<RadioSet>("radios"));
                settings.m_SourcesToRefresh.AddRange(reader.GetList<string>("sources_to_refresh"));
                return new(settings);
            }

            protected override void Serialize(Settings obj, DataObject writer)
            {
                writer["password"] = obj.m_Password;
                writer["host"] = obj.m_Host;
                writer["port"] = obj.m_Port;
                writer["radios"] = obj.m_Radios;
                writer["sources_to_refresh"] = obj.m_SourcesToRefresh;
            }
        }

        private readonly List<RadioSet> m_Radios = [];
        private readonly List<string> m_SourcesToRefresh = [];
        private string m_Password = string.Empty;
        private string m_Host = "localhost";
        private int m_Port = 4455;

        public RadioSet[] Radios => [..m_Radios];
        public string[] SourcesToRefresh => [.. m_SourcesToRefresh];
        public string Password => m_Password;
        public string Host => m_Host;
        public int Port => m_Port;

        public void SetPassword(string password) => m_Password = password;
        public void SetHost(string host) => m_Host = host;
        public void SetPort(int port) => m_Port = port;
    }
}
