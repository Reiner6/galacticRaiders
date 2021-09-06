using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    CharacterController myCharacterController;

    private void Awake()
    {
        myCharacterController = GetComponent<CharacterController>();
    }
    /// <summary>
    /// Receives input for the character controller
    /// </summary>
    void Update()
    {
        if (GameManager.instance.MyGameState != GameState.gamePlay)
            return;

        myCharacterController.Move(Input.GetAxis("Horizontal"));

        if(Input.GetKeyDown(KeyCode.UpArrow) | Input.GetKeyDown(KeyCode.W))
        myCharacterController.Fire();
    }
}
