using System.Linq;

namespace BilibiliBatchBlock
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: BilibiliBatchBlock.exe <uidListFile>");
                return;
            }
            string uidList = File.ReadAllText(args[0]);
            string[] uidArray = uidList.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < uidArray.Length; i++)
            {
                uidArray[i] = uidArray[i].Trim();
                if (uidArray[i].StartsWith("#"))
                {
                    uidArray[i] = "";
                }
            }
            uidArray = uidArray.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            for (int i = 0; i < uidArray.Length; i++)
            {
                uidArray[i] = uidArray[i].Trim();
                if (uidArray[i].Contains("#"))
                {
                    uidArray[i] = uidArray[i].Split('#')[0];
                    uidArray[i] = uidArray[i].Trim();
                }
            }
            string script = @"
const csrf = ""这个换成你的csrf""
const uidList = ""{HereShouldBeUidList}""
await fetch(""https://api.bilibili.com/x/relation/batch/modify"", {
    ""credentials"": ""include"",
    ""headers"": {
        ""Accept"": ""application/json, text/plain, */*"",
        ""Accept-Language"": ""zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2"",
        ""Content-Type"": ""application/x-www-form-urlencoded"",
        ""Sec-Fetch-Dest"": ""empty"",
        ""Sec-Fetch-Mode"": ""cors"",
        ""Sec-Fetch-Site"": ""same-site"",
        ""Sec-GPC"": ""1"",
        ""Pragma"": ""no-cache"",
        ""Cache-Control"": ""no-cache""
    },
    ""body"": `fids=${uidList}&act=5&re_src=11&csrf=${csrf}`,
    ""method"": ""POST"",
    ""mode"": ""cors""
});
            ";
            Console.WriteLine(script.Replace("{HereShouldBeUidList}", string.Join(",", uidArray)));
        }
    }
}