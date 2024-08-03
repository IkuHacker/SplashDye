using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    public void AddSpeed(float speedGiven, float speedDuration)
    {
        PlayerMovement.instance.moveSpeed += speedGiven;
        StartCoroutine(RemoveSpeed(speedGiven, speedDuration));
    }

    private IEnumerator RemoveSpeed(float speedGiven, float speedDuration)
    {
        yield return new WaitForSeconds(speedDuration);
        PlayerMovement.instance.moveSpeed -= speedGiven;
    }

    public void AddDoubleShoot(bool canDoubleShot, float duration)
    {
        PlayerCombat.instance.isDoubleAttacking = canDoubleShot;
        StartCoroutine(RemoveDoubleShoot(duration));
    }

    private IEnumerator RemoveDoubleShoot(float duration)
    {
        yield return new WaitForSeconds(duration);
        PlayerCombat.instance.isDoubleAttacking = false;

    }

    public void AddDamageShoot(int damage, float duration)
    {
        PlayerCombat.instance.enemyDamage = damage;
        StartCoroutine(RemoveDamageShoot(duration));
    }

    private IEnumerator RemoveDamageShoot(float duration)
    {
        yield return new WaitForSeconds(duration);
        PlayerCombat.instance.enemyDamage = 1;

    }

    public void AddTimeCounte(float time)
    {
        Timer.instance.AddTimer(time);
    }

}
