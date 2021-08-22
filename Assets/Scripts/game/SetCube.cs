using System;
using UnityEngine;

public class SetCube : MonoBehaviour
{
    public GameObject cube;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var hitObjTransform = hit.collider.transform;
                //如果只是按住鼠标左键，则放置一个方块
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    if (cube)
                    {
                        //TODO:放置的时候根据放置目的地和放置的物体边界，使其匹配上边界，而不是放置在hitPoint
                        // var hitPoint = hit.point;
                        // Vector3 setPos =  new Vector3(hitPoint.x,
                        //     hitObjTransform.position.y, hitPoint.z);
                        // Collider hitCollider = hitObjTransform.GetComponent<Collider>();
                        // if (hitCollider)
                        //     setPos.y += hitCollider.bounds.extents.y * .5f;
                        // else
                        //     setPos.y += hitObjTransform.localScale.y * .5f;
                        // Collider setCollider = cube.GetComponent<Collider>();
                        // if (setCollider)
                        //     setPos.y += setCollider.bounds.extents.y * .5f;
                        // else
                        //     setPos.y += cube.transform.localScale.y * .5f;
                        // var command = new PlaceCommand(cube,  setPos);
                        var command = new PlaceCommand(cube, hit.point + new Vector3(0, cube.transform.localScale.y * .5f, 0));
                        CommandManager.Instance.AddCommand(command);
                    }
                }
                else
                {
                    //如果按住的是Ctrl+鼠标左键，则删除物体
                    if (hit.collider.CompareTag("SetAble"))
                    {
                        CommandManager.Instance.AddCommand(new DeleteCommand(hit.collider.gameObject, cube, hitObjTransform.position, hitObjTransform.rotation.eulerAngles));
                    }
                }
                
                
            }
        }
    }
}
