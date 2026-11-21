using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
        public float playerSpeed = 9f;
        private Vector2 movement;
        public Vector2 facingDirection = Vector2.up;
        public Rigidbody2D bodyPlayer;


    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        PlayerInput();
        RotatePlayer();
    }
        void UpdateMovement(){
            bodyPlayer.linearVelocity = movement * playerSpeed;
        }
        void PlayerInput(){

            float Horizontial = 0f; 
            float Vertical = 0f;
            if(Keyboard.current == null){
                return;
            }
            if(Keyboard.current.wKey.isPressed){
                Vertical += 1;
            }
            if(Keyboard.current.aKey.isPressed){
                Horizontial -= 1;
            }
            if(Keyboard.current.sKey.isPressed){
                Vertical -= 1;
            }
            if(Keyboard.current.dKey.isPressed){
                Horizontial += 1;
            }
            movement = new Vector2(Horizontial,Vertical).normalized;
            
            if (movement != Vector2.zero)
            {
                facingDirection = movement;
            }
        }
        void RotatePlayer()
        {
            if (facingDirection != Vector2.zero) {
            float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
            bodyPlayer.rotation = angle - 90f;
            }
        }
}
