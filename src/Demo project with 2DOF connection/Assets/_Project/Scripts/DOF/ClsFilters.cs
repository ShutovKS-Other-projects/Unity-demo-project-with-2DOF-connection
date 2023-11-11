#region

using System;
using System.Threading;
using DOF.Data.Static;

#endregion

namespace DOF
{
    public class ClsFilters
    {
        private readonly double _inputValueDeadzoneTozeroSim = 0.0;
        private double _antirollMaxValue;
        private double _antirollMinValue;
        private double _antirollValue;
        private double _deadToZeroInterval;
        private double _deadToZeroTime;
        private double _deadzoneTozeroValue;
        private double _deadzoneValue;
        private bool _first = true;
        private double _inputValueAntirollSim;
        private double _inputValueSim;
        private double _lastValueDeadzone;
        private double _lastValuePlusMinus;
        private double _nonlinearValue;
        private double _outputValueAntirollSim;
        private double _outputValueDeadzoneTozeroSim;
        private double _outputValueSim;
        private double _smoothingValue;
        private double _smoothingValueSim;
        private int _startPlusMinusTick;
        private bool _stopSmoothingSim;
        private double _stored1;
        private double _stored1Sim;
        private double _stored2;
        private double _stored2Sim;
        private double _stored3;
        private double _stored3Sim;
        private double _storedToZero1;
        private double _storedToZero2;
        private double _storedToZero3;
        private Thread _th;
        private Thread _thAntiroll;
        private Thread _thDeadzoneToZero;

        public void SetDeadToZeroTimeValue(int deadToZeroTime)
        {
            _deadToZeroTime = deadToZeroTime;
        }

        public void SetDeadToZeroIntervalValue(int deadToZeroInterval)
        {
            _deadToZeroInterval = deadToZeroInterval;
        }

        public void SetDeadzoneToZeroValue(int deadzoneToZeroValue)
        {
            _lastValuePlusMinus = 0.0;
            _startPlusMinusTick = 0;
            _deadzoneTozeroValue = 30.0 / (30 + deadzoneToZeroValue);
            _storedToZero1 = _storedToZero2 = _storedToZero3 = 0.0;
        }

        public void SetDeadzoneValue(int deadzoneValue)
        {
            _deadzoneValue = deadzoneValue;
            _lastValueDeadzone = 0.0;
            _first = true;
        }

        public void SetAntirollValue(int antirollValue, double minValue, double maxValue)
        {
            _antirollValue = antirollValue == 0 ? 0.0 : 2.0 / (2 + antirollValue);
            _antirollMinValue = minValue;
            _antirollMaxValue = maxValue;
            _stored1 = 0.0;
            _stored2 = 0.0;
            _stored3 = 0.0;
            if (antirollValue == 0)
            {
                return;
            }

            AntirollSimLoop();
        }

        public void SetNonlinearValue(int nonlinearValue)
        {
            _nonlinearValue = nonlinearValue / 4.0;
        }

        public void SetSmoothingValue(int smoothingValue)
        {
            _smoothingValue = 30.0 / (30 + smoothingValue);
            _stored1 = 0.0;
            _stored2 = 0.0;
            _stored3 = 0.0;
        }

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

        private double emaLP_of_emaLP_of_emaLP(double value)
        {
            _stored1Sim += _smoothingValueSim * (value - _stored1Sim);
            _stored2Sim += _smoothingValueSim * (_stored1Sim - _stored2Sim);
            _stored3Sim += _smoothingValueSim * (_stored2Sim - _stored3Sim);
            return _stored3Sim;
        }

        private double emaLP_of_emaLP_of_emaLP_deathzone_tozero(double value)
        {
            _storedToZero1 += _deadzoneTozeroValue * (value - _storedToZero1);
            _storedToZero2 += _deadzoneTozeroValue * (_storedToZero1 - _storedToZero2);
            _storedToZero3 += _deadzoneTozeroValue * (_storedToZero2 - _storedToZero3);
            return _storedToZero3;
        }

        private double emaLP_of_emaLP_of_emaLP_antiroll(double value)
        {
            _stored1 += _antirollValue * (value - _stored1);
            _stored2 += _antirollValue * (_stored1 - _stored2);
            _stored3 += _antirollValue * (_stored2 - _stored3);
            return _stored3;
        }

