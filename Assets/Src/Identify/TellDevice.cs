using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TellDevice : MonoBehaviour
{
    private Text _uiText;
	// Use this for initialization
	void Start () {

        InvokeRepeating("TellDeviceIdentifier",1,2);
	    _uiText = gameObject.GetComponent<Text>();

        Invoke("Quite", 20);
	}
	

    void TellDeviceIdentifier()
    {
        _uiText.text += SystemInfo.deviceUniqueIdentifier;
    }

    void Quite()
    {
        Application.Quit();
    }
}
