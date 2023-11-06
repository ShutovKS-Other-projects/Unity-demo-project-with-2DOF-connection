#region

using System;
using System.Threading;

#endregion

namespace DOF.Data
{
    /// <summary>
    ///     Класс ClsFilters предоставляет методы для фильтрации входных данных и обработки значений с использованием различных
    ///     фильтров и эффектов.
    /// </summary>
    public class ClsFilters
    {
        /// <summary>
        ///     Константа, представляющая входное значение для симуляции фильтра "Зона затухания до нуля" (Dead Zone To Zero).
        /// </summary>
        private const double INPUT_VALUE_DEAD_ZONE_TO_ZERO_SIM = 0.0;

        /// <summary>
        ///     Максимальное значение антиролла для фильтра "Антиролл" (AntiRoll).
        /// </summary>
        private double _antiRollMaxValue;

        /// <summary>
        ///     Минимальное значение антиролла для фильтра "Антиролл" (AntiRoll).
        /// </summary>
        private double _antiRollMinValue;

        /// <summary>
        ///     Значение антиролла для фильтра "Антиролл" (AntiRoll).
        /// </summary>
        private double _antiRollValue;

        /// <summary>
        ///     Интервал затухания зоны до нуля для фильтра "Зона затухания" (Dead Zone).
        /// </summary>
        private double _deadToZeroInterval;

        /// <summary>
        ///     Время затухания зоны до нуля для фильтра "Зона затухания" (Dead Zone).
        /// </summary>
        private double _deadToZeroTime;

        /// <summary>
        ///     Значение зоны затухания до нуля для фильтра "Зона затухания до нуля" (Dead Zone To Zero).
        /// </summary>
        private double _deadZoneToZeroValue;

        /// <summary>
        ///     Значение зоны затухания для фильтра "Зона затухания" (Dead Zone).
        /// </summary>
        private double _deadZoneValue;

        /// <summary>
        ///     Флаг, указывающий на первую итерацию фильтрации.
        /// </summary>
        private bool _first = true;

        /// <summary>
        ///     Входное значение для симуляции фильтра "Антиролл" (AntiRollSim).
        /// </summary>
        private double _inputValueAntiRollSim;

        /// <summary>
        ///     Входное значение для симуляции фильтра "Сглаживание" (Smoothing).
        /// </summary>
        private double _inputValueSim;

        /// <summary>
        ///     Последнее значение зоны затухания (Dead Zone).
        /// </summary>
        private double _lastValueDeadZone;

        /// <summary>
        ///     Последнее значение с учетом знака (положительное или отрицательное).
        /// </summary>
        private double _lastValuePlusMinus;

        /// <summary>
        ///     Значение нелинейности для фильтрации.
        /// </summary>
        private double _nonlinearValue;

        /// <summary>
        ///     Выходное значение симуляции фильтра "Антиролл" (AntiRollSim).
        /// </summary>
        private double _outputValueAntiRollSim;

        /// <summary>
        ///     Выходное значение симуляции фильтра "Зона затухания до нуля" (Dead Zone To Zero).
        /// </summary>
        private double _outputValueDeadZoneToZeroSim;

        /// <summary>
        ///     Выходное значение симуляции фильтра "Сглаживание" (Smoothing).
        /// </summary>
        private double _outputValueSim;

        /// <summary>
        ///     Временное значение для фильтра "Сглаживание" (Smoothing).
        /// </summary>
        private double _smoothingValue;

        /// <summary>
        ///     Временное значение для фильтра "Сглаживание" (Smoothing).
        /// </summary>
        private double _smoothingValueSim;

        /// <summary>
        ///     Начальное значение метки времени для обнаружения перехода через ноль.
        /// </summary>
        private int _startPlusMinusTick;

        /// <summary>
        ///     Определяет, следует ли остановить симуляцию фильтра "Сглаживание" (Smoothing).
        /// </summary>
        private bool _stopSmoothingSim;

        /// <summary>
        ///     Хранилище для первого значения фильтра "Сглаживание" (Smoothing).
        /// </summary>
        private double _stored1;

