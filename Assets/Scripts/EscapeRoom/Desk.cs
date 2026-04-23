using System.Threading.Tasks;
using UnityEngine;

public class Desk : MonoBehaviour
{
    public async void Open()
    {
        GetComponent<Animator>().SetTrigger("Open");

        InputHint.Instance.Show(InputHint.Instance.ThoughtBubble, "The desk draw popped open...");
        await Task.Delay(8000);
        InputHint.Instance.Hide();
    }
}
