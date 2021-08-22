using System;

public abstract class BaseCommand: ICommand, IShareData, IDisposable
{
    public abstract void Execute();

    public abstract void Undo();

    public abstract void ChangeShareData(UnityEngine.Object newShareObj);

    public abstract void Dispose();
}