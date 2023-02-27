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
    PlayerController opponent;
    public enum CardType { Attack, Armor, Energy};
    public CardType cardType;

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
                //when cards become in deck they get a player so this works
                if (player == gameManager.GetWhosTurn())
                {
                    player.ChangeEnergy(energyCost);
                    gameObject.SetActive(false);
                    isPlayable = false;
                    player.discardPile.Add(this);
                    player.hand.Remove(this);
                    switch (cardType)
                        {
                        case CardType.Attack:
                        //have to make it so considers armor and everything
                            opponent.ChangeHealth(-1);
                            break;
                        case CardType.Armor:
                            player.ChangeShields(1);
                            break;
                        case CardType.Energy:
                            player.ChangeEnergy(2);
                            break;

                        }

                }
               

            }
            else
            {
                //check whos turn and add it to discard pile. card is being collected from river. gotta remove self from river as well
                player = gameManager.GetWhosTurn();
                opponent = gameManager.GetWhosNotsTurn();
                gameManager.RemoveCardFromRiver(this);
                player.discardPile.Add(this);
                player.ChangeEnergy(energyCost);
                gameObject.SetActive(false);
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
