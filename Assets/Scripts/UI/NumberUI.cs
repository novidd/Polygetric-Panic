using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textGUI;
    
    public void UpdateUI(string preString, float currentValue) {
        if (textGUI == null) return;

        textGUI.text = FormatText(preString, currentValue);
    }

    private string FormatText(string preFixString, float currentValue) {
        string returnString = preFixString;

        if (currentValue >= 100) {
            returnString += "";
        }
        else if (currentValue >= 10) {
            returnString += "0";
        }
        else if (currentValue < 10) {
            returnString += "00";
        } else if (currentValue <= 0) {
            returnString = "000";
        }

        returnString += currentValue.ToString();

        return returnString;
    }
}
