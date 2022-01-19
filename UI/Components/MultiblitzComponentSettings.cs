using LiveSplit.Options;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using LiveSplit.Model.Input;
using System.Threading;

namespace LiveSplit.UI.Components {
  public partial class MultiblitzComponentSettings : UserControl {
    public string ServerHostname { get; set; }
    public string UserKey { get; set; }

    public MultiblitzComponentSettings() {
      InitializeComponent();

      ServerHostname = "localhost:9090";
      UserKey = "";

      txtServerHostname.DataBindings.Add("Text", this, "ServerHostname", false, DataSourceUpdateMode.OnPropertyChanged);
      txtUserKey.DataBindings.Add("Text", this, "UserKey", false, DataSourceUpdateMode.OnPropertyChanged);
    }
    public void SetSettings(XmlNode node) {
      var element = (XmlElement)node;
      ServerHostname = SettingsHelper.ParseString(element["ServerHostname"]);
      UserKey = SettingsHelper.ParseString(element["UserKey"]);

    }

    public XmlNode GetSettings(XmlDocument document) {
      var parent = document.CreateElement("Settings");
      CreateSettingsNode(document, parent);
      return parent;
    }

    public int GetSettingsHashCode() {
      return CreateSettingsNode(null, null);
    }

    private int CreateSettingsNode(XmlDocument document, XmlElement parent) {
      return SettingsHelper.CreateSetting(document, parent, "Version", "1.0") ^
      SettingsHelper.CreateSetting(document, parent, "ServerHostname", ServerHostname) ^
      SettingsHelper.CreateSetting(document, parent, "UserKey", UserKey);
    }

    private void ColorButtonClick(object sender, EventArgs e) {
      SettingsHelper.ColorButtonClick((Button)sender, this);
    }
  }
}
