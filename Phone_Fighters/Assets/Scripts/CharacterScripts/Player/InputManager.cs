using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class InputManager : MonoBehaviour
{

    Vector2 moveDirection;
    Animator anim;
    Player player;
    [SerializeField]
    float dashSpeed = 35f, dashTime = 0.1f, dashDrain = 40f;
    bool isDashing = false;
    float speed;
    [SerializeField]
    float pressTimeToDash = 0.2f;
    public float PressTimeToDash {get {return pressTimeToDash;}}

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        player = GetComponentInChildren<Player>();
        speed = player.Speed;
        dashSpeed *= 50f;
    }

    void FixedUpdate()
    {
        if (!player.IsDead && !player.TakingDamage && !IsDashing && !player.Attack)
        {
            if (CrossPlatformInputManager.GetAxis("Horizontal") != 0 ||
            CrossPlatformInputManager.GetAxis("Vertical") != 0)
            {
                Movement();
                anim.SetFloat("Run", 1f);
            }
            else
            {
                anim.SetFloat("Run", 0f);
            }

            if (CrossPlatformInputManager.GetAxis("Horizontal") != 0)
                Rotation();
        }
    }

    public bool IsFacingRight
    {
        get
        {
            if (CrossPlatformInputManager.GetAxis("Horizontal") > 0)
            {
                return true;
            }                
            else if (CrossPlatformInputManager.GetAxis("Horizontal") < 0)
            {
                return false;
            }
            else if (transform.rotation.y == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void Movement()
    {
        if (!player.Attack)
        {
            moveDirection = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"),
                                         CrossPlatformInputManager.GetAxis("Vertical"));
            transform.position += (new Vector3(moveDirection.x, moveDirection.y)).normalized * speed * Time.deltaTime;
        }
    }

    void Rotation()
    {
        if (IsFacingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void Resetbutton(string Button)
    {
        anim.SetBool(Button, false);
    }

    IEnumerator DashEnum ()
    {
        Vector2 moveDirection = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"),
                                            CrossPlatformInputManager.GetAxis("Vertical"));
        if (moveDirection.normalized != Vector2.zero)
        {
            isDashing = true;
            anim.SetBool("Dash", true);
            player.LowerStamina(dashDrain);
            player.MyRigidbody.AddForce(moveDirection.normalized * dashSpeed * Time.deltaTime, ForceMode2D.Impulse);        

            yield return new WaitForSeconds(dashTime);
            anim.SetBool("Dash", false);
            isDashing = false;
            player.MyRigidbody.velocity = new Vector2();        
        }
    }

    public void Dash ()
    {
        if (!IsDashing && player.Stamina >= dashDrain)
        {
            StartCoroutine("DashEnum");
        }
    }

    public bool IsDashing
    {
        get
        {
            return isDashing;
        }
    }
}
