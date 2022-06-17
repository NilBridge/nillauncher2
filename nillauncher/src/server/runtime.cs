using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace nillauncher.src.server
{
    public class Encrypt
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string password { get; set; }
    }

    public class Ws
    {
        /// <summary>
        /// 
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endpoint { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int port { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Encrypt encrypt { get; set; }
    }

    public class Rcon
    {
        /// <summary>
        /// 
        /// </summary>
        public bool enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int port { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string passwd { get; set; }
    }
    public class CEncoding
    {
        /// <summary>
        /// 
        /// </summary>
        public string output { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string input { get; set; }
    }

    public class Config
    {
        /// <summary>
        /// 
        /// </summary>
        public string file_path { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Ws ws { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CEncoding encoding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Rcon rcon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int restart_time { get; set; }
        public string version { get; set; }
    }
    public class ws_key {
        public  string k { get; set; }
        public string iv { get; set; }
    }
    public class Runtime
    {
        public static Process bds;
        public static Config config;
        public static ws_key key = new ws_key();
        public static bool exit_by_stop = false;
        public static void loadConfig()
        {
            if(File.Exists("./nillauncher/config.json") == false)
            {
                Directory.CreateDirectory("nillauncher");
                File.WriteAllText("./nillauncher/config.json",JsonConvert.SerializeObject(new Config
                {
                    encoding = new CEncoding { input="UTF-8",output="UTF-8"},
                    ws = new Ws { encrypt = new Encrypt { enable = false,password = "passwd"},endpoint= "/mcws",port=8080,type=0 },
                    file_path = "./bedrock_server_mod.exe",
                    rcon = new Rcon { enable=false,passwd="passwd",port=8081},
                    restart_time = 5,
                    version = "1.0.0"
                },Formatting.Indented));
            }
            string configContent = File.ReadAllText("./nillauncher/config.json");
            config = JsonConvert.DeserializeObject<Config>(configContent);
            key.k = MD5.MD5Encrypt(config.ws.encrypt.password).Substring(0, 16);
            key.iv = MD5.MD5Encrypt(config.ws.encrypt.password).Substring(16);
        }
    }
    public class ProcessHelper
    {
        static int start_time = 0;
        public static Process CreateProcess(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"无法找到{filename}!");
            FileInfo fi = new FileInfo(filename);
            Process ps = new Process();
            ps.StartInfo.FileName = filename;
            ps.StartInfo.RedirectStandardOutput = true;
            ps.StartInfo.UseShellExecute = false;
            ps.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            ps.StartInfo.CreateNoWindow = true;
            ps.StartInfo.RedirectStandardInput = true;
            ps.StartInfo.RedirectStandardError = true;
            ps.StartInfo.WorkingDirectory = fi.DirectoryName;
            ps.EnableRaisingEvents = true;
            ps.StartInfo.StandardOutputEncoding =Encoding.GetEncoding(Runtime.config.encoding.output);
            ps.Exited += bds_exit;
            ps.OutputDataReceived += console_output.on_out_put;
            return ps;
        }


        public static void start_bds()
        {
            Runtime.bds = CreateProcess(Runtime.config.file_path);
            Runtime.exit_by_stop = false;
            start_time += 1;
            var b = new Thread(() =>
            {
                Runtime.bds.Start();
                Runtime.bds.BeginErrorReadLine();
                Runtime.bds.BeginOutputReadLine();
                Runtime.bds.WaitForExit(0);
            });
            b.IsBackground = true;
            b.Start();
        }
        private static void bds_exit(object sender, EventArgs e)
        {
            start_time = 0;
        }
    }
}
