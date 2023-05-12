using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : PlayerController
{
    //this script should have a play turn function
    //in this the npc will look at cards in the river, cards in the hand, their stats, 

        /*
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
    public PlayerArea playerArea;*/
    //private GameManager gameManager;
    

    // Start is called before the first frame update
    void Start()
    {
        ChangeEnergy(baseEnergy);
        SetHandSize(6);
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

    List<Card> GetBidableCards()
    {
        List<Card> cards = gameManager.GetRiverCards();

        List<Card> bidableCards = new List<Card>();
        if (cards.Count > 0)
        {
            foreach (Card card in cards)
            {
                if (card.GetEnergyCost() <= GetEnergy())
                {
                    bidableCards.Add(card);
                }
            }
        }
        return bidableCards;
    }

    List<Card> GetPlayable()
    {
        List<Card> cards = GetCardsInHand();

        List<Card> playableCards = new List<Card>();
        if (cards.Count > 0)
        {
            foreach (Card card in cards)
            {
                if (card.GetEnergyCost() <= GetEnergy())
                {
                    playableCards.Add(card);
                }
            }
        }
        return playableCards;
    }

    public void PlayTurn()
    {
        //so on an npc turn they want to play cards and bid for cards
        //make playable cards
        List<Card> bidableCards = GetBidableCards();
        

        if (bidableCards.Count > 0) { 
        //if no hand isnt full then use all energy filling hand
            if (GetHandLength() + drawPile.Count + discardPile.Count < 10)
            {
                int i = 0;

                //maybe have to do count -1
                while (GetEnergy() > 0 && i < 10 && bidableCards.Count > 0)
                {
                    var num = UnityEngine.Random.Range(0, bidableCards.Count);
                    if (bidableCards[num].GetNPCBid() == 0)
                    {
                        int energyCost = bidableCards[num].GetEnergyCost();
                        if (energyCost < GetEnergy())
                        {
                            bidableCards[num].ChangeNPCBid(energyCost);
                        }
                        //so if the npc cant bid on cards they add to counter, just implementation for now
                        else
                        {
                            i++;    
                        }    
                    }
                    else
                    {
                        i++;
                    }
                    bidableCards = GetBidableCards();

                }
            }
            else if ((GetHandLength() + drawPile.Count + discardPile.Count) >= 10 && (GetHandLength() + drawPile.Count + discardPile.Count) < 20)
            {
                int i = 0;
                while (GetEnergy() > baseEnergy/2 && i < 10 && bidableCards.Count > 0)
                {
                    var num = UnityEngine.Random.Range(0, bidableCards.Count);
                    if (bidableCards[num].GetNPCBid() == 0)
                    {
                        int energyCost = bidableCards[num].GetEnergyCost();
                        if (energyCost < GetEnergy())
                        {
                            bidableCards[num].ChangeNPCBid(energyCost);
                        }
                        //so if the npc cant bid on cards they add to counter, just implementation for now
                        else
                        {
                            i++;
                        }
                    }
                    else
                    {
                        i++;
                    }
                    bidableCards = GetBidableCards();

                }
            }
        }
        //should play some cards as well as bid in middle

        if(GetHandLength()>0 && GetEnergy()>0)
        {
            //this method should really be trying to play first random card, if can then play it, if not remove it from options
            int i = 0;
            
            while(GetEnergy() > 0 && i<10 && GetPlayable().Count > 0)
            {
                List<Card> playableCards = GetPlayable();
                if (playableCards.Count > 0)
                {
                    var num = UnityEngine.Random.Range(0, playableCards.Count);
                    playableCards[num].PlayCard();
                }
                i++;
            }
        }
    }


    


}