        /// <summary>
        ///     Хранилище для первого значения симуляции фильтра "Сглаживание" (SmoothingSim).
        /// </summary>
        private double _stored1Sim;

        /// <summary>
        ///     Хранилище для второго значения фильтра "Сглаживание" (Smoothing).
        /// </summary>
        private double _stored2;

        /// <summary>
        ///     Хранилище для второго значения симуляции фильтра "Сглаживание" (SmoothingSim).
        /// </summary>
        private double _stored2Sim;

        /// <summary>
        ///     Хранилище для третьего значения фильтра "Сглаживание" (Smoothing).
        /// </summary>
        private double _stored3;

        /// <summary>
        ///     Хранилище для третьего значения симуляции фильтра "Сглаживание" (SmoothingSim).
        /// </summary>
        private double _stored3Sim;

        /// <summary>
        ///     Хранилище для первого значения симуляции фильтра "Зона затухания до нуля" (DeadZoneToZeroSim).
        /// </summary>
        private double _storedToZero1;

        /// <summary>
        ///     Хранилище для второго значения симуляции фильтра "Зона затухания до нуля" (DeadZoneToZeroSim).
        /// </summary>
        private double _storedToZero2;

        /// <summary>
        ///     Хранилище для третьего значения симуляции фильтра "Зона затухания до нуля" (DeadZoneToZeroSim).
        /// </summary>
        private double _storedToZero3;

        /// <summary>
        ///     Поток для выполнения фильтра "Сглаживание" (Smoothing).
        /// </summary>
        private Thread _th;

        /// <summary>
        ///     Поток для выполнения фильтра "Антиролл" (AntiRoll).
        /// </summary>
        private Thread _thAntiRoll;

        /// <summary>
        ///     Поток для выполнения фильтра "Зона затухания до нуля" (Dead Zone To Zero).
        /// </summary>
        private Thread _thDeadZoneToZero;

        /// <summary>
        ///     Устанавливает значение для параметра "Время затухания зоны до нуля" (DeadToZeroTime).
        /// </summary>
        /// <param name="deadToZeroTime">Значение времени затухания зоны до нуля.</param>
        public void SetDeadToZeroTimeValue(int deadToZeroTime)
        {
            _deadToZeroTime = deadToZeroTime;
        }

        /// <summary>
        ///     Устанавливает значение для параметра "Интервал затухания зоны до нуля" (DeadToZeroInterval).
        /// </summary>
        /// <param name="deadToZeroInterval">Значение интервала затухания зоны до нуля.</param>
        public void SetDeadToZeroIntervalValue(int deadToZeroInterval)
        {
            _deadToZeroInterval = deadToZeroInterval;
        }

        /// <summary>
        ///     Устанавливает значение для параметра "Зона затухания до нуля" (DeadZoneToZeroValue).
        /// </summary>
        /// <param name="deadZoneToZeroValue">Значение зоны затухания до нуля.</param>
        public void SetDeadZoneToZeroValue(int deadZoneToZeroValue)
        {
            _lastValuePlusMinus = 0.0;
            _startPlusMinusTick = 0;
            _deadZoneToZeroValue = 30.0 / (30 + deadZoneToZeroValue);
            _storedToZero1 = _storedToZero2 = _storedToZero3 = 0.0;
        }

        /// <summary>
        ///     Устанавливает значение для параметра "Зона затухания" (DeadZoneValue).
        /// </summary>
        /// <param name="deadZoneValue">Значение зоны затухания.</param>
        public void SetDeadZoneValue(int deadZoneValue)
        {
            _deadZoneValue = deadZoneValue;
            _lastValueDeadZone = 0.0;
            _first = true;
        }

