using Discord; 
using Discord.Commands; 
using Newtonsoft.Json.Linq; 
using System; 
using System.Threading.Tasks; 

namespace TrtlBotSharp 
{
    public partial class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("price", RunMode = RunMode.Async)]
        public async Task PriceAsync([Remainder]string Remainder = "")
        {
            // Begin building a response
            await TrtlBotSharp.GetMarketCache();
            
            // Exchange Data
            var Response = new EmbedBuilder();
            if (TrtlBotSharp.marketCacheArray[0] == "01/01/2019 12:00:00 PM")
            {
                Response.WithTitle("Current Price of ARMS: " + TrtlBotSharp.marketSource);
                Response.WithUrl(TrtlBotSharp.marketEndpoint);
                Response.AddInlineField("--Note--", "Data is not currently available");
            }
            else
            {
                Response.WithTitle("Current Price of ARMS: " + TrtlBotSharp.marketSource);
                Response.WithUrl(TrtlBotSharp.marketEndpoint);
                Response.AddInlineField("Low", string.Format("{0} sats", Math.Round((decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[1]) * 100000000)));
                Response.AddInlineField("Current", string.Format("{0} sats", Math.Round((decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[3]) * 100000000)));
                Response.AddInlineField("High", string.Format("{0} sats", Math.Round((decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[2]) * 100000000)));
                Response.AddInlineField(TrtlBotSharp.coinSymbol + "-USD", string.Format("${0:N5} USD", (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[3]) * (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[11])));
                Response.AddInlineField("Volume BTC/USD", string.Format("{0:F4}/{1:C}", (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[4]), (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[4]) * (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[11])));
                Response.AddInlineField("BTC-USD", string.Format("{0:C} USD", (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[11])));
                DateTime cachedDate = DateTime.Parse(TrtlBotSharp.marketCacheArray[0]);
                Response.AddInlineField("Data as of", string.Format("{0}", cachedDate));
            }

            // Send Exchange data reply
            if (Context.Guild != null && TrtlBotSharp.marketDisallowedServers.Contains(Context.Guild.Id))
            {
                try { await Context.Message.DeleteAsync(); }
                catch { }
                await Context.Message.Author.SendMessageAsync("", false, Response);
            }
            else await ReplyAsync("", false, Response);
        }
        
        [Command("mcap", RunMode = RunMode.Async)]
        public async Task MarketCapAsync([Remainder]string Remainder = "")
        {
            // Begin building a response
            await TrtlBotSharp.GetMarketCache();

            decimal CoinPrice = (decimal)Convert.ToDecimal(TrtlBotSharp.marketCacheArray[3]);
            
            // Begin building a response
            string Response = string.Format("{0}'s market cap is **{1:c}** USD", TrtlBotSharp.coinName,
                ((decimal)CoinPrice * (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[11])) * TrtlBotSharp.GetSupply());

            // Send reply
            if (Context.Guild != null && TrtlBotSharp.marketDisallowedServers.Contains(Context.Guild.Id))
            {
                try { await Context.Message.DeleteAsync(); }
                catch { }
                await Context.Message.Author.SendMessageAsync(Response);
            }
            else await ReplyAsync(Response);
        }
    }
}
