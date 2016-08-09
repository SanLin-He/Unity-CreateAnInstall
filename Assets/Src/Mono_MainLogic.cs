using System;
using UnityEngine;
using System.IO;
using System.Security.AccessControl;
using Ionic.Zip;

//using UnityEditor;


public class Mono_MainLogic : MonoBehaviour
{
    ConfigScriptObject config;
    void Start()
    {
        config = Resources.Load<ConfigScriptObject>("config");
    }

    public void CopyFile2ProgramFiles()
    {
        #region 编辑器下
        //FileUtil.MoveFileOrDirectory(Application.streamingAssetsPath + "/Project", config.ProductPath + "/Project"); 
        #endregion

        #region 错误的实现方式，但也有借鉴的地方
        //Directory.Move(Application.streamingAssetsPath + "/Project", config.ProductPath + "/Project");

        //DirectoryInfo diS = new DirectoryInfo(Application.streamingAssetsPath + "/Project");
        //DirectoryInfo diT = new DirectoryInfo(config.ProductPath + "/Project");
        //if (!diS.Exists)
        //{
        //    //源文件不存在
        //    return;
        //}
        //if (diT.Exists)
        //{
        //    //目标文件已经存在
        //    return;
        //}
        //diS.MoveTo(config.ProductPath + "/Project");//用move的错误在于程序正在访问源文件 
        #endregion
       
        FileOperationsUtil.CopyFolder(Application.streamingAssetsPath + "/Project", config.ProductPath + "/Project");
    }

    public void InstallFile()
    {
        #region 安装（解压缩）程序到指定位置
        using (ZipFile zip = ZipFile.Read(Application.streamingAssetsPath + "/" + config.CompressedFileName + ".zip"))
        {
            zip.Password = config.PassWord;
            zip.ExtractAll(config.ProductPath);
        }
        #endregion

        //创建快捷方式
        AppShortcutToDesktop(config.ProductName);
        //创建唯一码文件
        CreateIdentifierFile.CreateFile();
    }

    private void AppShortcutToDesktop(string linkName)
    {
        string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        using (StreamWriter writer = new StreamWriter(deskDir + "\\" + linkName + ".url"))
        {
            string app = config.ProductPath + "/" + config.ProductName + ".exe";
            writer.WriteLine("[InternetShortcut]");
            writer.WriteLine("URL=file:///" + app);
            writer.WriteLine("IconIndex=0");
            string icon = app.Replace('\\', '/');
            writer.WriteLine("IconFile=" + icon);
            writer.Flush();
        }
    }
}
