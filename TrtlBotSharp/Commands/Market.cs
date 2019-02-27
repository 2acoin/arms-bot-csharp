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
			               
            // Begin building a response
            // FCB
            var Response = new EmbedBuilder();
            Response.WithTitle("Current Price of ARMS: " + TrtlBotSharp.marketSourceK);
            Response.WithUrl(TrtlBotSharp.marketEndpointK);
            Response.AddInlineField("Low", string.Format("{0} sats", Math.Round((decimal)CoinPriceK["data"]["low"] * 100000000)));
            Response.AddInlineField("Current", string.Format("{0} sats", Math.Round((decimal)CoinPriceK["data"]["last"] * 100000000)));
            Response.AddInlineField("High", string.Format("{0} sats", Math.Round((decimal)CoinPriceK["data"]["high"] * 100000000)));
            Response.AddInlineField(TrtlBotSharp.coinSymbol + "-USD", string.Format("${0:N5} USD", (decimal)CoinPriceK["data"]["last"] * (decimal)BTCPrice["last"]));
            Response.AddInlineField("Volume BTC/USD", string.Format("{0:N}/{1:C}", (decimal)CoinPriceK["data"]["volume"], (decimal)CoinPriceK["data"]["volume"] * (decimal)BTCPrice["last"]));
            Response.AddInlineField("BTC-USD", string.Format("{0:C} USD", (decimal)BTCPrice["last"]));
 
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
            Response.AddInlineField("Low", string.Format("{0} sats", Math.Round((decimal)CoinPriceR["best_bid"] * 100000000)));
            Response.AddInlineField("Current", string.Format("{0} sats", Math.Round((decimal)CoinPriceR["last"] * 100000000)));
            Response.AddInlineField("High", string.Format("{0} sats", Math.Round((decimal)CoinPriceR["best_ask"] * 100000000)));
            Response.AddInlineField(TrtlBotSharp.coinSymbol + "-USD", string.Format("${0:N5} USD", (decimal)CoinPriceR["last"] * (decimal)BTCPrice["last"]));
            Response.AddInlineField("Volume BTC/USD", string.Format("{0:N}/{1:C}", ((decimal)CoinPriceR["volume"] * (decimal)CoinPriceR["last"]), ((decimal)CoinPriceR["volume"] * (decimal)CoinPriceR["last"]) * (decimal)BTCPrice["last"]));
            Response.AddInlineField("BTC-USD", string.Format("{0:C} USD", (decimal)BTCPrice["last"]));
			
            DateTime cachedDate = DateTime.Parse(TrtlBotSharp.marketCacheArray[0]);
            Response.AddInlineField("Data as of - ", string.Format("{0}", cachedDate));


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
