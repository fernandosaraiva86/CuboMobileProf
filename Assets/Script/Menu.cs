using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("GameManager")]
    public GameManager _gameManager;//Arrastar o Game Manager Inspetor

    public void Play()
    {
        GetComponent<CanvasGroup>().LeanAlpha(0, 0.2f).setOnComplete(IniciaGame);
    }

    private void Start()
    {
        GetComponentInChildren<TMPro.TextMeshProUGUI>().gameObject.
            LeanScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).setLoopPingPong();
    }

    private void IniciaGame()
    {
        _gameManager.Enable();
        Destroy(gameObject);
    }


}
