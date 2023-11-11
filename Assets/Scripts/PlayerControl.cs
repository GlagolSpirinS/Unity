using InfimaGames.LowPolyShooterPack;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject handMeshes;
    private Rigidbody rigidBody;
    private GameManager gameManager;
    private AnimationManager animationManager; //

    public GameObject[] weaponInventory;
    public GameObject[] weaponMeshes;
    private int selectedWeaponId = 0;
    private Weapon weapon;

    public float moveSpeed = 2.0f;
    public float sprintSpeed = 3.0f;
    public float jumpForce = 0.1f;
    public float distantionToGround = 1.0f;
    public float rotSmoothing = 20f;
    private bool isGround;
    private bool isSprinting; //
    private float pitch, yaw;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();

        weapon = weaponInventory[selectedWeaponId].GetComponent<Weapon>();
        weaponMeshes[selectedWeaponId].SetActive(true);

        animationManager = weaponMeshes[selectedWeaponId].GetComponent<AnimationManager>(); //
    }

    void Update()
    {
        GroundCheck();
        if (Input.GetKey(KeyCode.Space) && isGround) Jump();
        if (Input.GetKey(KeyCode.Mouse0))
        {
            animationManager.SetAnimationFire(); //
            weapon.Fire();
        }

        if (Input.GetKey(KeyCode.R))
        {
            animationManager.SetAnimationReload(); //
            weapon.Reload();
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) SelectNextWeapon();
        else if (Input.GetAxis("Mouse ScrollWheel") < 0) SelectPrevWeapon();

        if(Input.GetKey(KeyCode.LeftShift) && !gameManager.isStaminaRestoring)
        {
            gameManager.SpendStamina();
            rigidBody.MovePosition(GetCalcSprint());
        }
        else rigidBody.MovePosition(GetCalcMove());

        SetRoot();
        SetAnimation(); //
    }

    private void GroundCheck()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, distantionToGround);
    }

    private Vector3 GetCalcMove()
    {
        isSprinting = false; //
        float horizontalDirection = Input.GetAxis("Horizontal");
        float verticalDirection = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontalDirection + transform.forward * verticalDirection;

        return rigidBody.transform.position + move * Time.fixedDeltaTime * moveSpeed;
    }

    private Vector3 GetCalcSprint()
    {
        isSprinting = true; //
        float horizontalDirection = Input.GetAxis("Horizontal");
        float verticalDirection = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontalDirection + transform.forward * verticalDirection;

        return rigidBody.transform.position + move * Time.fixedDeltaTime * sprintSpeed;
    }

    private bool IsMoving() // 
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    private void SetAnimation() // 
    {
        if (IsMoving())
        {
            if (isSprinting) animationManager.SetAnimationRun();
            else animationManager.SetAnimationWalk();
        }
        else animationManager.SetAnimationIdle();
    }

    private void Jump()
    {
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void SetRoot()
    {
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");

        pitch = Mathf.Clamp(pitch, -60, 90);

        Quaternion smootRoot = Quaternion.Euler(pitch, yaw, 0);
        handMeshes.transform.rotation = Quaternion.Slerp(handMeshes.transform.rotation, smootRoot, rotSmoothing * Time.fixedDeltaTime);

        smootRoot = Quaternion.Euler(0, yaw, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, smootRoot, rotSmoothing * Time.fixedDeltaTime);

    }

    private void SelectPrevWeapon()
    {
        if (selectedWeaponId != 0)
        {
            weaponMeshes[selectedWeaponId].SetActive(false);
            selectedWeaponId -= 1;
            weapon = weaponInventory[selectedWeaponId].GetComponent<Weapon>();
            weaponMeshes[selectedWeaponId].SetActive(true);
            animationManager = weaponMeshes[selectedWeaponId].GetComponent<AnimationManager>();
            Debug.Log("Оружие: " + weapon.weaponType);
        }
    }

    private void SelectNextWeapon()
    {
        if (weaponInventory.Length > selectedWeaponId + 1)
        {
            weaponMeshes[selectedWeaponId].SetActive(false);
            selectedWeaponId += 1;
            weapon = weaponInventory[selectedWeaponId].GetComponent<Weapon>();
            weaponMeshes[selectedWeaponId].SetActive(true);
            animationManager = weaponMeshes[selectedWeaponId].GetComponent<AnimationManager>();
            Debug.Log("Оружие: " + weapon.weaponType);
        }
    }

    private void OnDrawGrizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * distantionToGround));
    }
}
