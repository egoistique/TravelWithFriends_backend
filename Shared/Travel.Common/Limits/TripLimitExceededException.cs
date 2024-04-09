using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel.Common.Limits;

public class TripLimitExceededException : Exception
{
    public TripLimitExceededException() : base("User has reached the maximum trips limit.")
    {
    }

    public TripLimitExceededException(string message) : base(message)
    {
    }

    public TripLimitExceededException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public override string ToString()
    {
        return Message;
    }
}



//"creatorId": "30f74868-c4b2-44db-86e7-408caac47f93",
//"title": "t6",
//"numOfParticipants": 0,
//"dateStart": "12.02.2024",
//"dateEnd": "18.02.2024",
//"city": "city",
//"hotelTitle": "string",
//"isPublicated": true