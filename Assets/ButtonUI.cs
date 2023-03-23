using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUI : MonoBehaviour
{
    public GameManager gameManager;
    public void OnButtonPress()
    {
        gameManager.GetWhosTurn().EndTurn();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
