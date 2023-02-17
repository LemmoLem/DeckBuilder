using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    //so one way could handle card is that cards have an enum rather than inheriting from card parent.
    //then when trying to do action it looks at the enum type and then performs that.

    bool isPlayable;
    bool isInDeck = false;
    int energyCost = -1;
    int damage;
    int shield;
    private GameManager gameManager;
    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        isPlayable = true;
        gameManager = FindObjectOfType<GameManager>();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        //if card doesnt have an owner then whoever turn it is then go into that deck
        //else it should look at whos deck its in and attack other player
        if (isPlayable)
        {
            if (isInDeck)
            {
                //this if should check if the player of the card and whether its their turn
                if (player == gameManager.GetWhosTurn())
                {
                    player.SetEnergy(energyCost);
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    Debug.Log("turning off sprite renderer");
                    isPlayable = false;
                    player.discardPile.Add(this);
                    player.hand.Remove(this);
                }
               

            }
            else
            {
                //check whos turn and add it to discard pile. card is being collected from river. gotta remove self from river as well
                player = gameManager.GetWhosTurn();
                player.discardPile.Add(this);
                player.SetEnergy(energyCost);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Debug.Log("turning off sprite renderer");
                isPlayable = false;
                isInDeck = true;

            }
        }
        player.CheckIfTurnOver();
    }

    void PlayCard()
    {
        //need way to know player n that !!!!!!!!!!!!
    }
    public void SetPlayable(bool option)
    {
        isPlayable = option;
    }
}
