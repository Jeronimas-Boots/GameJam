using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{

    public Transform[] transforms;
    private int playerCount = 0;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.transform.position = transforms[playerCount].position;
        playerInput.transform.rotation = transforms[playerCount].rotation;
        playerCount++;
    }
}
