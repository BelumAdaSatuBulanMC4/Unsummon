using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    protected void checkAllPlayers(Collider2D player, string name)
    {
        PlayerKid kid = player.gameObject.GetComponent<PlayerKid>();
        Pocong pocong = player.gameObject.GetComponent<Pocong>();
        PlayerSpirit spirit = player.gameObject.GetComponent<PlayerSpirit>();

        if (kid != null)
        {
            kid.CurrentLocationChanged(name);
        }
        if (pocong != null)
        {
            pocong.CurrentLocationChanged(name);
        }
        if (spirit != null)
        {
            spirit.CurrentLocationChanged(name);
        }
    }
}
