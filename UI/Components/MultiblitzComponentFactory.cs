using LiveSplit.Model;
using System;

namespace LiveSplit.UI.Components
{
    public class MultiblitzComponentFactory : IComponentFactory
    {
        public string ComponentName => "Multiblitz";

        public string Description => "Automatic reporting to a multiblitz server.";

        public ComponentCategory Category => ComponentCategory.Other;

        public IComponent Create(LiveSplitState state) => new MultiblitzComponent(state);

        public string UpdateName => ComponentName;

        public string XMLURL => "http://livesplit.org/update/Components/update.LiveSplit.Multiblitz.xml";

        public string UpdateURL => "http://livesplit.org/update/";

        public Version Version => Version.Parse("1.8.0");
    }
}
