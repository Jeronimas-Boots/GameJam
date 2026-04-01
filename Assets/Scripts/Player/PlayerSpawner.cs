using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{

    public Transform[] transforms;
    private int playerCount = 0;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerCount < transforms.Length)
        {
            playerInput.GetComponent<CharacterController>().InnitializePlayer(transforms[playerCount]);
            playerCount++;
        }
    }
}
