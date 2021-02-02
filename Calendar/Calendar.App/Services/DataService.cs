﻿using Calendar.App.Data;
using Calendar.App.ViewModels;
using Funeral.App.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calendar.App.Services
{    
    public class DataService : IDataService
    {
        private ApplicationDbContext db;
        private readonly IPriceService priceService;
        private readonly IEFRepository<Date> dateRepository;
        private readonly IEFRepository<Price> pricePepository;

        public DataService(
            ApplicationDbContext db,
            IPriceService priceService,
            IEFRepository<Date> dateRepository,
            IEFRepository<Price> pricePepository)
        {
            this.db = db;
            this.priceService = priceService;
            this.dateRepository = dateRepository;
            this.pricePepository = pricePepository;
        }

        public async Task ReleaseReservation(string reservationId)
        {
            var reservation = await dateRepository.All()
                .Where(r => r.Id == reservationId).FirstOrDefaultAsync();

            dateRepository.Delete(reservation);          

            await dateRepository.SaveChangesAsync();            
        }

        public async Task<ICollection<ReservationViewModel>> ShowAllReservations()
        { 
            var reservations = await dateRepository.All().Select(p => new ReservationViewModel
            {
                ReservationDateId = p.Id,
                UserId = p.UserId,
                Username = db.Users.Where(u => u.Id == p.UserId).Select(u => u.UserName).FirstOrDefault(),
                ReservedDate = p.ReservedDate,
                Price = p.Price.ToString(),
            }).OrderBy(p => p.ReservedDate)
                .ToListAsync();

            return reservations;
        }

        public async Task AddAvailableDate(DateTime date, bool isNonWorkDay, string userId)
        {
            var actualPrice = await priceService.ReturnActualPrice(isNonWorkDay);

            var availableDate = new Date
            {
                ReservedDate = date,
                Price = actualPrice,
                UserId = userId,
                IsNonWorkDay = isNonWorkDay,
            };

            await dateRepository.AddAsync(availableDate);
            await dateRepository.SaveChangesAsync();
        }

        public bool IsNonWorkDay(DateTime date)
        {
            var day = date.DayOfWeek;
            bool IsNonWorkDay = false;

            if (day == DayOfWeek.Sunday || day == DayOfWeek.Saturday)
            {
                IsNonWorkDay = true;
            }

            return IsNonWorkDay;
        }

        public async Task<ICollection<ReservationViewModel>> GetDates(int year, int month)
        {
            return await dateRepository.All()
                .Select(p => new ReservationViewModel
                {
                    ReservationDateId = p.Id,
                    UserId = p.UserId,
                    Username = db.Users.Where(u => u.Id == p.UserId)
                    .Select(u => u.UserName).FirstOrDefault(),
                    ReservedDate = p.ReservedDate,
                    Price = p.Price.ToString(),
                })
                .Where(x => x.ReservedDate.HasValue &&
                            x.ReservedDate.Value.Year == year &&
                            x.ReservedDate.Value.Month == month)
                .ToListAsync();
        }

        public async Task ChangePrices(decimal workday, decimal weekends)
        {
            var prices = await pricePepository.All().FirstOrDefaultAsync();

            if (prices == null)
            {
                prices = new Price
                {
                    WorkDay = workday,
                    NonWorkDay = weekends,
                };

                await pricePepository.AddAsync(prices);
            }
            else
            {
                prices.WorkDay = workday;
                prices.NonWorkDay = weekends;
            }

            await pricePepository.SaveChangesAsync();
        }
    }
}
