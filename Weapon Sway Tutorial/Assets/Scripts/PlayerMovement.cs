using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public float mouseSensitivityX = 1;
	public float mouseSensitivityY = 1;
	public float speed = 6;

	Vector3 moveAmount;
	Vector3 smoothMoveVelocity;
	float verticalLookRotation;
	Transform cameraTransform;
	Rigidbody rb;

    public WeaponSwayAndBob weaponSwayAndBobScript;

	void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
		cameraTransform = Camera.main.transform;
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
		verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
		cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;

		float inputX = Input.GetAxisRaw("Horizontal");
		float inputY = Input.GetAxisRaw("Vertical");

		Vector3 moveDir = new Vector3(inputX, 0, inputY).normalized;
		Vector3 targetMoveAmount = moveDir * speed;
		moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, 0.1f);

        weaponSwayAndBobScript.currentSpeed = AllowWeaponSway() ? speed : 0;
	}

	void FixedUpdate()
	{
		Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
		rb.MovePosition(rb.position + localMove);
	}

    private bool AllowWeaponSway() {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            return true;
        }

        return false;
    }
}