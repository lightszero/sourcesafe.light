using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace corelib
{
    public class ServerConfig
    {
        private ServerConfig()
        {

        }
        public string superadmin = "";
        public string basehost = "ss.light";
        public class Artist
        {
            public string name = "";
            public string code = "";
            public int level = 1;
        }
        public List<Artist> users = new List<Artist>();

        public class Game
        {
            public string name = "";
            public string desc = "";
            public string admin = "";
            public List<string> users = new List<string>();
        }
        public List<Game> games = new List<Game>();

        public Artist getArtist(string name)
        {
            foreach(var a in users)
            {
                if (name == a.name)
                    return a;
            }
            return null;
        }
        public Game getGame(string name)
        {
            foreach(var g in games)
            {
                if(g.name==name)
                {
                    return g;
                }
            }
            return null;
        }
        void Parse()
        {

        }
        public static ServerConfig Load()
        {
            ServerConfig config = null;
            try
            {
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(typeof(ServerConfig));
                if (System.IO.File.Exists("config.xml"))
                {
                    using (System.IO.Stream s = System.IO.File.OpenRead("config.xml"))
                    {
                        config = xmls.Deserialize(s) as ServerConfig;
                    }
                }
            }
            catch
            {
                config = new ServerConfig();
            }
            if (config == null)
            {
                config = new ServerConfig();
            }
            config.Parse();
            return config;
        }
        public void Save()
        {
            System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(typeof(ServerConfig));

            using (System.IO.Stream s = System.IO.File.Create("config.xml"))
            {
                xmls.Serialize(s, this);
            }

        }
    }

}
