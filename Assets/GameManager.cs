using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;


/*
    

    ways to improve:
        will have slay the spire type thing of giving opponents cards like thorns and that, unplayable curse ykno
        Have cards in river spawned with offset with rotation and position to get messy look, could be good (could also be stuff)
        A card that makes cards go straight to hand rather than discard pile

*/

public class GameManager : MonoBehaviour
{
    public PlayerController thePlayer;
    public NPCController npc;
    public River river;
    List<Card> top = new List<Card>();
    List<Card> middle = new List<Card>();
    List<Card> bottom = new List<Card>();
    List<List<Card>> riverCards = new List<List<Card>>();
    List<Card> riverDrawPile = new List<Card>();
    List<Card> riverDiscardPile = new List<Card>();
    public int riverLength;
    public Card card, card2, card3, card4, card5, card6;
    public UnityEngine.UI.Text thePlayerText, npcText;
    public Card cardPrefab;
    public TextMeshProUGUI gameOverText;
    public List<CardData> cardDatas = new List<CardData>();
    private int turnCount = 1;
    public List<CardData> initialList = new List<CardData>();
    public List<CardData> midList = new List<CardData>();
    public List<CardData> midToEndList = new List<CardData>();
    public List<CardData> endList = new List<CardData>();
    List<CardData> modules = new List<CardData>();
    List<CardData> afterEndList = new List<CardData>();
    public CardData baseAddModuleCard;
    // Start is called before the first frame update




    void Start()
    {
        // river cards is a list of lists. cards are added into top, middle and bottom by accesing that
        riverCards.Add(top);
        riverCards.Add(middle);
        riverCards.Add(bottom);
        GameCardRamp();
        //
        SpawnRiver();
        thePlayer.SetOpponent(npc);
        npc.SetOpponent(thePlayer);


        List<int> intlist = new List<int>();


        for (int i = 0; i < 10; i++) {
            int num5 = UnityEngine.Random.Range(0, 9);
            bool isadded = false;
            int count = 0;
            if (intlist.Count == 0)
            {
                intlist.Add(num5);
                isadded = true;
            }
            while (!isadded)
            {
                //if priority at the index is more than the data then add it where that one is
                if (intlist[count] > num5)
                {
                    intlist.Insert(count, num5);
                    isadded = true;
                }
                count++;
                if (count == intlist.Count && !isadded)
                {
                    intlist.Add(num5);
                    isadded = true;
                }

            }
        }
        //Debug.Log("pring full list now");
        for (int j =0; j < intlist.Count; j++)
        {
            //Debug.Log(intlist[j]);
        }


    }

    // Update is called once per frame
    void Update()
    {
        DisplayText();
    }

    void SpawnRiver()
    {
        // this method should maybe changed to be like how spawn row works n be like full of nulls
        if (riverCards[0].Count == 0 && riverCards[1].Count == 0 && riverCards[2].Count == 0)
        {
            for (int j = 0; j < 3; j++)
            {


                for (int i = 0; i < 3; i++)

                {
                    
                    riverDrawPile[0].transform.position = river.GetRiverSlots()[j][i].position;
                    riverDrawPile[0].gameObject.SetActive(true);
                    riverCards[j].Add(riverDrawPile[0]);
                    riverDrawPile.RemoveAt(0);
                }
            }

        }
        else
        {

            // so if theres 3 cards in top, the last in there should be moved to the right most 
            // make so it moves cards to the right and then add cards
            // or have spawn cards script and add cards to it script
        }
        
    }
    void AddNewCardsToDrawPile(int amount)
    {
        while (amount != 0) {
            Card card = Instantiate(cardPrefab);
            int num = UnityEngine.Random.Range(0, cardDatas.Count);
            //Debug.Log(num);
            card.AddCardData(cardDatas[num]);
            int num2 = UnityEngine.Random.Range(0, 3);
            for (int i = 0; i < num2; i++)
            {
                int num3 = UnityEngine.Random.Range(0, cardDatas.Count);
                card.AddCardData(cardDatas[num3]);
            }

            riverDrawPile.Add(card);
            card.gameObject.SetActive(false);
            amount = amount - 1;
        }
    }

