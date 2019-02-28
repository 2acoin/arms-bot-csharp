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
            
            // FCB
            var Response = new EmbedBuilder();
            Response.WithTitle("Current Price of ARMS: " + TrtlBotSharp.marketSourceK);
            Response.WithUrl(TrtlBotSharp.marketEndpointK);
            Response.AddInlineField("Low", string.Format("{0} sats", Math.Round((decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[1]) * 100000000)));
            Response.AddInlineField("Current", string.Format("{0} sats", Math.Round((decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[3]) * 100000000)));
            Response.AddInlineField("High", string.Format("{0} sats", Math.Round((decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[2]) * 100000000)));
            Response.AddInlineField(TrtlBotSharp.coinSymbol + "-USD", string.Format("${0:N5} USD", (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[3]) * (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[9])));
            Response.AddInlineField("Volume BTC/USD", string.Format("{0:N}/{1:C}", (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[4]), (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[4]) * (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[9])));
            Response.AddInlineField("BTC-USD", string.Format("{0:C} USD", (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[9])));
 
            // Send FCB reply
            if (Context.Guild != null && TrtlBotSharp.marketDisallowedServers.Contains(Context.Guild.Id))
            {
                try { await Context.Message.DeleteAsync(); }
                catch { }
                await Context.Message.Author.SendMessageAsync("", false, Response);
            }
            else await ReplyAsync("", false, Response);
 
            // Raisex 
            Response = new EmbedBuilder();
            Response.WithTitle("Current Price of ARMS: " + TrtlBotSharp.marketSourceR);
            Response.WithUrl(TrtlBotSharp.marketEndpointR);
            Response.AddInlineField("Low", string.Format("{0} sats", Math.Round((decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[5]) * 100000000)));
            Response.AddInlineField("Current", string.Format("{0} sats", Math.Round((decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[7]) * 100000000)));
            Response.AddInlineField("High", string.Format("{0} sats", Math.Round((decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[6]) * 100000000)));
            Response.AddInlineField(TrtlBotSharp.coinSymbol + "-USD", string.Format("${0:N5} USD", (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[7]) * (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[9])));
            Response.AddInlineField("Volume BTC/USD", string.Format("{0:N}/{1:C}", ((decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[8]) * (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[7])), ((decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[8]) * (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[7]) * (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[9])));
            Response.AddInlineField("BTC-USD", string.Format("{0:C} USD", (decimal)decimal.Parse(TrtlBotSharp.marketCacheArray[9])));
			
//            await TrtlBotSharp.GetMarketCache();
            DateTime cachedDate = DateTime.Parse(TrtlBotSharp.marketCacheArray[0]);
            Response.AddInlineField("Data as of", string.Format("{0}", cachedDate));


            // Send Raisex reply
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
            // Get current coin price - FCB
            JObject CoinPriceK = Request.GET(TrtlBotSharp.marketEndpointK);
            if (CoinPriceK.Count < 1)
            {
                await ReplyAsync("Failed to connect to " + TrtlBotSharp.marketSourceK);
                return;
            }

            // Get current coin price - Raisex
            JObject CoinPriceR = Request.GET(TrtlBotSharp.marketEndpointR);
            if (CoinPriceR.Count < 1)
            {
                await ReplyAsync("Failed to connect to " + TrtlBotSharp.marketSourceR);
                return;
            }
            
            // Get current BTC price
            JObject BTCPrice = Request.GET(TrtlBotSharp.marketBTCEndpoint);
            if (BTCPrice.Count < 1)
            {
                await ReplyAsync("Failed to connect to " + TrtlBotSharp.marketBTCEndpoint);
                return;
            }

            // Calculate a weighted avg price
            decimal CoinPrice = ((decimal)CoinPriceK["data"]["last"] + (decimal)CoinPriceR["last"]) / 2;
            
            // Begin building a response
            string Response = string.Format("{0}'s market cap is **{1:c}** USD", TrtlBotSharp.coinName,
                ((decimal)CoinPrice * (decimal)BTCPrice["last"]) * TrtlBotSharp.GetSupply());
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
