using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void OnEnable()
    {
        //進行互動，將玩家設為靜止狀態
        player.GetComponent<Player>().SetDirection(Vector2.zero);
        player.GetComponent<Animator>().SetFloat("magnitude", 0f);
        player.GetComponent<Player>().isFreezed = true;
    }

    private void OnDisable()
    {
        //結束互動，解除玩家的靜止狀態
        player.GetComponent<Player>().isFreezed = false;
    }
}
