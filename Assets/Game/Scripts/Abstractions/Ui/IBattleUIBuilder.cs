using System;
using Cysharp.Threading.Tasks;
using Infrastructure;

namespace Ui
{
    public interface IBattleUIBuilder : ISceneCleanable
    {
        UniTask BuildUI(Action leaveBattle);
    }
}