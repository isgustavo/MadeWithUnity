using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [SerializeField]
    int itemID;
    public int ItemID { get { return itemID; }}
    [SerializeField]
    int count;

    public virtual bool CanUse() => count > 0;

    public virtual void Use()
    {
        
    }
}

public class UsableItem : PlayerItem
{

}