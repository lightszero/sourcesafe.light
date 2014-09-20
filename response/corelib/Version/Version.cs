using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace corelib
{
    public enum ResoureType
    {
        SingleFile,             //单个文件，只知道相同与不同
        MultilineText,          //按行处理的文本文件
        JsonText,               //按Json树处理的文本文件
        GridData8,              //按网格分块处理的图片数据
        GridData32,
    };
    public class Resource//每个资源
    {
        public string name; //名称
        public ResoureType type;   //类型
        public bool patch;  //是补丁还是存档
        public int basever;//基础版本，补丁就是针对此版本的补丁，存档就是当前版本
    }
    public class Version//每个版本
    {
        public int id;//版本ID
        public List<Resource> reslist = new List<Resource>();

        public static Version Load(string path, int ver)
        {
            return null;
        }
    }
}
