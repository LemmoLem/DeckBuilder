using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Card : MonoBehaviour
{
    public List<CardData> carddatas = new List<CardData>();
    bool isPlayable;
    bool isInDeck = false;
    private GameManager gameManager;
    public TextMeshProUGUI cardText;
    public GameObject upbutton, downbutton, nobutton;
    public GameObject energyBidding;
    public TextMeshProUGUI energyBidText;
    public PlayerController thePlayer;
    public NPCController npc;
    PlayerController cardOwner, cardOpponent;
    int playerBid, npcBid;
    bool isMinusLife;
    int life = 0;
    public GameObject[] moduleSlots;
    List<Sprite> cardModuleSprite = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Im start");  
        isPlayable = true;
        gameManager = FindObjectOfType<GameManager>();
        SetButtonUINotActive();

        playerBid = 0;
        npcBid = 0;
        energyBidText.text = playerBid.ToString();
        thePlayer = gameManager.GetThePlayer();
        npc = gameManager.GetTheNPC();
        foreach(CardData data in carddatas)
        {
            if (data.cardEffect.Equals(CardData.CardEffect.LimitedUse))
            {
                Debug.Log(life + " life");
                life = life + data.statValue;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        DisplayText();
    }
    void SetButtonUIActive()
    {
        upbutton.SetActive(true);
        downbutton.SetActive(true);
        energyBidding.SetActive(true);
        nobutton.SetActive(true);
    }
    void SetButtonUINotActive()
    {
        upbutton.SetActive(false);
        downbutton.SetActive(false);
        energyBidding.SetActive(false);
        nobutton.SetActive(false);
    }
    public void ExitBid()
    {
        thePlayer.ChangeEnergy(playerBid);
        playerBid = 0;
        energyBidText.text = playerBid.ToString();
        SetButtonUINotActive();
    }    

    
    public void ConfirmBid()
    {
        SetButtonUINotActive();
        // so as bids can only be zero or legal amount just have to check whether bigger or not
        if (playerBid > npcBid)
        {
            //dont have to remove energy from the player as thats done when assigning energy
            gameManager.RemoveCardFromRiver(this);
            thePlayer.discardPile.Add(this);
            cardOwner = thePlayer;
            cardOpponent = npc;
            gameObject.SetActive(false);
            isPlayable = false;
            isInDeck = true;


        }
        else if (npcBid > playerBid)
        {
            //dont have to remove energy from the player as thats done when assigning energy
            gameManager.RemoveCardFromRiver(this);
            npc.discardPile.Add(this);
            cardOwner = npc;
            cardOpponent = thePlayer;
            gameObject.SetActive(false);
            isPlayable = false;
            isInDeck = true;

            
        }
        // when the card is bid upon the buttons n that should dissapear

        
        //for when bids are equal reset bids
        else {
            npcBid = 0;
            playerBid = 0;
            energyBidText.text = playerBid.ToString();
        }
    }

    public void ChangeNPCBid(int amount)
    {
        npcBid = amount;
        npc.ChangeEnergy(-amount);
    }

    public int GetEnergyCost()
    {
        int energySum = 0;
        for (int i = 0; i < carddatas.Count; i++)
        {
            energySum += carddatas[i].energyCost;
        }
        // to ensure that cards cant be 0 cost as dont wanna implement that just yet
        if (energySum >= 0)
        {
            energySum = -1;
        }
        return Math.Abs(energySum);
    }
    public int GetNPCBid()
    {
        return npcBid;
    }

    public void ChangeBidEnergyAmount(int amount)
    {
        //energy bidding on card is 0, then goes to amount of energy for card IF the player has enough energy.
        //this energy is pre emptively taken away from the player
        
        if(playerBid == 0 && amount ==1 && thePlayer.GetEnergy() >= GetEnergyCost())
        {
            thePlayer.ChangeEnergy(-GetEnergyCost());
            playerBid = GetEnergyCost();
        }
        //the minimum u can bid is energy cost of the card. so if trying to go lower then reset bid amount and give player back energy
        else if(playerBid == GetEnergyCost() && amount == -1)
        {
            thePlayer.ChangeEnergy(GetEnergyCost());
            playerBid = 0;
        }
        //if energybidding is above the energy cost then player can go lower. doing this way for amount of -1, means making sure not going below 0
        else if(playerBid > GetEnergyCost() && amount == -1)
        {
            thePlayer.ChangeEnergy(1);
            playerBid--;
        }
        //player has bid some energy and wants to go higher, make sure player doesnt exceed how much energy they have
        else if(playerBid > 0 && thePlayer.GetEnergy() >0 && amount == 1)
        {
            thePlayer.ChangeEnergy(-1);
            playerBid++;
        }

        energyBidText.text = playerBid.ToString();
    }

    public void AddCardData(CardData data)
    {
        bool isadded = false;
        int count = 0;

        if (carddatas.Count == 0)
        {
            carddatas.Add(data);
            isadded = true;
        }
        while (!isadded)
        {
            //if priority at the index is more than the data then add it where that one is
            // try this (int)Enum.Parse(typeof(Vehicle), data)
            //Debug.Log(carddatas[count].cardEffect);
            if ((int) carddatas[count].cardEffect > (int) data.cardEffect)
            {
                carddatas.Insert(count, data);
                isadded = true;
            }
            count++;
            if (count == carddatas.Count && !isadded)
            {
                carddatas.Add(data);
                isadded = true;
            }


        }
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.color = data.cardColor;
        cardModuleSprite.Add(data.cardArt);
        for (int i = 0; i < carddatas.Count; i++)
        {
            moduleSlots[i].GetComponent<SpriteRenderer>().sprite = carddatas[i].cardArt;
        }
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

                //should check whether it is in the players hand
                
                if (cardOwner == thePlayer)
                {
                    if (cardOwner.GetEnergy() >= GetEnergyCost())
                    {

                        PlayCard();

                    }
                }


            }
            else
            {
                SetButtonUIActive();
            }
        }
        //if the card has a player check if the turn is over - will gotta change 
    }


    public void PlayCard()
    {

        /* 
        if (carddatas[i].isInvertTargert == false)
        {
            
        }
        else if (carddatas[i].isInvertTargert == true)
        {

        }
        */

        cardOwner.ChangeEnergy(-GetEnergyCost());
        gameObject.SetActive(false);
        isPlayable = false;
        cardOwner.discardPile.Add(this);
        int index  = cardOwner.hand.IndexOf(this);
        cardOwner.hand[index] = null;
        isMinusLife = false;

        for (int i =0; i < carddatas.Count; i++)
        {
            
            switch (carddatas[i].cardEffect)
            {
                case CardData.CardEffect.Attack:
                    if (carddatas[i].isInvertTargert == false)
                    {
                        ResolveAttack(carddatas[i].statValue);
                    }
                    else if (carddatas[i].isInvertTargert == true)
                    {
                        InvertResolveAttack(carddatas[i].statValue);
                    }
                    break;
                case CardData.CardEffect.Armor:
                    if (carddatas[i].isInvertTargert == false)
                    {
                        ResolveShield(carddatas[i].statValue);

                    }
                    else if (carddatas[i].isInvertTargert == true)
                    {
                        InvertResolveShield(carddatas[i].statValue);
                    }
                    break;
                case CardData.CardEffect.Energy:
                    cardOwner.ChangeEnergy(carddatas[i].statValue);
                    break;
                case CardData.CardEffect.StrengthUp:
                    if (carddatas[i].isInvertTargert == false)
                    {
                        cardOwner.ChangeStrength(carddatas[i].statValue);

                    }
                    else if (carddatas[i].isInvertTargert == true)
                    {
                        cardOpponent.ChangeStrength(carddatas[i].statValue);
                    }
                    break;
                case CardData.CardEffect.ShieldUp:
                    if (carddatas[i].isInvertTargert == false)
                    {
                        cardOwner.ChangeShieldBonus(carddatas[i].statValue);
                    }
                    else if (carddatas[i].isInvertTargert == true)
                    {
                        cardOpponent.ChangeShieldBonus(carddatas[i].statValue);
                    }
                    break;
                case CardData.CardEffect.BaseEnergyUp:
                    if (carddatas[i].isInvertTargert == false)
                    {
                        cardOwner.ChangeBaseEnergy(carddatas[i].statValue);
                    }
                    else if (carddatas[i].isInvertTargert == true)
                    {
                        cardOpponent.ChangeBaseEnergy(carddatas[i].statValue);
                    }
                    break;
                case CardData.CardEffect.Unblockable:
                    if (carddatas[i].isInvertTargert == false)
                    {
                        ResolveUnblockableAttack(carddatas[i].statValue);
                    }
                    else if (carddatas[i].isInvertTargert == true)
                    {
                        InvertResolveUnblockableAttack(carddatas[i].statValue);
                    }
                    break;
                case CardData.CardEffect.ShieldBreaker:
                    if (carddatas[i].isInvertTargert == false)
                    {
                        ResolveShieldBreaker(carddatas[i].statValue);
                    }
                    else if (carddatas[i].isInvertTargert == true)
                    {
                        InvertResolveShieldBreaker(carddatas[i].statValue);
                    }
                    break;
                case CardData.CardEffect.Shield5Turn:
                    if (carddatas[i].isInvertTargert == false)
                    {
                        cardOwner.AddToShieldNextTurns(carddatas[i].statValue + cardOwner.shieldBonus);
                    }
                    else if (carddatas[i].isInvertTargert == true)
                    {
                        cardOpponent.AddToShieldNextTurns(carddatas[i].statValue + cardOwner.shieldBonus);
                    }
                    break;
                case CardData.CardEffect.LimitedUse:
                    LimitUse();
                    break;
                case CardData.CardEffect.Attack5:
                    if (carddatas[i].isInvertTargert == false)
                    {
                        cardOwner.AddToDamageNextTurns(carddatas[i].statValue + cardOwner.strength);
                    }
                    else if (carddatas[i].isInvertTargert == true)
                    {
                        cardOpponent.AddToDamageNextTurns(carddatas[i].statValue + cardOwner.strength);
                    }
                    break;
                case CardData.CardEffect.AddModule:
                    for (int j = 0; j < carddatas[i].statValue; j++)
                    {
                        AddModuleToLeftestCardInHand(carddatas[i].addModule);
                    }
                   break;
                case CardData.CardEffect.Discard:
                    for (int j = 0; j < carddatas[i].statValue; j++)
                    {
                        DiscardRandomCardFromHand();
                    }
                   
                    break;
                case CardData.CardEffect.PlayRandomCard:
                    for (int j = 0; j < carddatas[i].statValue; j++)
                    {
                        PlayRandomCardFromHand();
                    }
                    break;
                case CardData.CardEffect.DrawCard:
                    // so this should draw a card, but as when u play a card it discards the card. i guess i dont mind drawing a card as playing a card leaves a space ye.
                    for (int j = 0; j < carddatas[i].statValue; j++)
                    {
                        cardOwner.DrawACard();
                    }
                    break;
                case CardData.CardEffect.HandSizeUp:
                    for (int j = 0; j < carddatas[i].statValue; j++)
                    {
                        if (carddatas[i].isInvertTargert == false)
                        {
                            cardOwner.ChangeHandSize(1);
                        }
                        else if (carddatas[i].isInvertTargert == true)
                        {
                            cardOpponent.ChangeHandSize(1);
                        }
                    }
                    break;
            }
        }
    }
    
    void PlayRandomCardFromHand()
    {
        // play a random card and reset players energy from what was played
        if (cardOwner.GetHandLength() > 0)
        {
            var r = UnityEngine.Random.Range(0, cardOwner.GetHandLength());
            int energySave = cardOwner.GetEnergy();
            cardOwner.GetCardsInHand()[r].PlayCard();
            cardOwner.SetEnergy(energySave);
        }
    }

    void DiscardRandomCardFromHand()
    {
        if (cardOwner.GetHandLength() > 0)
        {
            var r = UnityEngine.Random.Range(0, cardOwner.GetHandLength());
            Card cardToDiscard = cardOwner.GetCardsInHand()[r];
            cardToDiscard.gameObject.SetActive(false);
            cardToDiscard.isPlayable = false;
            cardOwner.discardPile.Add(cardToDiscard);
            int index = cardOwner.hand.IndexOf(cardToDiscard);
            cardOwner.hand[index] = null;
        }
    }

    void AddModuleToLeftestCardInHand(CardData data)
    {
        bool isAdded = false;
        foreach (Card card in cardOwner.GetCardsInHand())
        {
            if (card.carddatas.Count < 9 & isAdded == false)
            {
                card.AddCardData(data);
                isAdded = true;
            }
        }
    }

    // so limit use the card still gets played but afterwards its removed from discard pile :-o
    // minus one from life of card n then dont minus one for other modules. 
    // so life is sum of modules
    void LimitUse()
    {   
        if (isMinusLife == false) { 
            life = life - 1;
            isMinusLife = true;
            if (life == 0)
            {
                cardOwner.discardPile.Remove(this);
            }
        }
    }

    
    public void SetPlayable(bool option)
    {
        isPlayable = option;
    }

    void DisplayText()
    {
        cardText.text = GetEnergyCost().ToString();
        
        
    }
    // change this to return an amount and then elsewhere have a function for do damage
    void ResolveAttack(int amount)
    {
        // takes into account opposition shield and the strength of the player
        int trueAttack = amount - Math.Abs(cardOwner.GetStrength());
        cardOwner.AddToDamageNext(Math.Abs(trueAttack));
    }
    // this is done this way as want to apply attack from player 
    void InvertResolveAttack(int amount)
    {
        int trueAttack = amount - Math.Abs(cardOwner.GetStrength());
        cardOpponent.AddToDamageNext(Math.Abs(trueAttack));
    }
    void ResolveShield(int amount)
    {
        cardOwner.ChangeShields(amount + cardOwner.GetShieldBonus());
    }
    void InvertResolveShield(int amount)
    {
        cardOpponent.ChangeShields(amount + cardOwner.GetShieldBonus());
    }
    void ResolveUnblockableAttack(int amount)
    {
        cardOwner.AddToUnblockNext(Math.Abs(amount));
    }
    void InvertResolveUnblockableAttack(int amount)
    {
        cardOpponent.AddToUnblockNext(Math.Abs(amount));
    }
    void ResolveSelfInflict(int amount)
    {
        cardOpponent.AddToDamageNext(Math.Abs(amount));
    }
    void ResolveShieldBreaker(int amount)
    {
        cardOwner.AddToShieldBreakNext(Math.Abs(amount));
    }
    void InvertResolveShieldBreaker(int amount)
    {
        cardOpponent.AddToShieldBreakNext(Math.Abs(amount));
    }



    //below is old function for resolve damage
    void DoDamage(int amount)
    {
        // takes into account opposition shield and the strength of the player
        int trueAttack = amount - Math.Abs(cardOwner.GetStrength());
        if (cardOpponent.GetShields() > 0)
        {
            if (Math.Abs(trueAttack) > cardOpponent.GetShields())
            {
                trueAttack = trueAttack + cardOpponent.GetShields();
                cardOpponent.ChangeShields(cardOpponent.GetShields());
                cardOpponent.ChangeHealth(trueAttack);
            }
            else
            {
                cardOpponent.ChangeShields(trueAttack);
            }
        }
        else
        {
            cardOpponent.ChangeHealth(trueAttack);
        }
    }

    //old resolve shield breaker
    void DoShieldBreaker(int amount)
    {
        // have this card say it does for each damage to shield, does one more (if they still have shields)

        // so strenght shouldnt be doubled
        // check whether the opponent has enough to firm the damage
        // then if not get rid of all shields n leftover damage
        if (cardOpponent.GetShields() >= Math.Abs(amount * 2))
        {
            ResolveAttack(amount * 2);
        }
        else
        {
            //while the opponent has shields and the attack amount is less than 0
            while (cardOpponent.GetShields() > 0 && amount < 0)
            {
                cardOpponent.ChangeShields(1);
                if (cardOpponent.GetShields() > 0)
                {
                    cardOpponent.ChangeShields(1);
                }
                amount = amount + 1;
            }
            if (amount > 0)
            {
                ResolveAttack(amount);
            }
        }
    }
}
