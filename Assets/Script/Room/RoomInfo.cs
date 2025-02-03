using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    [SerializeField] private string roomName = "Storage";

    private void checkAllPlayers(Collider2D player, string name, bool isEntering)
    {
        PlayerKid kid = player.gameObject.GetComponent<PlayerKid>();
        Pocong pocong = player.gameObject.GetComponent<Pocong>();
        PlayerSpirit spirit = player.gameObject.GetComponent<PlayerSpirit>();

        if (kid != null)
        {
            kid.CurrentLocationChanged(name, isEntering);
        }
        if (pocong != null)
        {
            pocong.CurrentLocationChanged(name, isEntering);
        }
        if (spirit != null)
        {
            spirit.CurrentLocationChanged(name, isEntering);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Notify players they have entered this room
        checkAllPlayers(collision, roomName, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the player is transitioning directly into another room
        if (collision.GetComponent<PlayerKid>()?.IsTransitioning == false &&
            collision.GetComponent<Pocong>()?.IsTransitioning == false &&
            collision.GetComponent<PlayerSpirit>()?.IsTransitioning == false)
        {
            // Notify players they have exited into the "Yard"
            checkAllPlayers(collision, "Yard", false);
        }
    }
}
