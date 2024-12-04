﻿using AutoMapper;
using BusinessObjects;
using BusinessObjects.Entities;
using BusinessObjects.EnumClass;
using BusinessObjects.Utils.Request;
using DataAccess;
using MongoDB.Driver;
using Repositories.Interface;

namespace Repositories
{
    public class TourRepository : ITourRepository
    {
        private readonly TourDAO _tourDAO;
        private readonly IMapper _mapper;

        public TourRepository(TourDAO tourDAO, IMapper mapper)
        {
            _tourDAO = tourDAO;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Tour>> GetAllTours()
        {
            return _tourDAO.GetAllTours();
        }
        public async Task<IEnumerable<Tour>> GetAllToursOfLocal(int userId)
        {
            return _tourDAO.GetAllToursOfLocal(userId);
        }

        public async Task<IEnumerable<Tour>> GetToursByStatus(int userId, ApprovalStatus? approvalStatus)
        {
            return await _tourDAO.GetToursByStatus(userId, approvalStatus);
        }

        public async Task<Tour> GetTourById(string id)
        {
            return await _tourDAO.GetTourById(id);
        }

        public async Task AddTour(int userId, Tour tour)
        {
            //xu ly logic o day
            tour.Creator = new CreatorInfo();
            tour.Creator.Id = userId;
            tour.CreatedAt = GetTimeZone.GetVNTimeZoneNow();
            tour.ApprovalStatus = ApprovalStatus.Pending;
            await _tourDAO.AddTour(tour);
        }
        public async Task<bool> DoesParticipantExist(int userId)
        {
            return await _tourDAO.DoesParticipantExist(userId);
        }

        public async Task UpdateTour(string id, Tour updatedTour)
        {
            var existingTour = await _tourDAO.GetTourById(id);

            existingTour.TourName = updatedTour.TourName ?? existingTour.TourName;
            existingTour.Price = updatedTour.Price ?? existingTour.Price;
            existingTour.StartDate = updatedTour.StartDate != default ? updatedTour.StartDate : existingTour.StartDate;
            existingTour.EndDate = updatedTour.EndDate != default ? updatedTour.EndDate : existingTour.EndDate;
            existingTour.NumberOfDays = updatedTour.NumberOfDays > 0 ? updatedTour.NumberOfDays : existingTour.NumberOfDays;
            existingTour.NumberOfNights = updatedTour.NumberOfNights > 0 ? updatedTour.NumberOfNights : existingTour.NumberOfNights;
            existingTour.Location = updatedTour.Location ?? existingTour.Location;
            existingTour.MaxGuests = updatedTour.MaxGuests > 0 ? updatedTour.MaxGuests : existingTour.MaxGuests;
            existingTour.TourStatus = updatedTour.TourStatus ?? existingTour.TourStatus;
            existingTour.RegisteredGuests = updatedTour.RegisteredGuests > 0 ? updatedTour.RegisteredGuests : existingTour.RegisteredGuests;
            existingTour.ApprovalStatus = updatedTour.ApprovalStatus ?? existingTour.ApprovalStatus;
            existingTour.TourImage = updatedTour.TourImage ?? existingTour.TourImage;
            existingTour.Creator = updatedTour.Creator ?? existingTour.Creator;
            existingTour.Participants = updatedTour.Participants ?? existingTour.Participants;
            existingTour.Itinerary = updatedTour.Itinerary ?? existingTour.Itinerary;
            existingTour.CostDetails = updatedTour.CostDetails ?? existingTour.CostDetails;
            existingTour.AdditionalInfo = updatedTour.AdditionalInfo ?? existingTour.AdditionalInfo;
            existingTour.Reviews = updatedTour.Reviews ?? existingTour.Reviews;
            existingTour.UpdatedAt = GetTimeZone.GetVNTimeZoneNow();

            await _tourDAO.UpdateTour(id, existingTour);
        }

        public async Task DeleteTour(string id)
        {
            await _tourDAO.DeleteTour(id);
        }

        public async Task JoinTour(string tourId, int travelerId)
        {
            var newParticipant = new Participants()
            {
                ParticipantId = travelerId,
                RegisteredAt = GetTimeZone.GetVNTimeZoneNow(),
            };
            var existingTour = await _tourDAO.GetTourById(tourId);
            if (existingTour.Participants == null)
                existingTour.Participants = new List<Participants>();

            await _tourDAO.JoinTour(tourId, newParticipant);

            existingTour.RegisteredGuests = existingTour.Participants.Count();
            await _tourDAO.UpdateTour(tourId, existingTour);
        }

        public async Task AcceptTour(string tourId)
        {
            await _tourDAO.ProcessTourAdmin(tourId, ApprovalStatus.Accepted);
        }

        public async Task BanTour(string tourId)
        {
            await _tourDAO.ProcessTourAdmin(tourId, ApprovalStatus.Rejected);
        }

        public async Task AddReview(string tourId, TourReview tourReview)
        {
            await _tourDAO.AddReview(tourId, tourReview);
        }

        public async Task UpdateAvailability(string tourId, int slots)
        {
            await _tourDAO.UpdateAvailability(tourId, slots);
        }

        public async Task CancelTour(string tourId)
        {
            await _tourDAO.CancelTour(tourId);
        }

        public async Task<ApplicationUser> GetUserInfo(int userId)
        {
            return await _tourDAO.GetLocalInfor(userId);
        }

        public async Task<IEnumerable<TourBriefDto>> GetTourBriefByUserId(int userId)
        {
            var listTour = await _tourDAO.GetTourBriefByUserId(userId);
            return _mapper.Map<IEnumerable<TourBriefDto>>(listTour);
        }

        public async Task<double> GetUserAverageStar(int userId)
        {
            var listPost = await _tourDAO.GetUserAverageStar(userId);
            if (!listPost.Any())
                return 0;

            return listPost.Average(item => item.Star);
        }

        public async Task<IEnumerable<Participants>> GetListParticipantsAsync(string tourId)
        {
            var getListParticipants = await _tourDAO.GetTourById(tourId);
            var listUser = new List<ApplicationUser>();
            foreach (var item in getListParticipants.Participants)
            {
                var user = await _tourDAO.GetUserInfor(item.ParticipantId);
                listUser.Add(user);
            }

            return _mapper.Map<IEnumerable<Participants>>(listUser);
        }
    }
}
