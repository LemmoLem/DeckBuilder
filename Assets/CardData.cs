using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    /* so  different cards work. they make use of card data. what now. 
     * either implement different card function? implement auction system?
     * implement other game mechanics - player dying if they lose health, attacks focusing on shield first?
     * change how river spawns cards. scriptable objects will mean can do a upgrade system
     * 
     */
    public int energyCost;
    public int statValue;
    public enum CardEffect
    {
        Attack = 30,
        Armor = 5,
        Energy = 10,
        StrengthUp = 11,
        ShieldUp = 0,
        BaseEnergyUp = 6,
        Unblockable = 29,
        ShieldBreaker = 28,
        Shield5Turn = 7,
        LimitedUse = 35,
        Attack5 = 31,
        AddModule = 34,
        Discard = 33,
        PlayRandomCard = 32
    }
    public CardEffect cardEffect;
    public Sprite cardArt;
    private GameManager gameManager;
    PlayerController player;
    PlayerController opponent;
    public Color cardColor;
    public CardData addModule;
}
