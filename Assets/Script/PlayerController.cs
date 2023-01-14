using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables o propiedades
    public float moveSpeed = 5f;
    public float rotateSpeed = 60f;
    public float currentMoveSpeed;
    public float currentRotateSpeed;
    public float jumpSpeed = 5f;

    private float hInput;
    private float vInput;
    private Rigidbody _rb;

    public float distanceToGround = 0.1f;
    public LayerMask groundLayer;
    private CapsuleCollider _col;

    public GameObject bullet;
    public Transform shootPoint;
    public float bulletSpeed = 100.0f;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _col = GetComponent<CapsuleCollider>();

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        currentMoveSpeed = moveSpeed;
        currentRotateSpeed = rotateSpeed;
}

    // Update is called once per frame
    void Update()
    {

        // Correr con Control
        if (0.5 < Input.GetAxis("Fire1"))
        {
            currentMoveSpeed = moveSpeed * 2.0f;
            currentRotateSpeed = rotateSpeed * 2.0f;
        } else if (0.5 < Input.GetAxis("Fire3")) // Moverse mas lento con shift
        {
            currentMoveSpeed = moveSpeed / 2.0f;
            currentRotateSpeed = rotateSpeed / 2.0f;
        }
        else
        {
            currentMoveSpeed = moveSpeed;
            currentRotateSpeed = rotateSpeed;
        }

        //Calculo de la velocidad a partir del hardware
        hInput = Input.GetAxis("Horizontal") * currentRotateSpeed;
        vInput = Input.GetAxis("Vertical") * currentMoveSpeed;

        //Instanciar una nueva bala
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newBullet = Instantiate(bullet
                                                , shootPoint.position
                                                , shootPoint.rotation) as GameObject;
            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();
            bulletRB.velocity = shootPoint.forward * bulletSpeed;
        }

        if (IsOnTheGround() && Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
        
        /* Forma de mover al personaje sin el motor de físicas
        // S = i * v * t
        this.transform.Translate(Vector3.forward * vInput * Time.deltaTime);
        this.transform.Rotate(Vector3.up * hInput * Time.deltaTime);
        */
    }

    private void FixedUpdate()
    {
        Vector3 rotation = Vector3.up * hInput;
        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);

        _rb.MovePosition(this.transform.position + 
                         this.transform.forward * vInput * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation * angleRot);
    }

    /// <summary>
    /// Comprueba si el personaje este tocando el suelo
    /// </summary>
    /// <returns></returns>
    private bool IsOnTheGround()
    {

        //Cacular el punto más bajo del personaje
        Vector3 capsuleBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);

        //Comprueba si esta en el piso
        bool onTheGround = Physics.CheckCapsule(_col.bounds.center, 
                                                capsuleBottom, 
                                                distanceToGround, 
                                                groundLayer, 
                                                QueryTriggerInteraction.Ignore);
        return onTheGround;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            gameManager.HP--;
        }
    }
}
