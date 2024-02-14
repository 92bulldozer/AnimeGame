using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace EJ
{
   
    public static class ExtensionMethods
    {
        public static string ToCommaString(this BigInteger b)
        {
            if (b == 0)
            {
                return "0";
            }
            
            return $"{b:#,###}";
        }
       
        public static string ToUnitString(this BigInteger b)
        {
            if (b < 1000)
            {
                return b.ToString();
            }

            var unitNumber = Mathf.FloorToInt((float)BigInteger.Log10(b)) / 3;
            var significand = Mathf.FloorToInt((float)BigInteger.Divide(b, BigInteger.Pow(10, unitNumber * 3 - 2))) / 100f;
            //var unit = (char)('a' + unitNumber - 1);
            
            return $"{significand:N1} {SwitchEnglishGoldUnit(unitNumber)}";
        }
        
        public static string ToUnitString(this BigInteger b,int space)
        {
            if (b < 1000)
            {
                return b.ToString();
            }

            var unitNumber = Mathf.FloorToInt((float)BigInteger.Log10(b)) / 3;
            var significand = Mathf.FloorToInt((float)BigInteger.Divide(b, BigInteger.Pow(10, unitNumber * 3 - 2))) / 100f;
            //var unit = (char)('a' + unitNumber - 1);
            
            return $"{significand:N1} {SwitchEnglishGoldUnit(unitNumber)}";
        }
        
        public static string ToDecimalPointString(this BigInteger b,int decimalNumber)
        {
            if (b == 0)
            {
                return "0";
            }

            if (decimalNumber == 1)
            {
                return $"{Math.Truncate((double) b * Math.Pow(10, decimalNumber)) * Math.Pow(10, -(decimalNumber * 2)):F1}";
            }
            if (decimalNumber == 2)
            {
                return $"{Math.Truncate((double) b * Math.Pow(10, decimalNumber)) * Math.Pow(10, -(decimalNumber * 2)):F2}";
            }
            if (decimalNumber == 3)
            {
                return $"{Math.Truncate((double) b * Math.Pow(10, decimalNumber)) * Math.Pow(10, -(decimalNumber * 2)):F3}";
            }
            
             return $"{Math.Truncate((double) b * Math.Pow(10, decimalNumber)) * Math.Pow(10, -(decimalNumber * 2)):F2}";
            
        }

        
        public static string ToUnitStringKR(this BigInteger b)
        {
            if (b < 10000)
            {
                return b.ToString();
            } 
            var unitNumber = Mathf.FloorToInt((float)BigInteger.Log10(b)) / 4; 
            var significand = Mathf.FloorToInt((float)BigInteger.Divide(b,
                BigInteger.Pow(10, unitNumber * 4 - 3))) / 1000f; 
            //var unit = (char)('A' + unitNumber - 1); 
            var unit = SwitchKoreanGoldUnit(unitNumber);
            //Debug.Log(b);
            
            //return $"{significand:N1}   {unit}";
            return $"{significand:N0} {unit}";
           
        }
        
        public static string ToUnitStringKR(this BigInteger b,int space)
        {
            if (b < 10000)
            {
                return b.ToString();
            } 
            var unitNumber = Mathf.FloorToInt((float)BigInteger.Log10(b)) / 4; 
            var significand = Mathf.FloorToInt((float)BigInteger.Divide(b,
                BigInteger.Pow(10, unitNumber * 4 - 3))) / 1000f; 
            //var unit = (char)('A' + unitNumber - 1); 
            var unit = SwitchKoreanGoldUnit(unitNumber);
            //Debug.Log(b);

            switch (space)
            {
                case 1:
                    return $"{Math.Truncate(significand*10)*0.1} {unit}";
                case 2:
                    return $"{Math.Truncate(significand*100)*0.01} {unit}";
                default:
                    return $"{significand:N0} {unit}";
            }
        }
        
        public static string ToUnitStringKREJ(this BigInteger b,int space)
        {
            if (b < 10000)
            {
                return b.ToString();
            } 
            var unitNumber = Mathf.FloorToInt((float)BigInteger.Log10(b)) / 4; 
            var significand = Mathf.FloorToInt((float)BigInteger.Divide(b,
                BigInteger.Pow(10, unitNumber * 4 - 3))) / 1000f; 
            //var unit = (char)('A' + unitNumber - 1); 
            var unit = SwitchKoreanGoldUnit(unitNumber);
            //Debug.Log(b);

            switch (space)
            {
                case 1:
                    return $"{Math.Truncate(significand*10)*0.1}{unit}";
                case 2:
                    return $"{Math.Truncate(significand*100)*0.01}{unit}";
                default:
                    return $"{significand:N0} {unit}";
            }
        }


        
        
        public static void Log(this object value)
        {
#if UNITY_EDITOR
            Debug.Log(value.ToString());
#endif
        }

        public static IEnumerator Start(this IEnumerator coroutine, MonoBehaviour behaviour)
        {
            behaviour.StartCoroutine(coroutine);


            return coroutine;
        }

        public static void Stop(this IEnumerator coroutine, MonoBehaviour behaviour)
        {
            behaviour.StopCoroutine(coroutine);
        }

#if UNITY_EDITOR
        public static void ClearLogConsole()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
            Type logEntries = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo clearConsoleMethod = logEntries.GetMethod("Clear");
            clearConsoleMethod.Invoke(new object(), null);
        }
#endif

        public static string SwitchKoreanGoldUnit(int unitNumber)
        {
            switch (unitNumber)
            {
                case 1:
                    return "만";
                case 2:
                    return "억";
                case 3:
                    return "조";
                case 4:
                    return "경";
                case 5:
                    return "해";
                case 6:
                    return "자";
                case 7:
                    return "양";
                case 8:
                    return "구";
                case 9:
                    return "한";
                case 10:
                    return "정";
                case 11:
                    return "재";
                case 12:
                    return "극";
                case 13:
                    return "항하사";
                case 14:
                    return "아승기";
                case 15:
                    return "나유타";
                case 16:
                    return "불가사의";
                case 17:
                    return "무량대수";
                case 18:
                    return "극의";
                case 19:
                    return "윤";
                case 20:
                    return "윤회";
                case 21:
                    return "마의";
                case 22:
                    return "현무";
                case 23:
                    return "백호";
                case 24:
                    return "주작";
                case 25:
                    return "무희";
                case 26:
                    return "무진";
                default:
                    return "무한대";
                
            }
        }
        
        public static string SwitchEnglishGoldUnit(int unitNumber)
        {
            switch (unitNumber)
            {
                case 1:
                    return "a";
                case 2:
                    return "b";
                case 3:
                    return "c";
                case 4:
                    return "d";
                case 5:
                    return "e";
                case 6:
                    return "f";
                case 7:
                    return "g";
                case 8:
                    return "h";
                case 9:
                    return "i";
                case 10:
                    return "j";
                case 11:
                    return "k";
                case 12:
                    return "l";
                case 13:
                    return "m";
                case 14:
                    return "n";
                case 15:
                    return "o";
                case 16:
                    return "p";
                case 17:
                    return "q";
                case 18:
                    return "r";
                case 19:
                    return "s";
                case 20:
                    return "t";
                case 21:
                    return "u";
                case 22:
                    return "v";
                case 23:
                    return "w";
                case 24:
                    return "x";
                case 25:
                    return "y";
                case 26:
                    return "z";
                case 27:
                    return "A";
                case 28:
                    return "B";
                case 29:
                    return "C";
                case 30:
                    return "D";
                case 31:
                    return "E";
                case 32:
                    return "F";
                case 33:
                    return "G";
                case 34:
                    return "H";
                case 35:
                    return "I";
                case 36:
                    return "J";
                case 37:
                    return "K";
                case 38:
                    return "L";
                case 39:
                    return "M";
                case 40:
                    return "N";
                case 41:
                    return "O";
                case 42:
                    return "P";
                case 43:
                    return "Q";
                case 44:
                    return "R";
                case 45:
                    return "S";
                case 46:
                    return "T";
                case 47:
                    return "U";
                case 48:
                    return "V";
                case 49:
                    return "W";
                case 50:
                    return "X";
                case 51:
                    return "Y";
                case 52:
                    return "Z";
                case 53:
                    return "aa";
                case 54:
                    return "bb";
                case 55:
                    return "cc";
                case 56:
                    return "dd";
                case 57:
                    return "ee";
                case 58:
                    return "ff";
                case 59:
                    return "gg";
                case 60:
                    return "hh";
                case 61:
                    return "ii";
                case 62:
                    return "jj";
                case 63:
                    return "kk";
                case 64:
                    return "ll";
                case 65:
                    return "mm";
                case 66:
                    return "nn";
                case 67:
                    return "oo";
                case 68:
                    return "pp";
                case 69:
                    return "qq";
                case 70:
                    return "rr";
                case 71:
                    return "ss";
                case 72:
                    return "tt";
                case 73:
                    return "uu";
                case 74:
                    return "vv";
                case 75:
                    return "ww";
                case 76:
                    return "xx";
                case 77:
                    return "yy";
                case 78:
                    return "zz";
                default:
                    return "INF";
                
            }
        }        
        
    }
    
    

    
    public static class Dods_ChanceMaker
    {
        public static bool GetThisChanceResult(float Chance)
        {
            if (Chance < 0.0000001f)
            {
                Chance = 0.0000001f;
            }

            bool Success = false;
            int RandAccuracy = 10000000;
            float RandHitRange = Chance * RandAccuracy;
            int Rand = UnityEngine.Random.Range(1, RandAccuracy+1);
            if (Rand <= RandHitRange)
            {
                Success = true;
            }
            return Success;
        }

        public static bool GetThisChanceResult_Percentage(float Percentage_Chance)
        {
            if (Percentage_Chance < 0.0000001f)
            {
                Percentage_Chance = 0.0000001f;
            }

            Percentage_Chance = Percentage_Chance / 100;

            bool Success = false;
            int RandAccuracy = 10000000;
            float RandHitRange = Percentage_Chance * RandAccuracy;
            int Rand = UnityEngine.Random.Range(1, RandAccuracy+1);
            if (Rand <= RandHitRange)
            {
                Success = true;
            }
            return Success;
        }
    }
}