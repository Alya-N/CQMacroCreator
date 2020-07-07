using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace CQMacroCreator
{
    public struct AuctionBids
    {
        public bool? biddingEnabled;
        public string name;
        public int? maxLevel;
        public int? maxBid;
    }
    public class AppSettings
    {
        public string KongregateId { get; set; }
        public string token { get; set; }
        public int? actionOnStart { get; set; }
        public string defaultLowerLimit { get; set; }
        public string defaultUpperLimit { get; set; }
        public List<string> LoCLineup { get; set; }
        public List<string> MOAKLineup { get; set; }
        public List<string> KrytonLineup { get; set; }
        public List<string> DoyLineup { get; set; }
        public List<string> BorLineup { get; set; }
        public List<string> defaultDQLineup { get; set; }
        public List<string> calcEnabledHeroes { get; set; }
        public bool? DQSoundEnabled { get; set; }
        public bool? autoBestDQEnabled { get; set; }
        public bool? autoDQEnabled { get; set; }
        public bool? autoPvPEnabled { get; set; }
        public bool? autoChestEnabled { get; set; }
        public int? chestsToOpen { get; set; }
        public bool? autoWBEnabled { get; set; }
        public bool? safeModeWBEnabled { get; set; }
        public int? pvpLowerLimit { get; set; }
        public int? pvpUpperLimit { get; set; }
        public List<int> WBsettings { get; set; }
        public List<AuctionBids> bids { get; set; }
        public bool? autoLevelEnabled { get; set; }
        public int[] bankedCurrencies { get; set; }
        public string[] herosToLevel { get; set; }
        public int[] levelLimits { get; set; }
        public bool? waitAutoLevel { get; set; }
        public bool? instantMaxPriceBid { get; set; }
        public bool? autoEvEnabled { get; set; }
        public bool? warnManyHeroes { get; set; }
        public bool? usernameActivated { get; set; }
        public bool? doPVPHistory { get; set; }
        public bool? doAutoFT { get; set; }
        public bool? doAutoDG { get; set; }
        public bool? doAutoLF { get; set; }
        public bool? doAutoKT { get; set; }
        public bool? doAutoCC { get; set; }
        public bool? doAutoPG { get; set; }
        public bool? doAutoAD { get; set; }
        public int? optAutoAD { get; set; }
        public bool? doAutoLO { get; set; }
        public int? optAutoLO { get; set; }
        public string[] heroesToProm6 { get; set; }
        public int? calcTimeLimit { get; set; }
        public bool? autoWEvEnabled { get; set; }
        public int? sjUpgrade { get; set; }
        public string adminPassword { get; set; }

        public static AppSettings loadSettings()
        {        
            if (File.Exists(Form1.SettingsFilename))
            {
                StreamReader sr = new StreamReader(Form1.SettingsFilename);
                AppSettings a = JsonConvert.DeserializeObject<AppSettings>(sr.ReadToEnd());
                sr.Close();
                return a;
            }
            else
            {
                return new AppSettings();
            }
        }

        public void saveSettings()
        {
            try
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                StreamWriter sw = new StreamWriter(Form1.SettingsFilename);
                sw.Write(json);
                sw.Close();
            }
            catch (Exception)
            { }
        }
    }
}
