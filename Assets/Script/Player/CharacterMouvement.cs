using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMouvement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D coll;
    [SerializeField] private GameObject inventory;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SpriteRenderer playerSprite;

    public float SpeedX;
    static readonly int Speed = Animator.StringToHash("Speed");

    public void RightLeft(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(move.x * speed, rb.linearVelocity.y);
        playerAnimator.SetFloat(Speed, Mathf.Abs(rb.linearVelocity.magnitude));
    
        if(rb.linearVelocity.magnitude > 0)
            playerSprite.flipX = rb.linearVelocity.x < 0;
        
        SpeedX = rb.linearVelocity.x;
    }
    
    public void UpAndDown(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, move.y * speed);
        playerAnimator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.magnitude));
        
    }



    public void ZoomInZoomOut(InputAction.CallbackContext context)
    {
        Camera cam = Camera.main;
        if (cam != null) cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + context.ReadValue<float>(), 15, 30);
    }
}