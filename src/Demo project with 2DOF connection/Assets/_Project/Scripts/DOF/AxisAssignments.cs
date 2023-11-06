using System;
using System.Threading;
using DOF.Data;
using DOF.Data.Dynamic;
using DOF.Data.Static;

namespace DOF
{
    public class AxisAssignments
    {
        public AxisAssignments(bool a)
        {
            _a = a;
            for (var index = 0; index < 48; ++index)
            {
                _clsFilters[index] = new ClsFilters();
            }

            for (var index = 0; index < 48; ++index)
            {
                _clsFilters2[index] = new ClsFilters();
            }
        }

        private readonly bool _a;
        private bool[] _absValues = new bool[8];
        private double[] _axis = new double[9];
        private AxisDofData[] _axisDofs;
        private AxisDofData[] _axisDofs2;
        private readonly ClsFilters[] _clsFilters = new ClsFilters[48];
        private readonly ClsFilters[] _clsFilters2 = new ClsFilters[48];
        private int _coefWind;
        private double _maxExtra1;
        private double _maxExtra2;
        private double _maxExtra3;
        private double _maxHeave;
        private double _maxPitch;
        private double _maxRoll;
        private double _maxSurge;
        private double _maxSway;
        private double _maxYaw;
        private double _minExtra1;
        private double _minExtra2;
        private double _minExtra3;
        private double _minHeave;
        private double _minPitch;
        private double _minRoll;
        private double _minSurge;
        private double _minSway;
        private double _minYaw;
        private double _procWind;
        private int _typeWind;

        public void Free()
        {
            for (var index = 0; index < 48; ++index)
            {
                _clsFilters[index].Free();
                _clsFilters2[index].Free();
            }
        }

        public void SetAxisDofs(AxisDofData[] axisDofs, AxisDofData[] axisDofs2,
            double minPitch, double maxPitch, double minRoll, double maxRoll, double minYaw, double maxYaw,
            double minSurge, double maxSurge, double minSway, double maxSway, double minHeave, double maxHeave,
            double minExtra1, double maxExtra1, double minExtra2, double maxExtra2, double minExtra3, double maxExtra3,
            int procWind, int coefWind, int typeWind)
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

            _absValues[0] = axisDofs[0].Force == "Wind" || axisDofs[1].Force == "Wind" ||
                            axisDofs[2].Force == "Wind" || axisDofs[3].Force == "Wind" ||
                            axisDofs[4].Force == "Wind" || axisDofs[5].Force == "Wind";
            _absValues[1] = axisDofs[6].Force == "Wind" || axisDofs[7].Force == "Wind" ||
                            axisDofs[8].Force == "Wind" || axisDofs[9].Force == "Wind" ||
                            axisDofs[10].Force == "Wind" || axisDofs[11].Force == "Wind";
            _absValues[2] = axisDofs[12].Force == "Wind" || axisDofs[13].Force == "Wind" ||
                            axisDofs[14].Force == "Wind" || axisDofs[15].Force == "Wind" ||
                            axisDofs[16].Force == "Wind" || axisDofs[17].Force == "Wind";
            _absValues[3] = axisDofs[18].Force == "Wind" || axisDofs[19].Force == "Wind" ||
                            axisDofs[20].Force == "Wind" || axisDofs[21].Force == "Wind" ||
                            axisDofs[22].Force == "Wind" || axisDofs[23].Force == "Wind";
            _absValues[4] = axisDofs[24].Force == "Wind" || axisDofs[25].Force == "Wind" ||
                            axisDofs[26].Force == "Wind" || axisDofs[27].Force == "Wind" ||
                            axisDofs[28].Force == "Wind" || axisDofs[29].Force == "Wind";
            _absValues[5] = axisDofs[30].Force == "Wind" || axisDofs[31].Force == "Wind" ||
                            axisDofs[32].Force == "Wind" || axisDofs[33].Force == "Wind" ||
                            axisDofs[34].Force == "Wind" || axisDofs[35].Force == "Wind";
            _absValues[6] = axisDofs[36].Force == "Wind" || axisDofs[37].Force == "Wind" ||
                            axisDofs[38].Force == "Wind" || axisDofs[39].Force == "Wind" ||
                            axisDofs[40].Force == "Wind" || axisDofs[41].Force == "Wind";
            _absValues[7] = axisDofs[42].Force == "Wind" || axisDofs[43].Force == "Wind" ||
                            axisDofs[44].Force == "Wind" || axisDofs[45].Force == "Wind" ||
                            axisDofs[46].Force == "Wind" || axisDofs[47].Force == "Wind";

