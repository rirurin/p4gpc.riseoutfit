using p4gpc.riseoutfit.Configuration.Implementation;
using System.ComponentModel;

namespace p4gpc.riseoutfit.Configuration
{
    public class Config : Configurable<Config>
    {
        /*
            User Properties:
                - Please put all of your configurable properties here.
                - Tip: Consider using the various available attributes https://stackoverflow.com/a/15051390/11106111
        
            By default, configuration saves as "Config.json" in mod folder.    
            Need more config files/classes? See Configuration.cs
        */

        [DisplayName("Debug Mode")]
        [Description("Enables Debug Mode")]
        public bool DebugEnabled { get; set; } = false;

    }
}