        /// <summary>
        ///     Устанавливает значение для параметра "Антиролл" (AntiRollValue).
        /// </summary>
        /// <param name="antiRollValue">Значение антиролла.</param>
        /// <param name="minValue">Минимальное значение антиролла.</param>
        /// <param name="maxValue">Максимальное значение антиролла.</param>
        public void SetAntiRollValue(int antiRollValue, double minValue, double maxValue)
        {
            _antiRollValue = antiRollValue == 0 ? 0.0 : 2.0 / (2 + antiRollValue);
            _antiRollMinValue = minValue;
            _antiRollMaxValue = maxValue;

            _stored1 = 0.0;
            _stored2 = 0.0;
            _stored3 = 0.0;

            if (antiRollValue == 0)
            {
                return;
            }

            AntiRollSimLoop();
        }

        /// <summary>
        ///     Устанавливает значение для параметра "Нелинейность" (NonlinearValue).
        /// </summary>
        /// <param name="nonlinearValue">Значение нелинейности.</param>
        public void SetNonlinearValue(int nonlinearValue)
        {
            _nonlinearValue = nonlinearValue / 4.0;
        }

        /// <summary>
        ///     Устанавливает временное значение для фильтра "Сглаживание" (Smoothing).
        /// </summary>
        /// <param name="smoothingValue">Значение времени сглаживания.</param>
        public void SetSmoothingValue(int smoothingValue)
        {
            _smoothingValue = 30.0 / (30 + smoothingValue);

            _stored1 = 0.0;
            _stored2 = 0.0;
            _stored3 = 0.0;
        }

        /// <summary>
        ///     Устанавливает временное значение для симуляции фильтра "Сглаживание" (Smoothing).
        /// </summary>
        /// <param name="smoothingValue">Значение времени сглаживания для симуляции.</param>
        public void SetSmoothingValueSim(int smoothingValue)
        {
            _inputValueSim = 0.0;
            _outputValueSim = 0.0;
            _smoothingValueSim = 2.0 / (2 + smoothingValue);
            _stored1Sim = 0.0;
            _stored2Sim = 0.0;
            _stored3Sim = 0.0;

            if (smoothingValue == 0)
            {
                return;
            }

            SmoothingSimLoop();
        }

        /// <summary>
        ///     Применяет фильтр экспоненциального скользящего среднего (EMA) к входному значению для симуляции сглаживания.
        /// </summary>
        /// <param name="value">Входное значение для обработки.</param>
        /// <returns>Сглаженное значение с учетом фильтра EMA.</returns>
        private double emaLP_of_emaLP_of_emaLP(double value)
        {
            _stored1Sim += _smoothingValueSim * (value - _stored1Sim);
            _stored2Sim += _smoothingValueSim * (_stored1Sim - _stored2Sim);
            _stored3Sim += _smoothingValueSim * (_stored2Sim - _stored3Sim);

            return _stored3Sim;
        }

        /// <summary>
        ///     Применяет фильтр экспоненциального скользящего среднего (EMA) к входному значению для симуляции зоны затухания до
        ///     нуля.
        /// </summary>
        /// <param name="value">Входное значение для обработки.</param>
        /// <returns>Значение с учетом зоны затухания до нуля, полученное с помощью фильтра EMA.</returns>
        private double emaLP_of_emaLP_of_emaLP_deathZone_toZero(double value)
        {
            _storedToZero1 += _deadZoneToZeroValue * (value - _storedToZero1);
            _storedToZero2 += _deadZoneToZeroValue * (_storedToZero1 - _storedToZero2);
            _storedToZero3 += _deadZoneToZeroValue * (_storedToZero2 - _storedToZero3);

            return _storedToZero3;
        }

        /// <summary>
        ///     Применяет фильтр экспоненциального скользящего среднего (EMA) к входному значению для симуляции антиролла.
        /// </summary>
        /// <param name="value">Входное значение для обработки.</param>
        /// <returns>Значение с учетом антиролла, полученное с помощью фильтра EMA.</returns>
        private double emaLP_of_emaLP_of_emaLP_antiRoll(double value)
        {
            _stored1 += _antiRollValue * (value - _stored1);
            _stored2 += _antiRollValue * (_stored1 - _stored2);
            _stored3 += _antiRollValue * (_stored2 - _stored3);

            return _stored3;
        }

