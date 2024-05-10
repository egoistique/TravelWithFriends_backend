﻿namespace Travel.Context.Seeder;

using System.Net.NetworkInformation;
using Travel.Context.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class DemoHelper
{
    private User user1;
    private User user2;
    private User user3;

    private Category category1;
    private Category category2;    
    private Category category3;    
    private Category category4;

    public DemoHelper()
    {
        user1 = new User
        {
            FullName = "Василий",
            Status = UserStatus.Active
        };

        user2 = new User
        {
            FullName = "Петр",
            Status = UserStatus.Active
        };

        user3 = new User
        {
            FullName = "Мария",
            Status = UserStatus.Active
        };

        category1 = new Category
        {
            Title = "Галерея"
        };

        category2 = new Category()
        {
            Title = "Музей",
        };

        category3 = new Category()
        {
            Title = "Прогулка",
        };

        category4 = new Category()
        {
            Title = "Театр",
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
                    IsPublicated = true,
                    Participants = new List<User>()
                    {
                        user1,
                        user2,
                        user3
                    },
                    Days = new List<TripDay>
                    {
                        new TripDay()
                        {
                            Number = 1,
                            Activities = new List<Activiti>()
                            {
                                new Activiti()
                                {
                                    Title = "Третьяковская галерея",
                                    FromSearch = true,
                                    Category = category1,
                                    Participants = new List<User>()
                                    {
                                        user1,
                                        user2,
                                        user3
                                    },
                                    PricePerOne = 500,
                                    TotalPrice = 1500,
                                    Payers = new List<User>()
                                    {
                                        user1,
                                        user2,
                                        user3
                                    },
                                },
                                new Activiti()
                                {
                                    Title = "Кремль",
                                    FromSearch = true,
                                    Category = category2,
                                    Participants = new List<User>()
                                    {
                                        user1,
                                        user2,
                                        user3
                                    },
                                    PricePerOne = 300,
                                    TotalPrice = 900,
                                    Payers = new List<User>()
                                    {
                                        user2                                       
                                    },
                                }
                            }

                        },
                        new TripDay()
                        {
                            Number = 2,
                            Activities = new List<Activiti>()
                            {
                                new Activiti()
                                {
                                    Title = "Прогулка по парку Зарядье",
                                    FromSearch = false,
                                    Category = category3, 
                                    Participants = new List<User>()
                                    {
                                        user1,
                                        user3
                                    },
                                    PricePerOne = 0,
                                    TotalPrice = 0,
                                    Payers = new List<User>()
                                    {
                                        user2
                                    },
                                },
                                new Activiti()
                                {
                                    Title = "Большой театр",
                                    FromSearch = true,
                                    Category = category4,
                                    Participants = new List<User>()
                                    {
                                        user1,
                                        user2
                                    },
                                    PricePerOne = 5000,
                                    TotalPrice = 10000,
                                    Payers = new List<User>()
                                    {
                                        user2
                                    },
                                }
                            }

                        },
                    }
                   
                   

                }
            };
        }
    }
}
