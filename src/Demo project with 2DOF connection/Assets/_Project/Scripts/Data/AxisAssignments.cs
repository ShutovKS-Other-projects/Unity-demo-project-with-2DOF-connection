using System;
using System.Linq;

namespace Data
{
    public class AxisAssignments
    {
        private int _coefWind;
        private int _typeWind;

        private double _procWind;

        private double[] _axis = new double[9];
        private bool[] _absValues = new bool[8];

        private double _minPitch;
        private double _maxPitch;

        private double _minRoll;
        private double _maxRoll;

        private double _minYaw;
        private double _maxYaw;

        private double _minSurge;
        private double _maxSurge;

        private double _minSway;
        private double _maxSway;

        private double _minHeave;
        private double _maxHeave;

        private double _minExtra1;
        private double _maxExtra1;

        private double _minExtra2;
        private double _maxExtra2;

        private double _minExtra3;
        private double _maxExtra3;

        private AxisDofData[] _axisDofs;
        private AxisDofData[] _axisDofs2;

        private Cls_Filters[] _clsFilters = new Cls_Filters[48];
        private Cls_Filters[] _clsFilters2 = new Cls_Filters[48];

        public AxisAssignments()
        {
            for (var index = 0; index < 48; ++index)
            {
                _clsFilters[index] = new Cls_Filters();
                _clsFilters2[index] = new Cls_Filters();
            }
        }

        public void Free()
        {
            for (var index = 0; index < 48; ++index)
            {
                _clsFilters[index].Free();
                _clsFilters2[index].Free();
            }
        }

        public void SetAxisDofs(AxisDofData[] axisDofs, AxisDofData[] axisDofs2, double minPitch, double maxPitch,
            double minRoll, double maxRoll, double minYaw, double maxYaw, double minSurge, double maxSurge,
            double minSway, double maxSway, double minHeave, double maxHeave, double minExtra1, double maxExtra1,
            double minExtra2, double maxExtra2, double minExtra3, double maxExtra3, int procWind, int coefWind,
            int typeWind)
        {
            _coefWind = coefWind;
            _typeWind = typeWind;
            _axisDofs = axisDofs;
            _axisDofs2 = axisDofs2;
            _procWind = procWind;
            _minPitch = minPitch;
            _maxPitch = maxPitch;
            _minRoll = minRoll;
            _maxRoll = maxRoll;
            _minYaw = minYaw;
            _maxYaw = maxYaw;
            _minSurge = minSurge;
            _maxSurge = maxSurge;
            _minSway = minSway;
            _maxSway = maxSway;
            _minHeave = minHeave;
            _maxHeave = maxHeave;
            _minExtra1 = minExtra1;
            _maxExtra1 = maxExtra1;
            _minExtra2 = minExtra2;
            _maxExtra2 = maxExtra2;
            _minExtra3 = minExtra3;
            _maxExtra3 = maxExtra3;

            _absValues = new bool[8];

            for (int i = 0; i < 8; i++)
            {
                int startIndex = i * 6;
                _absValues[i] = false;
                for (int j = startIndex; j < startIndex + 6; j++)
                {
                    if (axisDofs[j].Force == "Wind")
                    {
                        _absValues[i] = true;
                        break;
                    }
                }
            }

            for (var index = 0; index < 48; ++index)
            {
                _clsFilters[index].SetSmoothingValue(axisDofs[index].Smoothing);
                _clsFilters[index].SetSmoothingValueSim(axisDofs[index].SmoothingSim);
                _clsFilters[index].SetNonlinearValue(axisDofs[index].Nonlinear);
                _clsFilters[index].SetDeadzoneValue(axisDofs[index].DeathZone);
                _clsFilters[index].SetDeadzoneToZeroValue(axisDofs[index].DeathToZero);
                _clsFilters[index].SetDeadToZeroTimeValue(axisDofs[index].DeathToZeroTime);
                _clsFilters[index].SetDeadToZeroIntervalValue(axisDofs[index].DeathToZeroInterval);

                var minValue = 0.0;
                var maxValue = 0.0;

                switch (axisDofs[index].Force)
                {
                    case "Pitch":
                        minValue = minPitch;
                        maxValue = maxPitch;
                        break;
                    case "Roll":
                        minValue = minRoll;
                        maxValue = maxRoll;
                        break;
                    case "Yaw":
                        minValue = minYaw;
                        maxValue = maxYaw;
                        break;
                    case "Heave":
                        minValue = minHeave;
                        maxValue = maxHeave;
                        break;
                    case "Sway":
                        minValue = minSway;
                        maxValue = maxSway;
                        break;
                    case "Surge":
                        minValue = minSurge;
                        maxValue = maxSurge;
                        break;
                    case "Ex1":
                        minValue = minExtra1;
                        maxValue = maxExtra1;
                        break;
                    case "Ex2":
                        minValue = minExtra2;
                        maxValue = maxExtra2;
                        break;
                    case "Ex3":
                        minValue = minExtra3;
                        maxValue = maxExtra3;
                        break;
                }

                _clsFilters[index].SetAntirollValue(axisDofs[index].AntiRoll, minValue, maxValue);
            }

            for (var index = 0; index < 48; ++index)
            {
                _clsFilters2[index].SetSmoothingValue(axisDofs2[index].Smoothing);
                _clsFilters2[index].SetSmoothingValueSim(axisDofs2[index].SmoothingSim);
                _clsFilters2[index].SetNonlinearValue(axisDofs2[index].Nonlinear);
                _clsFilters2[index].SetDeadzoneValue(axisDofs2[index].DeathZone);
                _clsFilters2[index].SetDeadzoneToZeroValue(axisDofs2[index].DeathToZero);
                _clsFilters2[index].SetDeadToZeroTimeValue(axisDofs2[index].DeathToZeroTime);
                _clsFilters2[index].SetDeadToZeroIntervalValue(axisDofs2[index].DeathToZeroInterval);

                var minValue = 0.0;
                var maxValue = 0.0;

                switch (axisDofs2[index].Force)
                {
                    case "Pitch":
                        minValue = minPitch;
                        maxValue = maxPitch;
                        break;
                    case "Roll":
                        minValue = minRoll;
                        maxValue = maxRoll;
                        break;
                    case "Yaw":
                        minValue = minYaw;
                        maxValue = maxYaw;
                        break;
                    case "Heave":
                        minValue = minHeave;
                        maxValue = maxHeave;
                        break;
                    case "Sway":
                        minValue = minSway;
                        maxValue = maxSway;
                        break;
                    case "Surge":
                        minValue = minSurge;
                        maxValue = maxSurge;
                        break;
                    case "Ex1":
                        minValue = minExtra1;
                        maxValue = maxExtra1;
                        break;
                    case "Ex2":
                        minValue = minExtra2;
                        maxValue = maxExtra2;
                        break;
                    case "Ex3":
                        minValue = minExtra3;
                        maxValue = maxExtra3;
                        break;
                }

                _clsFilters2[index].SetAntirollValue(axisDofs2[index].AntiRoll, minValue, maxValue);
            }
        }

