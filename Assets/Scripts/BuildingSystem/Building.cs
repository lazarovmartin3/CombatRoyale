using UnityEngine;

public class Building : MonoBehaviour
{
    protected bool isSelected;

    public void Select()
    {
        isSelected = true;
    }

    public void Deselect()
    {
        isSelected = false;
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    protected float max_HP;
    protected float current_HP;
    protected float damage;
}
