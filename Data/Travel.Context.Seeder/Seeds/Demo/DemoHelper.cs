namespace Travel.Context.Seeder;

using System.Net.NetworkInformation;
using Travel.Context.Entities;

public class DemoHelper
{
    private User user1;
    private User user2;
    private User user3;

    public DemoHelper()
    {
        user1 = new User
        {
            FullName = "Василий 1",
            Status = UserStatus.Active
        };

        user2 = new User
        {
            FullName = "Василий 2",
            Status = UserStatus.Active
        };

        user3 = new User
        {
            FullName = "Василий 3",
            Status = UserStatus.Active
        };
    }

    public IEnumerable<Trip> GetTrips
    {
        get
        {
            return new List<Trip>
            {
                new Trip()
                {
                    Uid = Guid.NewGuid(),
                    Title = "Поездка в Москву",
                    Creator = user1,
                    NumOfParticipants = 3,
                    DateStart = "26.03.2024",
                    DateEnd = "30.03.2024",
                    City = "Москва",
                    HotelTitle = "Hotel X",
                    Participants = new List<User>()
                    {
                        user1,
                        user2,
                        user3
                    },
                    Activities = new List<Activiti>()
                    {
                        new Activiti()
                        {
                            Title = "Третьяковская галерея",
                            Category = new Category()
                            {
                                Title = "Музей",
                            },
                            Participants = new List<User>()
                            {
                                user1,
                                user2,
                                user3
                            },
                            PricePerOne = 500,
                            TotalPrice = 1500,
                            Payer = user2
                        }
                    }
                   

                }
            };
        }
    }
}
