using System;
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
    int baseShields;
    public int shieldBonus;
    public List<Card> hand = new List<Card>();
    public List<Card> drawPile = new List<Card>();
    public List<Card> discardPile = new List<Card>();
    public PlayerArea playerArea;
    public GameManager gameManager;
    int handSize;
    public int maxHandSize;
    int damageNow, damageNext, shieldBreakNow, shieldBreakNext, unblockNow, unblockNext, shieldNext, shieldNow;
    List<int> shieldNextTurns = new List<int>{0,0,0,0,0};
    List<int> damageNextTurns = new List<int>{0,0,0,0,0};
    PlayerController opponent;

    //base energy/max energy is the amount of energy the player starts each turn with


    // Start is called before the first frame update
    void Start()
    {
        energy = baseEnergy;
        handSize = 6;
        strength = 0;
        shieldBonus = 0;
        damageNext = 0;
        damageNow = 0;
        shieldBreakNow = 0;
        shieldBreakNext = 0;
        unblockNow = 0;
        unblockNext = 0;
        shieldNow = 0;
        shieldNext = 0;
        maxHandSize = playerArea.slots.Length;
        for (int i = 0; i < maxHandSize; i++)
        {
            hand.Add(null);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetHandSize(int amount)
    {
        handSize = amount;
    }

    public void ChangeHandSize(int amount)
    {
        // so maybe change hand size minimum idk
        handSize += amount;
        if (handSize < 3)
        {
            handSize = 3;
        }
        if (handSize > maxHandSize)
        {
            handSize = maxHandSize;
        }
    }

    public int GetHandLength()
    {
        int sum = 0;
        for (int j = 0; j < hand.Count; j++)
        {
            if (hand[j] != null)
            {
                sum++;
            }
        }
        return sum;
    }

    void AddCardToHand(Card card)
    {
        bool isAdded = false;
        for (int i = 0; i < handSize; i++)
        {
            if (hand[i] == null & isAdded == false)
            {
                isAdded = true;
                hand[i] = card;
                card.transform.position = playerArea.slots[i].position;
            }
        }
    }

    public List<Card> GetCardsInHand()
    {
        List<Card> cardsInHand = new List<Card>();
        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i] != null)
            {
                cardsInHand.Add(hand[i]);
            }
        }
        return cardsInHand;
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
                drawPile[0].gameObject.SetActive(true);
                drawPile[0].SetPlayable(true);
                AddCardToHand(drawPile[0]);
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
                drawPile[0].gameObject.SetActive(true);
                drawPile[0].SetPlayable(true);
                AddCardToHand(drawPile[0]);
                drawPile.RemoveAt(0);
                // from blackthorn prod 

            }
        }
        if (GetHandLength() < handSize)
        {
            //so if hand count is less than 5 then that means there werent enough in draw pile n so shuffling and drawing more
            if (discardPile.Count > 0)
            {
                Shuffle();
                while (GetHandLength() < handSize && drawPile.Count > 0)
                {
                    drawPile[0].gameObject.SetActive(true);
                    drawPile[0].SetPlayable(true);
                    AddCardToHand(drawPile[0]);
                    drawPile.RemoveAt(0);

                }
            }
        }

    }

    // got to save the position card was
    public void DrawACard()
    {
        if (GetHandLength() < handSize)
        {
            bool isAdded = false;
            for (int i = 0; i < handSize; i++)
            {
                // so hand[i] isnt usable as less than hand size
                if (hand[i] == null & isAdded == false)
                {
                    Debug.Log("THis is it");
                    if (drawPile.Count == 0)
                    {
                        Shuffle();
                    }
                    drawPile[0].gameObject.SetActive(true);
                    drawPile[0].SetPlayable(true);
                    AddCardToHand(drawPile[0]);
                    drawPile.RemoveAt(0);
                    isAdded = true;
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
    public void SetEnergy(int amount)
    {
        energy = amount;
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

    public void SetShields(int amount)
    {
        shields = amount;
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
        List<Card> addToDiscard = new List<Card>();
        bool isAdded = false;
        for (int i = 0; i < maxHandSize; i++)
        {
            if (hand[i] != null)
            {
                addToDiscard.Add(hand[i]);
                hand[i].SetPlayable(false);
                hand[i].gameObject.SetActive(false);
                hand[i] = null;
                isAdded = true;
            }
        }
        Debug.Log(addToDiscard.Count + "THIS IS HWO MUCH TO ADD TO DSICARD");
        if (isAdded)
        {
            discardPile.AddRange(addToDiscard);
        }
    }

    public void EndTurn()
    {
        // so applys damage clears player hands, resets energy then draws new hand and applys next turn effects
        ApplyDamage();
        // empty players hand
        ClearHand();
        // set energy for next turn
        energy = baseEnergy;
        // draw new hand
        DrawNewHand();
        // apply all damage

        // start next turn        
    }
    public void StartTurn()
    {
        AddNextTurnEffectsAndCycleThem();
        NextEffectsMakeNow();
    }

    public void CheckIfTurnOver()
    {
        //also will need a way for players to willingly end turn without using all their energy. end turn button i.e.
        if (energy < 1)
        {
            //add line here to check whether the player has any cards in their hand and whether any cards in the river
            EndTurn();
        }
        if (gameManager.GetRiverCardLength() == 0 && GetHandLength() == 0)
        {
            EndTurn();
        }
        // want a method that goes thru each card in the river and if theres a card which is lower 
 
    }

    public GameManager GetGameManager()
    {
        return gameManager;
    }

    public int GetDamageNow()
    {
        return damageNow;
    }
    public void AddToDamageNext(int amount)
    {
        damageNext += amount;
    }

    public int GetShieldBreakNow()
    {
        return shieldBreakNow;
    }
    public void AddToShieldBreakNext(int amount)
    {
        shieldBreakNext += amount;
    }
    public int GetUnblockNow()
    {
        return unblockNow;
    }
    public void AddToUnblockNext(int amount)
    {
        unblockNext += amount;
    }

    public void SetOpponent(PlayerController oppo)
    {
        opponent = oppo;
    }

    public List<int> GetShieldsNextTurns()
    {
        return shieldNextTurns;
    }

    public string GetShield5String()
    {
        string str = shieldNextTurns[0]+"," + shieldNextTurns[1] + "," + shieldNextTurns[2] + "," + shieldNextTurns[3] + "," + shieldNextTurns[4];
        
        return str;
    }
    public string GetDamage5String()
    {
        string str = damageNextTurns[0] + "," + damageNextTurns[1] + "," + damageNextTurns[2] + "," + damageNextTurns[3] + "," + damageNextTurns[4];

        return str;
    }

    public void ApplyDamage()
    {
        //apply unblock
        // 0 is opponent 
        
        opponent.ChangeHealth(this.GetUnblockNow());

        //apply shieldbreak
        if (opponent.GetShields() >= Math.Abs(this.GetShieldBreakNow() * 2))
        {
            opponent.ChangeShields(-this.GetShieldBreakNow() * 2);
        }
        else
        {
            //while the opponent has shields and the attack amount is less than 0
            int amount = -this.GetShieldBreakNow();
            while (opponent.GetShields() > 0 && amount < 0)
            {
                opponent.ChangeShields(1);
                if (opponent.GetShields() > 0)
                {
                    opponent.ChangeShields(1);
                }
                amount = amount + 1;
            }
            if (Math.Abs(amount) > 0)
            {
                opponent.ChangeHealth(amount);
            }
        }

        //apply regular attack
        if (opponent.GetShields() > 0)
        {
            if (this.GetDamageNow() > opponent.GetShields())
            {
                int trueAttack = this.GetDamageNow() - opponent.GetShields();
                opponent.SetShields(0);
                opponent.ChangeHealth(-trueAttack);
            }
            else
            {
                opponent.ChangeShields(-this.GetDamageNow());
            }
        }
        else
        {
            opponent.ChangeHealth(-this.GetDamageNow());
        }



    }
    void AddNextTurnEffectsAndCycleThem()
    {
        //add to damage next turn and shield next turn
        damageNext += damageNextTurns[0];
        shieldNext += shieldNextTurns[0];
        damageNextTurns.RemoveAt(0);
        shieldNextTurns.RemoveAt(0);
        damageNextTurns.Add(0);
        shieldNextTurns.Add(0);
    }

    public void AddToShieldNextTurns(int amount)
    {
        for (int i = 0; i < shieldNextTurns.Count; i++)
        {
            shieldNextTurns[i] += amount;
        }
    }
    public void AddToDamageNextTurns(int amount)
    {
        for (int i = 0; i < damageNextTurns.Count; i++)
        {
            damageNextTurns[i] += amount;
        }
    }

    void NextEffectsMakeNow()
    {
        // make shield now whats in shield next
        shieldNow = shieldNext;
        // make shield next 0
        shieldNext = 0;
        // add shieldNow to baseShields
        shields = baseShields + shieldNow;

        damageNow = damageNext;
        damageNext = 0;

        shieldBreakNow = shieldBreakNext;
        shieldBreakNext = 0;
    }

}
