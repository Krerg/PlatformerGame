using PixelCrew.Creatures;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{

    [SerializeField] private Hero.Hero _hero;

    [SerializeField] private GrapplingHook _grapplingHook;
    
    public void OnSaySomething(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //_hero.saySomething();    
        }
    }
    
    public void OnMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        _hero.SetDirection(direction);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _hero.Interact();    
        }
    }
    
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _hero.Attack();    
        }
    }
    

    public void OnGrapplingHook(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _grapplingHook.TryGrap();
        }
        else if(context.canceled)
        {
            _grapplingHook.ReleaseHook();
        }
        
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _hero.Throw();
        }
    }
    
}