using DarkDiscipline.Domain.Fitness;

namespace DarkDiscipline.Application.Fitness
{
    public readonly struct WorkoutSessionLoggedEvent
    {
        public WorkoutSessionLoggedEvent(WorkoutSession session)
        {
            Session = session;
        }

        public WorkoutSession Session { get; }
    }
}
