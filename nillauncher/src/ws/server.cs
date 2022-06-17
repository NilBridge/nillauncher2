using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nillauncher.src.server;
using WebSocketSharp;
using WebSocketSharp.Server;
using nillauncher.src.packs;
using Newtonsoft.Json;

namespace nillauncher.src.ws
{
    public class serve : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Program.Server.clients.Add(base.ID,base.Context.WebSocket);
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            string raw_text = "";
            try
            {
                if (e.IsBinary == false)
                {
                    raw_text = Encoding.UTF8.GetString(e.RawData);
                }
                else
                {
                    raw_text = e.Data;
                }
                var pack = JsonConvert.DeserializeObject<packs.packBase>(raw_text);
                onmessageReceived ce = new onmessageReceived();
                switch (pack.type)
                {
                    case "pack":
                        ce =  onmessage.message(pack,base.ID);
                        break;
                    case "encrypt":
                        string unencrypt = AES.AesDecrypt(pack.@params.raw, Runtime.key.k, Runtime.key.iv);
                        ce = onmessage.message(JsonConvert.DeserializeObject<packBase>(unencrypt),base.ID);
                        break;
                }
                if (ce.reply)
                {
                    base.Send(ce.raw);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
        }
    }
    public class server
    {
        private WebSocketServer ws;
        public Dictionary<string, WebSocket> clients = new Dictionary<string, WebSocket>();
        public server(string url, string endpoint)
        {
            ws = new WebSocketServer(url);
            ws.Start();
            ws.AddWebSocketService<serve>(endpoint);
        }
        public void sendCmdResult(string id,string pack)
        {
            clients[id].Send(pack);
        }
    }
}
