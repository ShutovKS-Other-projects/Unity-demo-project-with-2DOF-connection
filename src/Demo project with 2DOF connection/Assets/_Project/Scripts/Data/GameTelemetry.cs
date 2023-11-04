namespace Data
{
    /// <summary>
    /// Класс для хранения телеметрических данных игры.
    /// </summary>
    public class GameTelemetry
    {
        /// <summary>
        /// Угол тангажа (Pitch) в градусах.
        /// </summary>
        public double Pitch { get; set; }

        /// <summary>
        /// Угол крена (Roll) в градусах.
        /// </summary>
        public double Roll { get; set; }

        /// <summary>
        /// Угол рыскания (Yaw) в градусах.
        /// </summary>
        public double Yaw { get; set; }

        /// <summary>
        /// Передний/задний скользящий крен (Surge) в метрах.
        /// </summary>
        public double Surge { get; set; }

        /// <summary>
        /// Левосторонний/правосторонний скользящий крен (Sway) в метрах.
        /// </summary>
        public double Sway { get; set; }

        /// <summary>
        /// Вертикальное движение (Heave) в метрах.
        /// </summary>
        public double Heave { get; set; }

        /// <summary>
        /// Дополнительный параметр 1 (Extra1).
        /// </summary>
        public double Extra1 { get; set; }

        /// <summary>
        /// Дополнительный параметр 2 (Extra2).
        /// </summary>
        public double Extra2 { get; set; }

        /// <summary>
        /// Дополнительный параметр 3 (Extra3).
        /// </summary>
        public double Extra3 { get; set; }

        /// <summary>
        /// Скорость ветра (Wind) в метрах в секунду.
        /// </summary>
        public double Wind { get; set; }

        /// <summary>
        /// Сброс всех параметров телеметрии игры до значений по умолчанию.
        /// </summary>
        public void Reset()
        {
            Pitch = 0.0;
            Roll = 0.0;
            Yaw = 0.0;
            Surge = 0.0;
            Sway = 0.0;
            Heave = 0.0;
            Wind = 0.0;
            Extra1 = 0.0;
            Extra2 = 0.0;
            Extra3 = 0.0;
        }
    }
}