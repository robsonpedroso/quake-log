using Microsoft.Extensions.Configuration;
using System;

namespace QuakeLog.Domain
{
    /// <summary>
    /// Centralization of configuration data 
    /// </summary>
    public class Config
    {
        private IConfiguration config { get; set; }

        public virtual string BasePath { get { return this.config["Game:BasePath"] ?? Environment.CurrentDirectory; } }
        public virtual string LogPath { get { return this.config["Game:LogPath"] ?? "assets"; } }
        public virtual string LogName { get { return this.config["Game:LogName"]; } }
        public virtual string LogPathName { get { return string.Concat(BasePath, "\\", LogPath, "\\", LogName); } }

        public virtual int World { get { return Convert.ToInt32(this.config["Game:World"]); } }

        public virtual string AllActions { get { return this.config["Game:AllActions"]; } }

        public Config(IConfiguration configuration)
        {
            config = configuration;
        }
    }
}
