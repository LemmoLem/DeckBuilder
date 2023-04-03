using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health;
    int energy;
    public int baseEnergy;
    public int strength;
    public int shields;
    public int shieldBonus;
    public List<Card> hand = new List<Card>();
    public int handSize;
    public List<Card> drawPile = new List<Card>();
    public List<Card> discardPile = new List<Card>();
    public PlayerArea playerArea;
    public GameManager gameManager;

    //base energy/max energy is the amount of energy the player starts each turn with


    // Start is called before the first frame update
    void Start()
    {
        energy = baseEnergy;
        handSize = 5;
        strength = 0;
        shieldBonus = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DrawNewHand()
    {

        // OI THIS METHOD DOESNT MAKE SURE THAT THERE ARE ENOUGH SLOTS AAAAA

        //will need to update this so it works with hands with less than 5 cards. 
        
        if (drawPile.Count > 0 && drawPile.Count <= handSize)
        {
            int length = drawPile.Count;
            
            
            //wasnt sure if it would update drawpile count when it checks for loop
            for (int i = 0; i < length; i++)
            {
                drawPile[0].transform.position = playerArea.slots[i].position;
                drawPile[0].gameObject.SetActive(true);
                drawPile[0].SetPlayable(true);
                hand.Add(drawPile[0]);
                drawPile.RemoveAt(0);
                // from blackthorn prod 

            }
            
        }
        //this else is for when the amount of cards is greater than 5 which is the amount of slots
        else if (drawPile.Count >= handSize)
        {
            //wasnt sure if it would update drawpile count when it checks for loop
            for (int i = 0; i < handSize; i++)
            {
                drawPile[0].transform.position = playerArea.slots[i].position;
                drawPile[0].gameObject.SetActive(true);
                drawPile[0].SetPlayable(true);
                hand.Add(drawPile[0]);
                drawPile.RemoveAt(0);
                // from blackthorn prod 

            }
        }
        if (hand.Count < handSize)
        {
            //so if hand count is less than 5 then that means there werent enough in draw pile n so shuffling and drawing more
            if (discardPile.Count > 0)
            {
                Shuffle();
                while (hand.Count < 5 && drawPile.Count> 0)
                {
                    drawPile[0].transform.position = playerArea.slots[hand.Count].position;
                    drawPile[0].gameObject.SetActive(true);
                    drawPile[0].SetPlayable(true);
                    hand.Add(drawPile[0]);
                    drawPile.RemoveAt(0);

                }
            }
        }

    }


    void Shuffle()
    {
        //Debug.Log("got to start of shuffle");
        //add all the cards in drawpile to the discard pile 
        //- even though draw pile should be empty to initiate shuffle (just better to have check, n could be used as a feature)
        if (drawPile.Count != 0)
        {
            discardPile.AddRange(drawPile);
            drawPile.Clear();
            Debug.Log("this shouldnt be run - Drawpile length " + drawPile.Count + " Discard pile length " + discardPile.Count);

        }

        //https://forum.unity.com/threads/clever-way-to-shuffle-a-list-t-in-one-line-of-c-code.241052/
        var count = discardPile.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            //sets r to a random number in between i and how many items there are
            var r = UnityEngine.Random.Range(i, count);
            //sets temp to card [i]
            var temp = discardPile[i];
            //sets where [i] was to whats in [r]
            discardPile[i] = discardPile[r];
            //sets where [r] is to temp
            discardPile[r] = temp;
            //Debug.Log("Drawpile length " + drawPile.Count + " Discard pile length " + discardPile.Count);
        }
        //discard pile just got shuffled so now put it into draw pile
        drawPile.AddRange(discardPile);
        discardPile.Clear();
       // Debug.Log("Drawpile length "+ drawPile.Count + " Discard pile length " + discardPile.Count);
    }


        
    void AddToDrawPile(Card card)
    {

    }

    public int GetBaseEnergy()
    {
        return baseEnergy;
    }

    public int GetDrawPileLength()
    {
        return drawPile.Count;
    }
    public int GetDiscardPileLength()
    {
        return discardPile.Count;
    }
    public int GetHealth()
    {
        return health;
    }
    public void ChangeHealth(int amount)
    {
        health += amount;
    }
    public int GetEnergy()
    {
        return energy;
    }
    public void ChangeEnergy(int amount)
    {
        energy += amount;
    }
    public int GetStrength()
    {
        return strength;
    }
    public void ChangeStrength(int amount)
    {
        strength += amount;
    }

    public int GetShields()
    {
        return shields;
    }
    public void ChangeShields(int amount)
    {
        shields += amount;
    }
    public int GetShieldBonus()
    {
        return shieldBonus;
    }
    public void ChangeShieldBonus(int amount)
    {
        shieldBonus += amount;
    }
    public void ChangeBaseEnergy(int amount)
    {
        baseEnergy += amount;
    }

    void ClearHand()
    {
        //so when a card is played it needs to be removed from the players hand and added into their discard pile
        for (int i =0; i < hand.Count; i++)
        {
            hand[i].SetPlayable(false);
            hand[i].gameObject.SetActive(false);
        }
        discardPile.AddRange(hand);
        hand.Clear();
    }

    public void EndTurn()
    {
        ClearHand();
        energy = baseEnergy;
        DrawNewHand();
    }
    public void CheckIfTurnOver()
    {
        //also will need a way for players to willingly end turn without using all their energy. end turn button i.e.
        if (energy < 1)
        {
            //add line here to check whether the player has any cards in their hand and whether any cards in the river
            EndTurn();
        }
        if (gameManager.GetRiverCardLength() == 0 && hand.Count == 0)
        {
            EndTurn();
        }
        // want a method that goes thru each card in the river and if theres a card which is lower 
 
    }

    public GameManager GetGameManager()
    {
        return gameManager;
    }
}
