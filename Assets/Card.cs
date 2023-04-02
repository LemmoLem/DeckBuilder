using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Card : MonoBehaviour
{
    //so one way could handle card is that cards have an enum rather than inheriting from card parent.
    //then when trying to do action it looks at the enum type and then performs that.
    public CardData carddata;
    bool isPlayable;
    bool isInDeck = false;
    private GameManager gameManager;
    PlayerController player;
    PlayerController opponent;
    public TextMeshProUGUI cardText;
    public TextMeshProUGUI cardDescription;
    public GameObject upbutton, downbutton, yesbutton, nobutton;
    public GameObject energyBidding;
    public TextMeshProUGUI energyBidText;
    int energyBidAmount;

    // Start is called before the first frame update
    void Start()
    {
        isPlayable = true;
        gameManager = FindObjectOfType<GameManager>();
        SetButtonUINotActive();

        energyBidAmount = 0;
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
        yesbutton.SetActive(true);
        nobutton.SetActive(true);
    }
    void SetButtonUINotActive()
    {
        upbutton.SetActive(false);
        downbutton.SetActive(false);
        energyBidding.SetActive(false);
        yesbutton.SetActive(false);
        nobutton.SetActive(false);
    }
    public void ExitBid()
    {
        gameManager.GetWhosTurn().ChangeEnergy(energyBidAmount);
        energyBidAmount = 0;
        SetButtonUINotActive();
    }    

    public void ConfirmBid()
    {
        player = gameManager.GetWhosTurn();
        opponent = gameManager.GetWhosNotsTurn();
        if (energyBidAmount >= Math.Abs(carddata.energyCost))
        {
            //dont have to remove energy from the player as thats done when assigning energy
            gameManager.RemoveCardFromRiver(this);
            player.discardPile.Add(this);
            gameObject.SetActive(false);
            isPlayable = false;
            isInDeck = true;

            // when the card is bid upon the buttons n that should dissapear
            SetButtonUINotActive();

        }
    }

    public int GetBidEnergyAmount()
    {
        return energyBidAmount;
    }

    public void ChangeBidEnergyAmount(int amount)
    {
        //energy bidding on card is 0, then goes to amount of energy for card IF the player has enough energy.
        //this energy is pre emptively taken away from the player
        
        if(energyBidAmount == 0 && amount ==1 && gameManager.GetWhosTurn().GetEnergy() >= Math.Abs(carddata.energyCost))
        {
            gameManager.GetWhosTurn().ChangeEnergy(carddata.energyCost);
            energyBidAmount = Math.Abs(carddata.energyCost);
        }
        //the minimum u can bid is energy cost of the card. so if trying to go lower then reset bid amount and give player back energy
        else if(energyBidAmount == Math.Abs(carddata.energyCost) && amount == -1)
        {
            gameManager.GetWhosTurn().ChangeEnergy(Math.Abs(carddata.energyCost));
            energyBidAmount = 0;
        }
        //if energybidding is above the energy cost then player can go lower. doing this way for amount of -1, means making sure not going below 0
        else if(energyBidAmount > Math.Abs(carddata.energyCost) && amount == -1)
        {
            gameManager.GetWhosTurn().ChangeEnergy(1);
            energyBidAmount--;
        }
        //player has bid some energy and wants to go higher, make sure player doesnt exceed how much energy they have
        else if(energyBidAmount > 0 && gameManager.GetWhosTurn().GetEnergy() >0 && amount == 1)
        {
            gameManager.GetWhosTurn().ChangeEnergy(-1);
            energyBidAmount++;
        }

        energyBidText.text = ""+energyBidAmount;
    }

    public void SetCardData(CardData data)
    {
        carddata = data;
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.color = data.cardColor; 
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
                    if (player.GetEnergy() >= Math.Abs(carddata.energyCost))
                    {

                        player.ChangeEnergy(carddata.energyCost);
                        gameObject.SetActive(false);
                        isPlayable = false;
                        player.discardPile.Add(this);
                        player.hand.Remove(this);
                        switch (carddata.cardEffect)
                        {
                            case CardData.CardEffect.Attack:
                                ResolveAttack(carddata.statValue);
                                break;
                            case CardData.CardEffect.Armor:
                                ResolveShield(carddata.statValue);
                                break;
                            case CardData.CardEffect.Energy:
                                player.ChangeEnergy(carddata.statValue);
                                break;
                            case CardData.CardEffect.AttackNArmor:
                                ResolveAttack(carddata.values[0]);
                                ResolveShield(carddata.values[1]);
                                break;
                            case CardData.CardEffect.StrengthUp:
                                player.ChangeStrength(carddata.statValue);
                                //Debug.Log(carddata.statValue);
                                break;
                            case CardData.CardEffect.ShieldUp:
                                player.ChangeShieldBonus(carddata.statValue);
                                break;
                            case CardData.CardEffect.BaseEnergyUp:
                                player.ChangeBaseEnergy(carddata.statValue);
                                break;
                            case CardData.CardEffect.Unblockable:
                                ResolveUnblockableAttack(carddata.statValue);
                                break;
                            case CardData.CardEffect.SelfInflict:
                                ResolveAttack(carddata.values[0]);
                                ResolveSelfInflict(carddata.values[1]);
                                break;
                            case CardData.CardEffect.ShieldBreaker:
                                ResolveShieldBreaker(carddata.statValue);
                                break;
                        }

                    }
                }


            }
            else
            {
                SetButtonUIActive();
            }
        }
        //if the card has a player check if the turn is over - will gotta change 
        if (player != null)
        {
            player.CheckIfTurnOver();
        }
    }


    public void SetPlayable(bool option)
    {
        isPlayable = option;
    }

    void DisplayText()
    {
        cardText.text = "energy" + carddata.energyCost;
        cardDescription.text = carddata.cardDescription;
    }

    void ResolveAttack(int amount)
    {
        // takes into account opposition shield and the strength of the player
        int trueAttack = amount - Math.Abs(player.GetStrength());
        if (opponent.GetShields() > 0)
        {
            if (Math.Abs(trueAttack) > opponent.GetShields())
            {
                trueAttack = trueAttack + opponent.GetShields();
                opponent.ChangeShields(opponent.GetShields());
                opponent.ChangeHealth(trueAttack);
            }
            else
            {
                opponent.ChangeShields(trueAttack);
            }
        }
        else
        {
            opponent.ChangeHealth(trueAttack);
        }
    }
    void ResolveShield(int amount)
    {
        player.ChangeShields(amount + player.GetShieldBonus());
    }
    void ResolveUnblockableAttack(int amount)
    {
        opponent.ChangeHealth(amount);
    }
    void ResolveSelfInflict(int amount)
    {
        if (player.GetShields() > 0)
        {
            if (Math.Abs(amount) > player.GetShields())
            {
                amount = amount + player.GetShields();
                player.ChangeShields(player.GetShields());
                player.ChangeHealth(amount);
            }
            else
            {
                player.ChangeShields(amount);
            }
        }
        else
        {
            player.ChangeHealth(amount);
        }
    }
    void ResolveShieldBreaker(int amount)
    {
        // have this card say it does for each damage to shield, does one more (if they still have shields)

        // so strenght shouldnt be doubled
        // check whether the opponent has enough to firm the damage
        // then if not get rid of all shields n leftover damage
        if (opponent.GetShields() >= Math.Abs(amount * 2))
        {
            ResolveAttack(amount * 2);
        }
        else
        {
            while (opponent.GetShields() > 0 || amount < 0)
            {
                opponent.ChangeShields(1);
                if (opponent.GetShields() > 0)
                {
                    opponent.ChangeShields(1);
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
