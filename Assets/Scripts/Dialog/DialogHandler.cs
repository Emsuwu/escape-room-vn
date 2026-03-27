using Unity.VisualScripting;
using UnityEngine;

public class DialogHandler : MonoBehaviour
{
    [SerializeField] private TextAsset _file;
    [SerializeField] private string _startSequence;

    private void Start()
    {
        if (_file != null)
        {
            DialogParser.LoadDialog(_file.text);
            if (!string.IsNullOrEmpty(_startSequence))
            {
                DialogParser.Branch("unicorn_1");
            }
        }
    } 
}
