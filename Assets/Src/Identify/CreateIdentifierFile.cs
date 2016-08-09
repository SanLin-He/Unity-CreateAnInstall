using System;
using UnityEngine;
using System.Collections;
using System.IO;

public class CreateIdentifierFile
{
    private const string IdentifierFileName = "id.txt";
    public static void CreateFile()
    {

        ConfigScriptObject config = Resources.Load<ConfigScriptObject>("config");
        string path = config.IdentifierPath + "/" + IdentifierFileName;
        //文件夹
        FileOperationsUtil.CreateFolder(config.IdentifierPath);
        //写文件
        FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs);
        sw.WriteLine(SystemInfo.deviceUniqueIdentifier);
        sw.Close();
        

         //隐藏文件
        FileOperationsUtil.HideFile(path);


    }
}
