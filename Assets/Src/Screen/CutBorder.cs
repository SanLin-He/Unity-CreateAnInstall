using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class CutBorder : MonoBehaviour
{
    //[HideInInspector]
    private  Rect screenPosition;
    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("User32.dll", EntryPoint = "GetSystemMetrics")]
    public static extern IntPtr GetSystemMetrics(int nIndex);

    const int SM_CXSCREEN = 0x00000000;
    const int SM_CYSCREEN = 0x00000001;

    const uint SWP_SHOWWINDOW = 0x0040; //显示窗口
    const int GWL_STYLE = -16;  //新的窗口风格
    const int WS_POPUP = 0x800000;  //弹出式窗口
    const int WS_BORDER = 1;

    private int resWidth;
    private int resHeight;
    

    void Start()
    {
#if(!UNITY_EDITOR)
        //当前屏幕分辨率
        resWidth = (int)GetSystemMetrics(SM_CXSCREEN);
        resHeight = (int)GetSystemMetrics(SM_CYSCREEN);
        screenPosition = new Rect((int)(resWidth/3),(int)(resHeight/2),400,300);
        //设置新的窗口风格，定义弹出式窗口
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_BORDER);

        //bool result = SetWindowPos(GetForegroundWindow(), 0, 0, 0, resWidth, resHeight, SWP_SHOWWINDOW);
        bool result = SetWindowPos(GetForegroundWindow(), 0, (int)screenPosition.x, (int)screenPosition.y, (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);
#endif
    }

}
