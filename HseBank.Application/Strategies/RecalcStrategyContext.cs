using System;
using HseBank.Domain.Enums;

namespace HseBank.Application.Strategies
{
    /// <summary>
    ///Хранит текущую стратегию и позволяет переключать режим
    /// </summary>
    public class RecalcStrategyContext
    {
        /// <summary>
        /// Автоматическая стратегия
        /// </summary>
        private readonly IRecalcStrategy _auto;
        
        /// <summary>
        /// Ручная стратегия
        /// </summary>
        private readonly IRecalcStrategy _manual;

        /// <summary>
        /// Текущая стратегия
        /// </summary>
        public IRecalcStrategy Current { get; private set; }

        /// <summary>
        /// Конструктор для создагия стратегии
        /// </summary>
        /// <param name="auto"></param>
        /// <param name="manual"></param>
        public RecalcStrategyContext(AutomaticRecalcStrategy auto, ManualRecalcStrategy manual)
        {
            _auto = auto; _manual = manual;
            Current = _auto;
        }

        /// <summary>
        /// Установить новую стратегию
        /// </summary>
        /// <param name="mode">Новая стратегия</param>
        public void SetMode(RecalcMode mode)
        {
            Current = mode == RecalcMode.Automatic ? _auto : _manual;
        }

        /// <summary>
        /// Метод для получения текущей стратегии
        /// </summary>
        /// <returns>Текущая стратегия</returns>
        public RecalcMode GetMode()
        {
            return Current.Mode;
        }
    }
}