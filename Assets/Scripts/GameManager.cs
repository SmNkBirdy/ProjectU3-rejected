using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int currencu;
    public GameObject menu;
    public GameObject restartMenu;
    public Text curBar;
    public bool pause = false;
    // Start is called before the first frame update

    public void unpause()
    {
        Time.timeScale = 1;
        menu.SetActive(false);
        pause = false;
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        unpause();
    }

    public void restartWindow()
    {
        restartMenu.SetActive(!restartMenu.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
        curBar.text = "- " + currencu;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause)
            {
                unpause();
            }
            else
            {
                Time.timeScale = 0;
                menu.SetActive(true);
                pause = true;
            }
        }
    }
}
