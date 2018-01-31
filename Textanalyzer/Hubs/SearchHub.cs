using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Textanalyzer.Data.Util;

namespace Textanalyzer.Web.Hubs
{
    public class SearchHub : Hub
    {
        private string userId;

        public void Search(string words)
        {
            this.userId = this.Context.User.Identity.Name;            
        }

        public async Task SendResult(string scores)
        {
            await this.Clients.User(userId).InvokeAsync(scores);
        }
    }
}
