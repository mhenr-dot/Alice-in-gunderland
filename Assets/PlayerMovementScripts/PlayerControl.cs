using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
        public float playerSpeed = 9f;
        private Vector2 movement;
        public Rigidbody2D bodyPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        PlayerInput();
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
            else if(Keyboard.current.aKey.isPressed){
                Horizontial -= 1;
            }
            else if(Keyboard.current.sKey.isPressed){
                Vertical -= 1;
            }
            else if(Keyboard.current.dKey.isPressed){
                Horizontial += 1;
            }
            movement = new Vector2(Horizontial,Vertical).normalized;
        }
        
}
