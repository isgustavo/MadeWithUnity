using System.Collections.Generic;
using UnityEngine;

public class SlotRefs : MonoBehaviour
{
    [SerializeField]
    List<Transform> slots;
    Dictionary<string, Transform> slotsDictionary = new Dictionary<string, Transform>();
    
    void Awake()
    {
        foreach (Transform slot in slots)
        {
           slotsDictionary.Add(slot.name, slot);
        }
    }

    public Transform TryGetParentWithName(string name)
    {
        if(slotsDictionary.ContainsKey(name))
        {
            return slotsDictionary[name];
        }
        return null;    
    }
}