        private void SmoothingSimLoop()
        {
            if (_th != null)
            {
                return;
            }

            _th = new Thread(() =>
            {
                while (SettingsData.isRunning)
                {
                    _outputValueSim = emaLP_of_emaLP_of_emaLP(_inputValueSim);
                    Thread.Sleep(5);
                }
            });
            _th.Start();
        }

        private void DeadzoneToZeroSimLoop()
        {
            if (_thDeadzoneToZero != null)
            {
                return;
            }

            _thDeadzoneToZero = new Thread(() =>
            {
                while (SettingsData.isRunning)
                {
                    _outputValueDeadzoneTozeroSim =
                        emaLP_of_emaLP_of_emaLP_deathzone_tozero(_inputValueDeadzoneTozeroSim);
                    Thread.Sleep(5);
                }
            });
            _thDeadzoneToZero.Start();
        }

        private void AntirollSimLoop()
        {
            if (_thAntiroll != null)
            {
                return;
            }

            _thAntiroll = new Thread(() =>
            {
                while (SettingsData.isRunning)
                {
                    _outputValueAntirollSim = emaLP_of_emaLP_of_emaLP_antiroll(_inputValueAntirollSim);
                    Thread.Sleep(5);
                }

                var num = 1;
                num = 2;
            });
            _thAntiroll.Start();
        }

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

        public double Smoothing(double value)
        {
            var num = 30.0 / (30.0 + Math.Abs(value - _stored3) * 2.0 * _nonlinearValue);
            _stored1 += _smoothingValue * num * (value - _stored1);
            _stored2 += _smoothingValue * num * (_stored1 - _stored2);
            _stored3 += _smoothingValue * num * (_stored2 - _stored3);
            return _stored3;
        }

        public double SmoothingPlusMinus(double value, ref bool plusMinus)
        {
            plusMinus = false;
            if (_deadzoneTozeroValue == 1.0)
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
                    _stored1Sim = _stored1 = _storedToZero1 += _deadzoneTozeroValue * (value - _storedToZero1);
                    _stored2Sim = _stored2 = _storedToZero2 += _deadzoneTozeroValue * (_storedToZero1 - _storedToZero2);
                    _stored3Sim = _stored3 = _storedToZero3 += _deadzoneTozeroValue * (_storedToZero2 - _storedToZero3);
                    var storedToZero3 = _storedToZero3;
                    _lastValuePlusMinus = value;
                    plusMinus = true;
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
                _stored1Sim = _stored1 = _storedToZero1 += _deadzoneTozeroValue * (value - _storedToZero1);
                _stored2Sim = _stored2 = _storedToZero2 += _deadzoneTozeroValue * (_storedToZero1 - _storedToZero2);
                _stored3Sim = _stored3 = _storedToZero3 += _deadzoneTozeroValue * (_storedToZero2 - _storedToZero3);
                var storedToZero3 = _storedToZero3;
                _lastValuePlusMinus = value;
                plusMinus = true;
                return storedToZero3;
            }

            _lastValuePlusMinus = value;
            return value;
        }

        public double dead_zone(double value, ref bool startDeadZoneToZero)
        {
            startDeadZoneToZero = false;
            if (_deadzoneValue == 0.0)
            {
                return value;
            }

            if (value < -_deadzoneValue && value < 0.0 || value > _deadzoneValue && value >= 0.0)
            {
                _first = false;
                _lastValueDeadzone = value;
                return value;
            }

            return _first ? 0.0 : _lastValueDeadzone >= 0.0 ? _deadzoneValue : -_deadzoneValue;
        }

        public double Antiroll(double value, ref bool startAntiroll)
        {
            startAntiroll = false;
            if (_antirollValue == 0.0 || _antirollMinValue == 0.0 && _antirollMaxValue == 0.0)
            {
                return value;
            }

            if (value < _antirollMinValue && value < 0.0 || value > _antirollMaxValue && value >= 0.0)
            {
                value = 0.0;
                startAntiroll = true;
                _inputValueAntirollSim = value;
                var valueAntirollSim = _outputValueAntirollSim;
                _stored1Sim = _storedToZero1 = _stored1;
                _stored2Sim = _storedToZero2 = _stored2;
                _stored3Sim = _storedToZero3 = _stored3;
                return valueAntirollSim;
            }

            _stored1 = _stored2 = _stored3 = value;
            return value;
        }

        public void Free()
        {
            _stopSmoothingSim = true;
        }
    }
}