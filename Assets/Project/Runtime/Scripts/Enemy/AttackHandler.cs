using UnityEngine;
using System;

public class AttackHandler : MonoBehaviour
{
    public event Action OnAttack;

    public void Attack()
    {
        if (OnAttack != null)
        {
            OnAttack.Invoke();
        }
    }
}
