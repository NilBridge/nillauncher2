using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nillauncher.src.packs
{

    public class packBase
    {
        public string type { get; set; }
        public @params @params { get; set; }
    }

    public class @params
    {
        /// <summary>
        /// 
        /// </summary>
        public string cmd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sender { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }
    }
}
