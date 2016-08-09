using System;
using UnityEngine;
using System.Collections;
using System.IO;
using Ionic.Zip;
using Ionic.Zlib;
using UnityEditor;

public class CreateInstallEditor : EditorWindow
{

    private string _productName;
    private string _productPath;
    private string _productVersion;
    private string _identifierPath;


    private string _compressedFileName;
    private string _passWord;

    bool _canCompress = false;
    bool _canSave = false;
    private ConfigScriptObject config;
    [MenuItem("Tools/ConfigWin")]
    static void ShowWindow()
    {
        CreateInstallEditor window = GetWindow<CreateInstallEditor>(false, "打包配置", true);
        window.Show();
    }



    void OnEnable()
    {
        config = Resources.Load<ConfigScriptObject>("config");
        if (config)
        {
            _productName = config.ProductName;
            _productPath = config.ProductPath;
            _productVersion = config.ProductVersion;
            _identifierPath = config.IdentifierPath;

            _compressedFileName = _productName;
        }
        _passWord = "ctc";
    }
    void OnGUI()
    {
        //GUILayout.Label("打包工具的配置");
        _productName = EditorGUILayout.TextField("软件名称：", _productName);
        _productVersion = EditorGUILayout.TextField("软件版本号:", _productVersion);
        _compressedFileName = EditorGUILayout.TextField("压缩文件名:", _compressedFileName);
        _passWord = EditorGUILayout.TextField("压缩密码:", _passWord);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("选择软件安装位置  "))
        {
            string defaultPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            _productPath = OpenFolderDialog.OpenFolder(defaultPath, _productName);
        }
        if (!string.IsNullOrEmpty(_productPath))
            EditorGUILayout.LabelField(_productPath);

        EditorGUILayout.Space();
        if (GUILayout.Button("选择唯一码存放位置"))
        {
            string defaultPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _identifierPath = OpenFolderDialog.OpenFolder(defaultPath, _productName);
        }
        if (!string.IsNullOrEmpty(_identifierPath))
            EditorGUILayout.LabelField(_identifierPath);


        EditorGUILayout.Space();
        _canSave = EditorGUILayout.BeginToggleGroup("确定保存配置文件", _canSave);
        if (GUILayout.Button("保存配置文件"))
        {
            if (string.IsNullOrEmpty(_productPath) || string.IsNullOrEmpty(_productVersion) || string.IsNullOrEmpty(_productName) || string.IsNullOrEmpty(_identifierPath))
            {
                EditorUtility.DisplayDialog("设置错误", "请按要求填写好配置", "确定");
            }
            else
            {
                ConfigScriptObject config = ScriptableObject.CreateInstance<ConfigScriptObject>();
                config.SetProperties(_productName, _productPath, _productVersion, _identifierPath, _compressedFileName, _passWord);
                AssetDatabase.CreateAsset(config, "Assets/Resources/config.asset");

            }
        }
        EditorGUILayout.EndToggleGroup();

        EditorGUILayout.Space();
        _canCompress = EditorGUILayout.BeginToggleGroup("压缩安装文件", _canCompress);
        

        if (GUILayout.Button("开始压缩"))
        {
            if (string.IsNullOrEmpty(_compressedFileName) || string.IsNullOrEmpty(_passWord))
            {
                EditorUtility.DisplayDialog("设置错误", "没有设置压缩文件名或密码", "确定");
            }
            else
            {
                CompressStreamingAssets();

            }
        }

        if (FileOperationsUtil.IsFileExists(Application.streamingAssetsPath + "/" + _compressedFileName + ".zip"))
        {

            if (GUILayout.Button("删除原文件"))
            {

                if (EditorUtility.DisplayDialog("确定", "是否确定删除原文件", "确定", "取消"))
                {
                    FileUtil.DeleteFileOrDirectory(Application.streamingAssetsPath + "/Project");
                }
                AssetDatabase.SaveAssets();
            }
            if (GUILayout.Button("删除压缩文件"))
            {

                if (EditorUtility.DisplayDialog("确定", "是否确定删除压缩文件", "确定", "取消"))
                {
                    FileUtil.DeleteFileOrDirectory(Application.streamingAssetsPath + "/" + _compressedFileName + ".zip");
                }
                AssetDatabase.SaveAssets();
            }
        }
        EditorGUILayout.EndToggleGroup();

    }

    void CompressStreamingAssets()
    {
        string folderPath = Application.streamingAssetsPath + "/Project";
        string fileName = _compressedFileName /*+ System.DateTime.Now.Ticks.ToString()*/;

        using (ZipFile zip = new ZipFile(Application.streamingAssetsPath + "/" + fileName + ".zip"))
        {
            zip.Password = _passWord;
            zip.Encryption = EncryptionAlgorithm.WinZipAes128;
            zip.CompressionLevel = CompressionLevel.BestCompression;
            zip.AddDirectory(folderPath);
            zip.Save();

        }
    }
}