            for (var index = 0; index < 48; ++index)
            {
                _clsFilters[index].SetSmoothingValue(axisDofs[index].Smoothing);
                _clsFilters[index].SetSmoothingValueSim(axisDofs[index].SmoothingSim);
                _clsFilters[index].SetNonlinearValue(axisDofs[index].Nonlinear);
                _clsFilters[index].SetDeadzoneValue(axisDofs[index].Deathzone);
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

                _clsFilters[index].SetAntirollValue(axisDofs[index].Antiroll, minValue, maxValue);
            }

            for (var index = 0; index < 48; ++index)
            {
                _clsFilters2[index].SetSmoothingValue(axisDofs2[index].Smoothing);
                _clsFilters2[index].SetSmoothingValueSim(axisDofs2[index].SmoothingSim);
                _clsFilters2[index].SetNonlinearValue(axisDofs2[index].Nonlinear);
                _clsFilters2[index].SetDeadzoneValue(axisDofs2[index].Deathzone);
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

                _clsFilters2[index].SetAntirollValue(axisDofs2[index].Antiroll, minValue, maxValue);
            }
        }

        private bool IsAntiRollEnabledForForce(string force)
        {
            for (var index = 0; index < 48; ++index)
            {
                if (_axisDofs[index].Force == force && _axisDofs[index].Antiroll != 0)
                {
                    return true;
                }
            }

            return false;
        }

        public double GetDofValue(
            int index,
            double pitch,
            double roll,
            double yaw,
            double heave,
            double sway,
            double surge,
            double extra1,
            double extra2,
            double extra3)
        {
            var num1 = 0.0;
            AxisDofData[] axisDofDataArray;
            ClsFilters[] clsFiltersArray;
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

            switch (axisDofDataArray[index].Force)
            {
                case nameof(pitch):
                    num1 = pitch;
                    break;
                case nameof(roll):
                    num1 = roll;
                    break;
                case nameof(yaw):
                    num1 = yaw;
                    break;
                case nameof(heave):
                    num1 = heave;
                    break;
                case nameof(sway):
                    num1 = sway;
                    break;
                case nameof(surge):
                    num1 = surge;
                    break;
                case "Ex1":
                    num1 = extra1;
                    break;
                case "Ex2":
                    num1 = extra2;
                    break;
                case "Ex3":
                    num1 = extra3;
                    break;
            }

            var startDeadZoneToZero = false;
            var num2 = clsFiltersArray[index].dead_zone(num1, ref startDeadZoneToZero);
            if (!startDeadZoneToZero)
            {
                var plusMinus = false;
                num2 = clsFiltersArray[index].SmoothingPlusMinus(num2, ref plusMinus);
                if (!plusMinus)
                {
                    var startAntiRoll = false;
                    num2 = clsFiltersArray[index].Antiroll(num2, ref startAntiRoll);
                    if (!startAntiRoll)
                    {
                        var num3 = clsFiltersArray[index].Smoothing(num2);
                        num2 = clsFiltersArray[index].smoothing_sim(num3);
                    }
                }
            }

            switch (axisDofDataArray[index].Force)
            {
                case nameof(pitch):
                    num2 = Function(num2, _minPitch, _maxPitch);
                    break;
                case nameof(roll):
                    num2 = Function(num2, _minRoll, _maxRoll);
                    break;
                case nameof(yaw):
                    num2 = Function(num2, _minYaw, _maxYaw);
                    break;
                case nameof(heave):
                    num2 = Function(num2, _minHeave, _maxHeave);
                    break;
                case nameof(sway):
                    num2 = Function(num2, _minSway, _maxSway);
                    break;
                case nameof(surge):
                    num2 = Function(num2, _minSurge, _maxSurge);
                    break;
                case "Ex1":
                    num2 = Function(num2, _minExtra1, _maxExtra1);
                    break;
                case "Ex2":
                    num2 = Function(num2, _minExtra2, _maxExtra2);
                    break;
                case "Ex3":
                    num2 = Function(num2, _minExtra3, _maxExtra3);
                    break;
            }

            if (axisDofDataArray[index].Dir)
            {
                num2 *= -1.0;
            }

            return num2 * (0.01 * axisDofDataArray[index].Proc);

            static double Function(double value, double valueMin, double valueMax)
            {
                return value < 0.0 ? valueMin != 0.0 ? value / -valueMin : 0.0 :
                    valueMax != 0.0 ? value / valueMax : 0.0;
            }
        }

