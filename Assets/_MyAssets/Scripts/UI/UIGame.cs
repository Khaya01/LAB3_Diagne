using TMPro;
using UnityEngine;

public class UIGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _txtTime;
    [SerializeField] private TextMeshProUGUI _txtCollision;

    private void Start()
    {
        _txtTime.text = "0.00";
    }
}
