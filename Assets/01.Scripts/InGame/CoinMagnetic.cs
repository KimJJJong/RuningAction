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
        Transform Target = GameManager.Instance.player.transform;
        if ((_isMagnetic&&GameManager.Instance.playerController.eCh == ECharacter.Magnetic)||GameManager.Instance.playerController.IsMagnetic)  //앞부분은 캐릭터 능력 뒤는 아이템 능력
        {
            if( Vector3.Distance(transform.position, Target.position) < 5)
            transform.position = Vector3.MoveTowards(transform.position, Target.position, 30 * Time.deltaTime);

        }
    }

  
    IEnumerator MagneticTimer()   // Weapon Magnetic Lv
    {
        while(true)
        {
            yield return new WaitForSeconds(10f);      //대기 시간
            _isMagnetic = true;
            yield return new WaitForSeconds(5f );        //자석 시간
            _isMagnetic = false;
        }

    }
}
