using UnityEngine;

public class EikoComboAbility : AbilitySO
{
    public override void Activate(GameObject parent)
    {
        parent.transform.position = new Vector3(0, 0, 0);
    }
}
