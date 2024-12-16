using AutoMapper;
using BusinessObjects;
using BusinessObjects.Entities;
using BusinessObjects.EnumClass;
using BusinessObjects.Utils.Request;
using DataAccess;
using Quartz;
using Repositories.Cron;
using Repositories.Interface;

namespace Repositories
{
    public class TourRepository : ITourRepository
    {
        private readonly TourDAO _tourDAO;
        private readonly IMapper _mapper;
        private readonly IScheduler _scheduler;

        public TourRepository(TourDAO tourDAO, IMapper mapper, IScheduler scheduler)
        {
            _tourDAO = tourDAO;
            _mapper = mapper;
            _scheduler = scheduler;
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
        public async Task<bool> DoesParticipantExist(string tourId, int userId)
        {
            return await _tourDAO.DoesParticipantExist(tourId, userId);
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
            existingTour.ApprovalStatus = updatedTour.ApprovalStatus ?? existingTour.ApprovalStatus;
            existingTour.TourImage = updatedTour.TourImage ?? existingTour.TourImage;
            existingTour.Creator = updatedTour.Creator ?? existingTour.Creator;
            existingTour.Itinerary = updatedTour.Itinerary ?? existingTour.Itinerary;
            existingTour.CostDetails = updatedTour.CostDetails ?? existingTour.CostDetails;
            existingTour.AdditionalInfo = updatedTour.AdditionalInfo ?? existingTour.AdditionalInfo;
            existingTour.TourDescription = updatedTour.TourDescription ?? existingTour.TourDescription;
            existingTour.Participants = updatedTour.Participants ?? existingTour.Participants;
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

            var getParticipant = await _tourDAO.GetTourById(tourId);

            foreach (var item in getParticipant.Participants)
            {
                if (item.ParticipantId == travelerId)
                {
                    var user = await _tourDAO.GetUserInfor(item.ParticipantId);
                    item.FullName = user.FullName;
                    item.Gender = user.Profiles.Gender;
                    item.Address = user.Profiles.City;
                    item.Phone = user.Profiles.Phone;
                    item.PaymentStatus = false;
                    await _tourDAO.UpdateTour(tourId, getParticipant);
                    break;
                }
            }

            IJobDetail job = JobBuilder.Create<Cronjob>()
               .WithIdentity("job1", "group1")
               .UsingJobData("tourId", tourId)
               .UsingJobData("participantId", travelerId)
               .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartAt(DateBuilder.FutureDate(1, IntervalUnit.Minute))
                .Build();


            await _scheduler.ScheduleJob(job, trigger);
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

        public async Task<IEnumerable<Participants>> GetListParticipantsAsync(string tourId)
        {
            //get list participant in tour
            var getListParticipants = await _tourDAO.GetTourById(tourId);

            return getListParticipants.Participants;
        }

        public async Task UpdatePaymentStatus(long orderCode, int totalAmount)
        {
            var getParticipant = await _tourDAO.GetParticipantWithOrderCode(orderCode);

            foreach (var item in getParticipant.Participants)
            {
                if (item.OrderCode == orderCode)
                {
                    item.PaymentStatus = true;
                    item.TotalAmount = totalAmount;
                    break;
                }
            }

            await _tourDAO.UpdateTour(getParticipant.TourId, getParticipant);
        }

        public async Task UpdateOrderCode(string tourId, int travelerId, long orderCode)
        {
            var getParticipant = await _tourDAO.GetParticipant(tourId, travelerId);

            foreach (var item in getParticipant.Participants)
            {
                if (item.ParticipantId == travelerId)
                {
                    item.OrderCode = orderCode;
                    break;
                }
            }

            await _tourDAO.UpdateTour(tourId, getParticipant);
        }

        public async Task<bool> DidParticipantPay(long orderCode)
        {
            return await _tourDAO.DidParticipantPay(orderCode);
        }

        public async Task<Tour> GetParticipantWithOrderCode(long orderCode)
        {
            return await _tourDAO.GetParticipantWithOrderCode(orderCode);
        }

        public async Task RemoveUnpaidParticipantsAsync(string tourId, int travelerId)
        {
            var getTour = await _tourDAO.GetTourById(tourId);

            foreach (var item in getTour.Participants)
            {
                if (item.ParticipantId == travelerId && item.PaymentStatus == false)
                {
                    getTour.Participants.Remove(item);
                    break;
                }
            }

            await _tourDAO.UpdateTour(tourId, getTour);
        }

        public async Task<DateTime> GetParticipantJoinTimeAsync(string tourId, int travelerId)
        {
            return await _tourDAO.GetParticipantJoinTimeAsync(tourId, travelerId);
        }
    }
}
