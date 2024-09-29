using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_DiningRoom : Room
{
    private string roomName = "Dining Room";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        checkAllPlayers(collision, roomName);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        checkAllPlayers(collision, "Hallway");
    }
}
