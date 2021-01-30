using UnityEngine;

public enum PowerUpType {
    MachineGun,
    Speed
};

public class PowerUp : MonoBehaviour
{
    
    [SerializeField] public PowerUpType type;

}
