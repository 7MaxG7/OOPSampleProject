using System;
using Cysharp.Threading.Tasks;
using Infrastructure;

namespace Ui
{
    public interface IShipSetupUIBuilder : ISceneCleanable
    {
        UniTask BuildUIAsync(Action switchState);
    }
}