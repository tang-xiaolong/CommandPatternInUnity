using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCommand : BaseCommand
{
    private GameObject prefab;
    private GameObject obj;
    private Vector3 position;
    private Vector3 rotation;
    private List<BaseCommand> careCommands = new List<BaseCommand>();
    
    public DeleteCommand(GameObject obj, GameObject prefab, Vector3 position, Vector3 rotation = default)
    {
        this.obj = obj;
        this.prefab = prefab;
        this.position = position;
        this.rotation = rotation;
    }
    public override void Execute()
    {
        // 我也关心这个物体
        CommandCareBoard.Instance.AddCare(this, obj);
        //获取谁关心这个物体，在撤销的时候会修改这个物体，所以在那里需要替换
        careCommands = CommandCareBoard.Instance.GetCareCommands(obj);
        //老物体即将被销毁，因此需要移除老的监听，否则这部分监听将迷失。后续通过上面拿到的关心列表来添加监听
        CommandCareBoard.Instance.DeleteCare(obj);
        GameObject.Destroy(obj);
    }

    public override void Undo()
    {
        obj = GameObject.Instantiate(prefab, position, Quaternion.Euler(rotation));
        //这个物体是新物体，把监听老物体的命令换成监听新的物体。并将新的监听列表加入到黑板中
        if (careCommands != null)
        {
            for (int i = 0; i < careCommands.Count; i++)
            {
                careCommands[i].ChangeShareData(obj);
                // CommandCareBoard.Instance.AddCare(careCommands[i], obj);
            }
            CommandCareBoard.Instance.AddCares(careCommands, obj);
        }
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
        prefab = null;
        obj = null;
    }

    public override string ToString()
    {
        return "DeleteCubeCommand_" + this.GetHashCode();
    }
    
}