        public void ProcessingData(double pitch, double roll, double yaw, double surge, double sway, double heave,
            double extra1, double extra2, double extra3, double wind)
        {
            if (!IsAntiRollEnabledForForce(nameof(pitch)))
            {
                if (pitch > _maxPitch) pitch = _maxPitch;
                if (pitch < _minPitch) pitch = _minPitch;
            }

            if (!IsAntiRollEnabledForForce(nameof(roll)))
            {
                if (roll > _maxRoll) roll = _maxRoll;
                if (roll < _minRoll) roll = _minRoll;
            }

            if (!IsAntiRollEnabledForForce(nameof(yaw)))
            {
                if (yaw > _maxYaw) yaw = _maxYaw;
                if (yaw < _minYaw) yaw = _minYaw;
            }

            if (!IsAntiRollEnabledForForce(nameof(surge)))
            {
                if (surge > _maxSurge) surge = _maxSurge;
                if (surge < _minSurge) surge = _minSurge;
            }

            if (!IsAntiRollEnabledForForce(nameof(sway)))
            {
                if (sway > _maxSway) sway = _maxSway;
                if (sway < _minSway) sway = _minSway;
            }

            if (!IsAntiRollEnabledForForce(nameof(heave)))
            {
                if (heave > _maxHeave) heave = _maxHeave;
                if (heave < _minHeave) heave = _minHeave;
            }

            if (!IsAntiRollEnabledForForce("Ex1"))
            {
                if (extra1 > _maxExtra1) extra1 = _maxExtra1;
                if (extra1 < _minExtra1) extra1 = _minExtra1;
            }

            if (!IsAntiRollEnabledForForce("Ex2"))
            {
                if (extra2 > _maxExtra2) extra2 = _maxExtra2;
                if (extra2 < _minExtra2) extra2 = _minExtra2;
            }

            if (!IsAntiRollEnabledForForce("Ex3"))
            {
                if (extra3 > _maxExtra3) extra3 = _maxExtra3;
                if (extra3 < _minExtra3) extra3 = _minExtra3;
            }

            if (_axisDofs == null)
            {
                return;
            }

            var axisIndices = new[] { 0, 1, 2, 3, 4, 5, 48, 49, 50, 51, 52, 53 };
            var axisIndices2 = new[] { 6, 7, 8, 9, 10, 11, 54, 55, 56, 57, 58, 59 };
            var axisIndices3 = new[] { 12, 13, 14, 15, 16, 17, 60, 61, 62, 63, 64, 65 };
            var axisIndices4 = new[] { 18, 19, 20, 21, 22, 23, 66, 67, 68, 69, 70, 71 };
            var axisIndices5 = new[] { 24, 25, 26, 27, 28, 29, 72, 73, 74, 75, 76, 77 };
            var axisIndices6 = new[] { 30, 31, 32, 33, 34, 35, 78, 79, 80, 81, 82, 83 };
            var axisIndices7 = new[] { 36, 37, 38, 39, 40, 41, 84, 85, 86, 87, 88, 89 };
            var axisIndices8 = new[] { 42, 43, 44, 45, 46, 47, 90, 91, 92, 93, 94, 95 };

            _axis[0] = CalculateAxisValue(axisIndices, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3);
            _axis[1] = CalculateAxisValue(axisIndices2, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3);
            _axis[2] = CalculateAxisValue(axisIndices3, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3);
            _axis[3] = CalculateAxisValue(axisIndices4, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3);
            _axis[4] = CalculateAxisValue(axisIndices5, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3);
            _axis[5] = CalculateAxisValue(axisIndices6, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3);
            _axis[6] = CalculateAxisValue(axisIndices7, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3);
            _axis[7] = CalculateAxisValue(axisIndices8, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3);

            _axis[8] = _typeWind != 0
                ? _coefWind
                : wind * (0.01 * _procWind) * (_coefWind / 100.0);

            return;

            double CalculateAxisValue(int[] indices, double pitch, double roll, double yaw, double heave, double sway,
                double surge, double extra1, double extra2, double extra3)
            {
                return indices.Sum(index =>
                    GetDofValue(index, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3));
            }
        }

