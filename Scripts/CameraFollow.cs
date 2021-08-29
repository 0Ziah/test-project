using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    Transform self;

    void Start()
    {
        self = this.transform;
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("target is is missing from " + this.name);
            return;
        }
        else
        {
            self.position = target.position + offset;
            self.LookAt(target);
        }
    }
}
