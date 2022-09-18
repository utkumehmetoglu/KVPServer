using KVP.KVP;



// See https://aka.ms/new-console-template for more information
Console.WriteLine("Kvp Server Starting");

KvpServer kvpServer = new KvpServer();

while (true) { Thread.Sleep(int.MaxValue); } // sleep indefinetly

