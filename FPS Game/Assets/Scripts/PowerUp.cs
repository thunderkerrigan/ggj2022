using UnityEngine;

public enum PowerUpType {
    MachineGun,
    Speed,
    ReverseControl,
    //Stunt
};

public class PowerUp : MonoBehaviour
{
    
    [SerializeField] public PowerUpType type;

}
