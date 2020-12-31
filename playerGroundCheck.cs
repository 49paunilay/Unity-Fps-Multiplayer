using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerGroundCheck : MonoBehaviour
{
    private playerController _playercontroller;
    private void Awake() {

        _playercontroller = GetComponentInParent<playerController>();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject == _playercontroller.gameObject){
            return;
        }
        _playercontroller.SetGroundedState(true);
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject == _playercontroller.gameObject){
            return;
        }
        _playercontroller.SetGroundedState(false);
    }
    private void OnTriggerStay(Collider other) {
        if(other.gameObject == _playercontroller.gameObject){
            return;
        }
        _playercontroller.SetGroundedState(true);
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject == _playercontroller.gameObject){
            return;
        }
        _playercontroller.SetGroundedState(true);
        
    }
    private void OnCollisionExit(Collision other) {
        if(other.gameObject == _playercontroller.gameObject){
            return;
        }
        _playercontroller.SetGroundedState(false);
        
    }
    private void OnCollisionStay(Collision other) {
        if(other.gameObject == _playercontroller.gameObject){
            return;
        }
        _playercontroller.SetGroundedState(true);
        
    }
}
