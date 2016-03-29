using Newtonsoft.Json;
using SAHL.Core.UI.Elements.Tiles;
using System.Collections.Generic;

namespace SAHL.Core.UI.Elements.Areas
{
    public class ActionTileArea : ElementArea
    {
        [JsonProperty]
        private List<ActionMiniTileElement> actionTiles;

        public ActionTileArea()
            : base(ElementNames.ActionTileArea)
        {
            this.actionTiles = new List<ActionMiniTileElement>();
        }

        public IEnumerable<ActionMiniTileElement> ActionTiles { get { return this.actionTiles; } }

        public void AddChildElements(IEnumerable<ActionMiniTileElement> actionTileElements)
        {
            this.actionTiles.Clear();
            this.actionTiles.AddRange(actionTileElements);
        }
    }
}