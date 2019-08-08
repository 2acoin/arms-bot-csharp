using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrtlBotSharp
{
    partial class TrtlBotSharp
    {
        // Discord.Net Variables
        public static DiscordSocketClient _client;
        public static CommandService _commands;
        public static IServiceProvider _services;

        // File Variables
        public static string
            configFile = "config.json",
            databaseFile = "users.db";

        // Operation Variables
        public static int
            logLevel = 1;

        // Permission Variables
        [JsonExtensionData]
        public static List<ulong>
            marketDisallowedServers = new List<ulong>
            {
                388915017187328002
            };

        // Bot Variables
        public static string
            botToken = "[BOT TOKEN]",
            botPrefix = ".";
        public static int
            botMessageCache = 100;

        // Currency Variables
        public static string
            coinName = "2ACoin",
            coinSymbol = "ARMS",
            coinAddressPrefix = "guns";
        public static decimal
            coinUnits = 100000000;
        public static int
            coinAddressLength = 98;

       // Tipping Variables
        public static decimal
            tipFee = 0005;
        public static int
            tipMixin = 3;
        public static string
            tipDefaultAddress = "",
            tipSuccessReact = "💸",
            tipFailedReact = "🆘",
            tipLowBalanceReact = "❌",
            tipJoinReact = "tip";
        public static List<string>
            tipAddresses = new List<string>();
        public static Dictionary<string, decimal>
            tipCustomReacts = new Dictionary<string, decimal>();

        // Faucet Variables
        public static string
            faucetHost = "https://faucet.2acoin.org/",
            faucetEndpoint = "https://faucet.2acoin.org/balance",
            faucetAddress = "gunsAwiL53dLvBQ9DKB6DD7svVXkyUPJ4BqHEGxooqL8FRb9Xti8CaCQWY8gCSkCCe1d77eQbyn1S7gsXYjQHFBEADRfG6vSVh";
        public static int
            faucetPort = 17760;

        // Market Variables
        public static string
            marketSourceK = "FirstCryptoBank",
            marketEndpointK = "https://fcbaccount.com/api/public/v1/get/exchange/market?pair=BTC-ARMS",
            marketSourceR = "FinexBox",
            marketEndpointR = "https://xapi.finexbox.com/v1/ticker?market=arms_btc",
            marketBTCEndpoint = "https://www.bitstamp.net/api/ticker/";
        
        // Market Cache 
        public static string[]
            marketCacheArray = { "01/01/2019 12:00:00 PM", "ck-low", "ck-high", "ck-last", "ck-vol", "cr-low", "cr-high", "cr-last", "cr-average", "cr-vol", "btc-price" };

        // Daemon Variables
        public static string
            daemonHost = "127.0.0.1";
        public static int
            daemonPort = 17910;

        // Wallet Variables
        public static string
            walletHost = "127.0.0.1",
            walletRpcPassword = "12345";
        public static int
            walletPort = 17760,
            walletUpdateDelay = 500;
    }
}
