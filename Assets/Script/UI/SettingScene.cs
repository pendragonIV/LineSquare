using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingScene : MonoBehaviour
{
    [SerializeField]
    private Text _mode;

    private void Start()
    {
        UpdateMode();
    }

    public void NextMode()
    {
        int mode = Mode.instance.mode;

        if(mode == 9)
        {
            Mode.instance.mode = 5; 
        }
        else
        {
            Mode.instance.mode++;
        }

        UpdateMode();
    }

    public void PreviousMode()
    {
        int mode = Mode.instance.mode;

        if(mode == 5)
        {
            Mode.instance.mode = 9;
        }
        else
        {
            Mode.instance.mode--;
        }

        UpdateMode();
    }

    private void UpdateMode()
    {
        int mode = Mode.instance.mode;

        if(mode == 5)
        {
            _mode.text = "5x5";
        }
        else if(mode == 6)
        {
            _mode.text = "6x6";
        }
        else if(mode == 7)
        {
            _mode.text = "7x7";
        }
        else if(mode == 8)
        {
            _mode.text = "8x8";
        }
        else if(mode == 9)
        {
            _mode.text = "9x9";
        }
    }
}
