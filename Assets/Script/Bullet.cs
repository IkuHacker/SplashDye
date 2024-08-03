using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 2f; // Dur�e de vie de la balle
    private Vector3 direction;
    private float speed;

    public void SetDirection(Vector3 _direction, float _speed)
    {
        direction = _direction;
        speed = _speed;
    }

    private void Start()
    {
        // D�truire la balle apr�s un certain temps pour �viter d'encombrer la sc�ne
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // D�placer la balle sans affecter l'axe Y
        transform.position += direction * speed * Time.deltaTime;
    }



}
