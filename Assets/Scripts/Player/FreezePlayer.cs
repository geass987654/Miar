using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayer : MonoBehaviour
{
    private void OnEnable()
    {
        //進行互動，將玩家設為靜止狀態
        Player.Instance.SetDirection(Vector2.zero);
        Player.Instance.GetComponent<Animator>().SetFloat("magnitude", 0f);
        Player.Instance.isFreezed = true;
    }

    private void OnDisable()
    {
        //結束互動，解除玩家的靜止狀態
        Player.Instance.isFreezed = false;
    }
}
