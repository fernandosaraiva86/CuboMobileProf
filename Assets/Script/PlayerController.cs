using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Player Movement")]    
    private Rigidbody rb;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxSpeed = 5f;
    private Vector2 movementInput;

    [Header("Player Destruction")]
    public ParticleSystem destructionParticle;//Particula de destruição do player
    private CinemachineImpulseSource _impulseSource;

    [Header("Câmeras")]
    public CinemachineCamera cam;
    public CinemachineCamera camZoom;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        //Se entrar aqui não deixa o player se mover
        if(GameManager.Instance == null || GameManager.Instance.isGameOver)
        {
            return; 
        }
        
        Vector3 moveDirection = 
            new Vector3(movementInput.x, 0, movementInput.y)* speed;

        if (rb.linearVelocity.magnitude < maxSpeed)
        {

            rb.linearVelocity = new Vector3(
                moveDirection.x, rb.linearVelocity.y, moveDirection.z);
        }
    }

    private void OnCollisionEnter(Collision other) { 

        if (other.gameObject.CompareTag("Obstacle"))
        {
            cam.gameObject.SetActive(false);
            camZoom.gameObject.SetActive(true);
            
            GameManager.Instance.isGameOver = true; //Seta o game over para true
            Instantiate(destructionParticle, transform.position, Quaternion.identity);
            _impulseSource.GenerateImpulse();
            Destroy(gameObject);
        }
    }
}
