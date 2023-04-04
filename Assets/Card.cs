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
    public TextMeshProUGUI cardDescription;
    public GameObject upbutton, downbutton, nobutton;
    public GameObject energyBidding;
    public TextMeshProUGUI energyBidText;
    public PlayerController thePlayer;
    public NPCController npc;
    PlayerController cardOwner, cardOpponent;
    int playerBid, npcBid;
    public GameObject[] moduleSlots;
    List<Sprite> cardModuleSprite = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        isPlayable = true;
        gameManager = FindObjectOfType<GameManager>();
        SetButtonUINotActive();

        playerBid = 0;
        npcBid = 0;
        energyBidText.text = playerBid.ToString();
        thePlayer = gameManager.GetThePlayer();
        npc = gameManager.GetTheNPC();
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
        carddatas.Add(data);
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

        cardOwner.ChangeEnergy(-GetEnergyCost());
        gameObject.SetActive(false);
        isPlayable = false;
        cardOwner.discardPile.Add(this);
        cardOwner.hand.Remove(this);
        for (int i =0; i < carddatas.Count; i++)
        { 
            switch (carddatas[i].cardEffect)
            {
                case CardData.CardEffect.Attack:
                    ResolveAttack(carddatas[i].statValue);
                    break;
                case CardData.CardEffect.Armor:
                    ResolveShield(carddatas[i].statValue);
                    break;
                case CardData.CardEffect.Energy:
                    cardOwner.ChangeEnergy(carddatas[i].statValue);
                    break;
                case CardData.CardEffect.StrengthUp:
                    cardOwner.ChangeStrength(carddatas[i].statValue);
                    //Debug.Log(carddata.statValue);
                    break;
                case CardData.CardEffect.ShieldUp:
                    cardOwner.ChangeShieldBonus(carddatas[i].statValue);
                    break;
                case CardData.CardEffect.BaseEnergyUp:
                    cardOwner.ChangeBaseEnergy(carddatas[i].statValue);
                    break;
                case CardData.CardEffect.Unblockable:
                    ResolveUnblockableAttack(carddatas[i].statValue);
                    break;
                case CardData.CardEffect.ShieldBreaker:
                    ResolveShieldBreaker(carddatas[i].statValue);
                    break;
            }
        }



    }


    
    public void SetPlayable(bool option)
    {
        isPlayable = option;
    }

    void DisplayText()
    {
        cardText.text = "energy" + GetEnergyCost();
        
        
    }

    void ResolveAttack(int amount)
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
    void ResolveShield(int amount)
    {
        cardOwner.ChangeShields(amount + cardOwner.GetShieldBonus());
    }
    void ResolveUnblockableAttack(int amount)
    {
        cardOpponent.ChangeHealth(amount);
    }
    void ResolveSelfInflict(int amount)
    {
        if (cardOwner.GetShields() > 0)
        {
            if (Math.Abs(amount) > cardOwner.GetShields())
            {
                amount = amount + cardOwner.GetShields();
                cardOwner.ChangeShields(cardOwner.GetShields());
                cardOwner.ChangeHealth(amount);
            }
            else
            {
                cardOwner.ChangeShields(amount);
            }
        }
        else
        {
            cardOwner.ChangeHealth(amount);
        }
    }
    void ResolveShieldBreaker(int amount)
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
            if (amount> 0)
            {
                ResolveAttack(amount);
            }
        }
    }
}
