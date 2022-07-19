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
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask(); //将在Inspector中指定LayerMask
    [SerializeField] private Transform debugTransform; //for debug
    [SerializeField] private Transform vfxHitGreen; //击中目标的粒子效果
    [SerializeField] private Transform vfxHitRed; //击中非目标的粒子效果
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
        // 射线检测
        Vector3 mouseWorldPostion = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        // 注意：Camera.main.ScreenPointToRay(Vector3 position);其中参数position为屏幕位置参考点，其Z轴分量值无效。
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point; //需要移除测试小球的Collider，否则小球会不断靠近
            mouseWorldPostion = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        // 瞄准状态
        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);

            //保证移动时的旋转只由ThirdPersonController或者ThirdPersonShooterController脚本控制。
            thirdPersonController.SetRotateOnMove(false);

            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10.0f));

            //瞄准时，调整人物朝向与射击方向一致
            Vector3 worldAimTarget = mouseWorldPostion;
            worldAimTarget.y = transform.position.y; //只调整左右，不关心上下
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f); //平滑转动
        }
        // 非瞄准状态
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);

            //保证移动时的旋转只由ThirdPersonController或者ThirdPersonShooterController脚本控制。
            thirdPersonController.SetRotateOnMove(true);

            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10.0f));
        }

        // 射击
        // 1.实体子弹法
        // 2.射线检测法 hit scan
        if (starterAssetsInputs.shoot)
        {
            // Hit something
            if (hitTransform != null)
            {
                if(hitTransform.GetComponent<BulletTarget>() != null)
                {
                    // Hit target
                    // 子弹击中的特效以后再做
                    //Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
                }
                else
                {
                    // Hit something else
                    // 子弹击中的特效以后再做
                    //Instantiate(vfxHitRed, transform.position, Quaternion.identity);
                }
            }
            starterAssetsInputs.shoot = false;
        }
    }
}
