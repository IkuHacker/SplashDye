using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 2f; // Durée de vie de la balle
    private Vector3 direction;
    private float speed;

    public void SetDirection(Vector3 _direction, float _speed)
    {
        direction = _direction;
        speed = _speed;
    }

    private void Start()
    {
        // Détruire la balle après un certain temps pour éviter d'encombrer la scène
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Déplacer la balle sans affecter l'axe Y
        transform.position += direction * speed * Time.deltaTime;
    }



}
