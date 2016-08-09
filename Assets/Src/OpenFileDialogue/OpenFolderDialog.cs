using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class OpenFolderDialog
{
    public static string OpenFolder(string defaultPath, string name)
    {

        string folderPath = defaultPath + "\\" + name;
        string path = "";
        //Debug.Log(folderPath);
        if (!string.IsNullOrEmpty(name))
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

#if UNITY_EDITOR
        path = EditorUtility.OpenFolderPanel("选择开文件夹", defaultPath, name);
#elif UNITY_STANDALONE_WIN
        FolderBrowserDialog dialog = new FolderBrowserDialog
        {
            ShowNewFolderButton = true,
            //RootFolder = Environment.SpecialFolder.MyDocuments,
            SelectedPath = "C:",
            Description = "请选择保存目录"
        };

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                path = dialog.SelectedPath;
            }
        dialog.Dispose();
        #endif
        if (string.IsNullOrEmpty(path))
        {
            path = folderPath;
        }
        if (!string.IsNullOrEmpty(name))
            if (!Path.GetFileName(path).Equals(name))
            {
                Directory.Delete(folderPath);
            }
        return path;
    }

}
