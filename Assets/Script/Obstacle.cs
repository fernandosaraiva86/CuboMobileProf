using Unity.Cinemachine;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Vector3 rotation;
    public ParticleSystem destructionParticle;//Particula de destruição do objstáculo
    private CinemachineImpulseSource _impulseSource;
    private PlayerController playerController;

    private void Start()
    {
        var xRotation = Random.Range(90f, 180f);

        rotation = new Vector3(xRotation, 0);
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        playerController = FindAnyObjectByType<PlayerController>();//Busca o script player
    }

    private void Update()
    {
        transform.Rotate(rotation * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Obstacle"))
        {
            Instantiate(destructionParticle, transform.position, Quaternion.identity);

            if (playerController != null)
            {
                //====================Metodos força da câmera====================//
                var distance =
                    Vector3.Distance(transform.position, playerController.transform.position);

                var force = 1 / distance;

                _impulseSource.GenerateImpulse(force);
                //=================================================================//
            }
            
            
            Destroy(gameObject);
        }
    }




}
