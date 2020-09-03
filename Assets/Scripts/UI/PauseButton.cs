using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnClickPauseButton()
    {
        GameManager.instance.Pause();
        UIManager.instance.Pause();
    }
}
