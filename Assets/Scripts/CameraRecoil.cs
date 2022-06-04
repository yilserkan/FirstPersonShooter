using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    [SerializeField] private Gun currentGun;

    private Vector3 currenRotation;
    private Vector3 targetRotation;

    private void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * currentGun.returnSpeed);
        currenRotation = Vector3.Slerp(currenRotation, targetRotation, Time.fixedDeltaTime * currentGun.snappiness);

        transform.localRotation = Quaternion.Euler(currenRotation);
    }

    public void RecoilFire()
    {
        if (PlayerMovement.GetPlayerState == PlayerState.Aiming)
        {
            targetRotation += new Vector3(currentGun.aimRecoilX,
                                            Random.Range(-currentGun.aimRecoilY, currentGun.aimRecoilY),
                                            Random.Range(-currentGun.aimRecoilZ, currentGun.aimRecoilZ));
        }
        else
        {
            targetRotation += new Vector3(currentGun.recoilX,
                                            Random.Range(-currentGun.recoilY, currentGun.recoilY),
                                            Random.Range(-currentGun.recoilZ, currentGun.recoilZ));
        }
    }
}
