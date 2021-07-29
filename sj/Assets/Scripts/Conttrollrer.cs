using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Conttrollrer : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public float SonH�z,Z�plamaG�c�,Z�plamaSonH�z�,Z�plamaS�resi;

    //Input
    [SerializeField] private Collider Kafa;
    private float movement,beklemeS;
    public bool tutunma;
    [SerializeField] private bool jump, isGrounded, hitted;
    private int jumpNumber = 1;

    void Start()
    {
        hitted = false;
        _rigidbody = GetComponent<Rigidbody>();
    }

    #region girdi
    public void Movement(InputAction.CallbackContext value)
    {
        movement = value.ReadValue<float>();
    }
    public void Jump(InputAction.CallbackContext value)
    {
        jump = value.ReadValueAsButton();
    }
    #endregion

    private void Update()
    {
        beklemeS += Time.deltaTime;
    }

    void FixedUpdate()
    {
        _rigidbody.velocity += new Vector3((SonH�z - Mathf.Abs(_rigidbody.velocity.x)) * movement / 15, 0, 0);


        if (isGrounded)
        {
            jumpNumber = 1;
        }
        if (isGrounded && jump && beklemeS > Z�plamaS�resi)
        {
            beklemeS = 0;
            _rigidbody.AddForce(Vector3.up * Z�plamaG�c�, ForceMode.VelocityChange);   
        }
        else if (jumpNumber < 2 && beklemeS > Z�plamaS�resi && jump )
        {
            beklemeS = 0;
            jumpNumber++;
            _rigidbody.velocity += new Vector3(0, (Z�plamaG�c� < _rigidbody.velocity.y ? 0 : Z�plamaG�c� ), 0);    
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
