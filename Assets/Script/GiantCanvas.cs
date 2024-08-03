using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantCanvas : MonoBehaviour
{
    public PlayerCombat playerCombat;
    public ParticleSystem inkParticleAttack;
    [SerializeField] Material inkMaterialParticle;
    [SerializeField] ParticlesController particlesController;
    void Update()
    {
        if (playerCombat.isSpecialAttacking) 
        {
            inkParticleAttack.Play();
            StartCoroutine(Paint());
        }
        else 
        {
            inkParticleAttack.Stop();
            StopCoroutine(Paint());
        }
    }

    public IEnumerator Paint()
    {
        yield return new WaitForSeconds(0.5f);
        Color randomColor = GetRandomColorParticule();
        particlesController.paintColor = randomColor;
        inkMaterialParticle.color = randomColor;// Définir le temps du prochain tir

    }

    Color GetRandomColorParticule()
    {
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        return randomColor;
    }

}
