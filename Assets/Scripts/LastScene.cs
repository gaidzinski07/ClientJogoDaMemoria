using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LastScene : MonoBehaviour
{

    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text.text = GameManager.resultado;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
