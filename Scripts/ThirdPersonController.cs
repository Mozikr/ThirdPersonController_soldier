using UnityEditor.Animations;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    CinemachineShake camShake;

    public CharacterController controller;

    public Animator anim;
    public AnimatorController _thirdPersonArmed;
    public AnimatorController _thirdPerson;

    public Transform cam;
    public Transform barrelTransform;
    public Transform bulletParent;

    public GameObject bulletPrefab;
    public GameObject gun;
    public GameObject pointer;

    public ParticleSystem shoot;

    public float _bulletHitMissDis = 25f;
    private bool _armed = false;
    public float _speed = 6f;
    public float _turnSmoothTime = 0.1f;
    public float fireRate = 15f;
    private float nextTimeToFire = 0f;
    float _turnSmoothVelocity;
    float _gravity = -10;
    float _jumpSpeed = 10;

    Vector3 velocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        camShake = FindObjectOfType<CinemachineShake>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.01f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * _speed * Time.deltaTime);
        }


        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = _jumpSpeed;
        }

        velocity.y += _gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical") && _armed == false)
        {
            anim.SetBool("IsWalking", true);

            if (Input.GetButtonDown("Sprint"))
            {
                anim.SetBool("IsRunning", true);
                _speed = 15f;
            }
        }
        else
        {
            _speed = 10f;
            anim.SetBool("IsWalking", false);
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsJumping", false);
        }


        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical") && _armed == true)
        {
            anim.SetBool("IsWalking_armed", true);

            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                camShake.Shakecamera(3f, .1f);
                nextTimeToFire = Time.time + 1f / fireRate;
                ShootGun();
                shoot.Play();
            }
        }
        else
        {
            anim.SetBool("IsWalking_armed", false);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.runtimeAnimatorController = _thirdPersonArmed as RuntimeAnimatorController;
            gun.SetActive(true);
            _armed = true;
            pointer.SetActive(true);
            
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            anim.runtimeAnimatorController = _thirdPerson as RuntimeAnimatorController;
            _armed = false;
            gun.SetActive(false);
            pointer.SetActive(false);
        }


    }

    void ShootGun()
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, barrelTransform.rotation, bulletParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();
    }
}
