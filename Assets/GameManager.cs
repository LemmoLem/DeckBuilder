using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                make it so cards do damage and that will be it 

       NEXT UP: Cards actually do something, different type of cards. attack card, shield card etc

        this seems useful for implementing cards. 
        https://gamedevbeginner.com/how-to-use-script-composition-in-unity/


    So what do i wanna achieve for this demo.  due in two weeks
    1. cards exist in river and player areas and lock into slot places
        1.0.5. add in card slots for the middle and the spawning in of them - will just be copying code for shuffle and draw pile from 
        player controller and putting it here. river instead of deck.
        1.1. game manager spawns random selection of armor, shield and special function card    
        1.2. will want cards to enable/disable sprite renderer when being spawned - when click on card add it to discard pile

    2. way to play cards
        2.1. need to click on card in the middle
    3. stats get managed n shit
    4. different look for each card type
    5. shuffling and drawing cards work
    6. way to win


    ways to improve:
        will have slay the spire type thing of giving opponents cards like thorns and that, unplayable curse ykno
        Have cards in river spawned with offset with rotation and position to get messy look, could be good (could also be shite)
        A card that makes cards go straight to hand rather than discard pile

*/

public class GameManager : MonoBehaviour
{
    public PlayerController player1, player2;
    public River river;
    List<Card> riverCards = new List<Card>();
    List<Card> riverDrawPile = new List<Card>();
    List<Card> riverDiscardPile = new List<Card>();
    public int riverLength;
    public Card card;
    public Card card2;
    public Card card3;
    public UnityEngine.UI.Text player1Text, player2Text;
    private int turnCount =1;
    // Start is called before the first frame update
    void Start()
    {
        FillDrawPile();
        SpawnRiver();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayText();
    }

    void SpawnRiver()
    {
        for(int i = 0; i < riverLength; i++)
        {
            riverDrawPile[0].transform.position = river.slots[i].position;
            riverCards.Add(riverDrawPile[0]);
            riverDrawPile.RemoveAt(0);
        }
    }

    void FillDrawPile()
    {
        for (int i = 0; i < riverLength/3; i++)
        {
            riverDrawPile.Add(Instantiate(card));
            riverDrawPile.Add(Instantiate(card2));
            riverDrawPile.Add(Instantiate(card3));
        }
    }

    void DisplayText()
    {
        player1Text.text = "Draw Pile: " + player1.GetDrawPileLength() + "\nDiscard Pile: " 
            + player1.GetDiscardPileLength() + "\nHealth: " + player1.GetHealth() + "\nEnergy: " + player1.GetEnergy()
            + "\nShields: " + player1.GetShields() + "\nStrength: " + player1.GetStrength();


        player2Text.text = "Draw Pile: " + player2.GetDrawPileLength() + "\nDiscard Pile: "
                    + player2.GetDiscardPileLength() + "\nHealth: " + player2.GetHealth() + "\nEnergy: " + player2.GetEnergy()
                    + "\nShields: " + player2.GetShields() + "\nStrength: " + player2.GetStrength();
    }

    PlayerController GetPlayer1()
    {
        return player1;
    }
    PlayerController GetPlayer2()
    {
        return player2;
    }
    public PlayerController GetWhosTurn()
    {
        if (turnCount%2 == 0)
        {//even number
            return player1;
        }
        else
        {
            return player2;
        }
    }
    public PlayerController GetWhosNotsTurn()
    {
        if (turnCount % 2 == 0)
        {//even number
            return player2;
        }
        else
        {
            return player1;
        }
    }
    public void NextTurn()
    {
        turnCount += 1;
    }
}
