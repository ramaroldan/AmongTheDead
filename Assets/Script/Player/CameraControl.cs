using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] Transform _followTarget;
    [SerializeField] float _turnSpeed;

    Vector3 _cameraOffset;

    void Start()
    {
        // Si ya tienes asignado el target en el Inspector, calcula el offset relativo
        if (_followTarget != null)
            _cameraOffset = transform.position - _followTarget.position;
    }

    void LateUpdate()
    {
        if (_followTarget == null)
            return;  // nada que hacer si no hay target

        // rota el offset seg�n el movimiento horizontal del rat�n
        _cameraOffset = Quaternion
            .AngleAxis(Input.GetAxis("Mouse X") * _turnSpeed, Vector3.up)
            * _cameraOffset;

        // reposiciona la c�mara y mira al target
        transform.position = _followTarget.position + _cameraOffset;
        transform.LookAt(_followTarget.position);
    }

    // �� M�TODO P�BLICO PARA ASIGNAR EL TARGET �� 
    public void SetFollowTarget(Transform target)
    {
        _followTarget = target;
        // recalcula offset inmediato para evitar �saltos�
        _cameraOffset = transform.position - _followTarget.position;
    }
}
