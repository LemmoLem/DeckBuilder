using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
    

    ways to improve:
        will have slay the spire type thing of giving opponents cards like thorns and that, unplayable curse ykno
        Have cards in river spawned with offset with rotation and position to get messy look, could be good (could also be stuff)
        A card that makes cards go straight to hand rather than discard pile

*/

public class GameManager : MonoBehaviour
{
    public PlayerController thePlayer;
    public NPCController npc;
    public River river;
    List<Card> top = new List<Card>();
    List<Card> middle = new List<Card>();
    List<Card> bottom = new List<Card>();
    List<List<Card>> riverCards = new List<List<Card>>();
    List<Card> riverDrawPile = new List<Card>();
    List<Card> riverDiscardPile = new List<Card>();
    public int riverLength;
    public Card card, card2, card3, card4, card5, card6;
    public UnityEngine.UI.Text thePlayerText, npcText;
    public Card cardPrefab;
    public TextMeshProUGUI gameOverText;
    public List<CardData> cardDatas = new List<CardData>();
    private int turnCount =1;
    // Start is called before the first frame update

    //get rid of player1 and player 2
    //change turn, next turn, get whos turn all that. as turns will be at the same time
    //card is what uses whos turn and that will no longer be a thing
    //turns should be comprised of player choosing a set of card to get and or play
    //
   


    void Start()
    {
        // river cards is a list of lists. cards are added into top, middle and bottom by accesing that
        riverCards.Add(top);
        riverCards.Add(middle);
        riverCards.Add(bottom);
        AddNewCardsToDrawPile(25);
        SpawnRiver();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayText();
    }

    void SpawnRiver()
    {
        // this method should maybe changed to be like how spawn row works n be like full of nulls
        if (riverCards[0].Count == 0 && riverCards[1].Count == 0 && riverCards[2].Count == 0)
        {
            for (int j = 0; j < 3; j++)
            {


                for (int i = 0; i < 6; i++)

                {
                    
                    riverDrawPile[0].transform.position = river.GetRiverSlots()[j][i].position;
                    riverDrawPile[0].gameObject.SetActive(true);
                    riverCards[j].Add(riverDrawPile[0]);
                    riverDrawPile.RemoveAt(0);
                }
            }

        }
        else
        {

            // so if theres 3 cards in top, the last in there should be moved to the right most 
            // make so it moves cards to the right and then add cards
            // or have spawn cards script and add cards to it script
        }
        
    }
    void AddNewCardsToDrawPile(int amount)
    {
        while (amount != 0) {
            Card card = Instantiate(cardPrefab);
            int num = UnityEngine.Random.Range(0, cardDatas.Count);
            //Debug.Log(num);
            card.AddCardData(cardDatas[num]);
            int num2 = UnityEngine.Random.Range(0, 4);
            Debug.Log(num2);

            for (int i = 0; i < num2; i++)
            {
                int num3 = UnityEngine.Random.Range(0, cardDatas.Count);
                card.AddCardData(cardDatas[num3]);
            }

            riverDrawPile.Add(card);
            card.gameObject.SetActive(false);
            amount = amount - 1;
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
        thePlayerText.text = "Draw Pile: " + thePlayer.GetDrawPileLength() + "\nDiscard Pile: " 
            + thePlayer.GetDiscardPileLength() + "\nHealth: " + thePlayer.GetHealth() + "\nEnergy: " + thePlayer.GetEnergy()
            + "\nShields: " + thePlayer.GetShields() + "\nStrength: " + thePlayer.GetStrength();


        npcText.text = "Draw Pile: " + npc.GetDrawPileLength() + "\nDiscard Pile: "
                    + npc.GetDiscardPileLength() + "\nHealth: " + npc.GetHealth() + "\nEnergy: " + npc.GetEnergy()
                    + "\nShields: " + npc.GetShields() + "\nStrength: " + npc.GetStrength();
        if (thePlayer.GetHealth() < 0 || npc.GetHealth() < 0)
        {
            gameOverText.text = "GAME OVER";
        }
    }

    public PlayerController GetThePlayer()
    {
        return thePlayer;
    }
    public NPCController GetTheNPC()
    {
        return npc;
    }
 
    public void NextTurn()
    {
        npc.PlayTurn();
        // so go through each card and check if bids
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 6; i++)
            {
                if (riverCards[j][i] != null)
                {
                    riverCards[j][i].ConfirmBid();
                }
            }
        }
        thePlayer.EndTurn();
        npc.EndTurn();
        turnCount++;
        if (turnCount%5 == 0)
        {
            MoveCardsRight();
            AddNewCardsToDrawPile(5);
            SpawnColumnOfCards();
        }
        
        
    }

    void SpawnColumnOfCards()
    {
        for (int j = 0; j < 3; j++)
        {
            if (riverCards[j][0] == null)
            {
                riverDrawPile[0].transform.position = river.GetRiverSlots()[j][0].position;
                riverDrawPile[0].gameObject.SetActive(true);
                riverCards[j][0] = riverDrawPile[0];
                riverDrawPile.RemoveAt(0);
            }
        }
    }

    void MoveCardsRight()
    {
        // should now go from right to left and check each if equal to null or not
        for (int j = 0; j < 3; j++)
        {
            for (int i = riverCards[j].Count - 1; i > 0; i--)
            {
                // if the card to left is null then it wont have a position so ignore it
                if (riverCards[j][i] == null && riverCards[j][i-1] != null)
                {
                    // swap position in list and set transform position to one to the left
                    riverCards[j][i-1].transform.position = river.GetRiverSlots()[j][i].position;
                    riverCards[j][i] = riverCards[j][i - 1];
                    riverCards[j][i-1] = null;
                    int printi = i - 1;

                }
            }
            
        }

    }
    
    public int GetRiverCardLength()
    {
        int sum = 0;
        for (int j = 0; j < riverCards.Count; j++)
        {
            for (int i = 0; i < riverCards[j].Count; i++)
            {
                if (riverCards[j][i] != null)
                {
                    sum++;
                }
            }
        }
        return sum;
    }
    public void RemoveCardFromRiver(Card card)
    {
        for(int i = 0; i<riverCards.Count; i++)
        {
            if (riverCards[i].Contains(card))
            {
                int index = riverCards[i].IndexOf(card);
                riverCards[i][index] = null;

            }
        }
    }

    public List<Card> GetRiverCards()
    {
        List<Card> cards = new List<Card>();
        for (int j = 0; j < riverCards.Count; j++)
        {
            for (int i = 0; i < riverCards[j].Count; i++)
            {
                if (riverCards[j][i] != null)
                {
                    cards.Add(riverCards[j][i]);
                }
            }
        }

        return cards;
    }

}
