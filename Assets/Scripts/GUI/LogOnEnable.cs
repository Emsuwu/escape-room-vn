using NUnit.Framework;
using UnityEngine;

public class LogOnEnable : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _logText;
    private void OnEnable()
    {
        _logText.text = string.Empty;

        foreach (string[] entry in DialogLog.LogList)
        {
            if (entry[0] == string.Empty)
            {
                _logText.text += $"<color=#80ff95>{entry[1]}</color>\n\n";    
            }
            else
            {
                _logText.text += $"<color=yellow>{entry[0]}</color>\n{entry[1]}\n\n";    
            }
        }
    }
}
