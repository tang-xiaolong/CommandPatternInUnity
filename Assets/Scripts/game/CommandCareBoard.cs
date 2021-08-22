using System;
using System.Collections.Generic;
using UnityEngine;



public class CommandCareBoard: IDisposable
{
    private static Dictionary<GameObject, List<BaseCommand>> shareDictionary;

    private static CommandCareBoard _instance;

    public static CommandCareBoard Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CommandCareBoard();
                shareDictionary = new Dictionary<GameObject, List<BaseCommand>>();
            }

            return _instance;
        }
    }
    /// <summary>
    /// 命令添加自己关心的物体，便于物体被改变时能察觉到
    /// </summary>
    /// <param name="command"></param>
    /// <param name="obj"></param>
    public void AddCare(BaseCommand command, GameObject obj)
    {
        if (obj)
        {
            if (shareDictionary.TryGetValue(obj, out var commandList))
            {
                if (!commandList.Contains(command))
                {
                    commandList.Add(command);
                }
            }
            else
            {
                shareDictionary.Add(obj, new List<BaseCommand>(){command});
            }
        }
    }

    public void AddCares(List<BaseCommand> commands, GameObject obj)
    {
        if (obj)
        {
            if (shareDictionary.TryGetValue(obj, out var commandList))
            {
                commandList.AddRange(commands);
            }
            else
            {
                shareDictionary.Add(obj, commands);
            }
        }
    }

    /// <summary>
    /// 移除某个命令对某个物体的监听
    /// </summary>
    /// <param name="command"></param>
    /// <param name="obj"></param>
    public void RemoveCare(BaseCommand command, GameObject obj)
    {
        if (shareDictionary.TryGetValue(obj, out var commandList))
        {
            if (commandList.Contains(command))
            {
                commandList.Remove(command);
            }
        }
    }

    /// <summary>
    /// 移除监听一个物体的所有命令
    /// </summary>
    /// <param name="obj"></param>
    public void DeleteCare(GameObject obj)
    {
        if (shareDictionary.ContainsKey(obj))
        {
            shareDictionary.Remove(obj);
        }
    }

    public List<BaseCommand> GetCareCommands(GameObject obj)
    {
        if (shareDictionary.TryGetValue(obj, out var commands))
        {
            return commands;
        }

        return null;
    }

    public void Dispose()
    {
        _instance = null;
        shareDictionary = null;
    }
}
