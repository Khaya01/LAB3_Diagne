using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIEnd : UI
{
    [SerializeField] private TextMeshProUGUI _txtTotalTime;
    [SerializeField] private TextMeshProUGUI _txtTotalCollision;
    [SerializeField] private TextMeshProUGUI _txtFinalTime;
    [SerializeField] private Button _resartButton;

    private void Awake()
    {
        UIGame uigame = FindAnyObjectByType<UIGame>();
        if (uigame != null)
        {
            Destroy(uigame.gameObject);
        }
    }

    private void Start()
    {
        Debug.Log($"EndTime dans UIEnd: {GestionJeu.Instance.EndTime}");
        _txtTotalCollision.text = $"Collision : {GestionJeu.Instance.Pointage}";
        _txtTotalTime.text = $"Temps total : {GestionJeu.Instance.EndTime:f2}";
        float final = GestionJeu.Instance.Pointage + GestionJeu.Instance.EndTime;
        _txtFinalTime.text = $"Temps Final : {final:f2}";
        EventSystem.current.SetSelectedGameObject(_resartButton.gameObject);
    }

}
