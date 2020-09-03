using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnClickResumeButton()
    {
        GameManager.instance.Resume();
        UIManager.instance.Resume();
    }
}
