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
        handSize = 5;
        for (int i = 0; i < handSize; i++)
        {
            hand.Add(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayTurn()
    {
        //so on an npc turn they want to play cards and bid for cards
        List<Card> cards = gameManager.GetRiverCards();


        if (cards.Count > 0) { 
        //if no hand isnt full then use all energy filling hand
            if (GetHandLength() + drawPile.Count + discardPile.Count < 15)
            {
                int i = 0;

                //maybe have to do count -1
                while (GetEnergy() > 0 && i < 10)
                {
                    var num = UnityEngine.Random.Range(0, cards.Count);
                    if (cards[num].GetNPCBid() == 0)
                    {
                        int energyCost = cards[num].GetEnergyCost();
                        if (energyCost < GetEnergy())
                        {
                            cards[num].ChangeNPCBid(energyCost);
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

                }
            }
        }
        //should play some cards as well as bid in middle

        if(GetHandLength()>0 && GetEnergy()>0)
        {
            //this method should really be trying to play first random card, if can then play it, if not remove it from options
            int i = 0;
            
            while(GetEnergy() > 0 && i<10 && GetHandLength() > 0)
            {
                List<Card> handCards = GetCardsInHand();
                var num = UnityEngine.Random.Range(0, GetHandLength());
                if(handCards[num].GetEnergyCost()<= GetEnergy())
                {
                    handCards[num].PlayCard();
                }
                else
                {
                    i++;
                }
            }
        }
    }


    


}
