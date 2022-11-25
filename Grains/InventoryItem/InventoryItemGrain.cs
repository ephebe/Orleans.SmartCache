using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrainInterface;
using LiteDB;
using Orleans;

namespace Grains.InventoryItem
{
    public class InventoryItemGrain : Grain, IInventoryItemGrain
    {
        private LiteDatabase _db;
        private InventoryItemState _state;

        public override Task OnActivateAsync()
        {
            // Should be passing in a Func<LiteDatabase> via ctor DI
            _db = new LiteDatabase(@"MyData.db");

            var itemId = this.GetPrimaryKey();
            _state = _db.GetCollection<InventoryItemState>().Find(x => x.Id == itemId).FirstOrDefault();
            if (_state == null)
            {
                _state = new InventoryItemState
                {
                    Id = itemId
                };
                _db.GetCollection<InventoryItemState>().Insert(_state);
            }
            return Task.CompletedTask;
        }

        public override Task OnDeactivateAsync()
        {
            _db?.Dispose();
            return Task.CompletedTask;
        }

        public Task Increment(int qty)
        {
            _state.Quantity += qty;
            UpdateStorage();
            return Task.CompletedTask;
        }

        public Task Decrement(int qty)
        {
            _state.Quantity -= qty;
            UpdateStorage();
            return Task.CompletedTask;
        }

        private void UpdateStorage()
        {
            _db.GetCollection<InventoryItemState>().Update(_state);
        }

        public Task<int> Quantity()
        {
            return Task.FromResult(_state.Quantity);
        }
    }
}
