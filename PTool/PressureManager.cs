using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ComunicationProtocol.Misc;

namespace PTool
{
    /// <summary>
    /// 注射器管理类
    /// 主要用于管理注射器器品牌各种语言名称及对应的产品ID
    /// </summary>
    public class PressureManager
    {
        private static PressureManager m_Manager = null;
        private Hashtable m_HashProductPressure = new Hashtable();//（Key：泵类型  value:ProductPressure）

        private PressureManager()
        {
        }

        public static PressureManager Instance()
        {
            if (m_Manager == null)
                m_Manager = new PressureManager();
            return m_Manager;
        }

        /// <summary>
        /// 按照不同语言添加不同的品牌名称
        /// </summary>
        /// <param name="lang">语言</param>
        /// <param name="brand">品牌</param>
        /// <param name="name">此语言下的品牌名称</param>
        public void Add(PumpID pid, OcclusionLevel level, int syringeSize,float mid)
        {
            if (!m_HashProductPressure.ContainsKey(pid))
            {
                LevelPressure lp = new LevelPressure(level);
                lp.Add(syringeSize, mid);
                ProductPressure pp = new ProductPressure(pid);
                pp.Add(lp);
                m_HashProductPressure.Add(pid, pp);
            }
            else
            {
                ProductPressure pp = m_HashProductPressure[pid] as ProductPressure;
                LevelPressure lp = pp.Find(level);
                if (lp!=null)
                {
                    lp.Add(syringeSize, mid);
                }
                else
                {
                    lp = new LevelPressure(level);
                    lp.Add(syringeSize, mid);
                    pp.Add(lp);
                }
            }
        }

        public void Clear()
        {
            m_HashProductPressure.Clear();
        }

        public ProductPressure GetPressureByProductID(PumpID pid)
        {
            switch (pid)
            {
                case PumpID.GrasebyF6_2:
                    pid = PumpID.GrasebyF6;
                    break;
                case PumpID.WZS50F6_2:
                    pid = PumpID.WZS50F6;
                    break;
                default:
                    break;
            }
            if (m_HashProductPressure.ContainsKey(pid))
                return m_HashProductPressure[pid] as ProductPressure;
            else
                return null;
        }

        /// <summary>
        /// 取某类型泵的某个压力等级下某个尺寸的最大压力配置值
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="size"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public float GetMaxBySizeLevel(PumpID pid, int size, OcclusionLevel level)
        {
            ProductPressure pp = GetPressureByProductID(pid);
            if (pp == null)
                return 0;
            LevelPressure lp = pp.Find(level);
            if (lp == null)
                return 0;
            SizePressure sp  = lp.Find(size);
            if (sp == null)
                return 0;
            return sp.m_Mid;
        }

        /// <summary>
        /// 读中间值
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="size"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public float GetMidBySizeLevel(PumpID pid, int size, OcclusionLevel level)
        {
            ProductPressure pp = GetPressureByProductID(pid);
            if (pp == null)
                return 0;
            LevelPressure lp = pp.Find(level);
            if (lp == null)
                return 0;
            SizePressure sp = lp.Find(size);
            if (sp == null)
                return 0;
            return sp.m_Mid;
        }
    }

    public class ProductPressure
    {
        public PumpID m_Pid = PumpID.GrasebyC6;
        public List<LevelPressure> m_LevelPressureList = new List<LevelPressure>();

        public ProductPressure()
        { }

        public ProductPressure(PumpID pid)
        {
            m_Pid = pid;
        }

        public void Add(LevelPressure lp)
        {
            if (lp == null)
                return;
            if (m_LevelPressureList.FindIndex((x) => { return x.m_Level == lp.m_Level; }) < 0)
                m_LevelPressureList.Add(lp);
        }

        public LevelPressure Find(OcclusionLevel level)
        {
            return m_LevelPressureList.Find((x) => { return x.m_Level == level; });
        }

        public List<LevelPressure> GetLevelPressureList()
        {
            return m_LevelPressureList;
        }
    }

    public class LevelPressure
    {
        public OcclusionLevel m_Level = OcclusionLevel.None;
        public List<SizePressure> m_SizePressureList = new List<SizePressure>();

        public LevelPressure()
        { }

        public LevelPressure(OcclusionLevel level)
        {
            m_Level = level;
        }

        public void Add(int syringeSize, float mid)
        {
            if (m_SizePressureList == null)
                m_SizePressureList = new List<SizePressure>();
            if (m_SizePressureList.FindIndex((x) => { return x.m_SyringeSize == syringeSize; }) < 0)
                m_SizePressureList.Add(new SizePressure(syringeSize, mid));
        }

        public SizePressure Find(int syringeSize)
        {
            return m_SizePressureList.Find((x) => { return x.m_SyringeSize == syringeSize; });
        }
    }


    public class SizePressure
    {
        public int   m_SyringeSize; //注射器尺寸
        //public float m_Min;         //压力最小值kg
        public float m_Mid;         //压力调试值kg
        //public float m_Max;         //压力最大值kg

        public SizePressure() { }

        public SizePressure(int syringeSize, float mid)
        {
            m_SyringeSize = syringeSize;
            //m_Min = min;
            m_Mid = mid;
            //m_Max = max;        
        }

    }


    public class PressureCalibrationParameter
    {
        public int m_SyringeSize; //注射器尺寸
        public float m_PressureL;   //L
        public float m_PressureC;   //C
        public float m_PressureH;   //H

        public PressureCalibrationParameter()
        {

        }

        public PressureCalibrationParameter(int syringeSize, float pressureL, float pressureC, float pressureH)
        {
            m_SyringeSize = syringeSize;
            m_PressureL = pressureL;
            m_PressureC = pressureC;
            m_PressureH = pressureH;
        }

    }

    public class PressureParameter
    {
        public int            m_SyringeSize; //注射器尺寸
        public OcclusionLevel m_Level = OcclusionLevel.None;
        public float          m_MidWeight;   //配置文件设置的重量值
        public float          m_Pressure;    //计算得出压力值，从采样数据中找

        public PressureParameter(int syringeSize, OcclusionLevel level, float midWeight, float pressure)
        {
            m_SyringeSize = syringeSize;
            m_Level       = level;
            m_MidWeight   = midWeight;
            m_Pressure    = pressure;  
        }

        public void SetPressure(float p)
        {
            m_Pressure = p;
        }
  
    }

}