    void GameCardRamp()
    { 
        // SO CALL THIS AT START OF END TURN OR SMTH. and have it so adds new module every few turns (use modulus)
        // and then use > 5 or > 10 to determine how many modules can be on a card

        // so initially should spawn a few cards
        // modules is what add module to
        if (turnCount == 1)
        {
            // so fill modules from initial list. then add buff versions of list to next list, and then add addmodules of that card
            modules.AddRange(initialList);
            initialList.Clear();
            List<CardData> newInitials = BuffScriptedAssets(initialList);
            midList.AddRange(newInitials);
            midList.AddRange(AddNewAddModulesFromBase(initialList));
            List<CardData> newMids = BuffScriptedAssets(midList);
            midToEndList.AddRange(newMids);
            midToEndList.AddRange(AddNewAddModulesFromBase(midList));
            List<CardData> newMidsToEnd = BuffScriptedAssets(midToEndList);
            endList.AddRange(newMidsToEnd);
            endList.AddRange(AddNewAddModulesFromBase(midToEndList));
            List<CardData> newEnds = BuffScriptedAssets(endList);
            afterEndList.AddRange(endList);
            afterEndList.AddRange(AddNewAddModulesFromBase(endList));
            FillDrawPileFromCards(25, modules, 1, 3);

        }
        // so then for next turns should start adding modules n that

        if (initialList.Count > 0)
        {
            CardData temp = initialList[UnityEngine.Random.Range(0, initialList.Count)];
            initialList.Remove(temp);
            modules.Add(temp);
        }
        else if (midList.Count > 0)
        {
            CardData temp = midList[UnityEngine.Random.Range(0, midList.Count)];
            midList.Remove(temp);
            modules.Add(temp);
        }
        else if (midToEndList.Count > 0)
        {
            CardData temp = midToEndList[UnityEngine.Random.Range(0, midToEndList.Count)];
            midToEndList.Remove(temp);
            modules.Add(temp);
        }
        else if (endList.Count > 0)
        {
            CardData temp = endList[UnityEngine.Random.Range(0, endList.Count)];
            endList.Remove(temp);
            modules.Add(temp);
        }
        else if (afterEndList.Count > 0)
        {
            CardData temp = afterEndList[UnityEngine.Random.Range(0, afterEndList.Count)];
            afterEndList.Remove(temp);
            modules.Add(temp);
        }


        if (turnCount > 1 && turnCount < 5 && riverDrawPile.Count < 6)
        {
            FillDrawPileFromCards(3, modules, 1, 3);
        }

        if (turnCount > 5 && turnCount < 10 && riverDrawPile.Count < 6)
        {
            FillDrawPileFromCards(3, modules, 2, 5);
        }

        if (turnCount > 10 && turnCount < 15 && riverDrawPile.Count < 8)
        {
            FillDrawPileFromCards(2, modules, 2, 7);
        }
        if (turnCount > 15 && turnCount < 20 && riverDrawPile.Count < 8)
        {
            FillDrawPileFromCards(2, modules, 3, 7);
        }
        if (turnCount > 20 && riverDrawPile.Count < 8)
        {
            FillDrawPileFromCards(1, modules, 2, 7);
            FillDrawPileFromCards(1, modules, 6, 10);
        }
        // then after initial cards, spawn new cards which half include new modules - baso add two modules each time
        // then keep doing it till run out
        // then add beefier modules n keep doing that
    }


    List<CardData> BuffScriptedAssets(List<CardData> weakestDatas)
    {
        // so this takes from modules and then mulitplies the energy cost and the statvalue
        // if energy is 0 then ignore multiplying that
        List<CardData> buffedList = new List<CardData>();
        foreach (CardData data in weakestDatas)
        {
            var newData = Instantiate(data);
            if (data.energyCost != 0)
            {
                newData.energyCost *= 2;
            }
            newData.statValue = newData.statValue * 2 + 1;
            buffedList.Add(newData);
        }

        return buffedList;
    }

    List<CardData> AddNewAddModulesFromBase(List<CardData> toBeAddedModuleDatas) 
    {
        List<CardData> newModules = new List<CardData>();
        foreach(CardData data in toBeAddedModuleDatas)
        {
            if (data.cardEffect != CardData.CardEffect.AddModule)
            {
                var newData = Instantiate(baseAddModuleCard);
                newData.addModule = data;

            }
        }
        return newModules;
    }

    void FillDrawPileFromCards(int amountOfCards, List<CardData> datas, int MinAmountOfModulesIncl, int MaxAmountOfModulesExcl)
    {
        while (amountOfCards != 0)
        {
            Card card = Instantiate(cardPrefab);
            int num = UnityEngine.Random.Range(0, datas.Count);
            //Debug.Log(num);
            // so min is actually like above zero
            int num2 = UnityEngine.Random.Range(MinAmountOfModulesIncl, MaxAmountOfModulesExcl);
            for (int i = 0; i < num2; i++)
            {
                int num3 = UnityEngine.Random.Range(0, datas.Count);
                card.AddCardData(datas[num3]);
            }
            riverDrawPile.Add(card);
            card.gameObject.SetActive(false);
            amountOfCards--;
        }
    }

