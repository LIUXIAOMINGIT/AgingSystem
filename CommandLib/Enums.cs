﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cmd
{
    /// <summary>
    /// 注射器品牌
    /// </summary>
    public enum SyringeBrand : byte
    {
        None = 0,
        Brand1,
        Brand2,
        Brand3,
        Brand4,
        Brand5,
        Brand6,
        Brand7,
        Brand8,
        Brand9,
        Brand10,
        Brand11,
        Brand12,
        Brand13,
        CustomBrand1,
        CustomBrand2,
        CustomBrand3,
        CustomBrand4,
        CustomBrand5,
        CustomBrand6,
    }

    /// <summary>
    /// 注射器尺寸
    /// </summary>
    public enum SyringeSize : byte
    {
        Five = 0,
        Ten = 1,
        Twenty = 2,
        Thirty = 3,
        Fifty = 4,
        Unknown = 0xFF,
    }

    /// <summary>
    /// ACK
    /// </summary>
    public enum ACKID : byte
    {
        CommandUndefine = 0x01, //命令未定义产品不支持命令
        CommandErrorFormat = 0x02, //命令格式错误,命令中包含的字段不完整
        CommandErrorParameter = 0x03, //字段参数格式错误,字段中参数不符合要求
        CommandErrorPackage = 0x04, //包格式错误,包的格式不符合要求
        Unknown = 0xFF,
    }


    /// <summary>
    /// PressureLevel
    /// </summary>
    public enum PressureLevel : byte
    {
        N = 0x00,
        L = 0x01,
        C = 0x02,
        H = 0x03,
        Unknown = 0xFF,
    }

    /// <summary>
    /// Scale Value
    /// </summary>
    public enum ScaleValue : byte
    {
        None = 0x00,
        Ten = 0x01,
        Hundred = 0x02,
        Thousand = 0x03,
        TenThousand = 0x04,
    }

    public enum ProductID : byte
    {
        GrasebyC8   = 0x00,
        GrasebyC9   = 0x01,
        GrasebyC6   = 0x04,
        GrasebyF6   = 0x05,
        Graseby1200 = 0x06,
        Graseby1200En = 0xFF-0x06,
        GrasebyF8 = 0x03,//F8默认情况下只用1通道，双通道启动先不考虑 GrasebyF8 = 0x07 一道
        //GrasebyF8_2 = 0x08,
        GrasebyC6T  = 0x09,
        Graseby2000 = 0x0A,
        Graseby2100 = 0x0B,
        WZ50C6      = 0x04,
        WZS50F6     = 0x05,
        WZ50C6T     = 0x09,
        Unknow      = 0xFF,
    }

    public enum CustomProductID:byte
    {
        GrasebyC6,
        WZ50C6,
        GrasebyC6T,
        WZ50C6T,
        GrasebyF6_Single,
        Graseby2000,
        Graseby2100,
        GrasebyC8,
        Graseby1200,
        Graseby1200En,
        GrasebyF6_Double,
        WZS50F6_Single,
        GrasebyC9,
        WZS50F6_Double,
        GrasebyF8,
        Unknow = 0xFF,
    }

    public class ProductIDConvertor
    {
        public static ProductID Custom2ProductID(CustomProductID cid)
        {
            ProductID pid = ProductID.Unknow;
            switch(cid)
            {
                case CustomProductID.GrasebyC6:
                    pid = ProductID.GrasebyC6;
                    break;
                case CustomProductID.WZ50C6:
                    pid = ProductID.WZ50C6;
                    break;
                case CustomProductID.GrasebyC6T:
                    pid = ProductID.GrasebyC6T;
                    break;
                case CustomProductID.GrasebyF6_Single:
                    pid = ProductID.GrasebyF6;
                    break;
                case CustomProductID.Graseby2000:
                    pid = ProductID.Graseby2000;
                    break;
                case CustomProductID.Graseby2100:
                    pid = ProductID.Graseby2100;
                    break;
                case CustomProductID.GrasebyC8:
                    pid = ProductID.GrasebyC8;
                    break;
                case CustomProductID.Graseby1200:
                    pid = ProductID.Graseby1200;
                    break;
                case CustomProductID.Graseby1200En:
                    pid = ProductID.Graseby1200En;
                    break;
                case CustomProductID.GrasebyF6_Double:
                    pid = ProductID.GrasebyF6;
                    break;
                case CustomProductID.WZS50F6_Single:
                    pid = ProductID.WZS50F6;
                    break;
                case CustomProductID.WZS50F6_Double:
                    pid = ProductID.WZS50F6;
                    break;
                case CustomProductID.GrasebyF8:
                    pid = ProductID.GrasebyF8;
                    break;
                case CustomProductID.GrasebyC9:
                    pid = ProductID.GrasebyC9;
                    break;
                default:
                    pid = ProductID.Unknow;
                    break;
            }
            return pid;
        }

        public static string CustomProductID2Name(CustomProductID cid)
        {
            string name = string.Empty;
            switch (cid)
            {
                case CustomProductID.GrasebyC6:
                    name = "GrasebyC6";
                    break;
                case CustomProductID.WZ50C6:
                    name = "WZ50C6";
                    break;
                case CustomProductID.GrasebyC6T:
                    name = "GrasebyC6T";
                    break;
                case CustomProductID.GrasebyF6_Single:
                    name = "GrasebyF6单道";
                    break;
                case CustomProductID.Graseby2000:
                    name = "Graseby2000";
                    break;
                case CustomProductID.Graseby2100:
                    name = "Graseby2100";
                    break;
                case CustomProductID.GrasebyC8:
                    name = "GrasebyC8";
                    break;
                case CustomProductID.Graseby1200:
                    name = "Graseby1200中文";
                    break;
                case CustomProductID.Graseby1200En:
                    name = "Graseby1200英文";
                    break;
                case CustomProductID.GrasebyF6_Double:
                    name = "GrasebyF6双道";
                    break;
                case CustomProductID.WZS50F6_Single:
                    name = "WZS50F6单道";
                    break;
                case CustomProductID.WZS50F6_Double:
                    name = "WZS50F6双道";
                    break;
                case CustomProductID.GrasebyF8:
                    name = "GrasebyF8";
                    break;
                case CustomProductID.GrasebyC9:
                    name = "GrasebyC9";
                    break;
                default:
                    name = string.Empty;
                    break;
            }
            return name;
        }

        public static CustomProductID Name2CustomProductID(string name)
        {
            CustomProductID cid = CustomProductID.Unknow;
            switch (name)
            {
                case "GrasebyC6":
                    cid = CustomProductID.GrasebyC6;
                    break;
                case "WZ50C6":
                    cid = CustomProductID.WZ50C6;
                    break;
                case "GrasebyC6T":
                    cid = CustomProductID.GrasebyC6T;
                    break;
                case "GrasebyF6单道":
                    cid = CustomProductID.GrasebyF6_Single;
                    break;
                case "Graseby2000":
                    cid = CustomProductID.Graseby2000;
                    break;
                case "Graseby2100":
                    cid = CustomProductID.Graseby2100;
                    break;
                case "GrasebyC8":
                    cid = CustomProductID.GrasebyC8;
                    break;
                case "Graseby1200中文":
                    cid = CustomProductID.Graseby1200;
                    break;
                case "Graseby1200英文":
                    cid = CustomProductID.Graseby1200En;
                    break;
                case "GrasebyF6双道":
                    cid = CustomProductID.GrasebyF6_Double;
                    break;
                case "WZS50F6单道":
                    cid = CustomProductID.WZS50F6_Single;
                    break;
                case "WZS50F6双道":
                    cid = CustomProductID.WZS50F6_Double;
                    break;
                case "GrasebyF8":
                    cid = CustomProductID.GrasebyF8;
                    break;
                case "GrasebyC9":
                    cid = CustomProductID.GrasebyC9;
                    break;
                default:
                    cid = CustomProductID.Unknow;
                    break;
            }
            return cid;
        }

        public static List<string> GetAllCustomProductIDName()
        {
            List<string> names = new List<string>();
            names.Add("GrasebyC6");
            names.Add("WZ50C6");
            names.Add("GrasebyC6T");
            names.Add("GrasebyF6单道");
            names.Add("Graseby2000");
            names.Add("Graseby2100");
            names.Add("GrasebyC8");
            names.Add("Graseby1200中文");
            names.Add("Graseby1200英文");
            names.Add("GrasebyF6双道");
            names.Add("WZS50F6单道");
            //names.Add("GrasebyC9");
            names.Add("WZS50F6双道");
            names.Add("GrasebyF8");
            return names;
        }
    }

   

    /// <summary>
    /// 文件传输相关枚举值
    /// </summary>
    public enum FileID : byte
    {
        MainPumpFirmware = 0x00,
        UIPumpFirmware = 0x01,
        LogoFile = 0x02,
        FontLib = 0x03,
        Unknow = 0xFF,
    }

    /// <summary>
    /// 文件传输成功或失败
    /// </summary>
    public enum SendFileResult : byte
    {
        Fail = 0x00,
        Success = 0x01,
    }

    /// <summary>
    /// 泵状态
    /// </summary>
    public enum PumpStatus : byte
    {
        Off     = 0x00,
        Stop    = 0x01,
        Run     = 0x02,
        Pause   = 0x03,
        Bolus   = 0x04,
        Purging = 0x05,
        KVO     = 0x06,
    }

     /// <summary>
    /// 泵使用什么样的电源
    /// </summary>
    public enum PumpPowerStatus : byte
    {
        AC         = 0x00,
        Battery    = 0x01,
        DC         = 0x02,
        External   = 0x03,
    }

    /// <summary>
    /// C9专用
    /// </summary>
    public enum C9PumpPowerStatus : byte
    {
        External = 0x00,
        ExternalBattery = 0x01,
        Battery = 0x02,
    }

    /// <summary>
    /// 剂量单位
    /// </summary>
    public enum DoseUnit : byte
    {
        UgKgMin = 0x00,
        MgKgH   = 0x01,
    }

    /// <summary>
    /// 开关选项
    /// </summary>
    public enum SwitchOption : byte
    {
        None                  = 0x00,
        DrugName              = 0x01,
        EditRate              = 0x02,
        LockedAffterStarted   = 0x03,
        SaveLastSolusion      = 0x04,
        NightMode             = 0x05,
    }

    /// <summary>
    /// 设置单个字节数据选项
    /// </summary>
    public enum SingleByteOption : byte
    {
        None = 0x00,
        CompleteInfusionAlarmTime = 0x01,
        AlarmVolume = 0x02,
        AlarmSoundType = 0x03,
        ScreenBrightness = 0x04,
    }

    public enum OcclusionLevel: byte
    {
        L = 0x00,
        C = 0x01,
        H = 0x02,
        N = 0x03,
    }

    public enum C9OcclusionLevel : byte
    {
        Level1 = 0x00,
        Level2 = 0x01,
        Level3 = 0x02,
        Level4 = 0x03,
        Level5 = 0x04,
    }

    public enum EAgingStatus
    {
        Unknown = 0,
        Waiting = 1,
        PowerOn = 2,
        Charging = 3,
        DisCharging = 4,
        Recharging = 5,
        AgingComplete = 6,
        Alarm = 7,
    }

}
