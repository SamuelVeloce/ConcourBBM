

namespace CompetitionV2
{
    public class CaseEventArgs : CancellableEventArgs
    {
        public AbsCase Case;

        public CaseEventArgs(AbsCase Case, bool cancelled) : base(cancelled)
        {
            this.Case = Case;
        }
    }
}
