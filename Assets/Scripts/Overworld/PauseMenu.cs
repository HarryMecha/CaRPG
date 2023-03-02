using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    /*
     * PauseMenu is a script that handles the pause menu
     */

    #region Fields
    public GameObject pauseMenu;
    #endregion
    
    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    /*
     * Update waits for user input and when to turn the pauseMenu on and off.
     */

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {if(pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
            }else
            pauseMenu.SetActive(true);
            
        }
    }

    /*
     * CloseMenu() close the pause menu GUI element.
     */
    public void CloseMenu()
    {
        pauseMenu.SetActive(false);
    }
}
