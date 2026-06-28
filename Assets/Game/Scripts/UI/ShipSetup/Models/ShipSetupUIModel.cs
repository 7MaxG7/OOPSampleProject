using System.Collections.Generic;
using Ships;

namespace UI.ShipSetup
{
    public sealed class ShipSetupUIModel
    {
        public readonly Dictionary<OpponentId, ShipSetupPanelUIModel> ShipSetupPanels = new();
    }
}