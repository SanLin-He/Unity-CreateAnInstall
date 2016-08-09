using System;
using UnityEngine;
using System.Collections;
using System.IO;

public class FileOperationsUtil : Singleton<FileOperationsUtil> {
    

    ///目录文件夹下的所有文件
    public ArrayList ShowDirectFiles(string path)
    {
        string[] directoryEntries;
        ArrayList nameList = new ArrayList() ;
        try
        {
            //返回指定的目录中文件和子目录的名称的数组或空数组
            directoryEntries = Directory.GetFileSystemEntries(path);

            for (int i = 0; i < directoryEntries.Length; i++)
            {
                string p = directoryEntries[i];
                nameList.Add(p);
            }

        }
        catch (System.IO.DirectoryNotFoundException)
        {
            Debug.Log("The path " + path + "Directory object does not exist.");
        }

        return nameList;

    }
    /// 复制文件
    public void CopyFile(string srcFilePath, string destFilePath)
    {
        if (IsFileExists(srcFilePath) && !srcFilePath.Equals(destFilePath))
        {
            int index = destFilePath.LastIndexOf("/", StringComparison.Ordinal);
            string filePath = string.Empty;

            if (index != -1)
            {
                filePath = destFilePath.Substring(0, index);
            }

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            File.Copy(srcFilePath, destFilePath, true);
        }
    }

    /// 删除文件
    public void DeleteFile(string fileUrl)
    {
       // Debuger.Log("delete:" + fileUrl);
        if (IsFileExists(fileUrl))
        {
            File.Delete(fileUrl);
        }else
        {
            Debug.Log("FileOperationsUtil.DeleteFile删除文件不存在");
        }
    }

    /// 检测是否存在文件夹
    public static bool IsFolderExists(string folderPath)
    {
        if (folderPath.Equals(string.Empty))
        {
            return false;
        }

        return Directory.Exists(folderPath);
    }
    /// 检测文件是否存在Application.dataPath目录
    public static bool IsFileExists(string fileName)
    {
        if (fileName.Equals(string.Empty))
        {
            return false;
        }

        return File.Exists(fileName);
    }

    /// 创建文件夹
    public static void CreateFolder(string folderPath)
    {
        if (!IsFolderExists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    /// 隐藏文件
    public static void HideFile(string path)
    {
        //隐藏文件
        FileAttributes MyAttributes = File.GetAttributes(path);
        File.SetAttributes(path, MyAttributes | FileAttributes.Hidden);
    }


    /// 拷贝文件夹和文件夹文件
    public static void CopyFolder(string fromPath, string toPath)
    {
        if (!Directory.Exists(fromPath))
            return;

        if (!Directory.Exists(toPath))
        {
            Directory.CreateDirectory(toPath);
        }

        string[] files = Directory.GetFiles(fromPath);
        foreach (string formFileName in files)
        {
            string fileName = Path.GetFileName(formFileName);
            string toFileName = Path.Combine(toPath, fileName);
            File.Copy(formFileName, toFileName);
        }
        string[] fromDirs = Directory.GetDirectories(fromPath);
        foreach (string fromDirName in fromDirs)
        {
            string dirName = Path.GetFileName(fromDirName);
            string toDirName = Path.Combine(toPath, dirName);
            FileOperationsUtil.CopyFolder(fromDirName, toDirName);
        }
    }
}

