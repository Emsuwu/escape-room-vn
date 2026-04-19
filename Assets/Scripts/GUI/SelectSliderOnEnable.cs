using UnityEngine;
using UnityEngine.UI;

public class SelectSliderOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Slider>().Select();
    }
}
