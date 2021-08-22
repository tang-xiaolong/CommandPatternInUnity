using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CommandManager : MonoBehaviour, IDisposable
{
    public static CommandManager Instance;
    private List<BaseCommand> _historyCommands;
    private int count;

    public Text txtCommand; 
    public Text txtOperation; 
    StringBuilder _txtStringBuilder = new StringBuilder();

    void Awake()
    {
        Instance = this;
        _historyCommands = new List<BaseCommand>();
        count = 0;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log(count);
            if (count > 0)
            {
                count -= 1;
                _historyCommands[count].Undo();
                RefreshCommandShow();
                txtOperation.text = "撤销";
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            if (count < _historyCommands.Count)
            {
                var command = _historyCommands[count];
                command.Execute();
                count += 1;
                RefreshCommandShow();
                txtOperation.text = "反撤销";
            }
        }
    }

    public void AddCommand(BaseCommand command)
    {
        //添加命令时直接从Count的位置开始加，原来在Count后面的都移除掉
        while (_historyCommands.Count > count)
        {
            _historyCommands[count].Dispose();
            _historyCommands.RemoveAt(count);
        }
        //如果同一帧里被添加的命令过多，可以加一个命令队列来分帧处理
        _historyCommands.Add(command);
        count++;
        command.Execute();
        txtOperation.text = command.ToString();
        RefreshCommandShow();
    }

    void RefreshCommandShow()
    {
        _txtStringBuilder.Clear();
        _txtStringBuilder.Append($"History count = {_historyCommands.Count} \n");
        _txtStringBuilder.Append($"Current Counter = {count} \n");
        for (int i = 0; i < _historyCommands.Count; i++)
        {
            _txtStringBuilder.Append($"{i + 1} is {_historyCommands[i]} \n");
        }

        txtCommand.text = _txtStringBuilder.ToString();
    }

    public void Dispose()
    {
        if (_historyCommands != null)
        {
            for (int i = 0; i < _historyCommands.Count; i++)
            {
                _historyCommands[i].Dispose();
            }

            _historyCommands = null;
        }
    }
}
