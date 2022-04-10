using UnityEngine.InputSystem;
using UnityEngine;

public class Controller_Fps : MonoBehaviour
{

    private Rigidbody _rigidbody;
    public float SonH�z, Z�plamaG�c�, Z�plamaSonH�z�, Z�plamaS�resi;

    //Input
    [SerializeField] private Collider Kafa;
    private Vector2 movement,rotate;
    private float beklemeS;
    public bool tutunma;
    [SerializeField] private bool jump, isGrounded, hitted;
    [SerializeField] private Transform cam,t;
    [Range(0f,50f)][SerializeField] private float sensivityX = 40, sensivityY =1.1f;
    private int jumpNumber = 1;

    void Start()
    {
        hitted = false;
        _rigidbody = GetComponent<Rigidbody>();
    }

    #region girdi
    public void Movement(InputAction.CallbackContext value)
    {
        movement = value.ReadValue<Vector2>();
    }
    public void Jump(InputAction.CallbackContext value)
    {
        jump = value.ReadValueAsButton();
    }

    public void Look(InputAction.CallbackContext value)
    {
        rotate = value.ReadValue<Vector2>();
    }
    #endregion

    private void Update()
    {
        beklemeS += Time.deltaTime;
        t.Rotate(0, rotate.x * 0.01f * sensivityX, 0);
        cam.Rotate(rotate.y * -0.01f * sensivityY, 0, 0);
    }

    void FixedUpdate()
    { 
        _rigidbody.velocity += 
            (   (SonH�z - Mathf.Abs(_rigidbody.velocity.magnitude)) * movement.y / 15 ) * t.forward 
            + 
            (   (SonH�z - Mathf.Abs(_rigidbody.velocity.magnitude)) * movement.x / 15 ) * t.right;


        if (isGrounded)
        {
            jumpNumber = 1;
        }
        if (isGrounded && jump && beklemeS > Z�plamaS�resi)
        {
            beklemeS = 0;
            _rigidbody.AddForce(Vector3.up * Z�plamaG�c�, ForceMode.VelocityChange);
        }
        else if (jumpNumber < 2 && beklemeS > Z�plamaS�resi && jump)
        {
            beklemeS = 0;
            jumpNumber++;
            _rigidbody.velocity += new Vector3(0, (Z�plamaG�c� < _rigidbody.velocity.y ? 0 : Z�plamaG�c�), 0);
        }
    }

    #region tutunma
    private void OnCollisionExit(Collision col)
    {
        isGrounded = false;
    }
    private void OnCollisionEnter(Collision col)
    {
        for (int i = 0; i < col.contactCount; i++)
        {
            if (col.GetContact(i).thisCollider == Kafa)
                return;
        }
        if (col.gameObject.CompareTag("Yer"))
            isGrounded = true;

    }
    private void OnCollisionStay(Collision col)
    {
        for (int i = 0; i < col.contactCount; i++)
        {
            if (col.GetContact(i).thisCollider == Kafa)
                return;
        }
        if (col.gameObject.CompareTag("Yer"))
            isGrounded = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Yer"))
        {
            tutunma = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Yer"))
        {
            tutunma = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Yer"))
        {
            tutunma = false;
        }
    }

    #endregion

}
