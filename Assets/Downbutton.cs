using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Downbutton : MonoBehaviour
{
    // Start is called before the first frame update
    public Card card;
    void Start()
    {

    }
    public void OnButtonPress()
    {
        card.ChangeBidEnergyAmount(-1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
