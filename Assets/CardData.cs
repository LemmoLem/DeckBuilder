using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewCard", menuName = "CardData/BasicCard")]
public class CardData : ScriptableObject
{
    /* https://www.youtube.com/watch?v=aPXvoWVabPY&t=7s
    https://ldjam.com/events/ludum-dare/51/justin-time-man-of-the-hour/building-extensible-systems-with-unitys-scriptable-objects
    https://gamedev.stackexchange.com/questions/204067/idiomatic-way-to-create-card-instances-in-unity
    https://www.reddit.com/r/Unity3D/comments/ssl5wn/cards_game_unique_effects_and_scriptable_objects/
    https://levelup.gitconnected.com/tip-of-the-day-scriptable-objects-101-in-unity-a1730c911b4d
    https://gamedevbeginner.com/scriptable-objects-in-unity/
    https://stackoverflow.com/questions/71512234/why-cant-i-drag-and-drop-a-sprite-in-a-scriptable-objects-inspector
    */

    /* so card data shouldnt replace card. it should be used by card
     * card texts works now
     * next work on reformating game 
     * making card data work for different types of game cards
     * gameplay, card data and cards
     */
    public bool isPlayable;
    public bool isInDeck;
    public int energyCost;
    public int damage;
    public int shield;
    public string card description;
    public enum CardEffect { Attack, Armor, Energy };
    public CardEffect cardEffect;
    public Sprite cardArt;
    private GameManager gameManager;
    PlayerController player;
    PlayerController opponent;

    //have so image inside card design and there is description below it
    //so artwork for each but inside other sprite. this is shown in brackeys video , top link. ye



    

    // Start is called before the first frame update
    void Start()
    {
        isPlayable = true;
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    /*
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
                    player.ChangeEnergy(energyCost);
                    gameObject.SetActive(false);
                    isPlayable = false;
                    player.discardPile.Add(this);
                    player.hand.Remove(this);
                    switch (cardType)
                    {
                        case CardType.Attack:
                            //have to make it so considers armor and everything
                            opponent.ChangeHealth(-1);
                            break;
                        case CardType.Armor:
                            player.ChangeShields(1);
                            break;
                        case CardType.Energy:
                            player.ChangeEnergy(2);
                            break;

                    }

                }


            }
            else
            {
                //check whos turn and add it to discard pile. card is being collected from river. gotta remove self from river as well
                player = gameManager.GetWhosTurn();
                opponent = gameManager.GetWhosNotsTurn();
                gameManager.RemoveCardFromRiver(this);
                player.discardPile.Add(this);
                player.ChangeEnergy(energyCost);
                gameObject.SetActive(false);
                isPlayable = false;
                isInDeck = true;

            }
        }
        player.CheckIfTurnOver();
    }
    */
    void PlayCard()
    {
        //need way to know player n that !!!!!!!!!!!!
    }
    public void SetPlayable(bool option)
    {
        isPlayable = option;
    }



}
