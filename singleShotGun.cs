using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singleShotGun : Guns
{
    [SerializeField]
    private Camera Cam;
    public override void Use(){
        Shoot();
    } 
    void Shoot(){
        Ray ray = Cam.ViewportPointToRay(new Vector3(0.5f,0.5f));
        ray.origin=Cam.transform.position;
        if(Physics.Raycast(ray,out RaycastHit hit)){
            hit.collider.gameObject.GetComponent<Damagabe>()?.TakeDamage(((GunInfo)_iteminfo).damage);
        }

    }
}
