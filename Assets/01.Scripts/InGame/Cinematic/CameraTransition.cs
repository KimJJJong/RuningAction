using UnityEngine;
using Cinemachine;
using Unity.IO.LowLevel.Unsafe;

public class CameraTransition : MonoBehaviour
{
    //This file is for smooth transition between the end position of virtual camera after its move and the purposed start position of main camera.

    public CinemachineVirtualCamera virtual_camera; 
    public Camera main_camera;                          
    public float trs_duration = 1.0f;             

    private bool is_trsing = false;
    private float trs_progress = 0f;

    private Vector3 start_position;
    private Quaternion start_rotation;
    private float start_FOV;



    private Vector3 end_position;
    private Quaternion end_rotation;
    private float end_FOV;

    private CineUnit cine_unit;

    private bool is_trs_ended = false;

    public bool if_aft_trs_wait = true;
    public float aft_trs_duration = 2;

    private float aft_trs_timer;


    void Start()
    {
        cine_unit = GetComponent<CineUnit>();

        if(!cine_unit)
            Debug.Log("Camera Transition: No Cine Unit Found!");
        
    }

    void Update()
    {
        if (!is_trsing && !is_trs_ended && CheckTransitionCondition())
            StartTransition();
        

        if (is_trsing)
            PerformTransition();
        

        if(is_trs_ended)
        {
            //if there is any additional wait after transitioning, wait
            if(if_aft_trs_wait)            
                aft_trs_timer += Time.deltaTime;
            
            if(!if_aft_trs_wait || aft_trs_timer >= aft_trs_duration)
            {
                GameManager.Instance.GamePlay();
                this.enabled = false;
            }

        }
    }

    bool CheckTransitionCondition()
    {
        //if the cine_unit's virtual camera action (dolly cart or sth) is ended, the operating condition for transitioning is fulfilled
        return cine_unit.IsCineEnded();
    }

    void StartTransition()
    {
        Vector3 intial_pos = main_camera.GetComponent<CameraManager>().initial_pos;
        Quaternion initial_rot = main_camera.GetComponent<CameraManager>().getCamRotation();
        int initial_FOV = main_camera.GetComponent<CameraManager>().getFOV();
        is_trsing = true;
        trs_progress = 0f;

        virtual_camera.enabled = false;
        
        start_position = virtual_camera.transform.position;
        start_rotation = virtual_camera.transform.rotation;
        start_FOV = virtual_camera.m_Lens.FieldOfView;
        
        end_position = intial_pos;
        end_rotation = initial_rot;
        end_FOV = initial_FOV;
        
        main_camera.enabled = true;
        main_camera.transform.position = start_position;
        main_camera.transform.rotation = start_rotation;
        main_camera.fieldOfView = start_FOV;
    }

    void PerformTransition()
    {
        // Lerp 비율 증가
        trs_progress += Time.deltaTime / trs_duration;
        float lerpFactor = Mathf.Clamp01(trs_progress);

        // 위치, 회전, FOV을 Lerp로 변화
        main_camera.transform.position = Vector3.Lerp(start_position, end_position, lerpFactor);
        main_camera.transform.rotation = Quaternion.Lerp(start_rotation, end_rotation, lerpFactor);
        main_camera.fieldOfView = Mathf.Lerp(start_FOV, end_FOV, lerpFactor);

        // 전환 완료 시 Virtual Camera 비활성화
        if (lerpFactor >= 1.0f)
        {
            EndTransition();
        }
    }

    void EndTransition()
    {
        // Virtual Camera 비활성화 또는 Priority 조정
        virtual_camera.Priority = 0;

        is_trsing = false;
        is_trs_ended = true;
    }
}