        /// <summary>
        ///     Запускает поток симуляции сглаживания.
        /// </summary>
        private void SmoothingSimLoop()
        {
            if (_th != null)
            {
                return;
            }

            _th = new Thread(() =>
            {
                while (Settings.isRunning)
                {
                    _outputValueSim = emaLP_of_emaLP_of_emaLP(_inputValueSim);

                    Thread.Sleep(5);
                }
            });
            _th.Start();
        }

        /// <summary>
        ///     Запускает поток симуляции зоны затухания до нуля.
        /// </summary>
        private void DeadZoneToZeroSimLoop()
        {
            if (_thDeadZoneToZero != null)
            {
                return;
            }

            _thDeadZoneToZero = new Thread(() =>
            {
                while (Settings.isRunning)
                {
                    _outputValueDeadZoneToZeroSim =
                        emaLP_of_emaLP_of_emaLP_deathZone_toZero(INPUT_VALUE_DEAD_ZONE_TO_ZERO_SIM);

                    Thread.Sleep(5);
                }
            });
            _thDeadZoneToZero.Start();
        }

        /// <summary>
        ///     Запускает поток симуляции антиролла.
        /// </summary>
        private void AntiRollSimLoop()
        {
            if (_thAntiRoll != null)
            {
                return;
            }

            _thAntiRoll = new Thread(() =>
            {
                while (Settings.isRunning)
                {
                    _outputValueAntiRollSim = emaLP_of_emaLP_of_emaLP_antiRoll(_inputValueAntiRollSim);

                    Thread.Sleep(5);
                }
            });

            _thAntiRoll.Start();
        }

        /// <summary>
        ///     Применяет фильтр сглаживания к входному значению для симуляции сглаживания.
        /// </summary>
        /// <param name="value">Входное значение для обработки.</param>
        /// <returns>Сглаженное значение с учетом фильтра сглаживания.</returns>
        public double smoothing_sim(double value)
        {
            if (_smoothingValueSim == 1.0)
            {
                return value;
            }

            _inputValueSim = value;

            var outputValueSim = _outputValueSim;

            _storedToZero1 = _stored1 = _stored1Sim;

            return outputValueSim;
        }

        /// <summary>
        ///     Выполняет сглаживание входного значения с использованием фильтра "Сглаживание" (Smoothing).
        /// </summary>
        /// <param name="value">Входное значение для сглаживания.</param>
        /// <returns>Сглаженное значение.</returns>
        public double Smoothing(double value)
        {
            var num = 30.0 / (30.0 + Math.Abs(value - _stored3) * 2.0 * _nonlinearValue);

            _stored1 += _smoothingValue * num * (value - _stored1);
            _stored2 += _smoothingValue * num * (_stored1 - _stored2);
            _stored3 += _smoothingValue * num * (_stored2 - _stored3);

            return _stored3;
        }

