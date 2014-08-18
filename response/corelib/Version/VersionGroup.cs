using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace corelib
{

    public class VersionGroup//管理版本的类
    {
        private VersionGroup()
        {

        }


        public static VersionGroup Load(string name, string path)
        {
            System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(typeof(VersionGroup));
            VersionGroup group = null;
            try
            {

                using (var s = System.IO.File.OpenRead(path + "/vergroup.xml"))
                {
                    group = xmls.Deserialize(s) as VersionGroup;
                }
            }
            catch
            {

            }

            if (group == null)
            {
                group = new VersionGroup();
                group.path = path;
                group.Save();
            }
            group.name = name;
            group.path = path;

            return group;
        }
        public void Save()
        {
            System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(typeof(VersionGroup));
            using (var s = System.IO.File.Create(path + "/vergroup.xml"))
            {
                xmls.Serialize(s, this);
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public string path;
        public string name;

        public int nowver;

        [System.Xml.Serialization.XmlIgnore]
        public List<Version> versions = new List<Version>();


        public Version getVersion(int ver)
        {
            if (ver <= 0 || ver >nowver )
            {
                return null;
            }
            else
            {
                if(versions[ver]==null)
                {
                    versions[ver] = Version.Load(this.path, ver);
                }
                return versions[ver];
            }
        }
    }

}
