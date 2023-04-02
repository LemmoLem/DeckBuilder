using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoButton : MonoBehaviour
{
    public Card card;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnButtonPress()
    {
        card.ExitBid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
