using System;
using System.Threading;

namespace Data
{
    public class ClsFilters
    {
        private bool _stopSmoothingSim;

        private double _deadToZeroTime;
        private double _deadToZeroInterval;

        private double _inputValueSim;
        private double _outputValueSim;

        private const double INPUT_VALUE_DEAD_ZONE_TO_ZERO_SIM = 0.0;
        private double _outputValueDeadZoneToZeroSim;

        private double _inputValueAntiRollSim;
        private double _outputValueAntiRollSim;

        private double _stored1;
        private double _stored2;
        private double _stored3;

        private double _stored1Sim;
        private double _stored2Sim;
        private double _stored3Sim;

        private double _storedToZero1;
        private double _storedToZero2;
        private double _storedToZero3;

        private double _smoothingValueSim;
        private double _smoothingValue;

        private double _nonlinearValue;

        private double _deadZoneValue;
        private double _deadZoneToZeroValue;

        private double _antiRollValue;
        private double _antiRollMinValue;
        private double _antiRollMaxValue;

        private Thread _th;
        private Thread _thDeadZoneToZero;
        private Thread _thAntiRoll;

        private double _lastValueDeadZone;
        private double _lastValuePlusMinus;

        private int _startPlusMinusTick;

        private bool _first = true;

        public void SetDeadToZeroTimeValue(int deadToZeroTime)
        {
            _deadToZeroTime = deadToZeroTime;
        }

        public void SetDeadToZeroIntervalValue(int deadToZeroInterval)
        {
            _deadToZeroInterval = deadToZeroInterval;
        }

        public void SetDeadZoneToZeroValue(int deadZoneToZeroValue)
        {
            _lastValuePlusMinus = 0.0;
            _startPlusMinusTick = 0;
            _deadZoneToZeroValue = 30.0 / (30 + deadZoneToZeroValue);
            _storedToZero1 = _storedToZero2 = _storedToZero3 = 0.0;
        }

        public void SetDeadZoneValue(int deadZoneValue)
        {
            _deadZoneValue = deadZoneValue;
            _lastValueDeadZone = 0.0;
            _first = true;
        }

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

        private double emaLP_of_emaLP_of_emaLP_deathZone_toZero(double value)
        {
            _storedToZero1 += _deadZoneToZeroValue * (value - _storedToZero1);
            _storedToZero2 += _deadZoneToZeroValue * (_storedToZero1 - _storedToZero2);
            _storedToZero3 += _deadZoneToZeroValue * (_storedToZero2 - _storedToZero3);
            
            return _storedToZero3;
        }

        private double emaLP_of_emaLP_of_emaLP_antiRoll(double value)
        {
            _stored1 += _antiRollValue * (value - _stored1);
            _stored2 += _antiRollValue * (_stored1 - _stored2);
            _stored3 += _antiRollValue * (_stored2 - _stored3);
            
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
                while (Settings.isRunning)
                {
                    _outputValueSim = emaLP_of_emaLP_of_emaLP(_inputValueSim);
                    
                    Thread.Sleep(5);
                }
            });
            _th.Start();
        }

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
                    _outputValueDeadZoneToZeroSim = emaLP_of_emaLP_of_emaLP_deathZone_toZero(INPUT_VALUE_DEAD_ZONE_TO_ZERO_SIM);
                    
                    Thread.Sleep(5);
                }
            });
            _thDeadZoneToZero.Start();
        }

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

        public void Free()
        {
            _stopSmoothingSim = true;
        }
    }
}