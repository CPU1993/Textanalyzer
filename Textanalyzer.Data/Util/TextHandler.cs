using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Textanalyzer.Data.Data;
using Textanalyzer.Data.Entities;

namespace Textanalyzer.Data.Util
{
    public class TextHandler
    {
        private string userName;
        public TextHandler(ApplicationDbContext context, string userName)
        {
            this.Context = context;
            this.userName = userName;
        }

        public ApplicationDbContext Context { get; private set; }

        public IList<Text> GetCurrentUserTexts()
        {
            IList<Text> result = new List<Text>();

            result = Context.Texts.Where(text => text.UserName == this.userName).ToList();

            return result;
        }
    }
}
