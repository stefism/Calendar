using AutoMapper;
using Calendar.App.Data;
using Calendar.App.Services;
using Funeral.App.Repositories;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Calendar.Tests
{
    public class DataServiceTests
    {
        private Mock<IMapper> mapper;

        List<Date> dates = new List<Date>
        {
            new Date
            {
                Id = "id1",
                ReservedDate = DateTime.Now,
                Price = 10.99m,
                UserId = "userId1"
            },
            new Date
            {
                Id = "id2",
                ReservedDate = DateTime.Now.AddMinutes(5),
                Price = 11.99m,
                UserId = "userId2"
            },
            new Date
            {
                Id = "id3",
                ReservedDate = DateTime.Now.AddMinutes(7),
                Price = 11.99m,
                UserId = "userId3"
            },
        };

        private Mock<IEFRepository<Date>> datesRepository;

        public DataServiceTests()
        {
            
        }

        [Fact]
        public async Task TestReleaseReservationMethod()
        {  
            
        }
    }
}
