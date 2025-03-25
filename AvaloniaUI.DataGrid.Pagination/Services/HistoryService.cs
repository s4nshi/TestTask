using AvaloniaUI.DataGrid.Pagination.Enums;
using AvaloniaUI.DataGrid.Pagination.Interfaces;
using AvaloniaUI.DataGrid.Pagination.Models;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaUI.DataGrid.Pagination.Services
{
    public class HistoryService : IHistoryLoader
    {
        private readonly Faker<HistoryPosition> _faker;

        public HistoryService()
        {
            var faker = new Faker();
            _faker = new Faker<HistoryPosition>()
                .RuleFor(x => x.PosId, f => Guid.NewGuid().ToString())
                .RuleFor(x => x.Ticker, f => f.PickRandom(new[] { "BTCUSDT", "ETHUSDT", "XRPUSDT", "SOLUSDT", "DOGEUSDT" }))
                .RuleFor(x => x.Side, f => f.PickRandom(new[] { PositionSide.BUY, PositionSide.SELL }))
                .RuleFor(x => x.Quantity, f => (decimal)f.Random.Number(1, 1000))
                .RuleFor(x => x.OpenPrice, f => (decimal)f.Random.Number(1, 100))
                .RuleFor(x => x.ClosePrice, f => (decimal)f.Random.Number(1, 100))
                .RuleFor(x => x.CloseTime, f => DateTime.UtcNow.AddMinutes(-f.Random.Number(1, 100)))
                .RuleFor(x => x.OpenTime, (f, x) => x.CloseTime.AddMinutes(-60));
        }

        public async Task<IEnumerable<HistoryPosition>> LoadHistoryPositionsAsync()
        {
            var count = _faker.Generate(1).First().Quantity;
            return await Task.FromResult(_faker.Generate((int)count));
        }
    }
}
