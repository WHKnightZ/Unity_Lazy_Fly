using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    private PlayerController playerController;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (playerController.IsPlaying())
        {
            int key = 0;
            if (Input.GetMouseButton(0))
            {
                float x = Input.mousePosition.x;
                if (x < 400f)
                    key = 1;
                else
                    key = 2;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
                key = 1;
            else if (Input.GetKey(KeyCode.RightArrow))
                key = 2;
            playerController.Move(key);
            playerController.Blow();
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            int level = SceneManager.GetActiveScene().buildIndex - 1;
            if (level < 0)
                level = SceneManager.sceneCountInBuildSettings - 1;
            SceneManager.LoadScene(level);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            int level = SceneManager.GetActiveScene().buildIndex + 1;
            if (level == SceneManager.sceneCountInBuildSettings)
                level = 0;
            SceneManager.LoadScene(level);
        }
    }
}
