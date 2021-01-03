using UnityEngine;

public class WeaponItem : PlayerItem
{
    [SerializeField]
    UsableItem ammunition;
    [SerializeField]
    int ammunitionTotalCount;
    [SerializeField]
    int ammunitionCurrentCount;
    [SerializeField]
    int ammunitionReloadCount;
    

    public override bool CanUse() => ammunitionCurrentCount > 0;
    
    public override void Use()
    {
        ammunitionCurrentCount -= 1;
    }

    public virtual void Reload ()
    {
        if(ammunitionTotalCount > ammunitionReloadCount)
        {
            ammunitionCurrentCount = ammunitionReloadCount;
            ammunitionTotalCount -= ammunitionReloadCount;
        } else 
        {
            ammunitionCurrentCount = ammunitionTotalCount;
            ammunitionTotalCount = 0;
        }
    }
}
