using AutoMapper;
using BusinessObjects;
using BusinessObjects.Entities;
using DataAccess;
using Quartz;
using Repositories.Cron;
using Repositories.Interface;

namespace Repositories
{
    public class TourParticipantRepository : ITourParticipantRepository
    {
        private readonly TourParticipantDAO _tourParticipantDAO;
        private readonly IMapper _mapper;
        private readonly IScheduler _scheduler;

        public TourParticipantRepository(TourParticipantDAO tourParticipantDAO, IMapper mapper, IScheduler scheduler)
        {
            _tourParticipantDAO = tourParticipantDAO;
            _mapper = mapper;
            _scheduler = scheduler;
        }

        public async Task<Tour> GetParticipantWithOrderCode(long orderCode)
        {
            return await _tourParticipantDAO.GetTourByParticipantOrderCodeAsync(orderCode);
        }

        public async Task<Tour> GetTourScheduleById(string scheduleId, string tourId)
        {
            return await _tourParticipantDAO.GetTourScheduleById(scheduleId, tourId);
        }

        public async Task<ApplicationUser> GetUserInfo(int userId)
        {
            return await _tourParticipantDAO.GetUserInfor(userId);
        }

        public async Task JoinTour(string scheduleId, string tourId, int travelerId)
        {
            var user = await _tourParticipantDAO.GetUserInfor(travelerId);

            var newParticipant = new Participants
            {
                ParticipantId = travelerId,
                RegisteredAt = GetTimeZone.GetVNTimeZoneNow(),
                PaymentStatus = false,
                FullName = user.FullName,
                Gender = user.Profiles.Gender,
                Address = user.Profiles.City,
                Phone = user.Profiles.Phone,
                PostId = string.Empty
            };

            await _tourParticipantDAO.JoinTour(scheduleId, tourId, newParticipant);

            IJobDetail job = JobBuilder.Create<Cronjob>()
               .WithIdentity($"{travelerId}", "group1")
               .UsingJobData("scheduleId", scheduleId)
               .UsingJobData("tourId", tourId)
               .UsingJobData("participantId", travelerId)
               .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity($"{scheduleId}_{tourId}_{travelerId}", "group1")
                .StartAt(DateBuilder.FutureDate(3, IntervalUnit.Minute))
                .Build();

            await _scheduler.ScheduleJob(job, trigger);
        }

        public async Task RemoveUnpaidParticipantsAsync(string scheduleId, string tourId, int travelerId)
        {
            var getTour = await _tourParticipantDAO.GetTourScheduleById(scheduleId, tourId);
            if (getTour == null) return;

            var tourSchedule = getTour.Schedules.FirstOrDefault(t => t.ScheduleId == scheduleId);
            if (tourSchedule == null) return;

            var participant = tourSchedule.Participants.FirstOrDefault(p => p.ParticipantId == travelerId && p.PaymentStatus == false);
            if (participant == null) return;

            tourSchedule.Participants.Remove(participant);

            await _tourParticipantDAO.UpdateTour(getTour.TourId, getTour);
        }

        public async Task UpdatePaymentStatus(Tour tour, int travelerId)
        {
            await _tourParticipantDAO.UpdateTour(tour.TourId, tour);

            await _scheduler.DeleteJob(new JobKey($"{travelerId}", "group1"));
        }
    }
}
