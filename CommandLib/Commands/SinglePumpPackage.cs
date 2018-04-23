using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cmd
{
    public class SinglePumpPackage
    {
        protected byte m_Chanel = 0;//此处通道号不是按位定义，而是自然数1~8
        protected PowerInfo m_Power;
        protected AlarmInfo m_Alarm;

        public byte Chanel
        {
            get { return m_Chanel; }
        }
        public AlarmInfo Alarm
        {
            get { return m_Alarm; }
        }

        public PowerInfo Power
        {
            get { return m_Power; }
        }

        public SinglePumpPackage() { }

        public SinglePumpPackage(byte[] data)
        {
            if (data.Length != 22)
            {
                Logger.Instance().ErrorFormat("单泵信息有误，长度不等于22字节,data={0}", data.Length);
            }
            else
            {
                m_Chanel = data[0];
                byte[] tempPower = new byte[9];
                Array.Copy(data, 1, tempPower, 0, 9);
                m_Power = new PowerInfo(tempPower);
                byte[] tempAlarm = new byte[12];
                Array.Copy(data, 10, tempAlarm, 0, 12);
                m_Alarm = new AlarmInfo(tempAlarm);
            }
        }

        public List<byte> GetBytes()
        {
            List<byte> buffer = new List<byte>();
            buffer.Add(m_Chanel);
            buffer.AddRange(m_Power.GetBytes());
            buffer.AddRange(m_Alarm.GetBytes());
            return buffer;
        }
    }

    public class SingleC9PumpPackage : SinglePumpPackage
    {
        protected C9PowerInfo m_C9Power;
        protected C9AlarmInfo m_C9Alarm;

        public C9AlarmInfo C9Alarm
        {
            get { return m_C9Alarm; }
        }

        public C9PowerInfo C9Power
        {
            get { return m_C9Power; }
        }

        public SingleC9PumpPackage() { }

        public SingleC9PumpPackage(byte[] data)
        {
            if (data.Length != 6)//C9的单泵数据包长度6
            {
                Logger.Instance().ErrorFormat("单泵信息有误，长度不等于6字节,data={0}", data.Length);
            }
            else
            {
                m_Chanel = data[0];
                m_C9Power = new C9PowerInfo(data[1]);
                byte[] tempAlarm = new byte[4];
                Array.Copy(data, 2, tempAlarm, 0, 4);
                m_C9Alarm = new C9AlarmInfo(tempAlarm);
            }
        }
    }

    public class PowerInfo
    {
        protected byte[] m_Head = new byte[] { 0x55, 0xAA };
        protected byte m_Length = 0x05;
        protected ProductID m_PID;
        protected byte m_Pump2PC = 0x00;
        protected byte m_MessageID = 0x58;
        protected byte m_AppDC = 0x01;
        protected PumpPowerStatus m_PowerStatus = PumpPowerStatus.External;
        protected byte m_CheckCode;

        public PumpPowerStatus PowerStatus
        {
            get { return m_PowerStatus; }
            set { m_PowerStatus = value; }
        }

        public PowerInfo() { }

        public PowerInfo(byte[] data)
        {
            if (data.Length != 9)
            {
                Logger.Instance().Error("PowerInfo实例化失败，没有9个字节！");
            }
            else
            {
                m_Head[0] = data[0];
                m_Head[1] = data[1];
                m_Length = data[2];
                //m_PID         =(ProductID)data[3];
                m_Pump2PC = data[4];
                m_MessageID = data[5];
                m_AppDC = data[6];
                if (Enum.IsDefined(typeof(PumpPowerStatus), data[7]))
                {
                    m_PowerStatus = (PumpPowerStatus)data[7];
                }
                else
                {
                    m_PowerStatus = PumpPowerStatus.External;
                }
                m_CheckCode = data[8];
            }
        }

        public List<byte> GetBytes()
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(m_Head);
            buffer.Add((byte)m_PID);
            buffer.Add(m_Pump2PC);
            buffer.Add(m_MessageID);
            buffer.Add(m_AppDC);
            buffer.Add((byte)m_PowerStatus);
            buffer.Add(m_CheckCode);
            return buffer;
        }
    }

    /// <summary>
    /// C9专用
    /// </summary>
    public class C9PowerInfo : PowerInfo
    {
        protected C9PumpPowerStatus m_C9PowerStatus = C9PumpPowerStatus.External;
        public C9PumpPowerStatus C9PowerStatus
        {
            get { return m_C9PowerStatus; }
            set { m_C9PowerStatus = value; }
        }

        public C9PowerInfo() { }

        public C9PowerInfo(byte data)
        {
            if (Enum.IsDefined(typeof(C9PumpPowerStatus), data))
                m_C9PowerStatus = (C9PumpPowerStatus)data;
            else
                m_C9PowerStatus = C9PumpPowerStatus.External;
        }
    }

    public class AlarmInfo
    {
        protected byte[] m_Head = new byte[] { 0x55, 0xAA };
        protected byte m_Length = 0x05;
        protected ProductID m_PID;
        protected byte m_Pump2PC = 0x00;
        protected byte m_MessageID = 0x57;
        protected byte m_AppDC = 0x04;
        protected uint m_Alarm = 0x00;
        protected byte m_CheckCode;

        public uint Alarm
        {
            get { return m_Alarm; }
            set { m_Alarm = value; }
        }

        public AlarmInfo() { }

        public AlarmInfo(byte[] data)
        {
            if (data.Length != 12)
            {
                Logger.Instance().Error("AlarmInfo实例化失败，没有12个字节！");
            }
            else
            {
                m_Head[0] = data[0];
                m_Head[1] = data[1];
                m_Length = data[2];
                //m_PID         =    (ProductID)data[3];
                m_Pump2PC = data[4];
                m_MessageID = data[5];
                m_AppDC = data[6];
                m_Alarm = (uint)(data[7] & 0x000000FF);
                m_Alarm += (uint)(data[8] << 8 & 0x0000FF00);
                m_Alarm += (uint)(data[9] << 16 & 0x00FF0000);
                m_Alarm += (uint)(data[10] << 24 & 0xFF000000);
                m_CheckCode = data[11];
            }
        }

        public List<byte> GetBytes()
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(m_Head);
            buffer.Add((byte)m_PID);
            buffer.Add(m_Pump2PC);
            buffer.Add(m_MessageID);
            buffer.Add(m_AppDC);
            buffer.Add((byte)(m_Alarm & 0x000000FF));
            buffer.Add((byte)(m_Alarm >> 8 & 0x000000FF));
            buffer.Add((byte)(m_Alarm >> 16 & 0x000000FF));
            buffer.Add((byte)(m_Alarm >> 24 & 0x000000FF));
            buffer.Add(m_CheckCode);
            return buffer;
        }

    }

    /// <summary>
    /// C9报警ID列表
    /// </summary>
    public class C9AlarmInfo : AlarmInfo
    {
        protected List<uint> m_AlarmList = new List<uint>();

        public List<uint> AlarmList
        {
            get { return m_AlarmList; }
            set { m_AlarmList = value; }
        }

        public C9AlarmInfo() { }

        public C9AlarmInfo(byte[] data)
        {
            if (data.Length != 4)
            {
                Logger.Instance().Error("C9AlarmInfo实例化失败，没有4个字节！");
            }
            else
            {
                m_AlarmList.Add(data[0]);
                m_AlarmList.Add(data[1]);
                m_AlarmList.Add(data[2]);
                m_AlarmList.Add(data[3]);
            }
        }
    }
}
