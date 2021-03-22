using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //移動速度 最少為2
    [Min(2)]public float speed = 5;
    //跑時移動X倍
    private float runSpeed = 1;
    Vector2 velocity;

    //Camera
    [SerializeField]
    Transform camera;
    Vector2 currentMouseLook;
    Vector2 appliedMouseDelta;
    public float sensitivity = 1;
    public float smoothing = 2;

    private float moveVelocity;


    private void Awake()
    {
        camera = GetComponentInChildren<Camera>().transform;
        moveVelocity = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        keyInput();
        Move();
        CameraLock();
    }

    private void keyInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            runSpeed = 2f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            runSpeed = 1f;
        }
    }

    void Move()
    {
        velocity.y = Input.GetAxis("Vertical") * speed * runSpeed * Time.deltaTime;
        velocity.x = Input.GetAxis("Horizontal") * speed * runSpeed * Time.deltaTime;
        transform.Translate(velocity.x, 0, velocity.y);
        if(velocity.y != 0.0f)
            moveVelocity = velocity.y;
    }

    void CameraLock()
    {
        // mouse look
        Vector2 smoothMouseDelta = Vector2.Scale(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")), Vector2.one * sensitivity * smoothing);
        appliedMouseDelta = Vector2.Lerp(appliedMouseDelta, smoothMouseDelta, 1 / smoothing);
        currentMouseLook += appliedMouseDelta;
        currentMouseLook.y = Mathf.Clamp(currentMouseLook.y, -90, 90);

        // Rotate camera
        camera.localRotation = Quaternion.AngleAxis(-currentMouseLook.y, Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(currentMouseLook.x, Vector3.up);
    }

    public float GetMoveVelocity()
    {
        return velocity.y > 0 ? moveVelocity : 0;
    }

}
