using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUI : MonoBehaviour
{
    public GameManager gameManager;
    public void OnButtonPress()
    {
        Debug.Log("end turn button been pressed");
        gameManager.NextTurn();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
