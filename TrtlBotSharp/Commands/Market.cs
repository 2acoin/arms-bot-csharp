using Discord; 
using Discord.Commands; 
using Newtonsoft.Json.Linq; 
using System; 
using System.Threading.Tasks; 

namespace TrtlBotSharp 
{
    public partial class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("price")]
        public async Task PriceAsync([Remainder]string Remainder = "")
        {
            // Get current coin price
            JObject CoinPrice = Request.GET(TrtlBotSharp.marketEndpoint);
            if (CoinPrice.Count < 1)
            {
                await ReplyAsync("Failed to connect to " + TrtlBotSharp.marketSource);
                return;
            }
			
            // Get current BTC price
            JObject BTCPrice = Request.GET(TrtlBotSharp.marketBTCEndpoint);
            if (BTCPrice.Count < 1)
            {
                await ReplyAsync("Failed to connect to " + TrtlBotSharp.marketBTCEndpoint);
                return;
            }
			
	    //Check for NULLs in Data
	    JToken tokenLow = CoinPrice["info24h"]["low"];
	    decimal CoinLow = 0;
		if (tokenLow.Type == JTokenType.Null)
			CoinLow = 0;
		else
			CoinLow = (decimal)CoinPrice["info24h"]["low"];
	    
            JToken tokenHigh = CoinPrice["info24h"]["high"];
	    decimal CoinHigh = 0;
		if (tokenHigh.Type == JTokenType.Null)
			CoinHigh = 0;
		else
			CoinHigh = (decimal)CoinPrice["info24h"]["high"];
                
            // Begin building a response
            var Response = new EmbedBuilder();
            Response.WithTitle("Current Price of ARMS: " + TrtlBotSharp.marketSource);
            Response.WithUrl(TrtlBotSharp.marketEndpoint);
            Response.AddInlineField("Low", string.Format("{0} sats", Math.Round((decimal)CoinLow * 100000000)));
            Response.AddInlineField("Current", string.Format("{0} sats", Math.Round((decimal)CoinPrice["lastPrice"] * 100000000)));
            Response.AddInlineField("High", string.Format("{0} sats", Math.Round((decimal)CoinHigh * 100000000)));
            Response.AddInlineField(TrtlBotSharp.coinSymbol + "-USD", string.Format("${0:N5} USD", (decimal)CoinPrice["lastPrice"] * (decimal)BTCPrice["last"]));
            Response.AddInlineField("Volume", string.Format("{0:N} BTC", (decimal)CoinPrice["volume"]));
            Response.AddInlineField("BTC-USD", string.Format("{0:C} USD", (decimal)BTCPrice["last"]));
			
            // Send reply
            if (Context.Guild != null && TrtlBotSharp.marketDisallowedServers.Contains(Context.Guild.Id))
            {
                try { await Context.Message.DeleteAsync(); }
                catch { }
                await Context.Message.Author.SendMessageAsync("", false, Response);
            }
            else await ReplyAsync("", false, Response);
        }
        [Command("mcap")]
        public async Task MarketCapAsync([Remainder]string Remainder = "")
        {
            // Get current coin price
            JObject CoinPrice = Request.GET(TrtlBotSharp.marketEndpoint);
            if (CoinPrice.Count < 1)
            {
                await ReplyAsync("Failed to connect to " + TrtlBotSharp.marketSource);
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
            string Response = string.Format("{0}'s market cap is **{1:c}** USD", TrtlBotSharp.coinName,
                (decimal)CoinPrice["lastPrice"] * (decimal)BTCPrice["last"] * TrtlBotSharp.GetSupply());
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