        /// <summary>
        ///     Выполняет сглаживание входного значения с использованием фильтра "Сглаживание" (Smoothing) с учетом знака
        ///     (положительное или отрицательное).
        /// </summary>
        /// <param name="value">Входное значение для сглаживания.</param>
        /// <param name="plusminus">Возвращает true, если был обнаружен переход через ноль, иначе false.</param>
        /// <returns>Сглаженное значение.</returns>
        public double SmoothingPlusMinus(double value, ref bool plusminus)
        {
            plusminus = false;

            if (_deadZoneToZeroValue == 1.0)
            {
                return value;
            }

            if (_startPlusMinusTick != 0)
            {
                if ((value > 0.0 && _lastValuePlusMinus < 0.0 || value < 0.0 && _lastValuePlusMinus > 0.0) &&
                    Math.Abs(value - _lastValuePlusMinus) > _deadToZeroInterval)
                {
                    _startPlusMinusTick = Environment.TickCount;
                }

                if (Environment.TickCount - _startPlusMinusTick < _deadToZeroTime)
                {
                    _stored1Sim = _stored1 = _storedToZero1 += _deadZoneToZeroValue * (value - _storedToZero1);
                    _stored2Sim = _stored2 = _storedToZero2 += _deadZoneToZeroValue * (_storedToZero1 - _storedToZero2);
                    _stored3Sim = _stored3 = _storedToZero3 += _deadZoneToZeroValue * (_storedToZero2 - _storedToZero3);

                    var storedToZero3 = _storedToZero3;

                    _lastValuePlusMinus = value;

                    plusminus = true;

                    return storedToZero3;
                }

                _startPlusMinusTick = 0;
                _stored3Sim = _stored3 = _storedToZero3 = value;
            }

            if ((value > 0.0 && _lastValuePlusMinus < 0.0 || value < 0.0 && _lastValuePlusMinus > 0.0) &&
                Math.Abs(value - _lastValuePlusMinus) > _deadToZeroInterval)
            {
                _startPlusMinusTick = Environment.TickCount;

                _stored3Sim = _stored3 = _storedToZero3 = _lastValuePlusMinus;
                _stored2Sim = _stored2 = _storedToZero2 = _lastValuePlusMinus;
                _stored1Sim = _stored1 = _storedToZero1 = _lastValuePlusMinus;

                _stored1Sim = _stored1 = _storedToZero1 += _deadZoneToZeroValue * (value - _storedToZero1);
                _stored2Sim = _stored2 = _storedToZero2 += _deadZoneToZeroValue * (_storedToZero1 - _storedToZero2);
                _stored3Sim = _stored3 = _storedToZero3 += _deadZoneToZeroValue * (_storedToZero2 - _storedToZero3);

                var storedToZero3 = _storedToZero3;

                _lastValuePlusMinus = value;

                plusminus = true;

                return storedToZero3;
            }

            _lastValuePlusMinus = value;

            return value;
        }

        /// <summary>
        ///     Применяет фильтр "Зона затухания" (Dead Zone) к входному значению и возвращает значение с учетом зоны затухания.
        /// </summary>
        /// <param name="value">Входное значение для обработки.</param>
        /// <param name="startDeadZoneToZero">Возвращает true, если начата зона затухания до нуля, иначе false.</param>
        /// <returns>Обработанное значение с учетом зоны затухания.</returns>
        public double dead_zone(double value, ref bool startDeadZoneToZero)
        {
            startDeadZoneToZero = false;

            if (_deadZoneValue == 0.0)
            {
                return value;
            }

            if (value < -_deadZoneValue && value < 0.0 || value > _deadZoneValue && value >= 0.0)
            {
                _first = false;
                _lastValueDeadZone = value;
                return value;
            }

            return _first ? 0.0 : _lastValueDeadZone >= 0.0 ? _deadZoneValue : -_deadZoneValue;
        }

        /// <summary>
        ///     Применяет фильтр "Антиролл" (AntiRoll) к входному значению и возвращает значение с учетом антиролла.
        /// </summary>
        /// <param name="value">Входное значение для обработки.</param>
        /// <param name="startAntiroll">Возвращает true, если начата антиролл, иначе false.</param>
        /// <returns>Обработанное значение с учетом антиролла.</returns>
        public double AntiRoll(double value, ref bool startAntiroll)
        {
            startAntiroll = false;

            if (_antiRollValue == 0.0 || _antiRollMinValue == 0.0 && _antiRollMaxValue == 0.0)
            {
                return value;
            }

            if (value < _antiRollMinValue && value < 0.0 || value > _antiRollMaxValue && value >= 0.0)
            {
                value = 0.0;
                startAntiroll = true;
                _inputValueAntiRollSim = value;
                var valueAntiRollSim = _outputValueAntiRollSim;
                _stored1Sim = _storedToZero1 = _stored1;
                _stored2Sim = _storedToZero2 = _stored2;
                _stored3Sim = _storedToZero3 = _stored3;
                return valueAntiRollSim;
            }

            _stored1 = _stored2 = _stored3 = value;

            return value;
        }

        /// <summary>
        ///     Освобождает ресурсы и завершает работу фильтров.
        /// </summary>
        public void Free()
        {
            _stopSmoothingSim = true;
        }
    }
}