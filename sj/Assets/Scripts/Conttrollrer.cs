using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Conttrollrer : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public float SonHız,ZıplamaGücü,ZıplamaSonHızı,ZıplamaSüresi;

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
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        _rigidbody.velocity += new Vector3((SonHız - Mathf.Abs(_rigidbody.velocity.x)) * movement / 15, 0, 0);


        if (isGrounded)
        {
            jumpNumber = 1;
        }
        if (isGrounded && jump && beklemeS > ZıplamaSüresi)
        {
            beklemeS = 0;
            _rigidbody.AddForce(Vector3.up * ZıplamaGücü, ForceMode.VelocityChange);   
        }
        else if (jumpNumber < 2 && beklemeS > ZıplamaSüresi && jump )
        {
            beklemeS = 0;
            jumpNumber++;
            _rigidbody.velocity += new Vector3(0, (ZıplamaGücü < _rigidbody.velocity.y ? 0 : ZıplamaGücü ), 0);    
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
