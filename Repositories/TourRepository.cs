using AutoMapper;
using BusinessObjects;
using BusinessObjects.Entities;
using BusinessObjects.EnumClass;
using BusinessObjects.Utils.Request;
using DataAccess;
using MongoDB.Bson;
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
            tour.Creator = new CreatorInfo();
            tour.Creator.Id = userId;
            tour.CreatedAt = GetTimeZone.GetVNTimeZoneNow();
            tour.ApprovalStatus = ApprovalStatus.Pending;

            foreach (var item in tour.Schedules)
            {
                item.ScheduleId = ObjectId.GenerateNewId().ToString();
                item.Participants = new List<Participants>();
                item.ActiveStatus = true;
            };

            await _tourDAO.AddTour(tour);
        }

        public async Task UpdateTour(string id, Tour updatedTour)
        {
            var existingTour = await _tourDAO.GetTourById(id);

            existingTour.TourName = updatedTour.TourName ?? existingTour.TourName;
            existingTour.Price = updatedTour.Price ?? existingTour.Price;
            existingTour.Location = updatedTour.Location ?? existingTour.Location;
            existingTour.MaxGuests = updatedTour.MaxGuests > 0 ? updatedTour.MaxGuests : existingTour.MaxGuests;
            existingTour.TourStatus = updatedTour.TourStatus ?? existingTour.TourStatus;
            existingTour.ApprovalStatus = updatedTour.ApprovalStatus ?? existingTour.ApprovalStatus;
            existingTour.TourImage = updatedTour.TourImage ?? existingTour.TourImage;
            existingTour.Creator = updatedTour.Creator ?? existingTour.Creator;
            existingTour.Itinerary = updatedTour.Itinerary ?? existingTour.Itinerary;
            existingTour.CostDetails = updatedTour.CostDetails ?? existingTour.CostDetails;
            existingTour.AdditionalInfo = updatedTour.AdditionalInfo ?? existingTour.AdditionalInfo;
            existingTour.TourDescription = updatedTour.TourDescription ?? existingTour.TourDescription;
            existingTour.Schedules = updatedTour.Schedules ?? existingTour.Schedules;
            if (updatedTour.Schedules != null)
            {
                foreach (var item in updatedTour.Schedules)
                {
                    item.ScheduleId = ObjectId.GenerateNewId().ToString();
                    item.Participants = new List<Participants>();
                    item.ActiveStatus = true;
                }
                existingTour.Schedules = updatedTour.Schedules;
            }
            existingTour.UpdatedAt = GetTimeZone.GetVNTimeZoneNow();

            await _tourDAO.UpdateTour(id, existingTour);
        }

        public async Task DeleteTour(string id)
        {
            await _tourDAO.DeleteTour(id);
        }

        public async Task AcceptTour(string tourId)
        {
            await _tourDAO.ProcessTourAdmin(tourId, ApprovalStatus.Accepted);
        }

        public async Task RejectTour(string tourId)
        {
            await _tourDAO.ProcessTourAdmin(tourId, ApprovalStatus.Rejected);
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

        public async Task<IEnumerable<ApplicationUser>> GetUsersInfoAsync(IEnumerable<int> userIds)
        {
            return await _tourDAO.GetUsersInfoAsync(userIds);
        }
    }
}
