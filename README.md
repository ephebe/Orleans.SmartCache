# Orleans.SmartCache
這是從https://github.com/dcomartin/PracticalOrleans ，翻改過來的，提昇到.Net5.0，Orleans提昇到3.5.1版。

關於Orleans的原理與使用，可參考下列網址
https://www.michalbialecki.com/en/2018/03/05/getting-started-microsoft-orleans/

Orleans的應用模式之一，Smart Cache Pattern，可參考以下網址
https://codeopinion.com/orleans-smart-cache-pattern/

專案中，有4種Grain
1. CounterGrain
  狀態置於Grain的私有變數
2. CustomerGrain
  狀態儲存配合Orleans的儲存機制，存在記憶體中
3. InventoryItemGrain
  使用一個外部DB-LiteDB來保存狀態
4. BankAccountGrain
  使用外部DB-Sql Server + SqlStreamStore，採用Event Sourcing的方式儲存。

會研究這個，是覺得比之Redis方便許多，讀取時全從記憶體拿，寫入時則只有單純的Insert。
Orleans自行管控生命周期，掛了立刻重啟。又可用C#編程，構成許多提供Cache的微服務。

這次並沒有使用微軟的儲存機制，也沒有研究怎麼配合JournaledGrain去使用Event Soucing，
Event Sourcing也最好配合專門的DB，如EventStoreDB、Marten、Kakfa。
