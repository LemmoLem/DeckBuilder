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

    // Start is called before the first frame update
    void Start()
    {
        isPlayable = true;
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayText();
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
                                Debug.Log(carddata.statValue);
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
                        }

                    }
                }


            }
            else
            {
                //check whos turn and add it to discard pile. card is being collected from river. gotta remove self from river as well
                player = gameManager.GetWhosTurn();
                opponent = gameManager.GetWhosNotsTurn();
                if (player.GetEnergy() >= Math.Abs(carddata.energyCost))
                {
                    gameManager.RemoveCardFromRiver(this);
                    player.discardPile.Add(this);
                    player.ChangeEnergy(carddata.energyCost);
                    gameObject.SetActive(false);
                    isPlayable = false;
                    isInDeck = true;
                }
            }
        }
        player.CheckIfTurnOver();
    }

    void PlayCard()
    {
        //need way to know player n that !!!!!!!!!!!!
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
}
