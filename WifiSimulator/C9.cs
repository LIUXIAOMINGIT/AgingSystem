using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsyncSocket;
using Cmd;

namespace WifiSimulator
{
    public class C9 : Graseby
    {

        public C9(AsyncClient client)
            : base(client)
        {

        }

        public override List<byte> CreateSinglePumpAlarm(byte chanel, uint alarm)
        {
            List<byte> single = new List<byte>();
            single.Add(chanel);
            //泵电源状态
            single.Add(0x00);
            //报警
            single.Add((byte)alarm);
            single.Add(0);
            single.Add(0);
            single.Add(0);
            return single;
        }


        public override List<byte> CreatePumpAlarmPackageEx(List<int> pumpIndexs, uint alarm, int pumpCount = 6)
        {
            List<byte> package = new List<byte>();
            package.Add(0x00);
            package.Add(0x07);
            package.Add(0x06);

            //数据长度
            package.Add((byte)(pumpCount * 6));
            package.Add(0x00);

            //数据长度取反
            package.Add((byte)((byte)0xFF - (byte)(pumpCount * 6)));
            package.Add(0xFF);

            for (int i = 0; i < pumpCount; i++)
            {
                if (pumpIndexs.Contains(i + 1))
                    package.AddRange(CreateSinglePumpAlarm((byte)(i + 1), alarm));
                else
                    package.AddRange(CreateSinglePumpAlarm((byte)(i + 1), 0));
            }
            //校验码
            package.Add(0x02);
            package.Add(0x00);
            package.Add(0x00);
            package.Add(0xEE);
            return package;
        }

        public override void SendAlarm()
        {
            base.SendAlarm();
        }

        /// <summary>
        /// 无报警
        /// </summary>
        public override void SendNoAlarm()
        {
            List<byte> debugBytes = CreatePumpAlarmPackageEx(m_PumpIndexs, 0x00000000, m_PumpCount);
            m_Client.Send(Common.Hex2Char(debugBytes.ToArray()));
        }

        /// <summary>
        /// 低位报警
        /// </summary>
        public override void SendLowAlarm()
        {
            List<byte> debugBytes = CreatePumpAlarmPackageEx(m_PumpIndexs, 20, m_PumpCount);
            m_Client.Send(Common.Hex2Char(debugBytes.ToArray()));
        }

        /// <summary>
        /// 低电压报警
        /// </summary>
        public override void SendLowVoltage()
        {
            List<byte> debugBytes = CreatePumpAlarmPackageEx(m_PumpIndexs, 12, m_PumpCount);
            m_Client.Send(Common.Hex2Char(debugBytes.ToArray()));
        }

        /// <summary>
        /// 耗尽
        /// </summary>
        public override void SendDeplete()
        {
            List<byte> debugBytes = CreatePumpAlarmPackageEx(m_PumpIndexs, 15, m_PumpCount);
            m_Client.Send(Common.Hex2Char(debugBytes.ToArray()));
        }
    }
}
