using LiveSplit.Model;
using System;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using LiveSplit.Options;

namespace LiveSplit.UI.Components {
  public class MultiblitzComponent : LogicComponent {

    private LiveSplitState state;
    private MultiblitzComponentSettings settings;

    private HttpClient client;

    public MultiblitzComponent(LiveSplitState state) {
      settings = new MultiblitzComponentSettings();
      Cache = new GraphicsCache();
      client = new HttpClient();

      this.state = state;

      state.OnStart += State_OnStart;
      state.OnSplit += State_OnSplit;
      state.OnReset += State_OnReset;
    }


    public GraphicsCache Cache { get; set; }

    public override string ComponentName => "Multiblitz";

    public override Control GetSettingsControl(LayoutMode mode) {
      return settings;
    }

    public override System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document) {
      return settings.GetSettings(document);
    }

    public override void SetSettings(System.Xml.XmlNode settings) {
      this.settings.SetSettings(settings);
    }

    public override void Update(IInvalidator invalidator, Model.LiveSplitState state, float width, float height, LayoutMode mode) { }

    public override void Dispose() {
      try {
        client.Dispose();
      } catch (Exception ex) {
        Log.Error(ex);
      }
    }

    private void State_OnStart(object sender, EventArgs e) {
      SendStartMessage().RunSynchronously();
    }

    private void State_OnSplit(object sender, EventArgs e) {
      if (state.CurrentPhase == TimerPhase.Ended) {
        SendStopMessage().RunSynchronously();
      }
    }

    private void State_OnReset(object sender, TimerPhase e) {
      if (e != TimerPhase.Ended) {
        SendStopMessage().RunSynchronously();
      }
    }
    private long UnixTimestampFromDateTime(DateTime date) {
      long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;

      unixTimestamp /= TimeSpan.TicksPerMillisecond;

      return unixTimestamp;
    }

    private async Task SendStartMessage() {
      try {
        await client.GetAsync($"{settings.ServerHostname}/multiblitz/start?key={settings.UserKey}&time={UnixTimestampFromDateTime(state.AttemptStarted.Time.ToUniversalTime())}");
      } catch (Exception ex) {
        Log.Error(ex);
      }
    }

    private async Task SendStopMessage() {
      try {
        await client.GetAsync($"{settings.ServerHostname}/multiblitz/stop?key={settings.UserKey}&time={UnixTimestampFromDateTime(state.AttemptEnded.Time.ToUniversalTime())}");
      } catch (Exception ex) {
        Log.Error(ex);
      }
    }

    public int GetSettingsHashCode() {
      return settings.GetSettingsHashCode();
    }

  }
}