    void FillDrawPile()
    {
        for (int i = 0; i < riverLength/6; i++)
        {
            riverDrawPile.Add(Instantiate(card));
            riverDrawPile.Add(Instantiate(card2));
            riverDrawPile.Add(Instantiate(card3));
            riverDrawPile.Add(Instantiate(card4));
            riverDrawPile.Add(Instantiate(card5));
            riverDrawPile.Add(Instantiate(card6));
        }
    }

    void DisplayText()
    {
        thePlayerText.text = "Player stats: \nDraw Pile: " + thePlayer.GetDrawPileLength() + "\nDiscard Pile: "
            + thePlayer.GetDiscardPileLength() + "\nHealth: " + thePlayer.GetHealth() + "\nEnergy: " + thePlayer.GetEnergy()
            + "\nShields: " + thePlayer.GetShields() + "\nStrength: " + thePlayer.GetStrength() + "\nShield5: " + thePlayer.GetShield5String() + "\nDamage5: " + thePlayer.GetDamage5String() 
            + "\nDamage now: " + thePlayer.GetDamageNow() + "\nShield break now: " + thePlayer.GetShieldBreakNow() + "\nUnblock now: " + thePlayer.GetUnblockNow();


        npcText.text = "AI stats: \nDraw Pile:  " + npc.GetDrawPileLength() + "\nDiscard Pile: "
                    + npc.GetDiscardPileLength() + "\nHealth: " + npc.GetHealth() + "\nEnergy: " + npc.GetEnergy()
                    + "\nShields: " + npc.GetShields() + "\nStrength: " + npc.GetStrength() + "\nShield5: " + npc.GetShield5String() + "\nDamage5: " + npc.GetDamage5String()
                    + "\nDamage now: " + npc.GetDamageNow() + "\nShield break now: " + npc.GetShieldBreakNow() + "\nUnblock now: " + npc.GetUnblockNow();

    }

    public PlayerController GetThePlayer()
    {
        return thePlayer;
    }
    public NPCController GetTheNPC()
    {
        return npc;
    }
 
    public void NextTurn()
    {
        npc.PlayTurn();
        // so go through each card and check if bids
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                if (riverCards[j][i] != null)
                {
                    riverCards[j][i].ConfirmBid();
                }
            }
        }
        thePlayer.EndTurn();
        npc.EndTurn();
        GameCardRamp();
        turnCount++;
        if (turnCount%2 == 0)
        {
            MoveCardsRight();
            SpawnColumnOfCards();
        }
        thePlayer.StartTurn();
        npc.StartTurn();
        if (thePlayer.GetHealth() < 1)
        {
            SceneManager.LoadScene("Loss Screen");
        }
        else if(npc.GetHealth() < 1)
        {
            SceneManager.LoadScene("Win screen");
        }
    }

    void SpawnColumnOfCards()
    {
        for (int j = 0; j < 3; j++)
        {
            if (riverCards[j][0] == null)
            {
                if (riverDrawPile.Count > 0)
                {
                    riverDrawPile[0].transform.position = river.GetRiverSlots()[j][0].position;
                    riverDrawPile[0].gameObject.SetActive(true);
                    riverCards[j][0] = riverDrawPile[0];
                    riverDrawPile.RemoveAt(0);
                }
            }
        }
    }

    void MoveCardsRight()
    {
        // should now go from right to left and check each if equal to null or not
        for (int j = 0; j < 3; j++)
        {
            for (int i = riverCards[j].Count - 1; i > 0; i--)
            {
                // if the card to left is null then it wont have a position so ignore it
                if (riverCards[j][i] == null && riverCards[j][i-1] != null)
                {
                    // swap position in list and set transform position to one to the left
                    riverCards[j][i-1].transform.position = river.GetRiverSlots()[j][i].position;
                    riverCards[j][i] = riverCards[j][i - 1];
                    riverCards[j][i-1] = null;
                    int printi = i - 1;

                }
            }
            
        }

    }
    
    public int GetRiverCardLength()
    {
        int sum = 0;
        for (int j = 0; j < riverCards.Count; j++)
        {
            for (int i = 0; i < riverCards[j].Count; i++)
            {
                if (riverCards[j][i] != null)
                {
                    sum++;
                }
            }
        }
        return sum;
    }
    public void RemoveCardFromRiver(Card card)
    {
        for(int i = 0; i<riverCards.Count; i++)
        {
            if (riverCards[i].Contains(card))
            {
                int index = riverCards[i].IndexOf(card);
                riverCards[i][index] = null;

            }
        }
    }

    public List<Card> GetRiverCards()
    {
        List<Card> cards = new List<Card>();
        for (int j = 0; j < riverCards.Count; j++)
        {
            for (int i = 0; i < riverCards[j].Count; i++)
            {
                if (riverCards[j][i] != null)
                {
                    cards.Add(riverCards[j][i]);
                }
            }
        }

        return cards;
    }

   

}
