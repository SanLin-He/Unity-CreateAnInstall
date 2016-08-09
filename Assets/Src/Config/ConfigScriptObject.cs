using UnityEngine;
using System.Collections;

public class ConfigScriptObject : ScriptableObject {

    public string ProductName;
    public string ProductPath;
    public string ProductVersion;
    public string IdentifierPath;

    public  string CompressedFileName;
    public string  PassWord;

    public void SetProperties(string productName, string productPath, string productVersion, string identifierPath,string compressedFileName,string passWord)
    {
        this.ProductName = productName;
        this.ProductPath = productPath;
        this.ProductVersion = productVersion;
        this.IdentifierPath = identifierPath;
        this.CompressedFileName = compressedFileName;
        this.PassWord = passWord;
    }
}
