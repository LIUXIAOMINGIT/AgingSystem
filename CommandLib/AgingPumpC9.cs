using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cmd
{
    /// <summary>
    /// 老化C9泵类，主要存放正在老化的泵信息
    /// </summary>
    public class AgingPumpC9 : AgingPump
    {
        private List<UInt32> m_AlarmList = new List<uint>();//C9的报警ID是32位，共有36个种类

        /// <summary>
        /// C9报警ID列表
        /// </summary>
        public List<UInt32> AlarmList
        {
            get { return m_AlarmList; }
            set { m_AlarmList = value; }
        }

        public AgingPumpC9()
        {
        }

        /// <summary>
        /// 添加一条新的ID
        /// </summary>
        /// <param name="alarmID"></param>
        public void AddAlarm(uint alarmID)
        {
            if (m_AlarmList == null)
                m_AlarmList = new List<uint>();
            if (!m_AlarmList.Contains(alarmID))
                m_AlarmList.Add(alarmID);
        }

        /// <summary>
        /// 添加一组报警信息，C9泵可能同时出现多个报警
        /// </summary>
        /// <param name="alarmIDs"></param>
        public void AddAlarm(List<uint> alarmIDs)
        {
            if (alarmIDs == null || alarmIDs.Count == 0)
                return;
            if (m_AlarmList == null)
                m_AlarmList = new List<uint>();
            foreach (var id in alarmIDs)
            {
                if (!m_AlarmList.Contains(id))
                    m_AlarmList.Add(id);
            }
        }

        public override string GetAlarmString()
        {
            CustomProductID cid = ProductIDConvertor.Name2CustomProductID(m_PumpType);
            if (cid == CustomProductID.Unknow)
            {
                Logger.Instance().ErrorFormat("泵类型转换出错，不支持的类型 PumpType ={0}", m_PumpType);
                return string.Empty;
            }
            ProductID pid = ProductIDConvertor.Custom2ProductID(cid);
            Hashtable alarmMetrix = null;
            #region //查询所属报警的映射表
            switch (pid)
            {
                case ProductID.GrasebyC9:
                    alarmMetrix = AlarmMetrix.Instance().AlarmMetrixC9;
                    break;
                default:
                    break;
            }
            #endregion
            if (alarmMetrix == null)
                return "";
            StringBuilder sb = new StringBuilder();
            int iCount = 0;
            foreach (var id in m_AlarmList)
            {
                if(alarmMetrix.ContainsKey(id))
                {
                    sb.Append(alarmMetrix[id] as string);
                    sb.Append(";");
                    ++iCount;
                    if (iCount % 2 == 0)
                        sb.Append("\n");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 查找某C9类泵所有出现的报警ID对应的首次发生时间
        /// </summary>
        /// <returns></returns>
        public override string GetAlarmStringAndOcurredTime()
        {
            CustomProductID cid = ProductIDConvertor.Name2CustomProductID(m_PumpType);
            if (cid == CustomProductID.Unknow)
            {
                Logger.Instance().ErrorFormat("泵类型转换出错，不支持的类型 PumpType ={0}", m_PumpType);
                return string.Empty;
            }
            ProductID pid = ProductIDConvertor.Custom2ProductID(cid);
            Hashtable alarmMetrix = null;
            #region //查询所属报警的映射表
            switch (pid)
            {
                case ProductID.GrasebyC9:
                    alarmMetrix = AlarmMetrix.Instance().AlarmMetrixC9;
                    break;
                default:
                    break;
            }
            #endregion
            if (alarmMetrix == null)
                return "";
            StringBuilder sb = new StringBuilder();
            DateTime ocurredTime = DateTime.MinValue;
            foreach (var id in m_AlarmList)
            {
                if (id > 0)
                {
                    if (m_AlarmOccurredTime.ContainsKey(id))
                    {
                        ocurredTime = (DateTime)m_AlarmOccurredTime[id];
                        if (ocurredTime.Year > 2000)
                        {
                            sb.Append(ocurredTime.ToString("yyyy-MM-dd HH:mm:ss"));
                            sb.Append("->");
                        }
                    }
                    sb.Append(alarmMetrix[id] as string);
                    sb.Append("\n");
                }
            }
            return sb.ToString().TrimEnd('\n');
        }

        /// <summary>
        /// C9是否通过测试
        /// </summary>
        /// <returns></returns>
        public override bool IsPass()
        {
            CustomProductID cid = ProductIDConvertor.Name2CustomProductID(m_PumpType);
            if (cid == CustomProductID.Unknow)
            {
                Logger.Instance().ErrorFormat("泵类型转换出错，不支持的类型 PumpType ={0}", m_PumpType);
                return false;
            }
            ProductID pid = ProductIDConvertor.Custom2ProductID(cid);
            Hashtable alarmMetrix = null;
            uint depletealArmIndex = 0, lowVolArmIndex = 0;//耗尽和低电压索引
            uint completeArmIndex = 0, willCompleteArmIndex = 0;//输液结束和输液即将结束
            uint forgetStartAlarmIndex = 0; //遗忘操作

            #region //查询耗尽、遗忘操作、低电、输液结束和输液即将结束5种类型的报警ID
            switch (pid)
            {
                case ProductID.GrasebyC9:
                    alarmMetrix = AlarmMetrix.Instance().AlarmMetrixC9;
                    depletealArmIndex = 15;
                    lowVolArmIndex = 12;
                    completeArmIndex = 17;
                    willCompleteArmIndex = 14;
                    forgetStartAlarmIndex = 13;
                    break;
                default:
                    break;
            }
            #endregion
            if (  depletealArmIndex == 0
               || lowVolArmIndex == 0
               || completeArmIndex == 0
               || willCompleteArmIndex == 0
               || forgetStartAlarmIndex == 0
               )
            {
                return false;
            }
            int findIndex = m_AlarmList.FindIndex((x) =>
            {
                return x > 0
                    && x != depletealArmIndex 
                    && x != lowVolArmIndex
                    && x != completeArmIndex
                    && x != willCompleteArmIndex
                    && x != forgetStartAlarmIndex;
            });
            if (findIndex >= 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 添加C9类泵报警首次发生的报警时间戳
        /// </summary>
        /// <param name="alarmID"></param>
        public override void UpdateAlarmTime(uint alarmID)
        {
            DateTime ocurredTime = DateTime.MinValue;
            if (m_AlarmOccurredTime.ContainsKey(alarmID))
            {
                ocurredTime = (DateTime)m_AlarmOccurredTime[alarmID];
                if (ocurredTime.Year < 2000)
                {
                    m_AlarmOccurredTime[alarmID] = DateTime.Now;
                }
            }
            else
            {
                m_AlarmOccurredTime.Add(alarmID, DateTime.Now);
            }
        }

        /// <summary>
        /// 更新报警首次出现的时间
        /// </summary>
        /// <param name="alarmIDs"></param>
        public void UpdateAlarmTime(List<uint> alarmIDs)
        {
            if (alarmIDs == null || alarmIDs.Count == 0)
                return;
            DateTime ocurredTime = DateTime.MinValue;
            foreach (var id in alarmIDs)
            {
                if (m_AlarmOccurredTime.ContainsKey(id))
                {
                    ocurredTime = (DateTime)m_AlarmOccurredTime[id];
                    if (ocurredTime.Year < 2000)
                    {
                        m_AlarmOccurredTime[id] = DateTime.Now;
                    }
                }
                else
                {
                    m_AlarmOccurredTime.Add(id, DateTime.Now);
                }
            }
        }

    }
}
