using Quartz;
using Repositories.Interface;

namespace Repositories.Cron
{
    public class Cronjob : IJob
    {
        private readonly ITourParticipantRepository _tourRepository;

        public Cronjob(ITourParticipantRepository tourRepository)
        {
            _tourRepository = tourRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var scheduleId = context.MergedJobDataMap.GetString("scheduleId");
            var tourId = context.MergedJobDataMap.GetString("tourId");
            var participantId = context.MergedJobDataMap.GetInt("participantId");

            await _tourRepository.RemoveUnpaidParticipantsAsync(scheduleId, tourId, participantId);
        }
    }
}
