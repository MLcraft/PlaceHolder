using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverButton : MonoBehaviour
{

    public void loadScene()
    {
        SceneManager.LoadScene("Movement", LoadSceneMode.Single);
    }

}
