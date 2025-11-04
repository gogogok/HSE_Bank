using System;
using HseBank.Domain.Enums;

namespace HseBank.Application.Strategies
{
    // Хранит текущую стратегию и позволяет переключать режим
    public sealed class RecalcStrategyContext
    {
        private readonly IRecalcStrategy _auto;
        private readonly IRecalcStrategy _manual;

        public IRecalcStrategy Current { get; private set; }

        public RecalcStrategyContext(AutomaticRecalcStrategy auto, ManualRecalcStrategy manual)
        {
            _auto = auto; _manual = manual;
            Current = _auto; // дефолт — Автоматический
        }

        public void SetMode(RecalcMode mode)
        {
            Current = mode == RecalcMode.Automatic ? _auto : _manual;
        }

        public RecalcMode GetMode()
        {
            return Current.Mode;
        }
    }
}