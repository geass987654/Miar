using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void OnEnable()
    {
        //�i�椬�ʡA�N���a�]���R��A
        player.GetComponent<Player>().SetDirection(Vector2.zero);
        player.GetComponent<Animator>().SetFloat("magnitude", 0f);
        player.GetComponent<Player>().isFreezed = true;
    }

    private void OnDisable()
    {
        //�������ʡA�Ѱ����a���R��A
        player.GetComponent<Player>().isFreezed = false;
    }
}
