using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River : MonoBehaviour
{
    public int amount;
    public Transform[] top, middle, bottom;
    public Transform[] slots;
    public List<Transform[]> riverSlots = new List<Transform[]>();

    // Start is called before the first frame update
    void Start()
    {
        riverSlots.Add(top);
        riverSlots.Add(middle);
        riverSlots.Add(bottom);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<Transform[]> GetRiverSlots()
    {
        return riverSlots;
    }
}
