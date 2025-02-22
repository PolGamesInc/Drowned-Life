using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float SpeedMovePlayer = 5f;
    [SerializeField] private float JumpStrong = 300f;
    [SerializeField] private float SpeedSprintPlayer = 10f;
    [SerializeField] private float SpeedSitPlayer = 2.5f;
    [SerializeField] private Transform Camera;
    [SerializeField] private Transform PlayerModel;
    private float Stamina = 100f;
    private Rigidbody RigidbodyPlayer;
    private bool CheckGround;
    private CapsuleCollider PlayerCollider;
    private float OriginalHeightPlayerCollider;
    private Vector3 OriginalCenterPlayerCollider;
    private Vector3 OriginalScalePlayer;
    private bool isSit = false;

    private void Start()
    {
        RigidbodyPlayer = GetComponent<Rigidbody>();
        PlayerCollider = GetComponent<CapsuleCollider>();

        OriginalHeightPlayerCollider = PlayerCollider.height;
        OriginalCenterPlayerCollider = PlayerCollider.center;
        OriginalScalePlayer = PlayerModel.localScale;
    }

    private void Update()
    {
        Move();
        CheckGroundForJump();

        if (Input.GetKeyDown(KeyCode.C))
        {
            isSit = !isSit;
        }

        if (isSit == true)
        {
            Crouch(true);
        }
        else
        {
            Crouch(false);
        }

        if (CheckGround == true && Input.GetButtonDown("Jump"))
        {
            if (Stamina >= 10)
            {
                Jump();
                Stamina -= 10;
            }
        }

        Stamina = Mathf.Clamp(Stamina, -1f, 100f);
    }

    private void Move()
    {
        float MoveHorizontal = Input.GetAxis("Horizontal");
        float MoveVertical = Input.GetAxis("Vertical");

        Vector3 Forward = Camera.forward;
        Vector3 Right = Camera.right;

        Forward.y = 0f;
        Right.y = 0f;

        Forward.Normalize();
        Right.Normalize();

        Vector3 Movement = (Forward * MoveVertical + Right * MoveHorizontal).normalized;
        float CurrentSpeed = (Stamina > 0 && Input.GetKey(KeyCode.LeftShift))? (Stamina -= 10 * Time.deltaTime, SpeedSprintPlayer).Item2 : (Stamina < 0 ? (isSit ? SpeedSitPlayer : SpeedMovePlayer) : SpeedMovePlayer);
        Stamina += (!Input.GetKey(KeyCode.LeftShift) && CurrentSpeed != SpeedSprintPlayer) ? 5 * Time.deltaTime : 0;
        RigidbodyPlayer.MovePosition(transform.position + Movement * CurrentSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        RigidbodyPlayer.AddForce(Vector3.up * JumpStrong);
    }

    private void CheckGroundForJump()
    {
        CheckGround = Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    private void Crouch(bool isCrouching)
    {
        if (isCrouching == true)
        {
            PlayerCollider.height = OriginalHeightPlayerCollider / 2f;
            PlayerCollider.center = new Vector3(0, -0.5f, 0);

            PlayerModel.localScale = new Vector3(OriginalScalePlayer.x, OriginalScalePlayer.y / 2f, OriginalScalePlayer.z);
        }
        else
        {
            PlayerCollider.height = OriginalHeightPlayerCollider;
            PlayerCollider.center = OriginalCenterPlayerCollider;

            PlayerModel.localScale = OriginalScalePlayer;
        }
    }
}