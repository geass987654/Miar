using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayer : MonoBehaviour
{
    private void OnEnable()
    {
        //�i�椬�ʡA�N���a�]���R��A
        Player.Instance.SetDirection(Vector2.zero);
        Player.Instance.GetComponent<Animator>().SetFloat("magnitude", 0f);
        Player.Instance.isFreezed = true;
    }

    private void OnDisable()
    {
        //�������ʡA�Ѱ����a���R��A
        Player.Instance.isFreezed = false;
    }
}
