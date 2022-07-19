using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem; //??

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask(); //����Inspector��ָ��LayerMask
    [SerializeField] private Transform debugTransform; //for debug
    [SerializeField] private Transform vfxHitGreen; //����Ŀ�������Ч��
    [SerializeField] private Transform vfxHitRed; //���з�Ŀ�������Ч��
    private Animator animator;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        // ���߼��
        Vector3 mouseWorldPostion = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        // ע�⣺Camera.main.ScreenPointToRay(Vector3 position);���в���positionΪ��Ļλ�òο��㣬��Z�����ֵ��Ч��
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point; //��Ҫ�Ƴ�����С���Collider������С��᲻�Ͽ���
            mouseWorldPostion = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        // ��׼״̬
        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);

            //��֤�ƶ�ʱ����תֻ��ThirdPersonController����ThirdPersonShooterController�ű����ơ�
            thirdPersonController.SetRotateOnMove(false);

            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10.0f));

            //��׼ʱ���������ﳯ�����������һ��
            Vector3 worldAimTarget = mouseWorldPostion;
            worldAimTarget.y = transform.position.y; //ֻ�������ң�����������
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f); //ƽ��ת��
        }
        // ����׼״̬
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);

            //��֤�ƶ�ʱ����תֻ��ThirdPersonController����ThirdPersonShooterController�ű����ơ�
            thirdPersonController.SetRotateOnMove(true);

            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10.0f));
        }

        // ���
        // 1.ʵ���ӵ���
        // 2.���߼�ⷨ hit scan
        if (starterAssetsInputs.shoot)
        {
            // Hit something
            if (hitTransform != null)
            {
                if(hitTransform.GetComponent<BulletTarget>() != null)
                {
                    // Hit target
                    // �ӵ����е���Ч�Ժ�����
                    //Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
                }
                else
                {
                    // Hit something else
                    // �ӵ����е���Ч�Ժ�����
                    //Instantiate(vfxHitRed, transform.position, Quaternion.identity);
                }
            }
            starterAssetsInputs.shoot = false;
        }
    }
}
