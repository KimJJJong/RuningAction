using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CoinMagnetic : MonoBehaviour
{
    bool _isMagnetic;

    private void Start()
    {
        // StartCoroutine(MagneticTimer());
        _isMagnetic = true;
    }

    void Update()
    {
        Transform Target = GameManager.Instance.playerManager.GetCurrentPlayer().transform;
        if (
            (
                _isMagnetic
                && GameManager.Instance.playerManager.GetCurrentController().eCh
                    == ECharacter.Magnetic
            ) || GameManager.Instance.playerManager.GetCurrentController().IsMagnetic
        ) //�պκ��� ĳ���� �ɷ� �ڴ� ������ �ɷ�
        {
            if (Vector3.Distance(transform.position, Target.position) < 5)
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    Target.position,
                    30 * Time.deltaTime
                );
        }
    }

    IEnumerator MagneticTimer() // Weapon Magnetic Lv
    {
        while (true)
        {
            yield return new WaitForSeconds(10f); //��� �ð�
            _isMagnetic = true;
            yield return new WaitForSeconds(5f); //�ڼ� �ð�
            _isMagnetic = false;
        }
    }
}
