using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/*
    so now remake game manager so that it can make its own card to fill the river.
    - create new cards using carddata 
    - so spawn river. spawn river draw pile. every 1-3 turns draw new cards to the left, shift cards to the right   
    
    methods to include:
    




    ways to improve:
        will have slay the spire type thing of giving opponents cards like thorns and that, unplayable curse ykno
        Have cards in river spawned with offset with rotation and position to get messy look, could be good (could also be stuff)
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
    public Card card, card2, card3, card4, card5, card6;
    public UnityEngine.UI.Text player1Text, player2Text;
    public TextMeshProUGUI gameOverText;
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
        for (int i = 0; i < riverLength/6; i++)
        {
            riverDrawPile.Add(Instantiate(card));
            riverDrawPile.Add(Instantiate(card2));
            riverDrawPile.Add(Instantiate(card3));
            riverDrawPile.Add(Instantiate(card4));
            riverDrawPile.Add(Instantiate(card5));
            riverDrawPile.Add(Instantiate(card6));
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
        if (player1.GetHealth() < 0 || player2.GetHealth() < 0)
        {
            gameOverText.text = "GAME OVER";
        }
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
    public int GetRiverCardLength()
    {
        return riverCards.Count;
    }
    public void RemoveCardFromRiver(Card card)
    {
        riverCards.Remove(card);
    }


}