        public double GetDofValue(int index, double pitch, double roll, double yaw, double heave, double sway,
            double surge, double extra1, double extra2, double extra3)
        {
            AxisDofData[] axisDofDataArray;
            Cls_Filters[] clsFiltersArray;

            var num1 = 0.0;
            if (index < 48)
            {
                axisDofDataArray = _axisDofs;
                clsFiltersArray = _clsFilters;
            }
            else
            {
                axisDofDataArray = _axisDofs2;
                clsFiltersArray = _clsFilters2;
                index -= 48;
            }

            num1 = axisDofDataArray[index].Force switch
            {
                nameof(pitch) => pitch,
                nameof(roll) => roll,
                nameof(yaw) => yaw,
                nameof(heave) => heave,
                nameof(sway) => sway,
                nameof(surge) => surge,
                "Ex1" => extra1,
                "Ex2" => extra2,
                "Ex3" => extra3,
                _ => num1
            };

            var startDeadZoneToZero = false;
            var num2 = clsFiltersArray[index].dead_zone(num1, ref startDeadZoneToZero);
            if (!startDeadZoneToZero)
            {
                var plusMinus = false;
                num2 = clsFiltersArray[index].smoothingPlusMinus(num2, ref plusMinus);
                if (!plusMinus)
                {
                    var startAntiRoll = false;
                    num2 = clsFiltersArray[index].antiroll(num2, ref startAntiRoll);
                    if (!startAntiRoll)
                    {
                        var num3 = clsFiltersArray[index].smoothing(num2);
                        num2 = clsFiltersArray[index].smoothing_sim(num3);
                    }
                }
            }

            num2 = axisDofDataArray[index].Force switch
            {
                nameof(pitch) => pitch,
                nameof(roll) => roll,
                nameof(yaw) => yaw,
                nameof(heave) => heave,
                nameof(sway) => sway,
                nameof(surge) => surge,
                "Ex1" => extra1,
                "Ex2" => extra2,
                "Ex3" => extra3,
                _ => num2
            };

            if (axisDofDataArray[index].Dir)
            {
                num2 *= -1.0;
            }

            return num2 * (0.01 * axisDofDataArray[index].Proc);
        }

        public double GetAxis(int axisNumber, ref bool absValue)
        {
            if (axisNumber < 0 || axisNumber >= _axis.Length)
            {
                throw new ArgumentOutOfRangeException($"{axisNumber}");
            }

            absValue = _absValues[axisNumber];
            var axisValue = _axis[axisNumber];

            axisValue = absValue
                ? Math.Max(0.0, Math.Min(axisValue, byte.MaxValue))
                : Math.Max(-1.0, Math.Min(axisValue, 1.0));

            return axisValue;
        }

        public double GetAxis9()
        {
            var axisValue = _axis[8];
            return axisValue > 999.0 ? 999.0 : axisValue;
        }
        
        private bool IsAntiRollEnabledForForce(string force)
        {
            for (var index = 0; index < 48; ++index)
            {
                if (_axisDofs[index].Force == force && _axisDofs[index].AntiRoll != 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}