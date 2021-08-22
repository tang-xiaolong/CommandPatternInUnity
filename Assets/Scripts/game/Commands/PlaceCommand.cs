using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCommand : BaseCommand
{
    private GameObject prefab;
    private GameObject obj;
    private Vector3 position;
    private Vector3 rotation;
    private List<BaseCommand> careCommands;

    public PlaceCommand(GameObject prefab, Vector3 position, Vector3 rotation = default)
    {
        this.prefab = prefab;
        this.position = position;
        this.rotation = rotation;
    }
    
    public override void Execute()
    {
        if (prefab)
        {
            obj = GameObject.Instantiate(prefab, position, Quaternion.Euler(rotation));
            if (careCommands != null)
            {
                for (int i = 0; i < careCommands.Count; i++)
                {
                    careCommands[i].ChangeShareData(obj);
                    CommandCareBoard.Instance.AddCare(careCommands[i], obj);
                }
            }
            CommandCareBoard.Instance.AddCare(this, obj);
        }
    }

    public override void Undo()
    {
        Debug.Log("DeleteCommand");
        //获取谁关心这个物体，在撤销的时候会修改这个物体，所以在那里需要替换
        careCommands = CommandCareBoard.Instance.GetCareCommands(obj);
        //移除老的
        CommandCareBoard.Instance.DeleteCare(obj);
        GameObject.Destroy(obj);
    }

    public override void ChangeShareData(Object newShareObj)
    {
        obj = newShareObj as GameObject;
    }

    public override void Dispose()
    {
        if (obj != null)
        {
            CommandCareBoard.Instance.RemoveCare(this, obj);
        }

        careCommands = null;
        careCommands = null;
        prefab = null;
        obj = null;
    }

    public override string ToString()
    {
        return "PlaceCubeCommand_" + this.GetHashCode();
    }
}