        public void ProcessingData(double pitch, double roll, double yaw, double surge, double sway, double heave,
            double extra1, double extra2, double extra3, double wind)
        {
            if (pitch > _maxPitch && !IsAntiRollEnabledForForce(nameof(pitch)))
            {
                pitch = _maxPitch;
            }

            if (pitch < _minPitch && !IsAntiRollEnabledForForce(nameof(pitch)))
            {
                pitch = _minPitch;
            }

            if (roll > _maxRoll && !IsAntiRollEnabledForForce(nameof(roll)))
            {
                roll = _maxRoll;
            }

            if (roll < _minRoll && !IsAntiRollEnabledForForce(nameof(roll)))
            {
                roll = _minRoll;
            }

            if (yaw > _maxYaw && !IsAntiRollEnabledForForce(nameof(yaw)))
            {
                yaw = _maxYaw;
            }

            if (yaw < _minYaw && !IsAntiRollEnabledForForce(nameof(yaw)))
            {
                yaw = _minYaw;
            }

            if (surge > _maxSurge && !IsAntiRollEnabledForForce(nameof(surge)))
            {
                surge = _maxSurge;
            }

            if (surge < _minSurge && !IsAntiRollEnabledForForce(nameof(surge)))
            {
                surge = _minSurge;
            }

            if (sway > _maxSway && !IsAntiRollEnabledForForce(nameof(sway)))
            {
                sway = _maxSway;
            }

            if (sway < _minSway && !IsAntiRollEnabledForForce(nameof(sway)))
            {
                sway = _minSway;
            }

            if (heave > _maxHeave && !IsAntiRollEnabledForForce(nameof(heave)))
            {
                heave = _maxHeave;
            }

            if (heave < _minHeave && !IsAntiRollEnabledForForce(nameof(heave)))
            {
                heave = _minHeave;
            }

            if (extra1 > _maxExtra1 && !IsAntiRollEnabledForForce("Ex1"))
            {
                extra1 = _maxExtra1;
            }

            if (extra1 < _minExtra1 && !IsAntiRollEnabledForForce("Ex1"))
            {
                extra1 = _minExtra1;
            }

            if (extra2 > _maxExtra2 && !IsAntiRollEnabledForForce("Ex2"))
            {
                extra2 = _maxExtra2;
            }

            if (extra2 < _minExtra2 && !IsAntiRollEnabledForForce("Ex2"))
            {
                extra2 = _minExtra2;
            }

            if (extra3 > _maxExtra3 && !IsAntiRollEnabledForForce("Ex3"))
            {
                extra3 = _maxExtra3;
            }

            if (extra3 < _minExtra3 && !IsAntiRollEnabledForForce("Ex3"))
            {
                extra3 = _minExtra3;
            }

            if (_axisDofs == null)
            {
                return;
            }

            _axis[0] = GetDofValue(0, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(1, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(2, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(3, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(4, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(5, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(48, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(49, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(50, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(51, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(52, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(53, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3);
            _axis[1] = GetDofValue(6, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(7, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(8, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(9, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(10, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(11, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(54, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(55, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(56, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(57, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(58, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(59, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3);
            _axis[2] = GetDofValue(12, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(13, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(14, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(15, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(16, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(17, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(60, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(61, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(62, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(63, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(64, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(65, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3);
            _axis[3] = GetDofValue(18, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(19, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(20, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(21, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(22, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(23, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(66, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(67, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(68, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(69, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(70, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(71, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3);
            _axis[4] = GetDofValue(24, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(25, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(26, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(27, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(28, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(29, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(72, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(73, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(74, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(75, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(76, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(77, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3);
            _axis[5] = GetDofValue(30, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(31, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(32, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(33, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(34, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(35, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(78, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(79, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(80, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(81, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(82, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(83, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3);
            _axis[6] = GetDofValue(36, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(37, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(38, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(39, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(40, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(41, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(84, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(85, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(86, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(87, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(88, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(89, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3);
            _axis[7] = GetDofValue(42, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(43, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(44, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(45, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(46, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(47, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(90, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(91, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(92, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(93, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(94, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3) +
                       GetDofValue(95, pitch, roll, yaw, heave, sway, surge, extra1, extra2, extra3);
            _axis[8] = _typeWind != 0 ? _coefWind : wind * (0.01 * _procWind) * (_coefWind / 100.0);
        }

        public double GetAxis(int index, ref bool absValue)
        {
            absValue = _absValues[index];
            return !absValue ? _axis[index] <= 1.0 ? _axis[index] >= -1.0 ? _axis[index] : -1.0 : 1.0 :
                _axis[index] <= byte.MaxValue ? _axis[index] >= 0.0 ? _axis[index] : 0.0 : byte.MaxValue;
        }

        public double GetAxis9()
        {
            return _axis[8] > 999.0 ? 999.0 : _axis[8];
        }
    }

    public class ClsFilters
    {
        private double _antirollMaxValue;
        private double _antirollMinValue;
        private double _antirollValue;
        private double _deadToZeroInterval;
        private double _deadToZeroTime;
        private double _deadzoneTozeroValue;
        private double _deadzoneValue;
        private bool _first = true;
        private double _inputValueAntirollSim;
        private readonly double _inputValueDeadzoneTozeroSim = 0.0;
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