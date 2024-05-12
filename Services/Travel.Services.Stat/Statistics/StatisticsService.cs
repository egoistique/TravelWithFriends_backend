using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Threading.Tasks;
using Travel.Context;
using Travel.Context.Entities;

namespace Travel.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IDbContextFactory<MainDbContext> dbContextFactory;
        private readonly IMapper mapper;

        public StatisticsService(IDbContextFactory<MainDbContext> dbContextFactory, IMapper mapper)
        {
            this.dbContextFactory = dbContextFactory;
            this.mapper = mapper;
        }

        private List<ChartData> GetTeamExpensesData(Trip trip)
        {
            
            var teamExpensesData = new Dictionary<string, decimal>();

            foreach (var day in trip.Days)
            {
                foreach (var activity in day.Activities)
                {
                    if (activity.TotalPrice.HasValue)
                    {
                        decimal totalPrice = activity.TotalPrice.Value;

                        foreach (var payer in activity.Payers)
                        {
                            var amountPerParticipant = totalPrice / activity.Payers.Count;

                            if (!teamExpensesData.ContainsKey(payer.Email))
                            {
                                teamExpensesData[payer.Email] = amountPerParticipant;
                            }
                            else
                            {
                                teamExpensesData[payer.Email] += amountPerParticipant;
                            }
                        }
                    }
                }
            }

            var result = teamExpensesData.Select(pair => new ChartData { Name = pair.Key, Value = pair.Value }).ToList();

            return result;
        }

        private List<ChartData> GetCategoryExpensesData(Trip trip)
        {
            var categoryExpensesData = new Dictionary<string, decimal>();

            // Проходимся по каждому дню в поездке
            foreach (var day in trip.Days)
            {
                // Проходимся по каждой активности в дне
                foreach (var activity in day.Activities)
                {
                    // Проверяем, что у активности есть категория
                    if (!string.IsNullOrEmpty(activity.Category.Title))
                    {
                        string category = activity.Category.Title;

                        // Проверяем, что TotalPrice не равно null
                        if (activity.TotalPrice.HasValue)
                        {
                            decimal totalPrice = activity.TotalPrice.Value;

                            // Увеличиваем общую сумму трат для категории
                            if (!categoryExpensesData.ContainsKey(category))
                            {
                                categoryExpensesData[category] = totalPrice;
                            }
                            else
                            {
                                categoryExpensesData[category] += totalPrice;
                            }
                        }
                    }
                }
            }

            // Преобразуем данные в список ChartData
            var result = categoryExpensesData.Select(pair => new ChartData { Name = pair.Key, Value = pair.Value }).ToList();

            return result;
        }



        public async Task<StatisticsModel> GetAll(Guid tripId)
        {
            using var context = await dbContextFactory.CreateDbContextAsync();

            var trip = await context.Trips
                .Include(x => x.Participants)
                .Include(x => x.Days)
                .ThenInclude(d => d.Activities)
                .ThenInclude(t => t.Payers)
                .FirstOrDefaultAsync(x => x.Uid == tripId);

            if (trip == null) return null;

            var teamExpensesData = GetTeamExpensesData(trip);

            var categoryExpensesData = GetCategoryExpensesData(trip);

            return new StatisticsModel
            {
                TeamExpensesData = teamExpensesData,
                CategoryExpensesData = categoryExpensesData
            };
        }
    }
}
