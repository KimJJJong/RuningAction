using UnityEngine;
using Cinemachine;

public class CameraTransition : MonoBehaviour
{
    public CinemachineVirtualCamera dollyVirtualCamera; // Cinemachine Virtual Camera
    public Camera mainCamera;                           // Unity Main Camera
    public float transitionDuration = 1.0f;             // 전환 시간

    private bool isTransitioning = false;
    private float transitionProgress = 0f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float startFOV;

    private Vector3 endPosition;
    private Quaternion endRotation;
    private float endFOV;

    private CineUnit cine_unit;

    private bool isTransitionEnd = false;


    void Start()
    {
        cine_unit = GetComponent<CineUnit>();

        if(!cine_unit)
            Debug.Log("Camera Transition: No Cine Unit Found!");
        
    }

    void Update()
    {
        // 특정 조건이 충족되면 전환 시작
        if (!isTransitioning && !isTransitionEnd && CheckTransitionCondition())
        {
            StartTransition();
        }

        // 전환 진행 중이면 Lerp 수행
        if (isTransitioning)
        {
            PerformTransition();
        }
    }

    bool CheckTransitionCondition()
    {
        return cine_unit.IsCineEnded();
    }

    void StartTransition()
    {
        Vector3 intial_pos = mainCamera.GetComponent<CameraManager>().initial_pos;
        Quaternion initial_rot = mainCamera.GetComponent<CameraManager>().getCamRotation();
        isTransitioning = true;
        transitionProgress = 0f;

        dollyVirtualCamera.enabled = false;

        // Cinemachine Virtual Camera의 현재 상태 가져오기
        startPosition = dollyVirtualCamera.transform.position;
        startRotation = dollyVirtualCamera.transform.rotation;
        startFOV = dollyVirtualCamera.m_Lens.FieldOfView;

        // Main Camera의 최종 상태 가져오기
        endPosition = intial_pos;
        endRotation = initial_rot;
        endFOV = 90;

        // 메인 카메라 활성화 (초기 위치는 Virtual Camera의 위치와 일치)
        mainCamera.enabled = true;
        mainCamera.transform.position = startPosition;
        mainCamera.transform.rotation = startRotation;
        mainCamera.fieldOfView = startFOV;
    }

    void PerformTransition()
    {
        // Lerp 비율 증가
        transitionProgress += Time.deltaTime / transitionDuration;
        float lerpFactor = Mathf.Clamp01(transitionProgress);

        // 위치, 회전, FOV을 Lerp로 변화
        mainCamera.transform.position = Vector3.Lerp(startPosition, endPosition, lerpFactor);
        mainCamera.transform.rotation = Quaternion.Lerp(startRotation, endRotation, lerpFactor);
        mainCamera.fieldOfView = Mathf.Lerp(startFOV, endFOV, lerpFactor);

        // 전환 완료 시 Virtual Camera 비활성화
        if (lerpFactor >= 1.0f)
        {
            EndTransition();
        }
    }

    void EndTransition()
    {
        // Virtual Camera 비활성화 또는 Priority 조정
        dollyVirtualCamera.Priority = 0;

        isTransitioning = false;
        isTransitionEnd = true;
        GameManager.Instance.GamePlay();
    }
}
