using Game.Code.Infrastructure.SO.Rounds;

namespace Game.Code.Infrastructure
{
    public class Gameplay
    {
        private readonly RoundData _round1;
        private readonly RoundData _round2;
        private readonly RoundData _round3;

        public Gameplay(RoundData round1, RoundData round2, RoundData round3)
        {
            _round1 = round1;
            _round2 = round2;
            _round3 = round3;
        }

        public void RunRound(Round round)
        {
            var targetRound = round switch
            {
                Round.Round1 => _round1,
                Round.Round2 => _round2,
                _ => _round3
            };
        }
    }
}
