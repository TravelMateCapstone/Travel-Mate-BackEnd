using Quartz;
using Repositories.Interface;

namespace Repositories.Cron
{
    public class Cronjob : IJob
    {
        private readonly ITourRepository _tourRepository;

        public Cronjob(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var tourId = context.MergedJobDataMap.GetString("tourId");
            var participantId = context.MergedJobDataMap.GetInt("participantId");

            await _tourRepository.RemoveUnpaidParticipantsAsync(tourId, participantId);
        }
    }
}
