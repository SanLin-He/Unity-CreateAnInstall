using UnityEngine;
using System.Collections.Generic;
using System;
/// <summary>
///  you should desc this script 
/// </summary>
public class Debuger : MonoBehaviour
{
    int LineCount = 50;
    List<string> mLogLines = new List<string>();
    List<string> mErroLines = new List<string>();
    bool drawBounds;
    List<Bounds> boundsList = new List<Bounds>();

    public bool showDebug = true;
    static Debuger mInstance = null;

    System.DateTime start;
    System.DateTime end;
    double past = 0;
    bool startTimeCount;

    static void _CreateInstance()
    {
        GameObject go;
        if (mInstance == null)
        {
            go = GameObject.Find("_Debuger");
            if (go)
            {
                mInstance = go.GetComponent<Debuger>();
                DontDestroyOnLoad(go);
            }
            else{
                go = new GameObject("_Debuger");
                mInstance = go.AddComponent<Debuger>();
                DontDestroyOnLoad(go);
            }
        }

    }

    #region TimeCount

    public static void StartTimeCount()
    {
        _CreateInstance();
        mInstance.start = System.DateTime.Now;
        mInstance.startTimeCount = true;
    }

    /// <returns> past time in ms</returns>
    public static double TimeCount()
    {
        if (mInstance == null || !mInstance.startTimeCount)
            return 0;

        mInstance.end = System.DateTime.Now;
        mInstance.past += mInstance.end.Subtract(mInstance.start).TotalMilliseconds;
        return mInstance.past;
    }

    /// <summary>
    /// stop time count, if showTimeCount=true，return  pasted time
    /// </summary>
    /// <param name="showTimeCount"></param>
    /// <returns> past time in ms </returns>
    public static double StopTimeCount(bool showTimeCount = false)
    {
        if (mInstance == null || !mInstance.startTimeCount)
            return 0;

        if (showTimeCount)
        {
            return TimeCount();
        }
        mInstance.startTimeCount = false;
        return 0;

    }
    #endregion

    #region Debug

    public static void DontShowLog()
    {
        _CreateInstance();
        mInstance.showDebug = false;
    }

#if UNITY_EDITOR
    public static Action<object> Log = Debug.Log;
#else
    public static void Log(object obj)
    {

        _CreateInstance();
        if (mInstance && Application.isPlaying && mInstance.showDebug)
        {
            if (mInstance.mLogLines.Count > mInstance.LineCount) mInstance.mLogLines.RemoveAt(0);
            mInstance.mLogLines.Add(obj.ToString());
            Debug.Log(obj.ToString());
        }
    }
#endif


#if UNITY_EDITOR
    public static Action<object> LogError = Debug.LogError;
#else
    public static void LogError(object obj)
    {
        _CreateInstance();
        if (mInstance && Application.isPlaying && mInstance.showDebug)
        {
            if (mInstance.mErroLines.Count > mInstance.LineCount) mInstance.mErroLines.RemoveAt(0);
            mInstance.mErroLines.Add(obj.ToString());
            Debug.LogError(obj.ToString());
        }
    }
#endif

    public static void ClearLog() { mInstance.mLogLines.Clear(); }
    #endregion

    #region DrawBounds

    public static void DrawBounds(Bounds b)
    {
        _CreateInstance();
        mInstance.drawBounds = true;
        mInstance.boundsList.Add(b);
    }

    void _DrawBounds(Bounds b)
    {
        Vector3 v0 = b.center - b.extents;
        Vector3 v1 = b.center + b.extents;
        Vector3 c = b.center;

        Debug.DrawLine(new Vector3(v0.x, v0.y, v0.z), new Vector3(v1.x, v0.y, v0.z), Color.red);
        Debug.DrawLine(new Vector3(v0.x, v1.y, v0.z), new Vector3(v1.x, v1.y, v0.z), Color.red);
        Debug.DrawLine(new Vector3(v0.x, v0.y, v1.z), new Vector3(v1.x, v0.y, v1.z), Color.red);
        Debug.DrawLine(new Vector3(v0.x, v1.y, v1.z), new Vector3(v1.x, v1.y, v1.z), Color.red);

        Debug.DrawLine(new Vector3(v0.x, v0.y, v0.z), new Vector3(v0.x, v1.y, v0.z), Color.red);
        Debug.DrawLine(new Vector3(v0.x, v0.y, v1.z), new Vector3(v0.x, v1.y, v1.z), Color.red);
        Debug.DrawLine(new Vector3(v1.x, v0.y, v0.z), new Vector3(v1.x, v1.y, v0.z), Color.red);
        Debug.DrawLine(new Vector3(v1.x, v0.y, v1.z), new Vector3(v1.x, v1.y, v1.z), Color.red);

        Debug.DrawLine(new Vector3(v0.x, v0.y, v0.z), new Vector3(v0.x, v0.y, v1.z), Color.red);
        Debug.DrawLine(new Vector3(v1.x, v0.y, v0.z), new Vector3(v1.x, v0.y, v1.z), Color.red);
        Debug.DrawLine(new Vector3(v0.x, v1.y, v0.z), new Vector3(v0.x, v1.y, v1.z), Color.red);
        Debug.DrawLine(new Vector3(v1.x, v1.y, v0.z), new Vector3(v1.x, v1.y, v1.z), Color.red);
    }

    public static void StopDrawBounds(Bounds b)
    {
        mInstance.drawBounds = false;
    }
    #endregion

    void Update()
    {
        if (drawBounds)
        {
            foreach (Bounds b in boundsList)
            {
                _DrawBounds(b);
            }

        }
    }

    void OnGUI()
    {
        Rect rect = new Rect(5f, 5f, 1000f, 18f);
        for (int i = 0, imax = mLogLines.Count; i < imax; ++i)
        {
            GUI.color = Color.black;
            GUI.Label(rect, mLogLines[i]);
            rect.y -= 1f;
            rect.x -= 1f;
            GUI.color = Color.white;
            GUI.Label(rect, mLogLines[i]);
            rect.y += 18f;
            rect.x += 1f;
        }

        for (int i = 0, imax = mErroLines.Count; i < imax; ++i)
        {
            GUI.color = Color.black;
            GUI.Label(rect, mErroLines[i]);
            rect.y -= 1f;
            rect.x -= 1f;
            GUI.color = Color.red;
            GUI.Label(rect, mErroLines[i]);
            rect.y += 18f;
            rect.x += 1f;
        }
    }


}
