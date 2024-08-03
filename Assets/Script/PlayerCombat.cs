using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerCombat : MonoBehaviour
{
    [Header("Shoot Settings")]
    private float raycastDistance = 50f;
    public LayerMask targetLayer;
    public Transform shotPoint;
    public float fireRate = 0.5f;
    public float bulletCount;
    public float maxBuletCount;
    public float rechargeTime;
    public Image bulletFill;
    private bool isRecharge;
    [SerializeField] ParticleSystem inkParticle;
    [SerializeField] Material inkMaterialParticle;
    [SerializeField] ParticlesController particlesController;
    public AudioClip shootClip;

    [Header("Special Attque Settings")]
    public Transform attackPoint;
    public float attackRange;
    public float attackRangeIncrement = 0.1f;
    public float attackFrequency = 0.1f;
    public Transform area;
    public int enemyDamage;
    public float dashSpeed = 50f;
    public float damageDelay = 0.1f;
    public bool isDoubleAttacking;
    public bool isSpecialAttacking;
    

    public static PlayerCombat instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerMovement dans la scène");
            return;
        }
        instance = this;
    }


    private float nextFireTime = 0f; // Temps du prochain tir possible

    private void Start()
    {
        bulletCount = maxBuletCount;
    }
    private void Update()
    {
       bulletFill.fillAmount = bulletCount / maxBuletCount;
        area.localScale = new Vector3(attackRange, attackRange, 0f);
        area.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.57f, gameObject.transform.position.z);

        if (Input.GetMouseButton(1) && Time.time >= nextFireTime && bulletCount > 0 && !isDoubleAttacking && !isRecharge) // Clic gauche maintenu et délai écoulé
        {
            StartCoroutine(Shoot());
            bulletCount--;
            nextFireTime = Time.time + fireRate;
            inkParticle.Play();
            Color randomColor = GetRandomColorParticule();
            particlesController.paintColor = randomColor;
            inkMaterialParticle.color = randomColor;// Définir le temps du prochain tir
        }
      
        if(Input.GetMouseButton(1) && Time.time >= nextFireTime && bulletCount > 0 && isDoubleAttacking && !isRecharge)
        {

            StartCoroutine(DoubleShoot());
            bulletCount--;
            nextFireTime = Time.time + fireRate;           
        }
        



        if (Input.GetKeyDown(KeyCode.R) && bulletCount == 0) 
        {
            StartCoroutine(Recharge());
        }

        if (Input.GetKeyDown(KeyCode.T)) 
        {
            StartCoroutine(EnlargeAreaAttack());
           
        }

        if (Input.GetKeyUp(KeyCode.T)) 
        {
            StartCoroutine(PerformAttack());

        }



    }

    Color GetRandomColorParticule()
    {
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        return randomColor;
    }
    private IEnumerator Shoot()
    {
        AudioManager.instance.PlayClipAt(shootClip, transform.position);
        Vector3 rayDirection = shotPoint.forward;

        RaycastHit hit;
        if (Physics.Raycast(shotPoint.position, rayDirection, out hit, raycastDistance, targetLayer))
        {

            // Ajouter votre logique ici pour gérer l'impact
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(enemyDamage);
            }
        }

        inkParticle.Play();
        yield return new WaitForSeconds(0.2f);
        inkParticle.Stop();
    }

    private IEnumerator DoubleShoot() 
    {
        AudioManager.instance.PlayClipAt(shootClip, transform.position);

        Vector3 rayDirection = shotPoint.forward;

        RaycastHit hit;
        if (Physics.Raycast(shotPoint.position, rayDirection, out hit, raycastDistance, targetLayer))
        {

            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(enemyDamage);
            }
        }
        inkParticle.Play();
        Color randomColor = GetRandomColorParticule();
        particlesController.paintColor = randomColor;
        inkMaterialParticle.color = randomColor;

        yield return new WaitForSeconds(0.2f);
        inkParticle.Stop();
        yield return new WaitForSeconds(0.1f);
        AudioManager.instance.PlayClipAt(shootClip, transform.position);

        if (Physics.Raycast(shotPoint.position, rayDirection, out hit, raycastDistance, targetLayer))
        {

            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(enemyDamage);
            }
        }

        inkParticle.Play();
        yield return new WaitForSeconds(0.2f);
        inkParticle.Stop();
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        // Dessiner une sph�re visuelle pour repr�senter la port�e de l'attaque
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    public IEnumerator Recharge()
    {
        isRecharge = true;
        float elapsed = 0f;
        float bulletsToAddPerStep = maxBuletCount / rechargeTime;

        while (bulletCount < maxBuletCount)
        {
            
            bulletCount += bulletsToAddPerStep * Time.deltaTime;
            bulletCount = Mathf.Clamp(bulletCount, 0, maxBuletCount);
            bulletFill.fillAmount = bulletCount / maxBuletCount;
            elapsed += Time.deltaTime;
            yield return null;
        }

        isRecharge = false;
    }


    private IEnumerator PerformAttack()
    {
        isSpecialAttacking = true;
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, targetLayer);
        attackRange = 0f;
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy != null)
            {
                if (enemy.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
                {
                    Vector3 originalPosition = transform.position;
                    Vector3 enemyPosition = enemy.transform.position;

                    float startTime = Time.time;
                    while (Vector3.Distance(transform.position, enemyPosition) > 0.1f)
                    {
                        transform.position = Vector3.Lerp(originalPosition, enemyPosition, (Time.time - startTime) * dashSpeed);
                        yield return null;
                    }

                    yield return new WaitForSeconds(damageDelay);
                    enemyHealth.TakeDamage(3);
                }
            }
        }

        isSpecialAttacking = false;
        attackRange = 0f;
    }
    IEnumerator EnlargeAreaAttack() 
    {
        while (Input.GetKey(KeyCode.T))
        {
            attackRange += attackRangeIncrement;
            yield return new WaitForSeconds(attackFrequency);
        }
    }
}
