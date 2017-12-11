using BioMatcher;
using BioMatcher.ServiceAdapter;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioMatcher.Server
{
    /// <summary>
    /// Echoes messages sent using the Send message by calling the
    /// addMessage method on the client. Also reports to the console
    /// when clients connect and disconnect.
    /// </summary>
    public class BioMatcherHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, DateTime.Now, message);
        }

        public void UpdateCache(int localeId = -1, bool fullUpdate = false)
        {
            Program.ServerForm.UpdateCache(localeId, fullUpdate);
        }

        public void Identify(MatchRequest request)
        {
            // Clients.All
            if (Program.ServerForm.BenchmarkMode)
                Clients.Caller.addMessage(DateTime.Now, "Got Request. Matching...");

            if(Program.ServerForm.Verbose)
                Program.ServerForm.LogMessage("Got Request. Matching...");

            //Func<CompareResult, CompareResult> callback = (result) =>
            //{
            //    if (result.Success)
            //        Clients.All.addMessage(name, date, "Callback > Scanned: " + result.Scanned);
            //    return null;
            //};

            var result = Program.ServerForm.Identify(request);
            // Clients.All
            Clients.Caller.IdentifyComplete(result);
            if (Program.ServerForm.Verbose)
                Program.ServerForm.LogMessage("Result sent.");
        }

        public override Task OnConnected()
        {
            Program.ServerForm.LogMessage("Client connected: " + Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Program.ServerForm.LogMessage("Client disconnected: " + Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}
