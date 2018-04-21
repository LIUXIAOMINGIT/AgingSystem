using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cmd
{
    //此类只存储常量，存放报警信息映射表
    public class AlarmMetrix
    {
        private static AlarmMetrix m_metrix = null;

        private Hashtable m_AlarmMetrixC6 = new Hashtable();
        private Hashtable m_AlarmMetrixF6 = new Hashtable();
        private Hashtable m_AlarmMetrixC6T = new Hashtable();
        private Hashtable m_AlarmMetrix2000 = new Hashtable();
        private Hashtable m_AlarmMetrix2100 = new Hashtable();
        private Hashtable m_AlarmMetrixC8 = new Hashtable();
        private Hashtable m_AlarmMetrix1200 = new Hashtable();
        private Hashtable m_AlarmMetrix1200En = new Hashtable();
        private Hashtable m_AlarmMetrixC9 = new Hashtable();

        public Hashtable AlarmMetrixC6
        {
            get { return m_AlarmMetrixC6; }
        }
        public Hashtable AlarmMetrixF6
        {
            get { return m_AlarmMetrixF6; }
        }
        public Hashtable AlarmMetrixC6T
        {
            get { return m_AlarmMetrixC6T; }
        }

        public Hashtable AlarmMetrix2000
        {
            get { return m_AlarmMetrix2000; }
        }
        public Hashtable AlarmMetrix2100
        {
            get { return m_AlarmMetrix2100; }
        }
        public Hashtable AlarmMetrixC8
        {
            get { return m_AlarmMetrixC8; }
        }

        public Hashtable AlarmMetrix1200
        {
            get { return m_AlarmMetrix1200; }
        }

        public Hashtable AlarmMetrix1200En
        {
            get { return m_AlarmMetrix1200En; }
        }

        public Hashtable AlarmMetrixC9
        {
            get { return m_AlarmMetrixC9; }
        }
        
        public static AlarmMetrix Instance()
        {
            if (m_metrix == null)
                m_metrix = new AlarmMetrix();
            return m_metrix;
        }
        private AlarmMetrix()
        {
            Init();
        }

        /// <summary>
        /// 初始化报警列表
        /// 
        /// </summary>
        private void Init()
        {
            //初始化C6报警列表
            m_AlarmMetrixC6.Add((UInt32)0x00000001, "系统出错");
            m_AlarmMetrixC6.Add((UInt32)0x00000002, "驱动出错");
            m_AlarmMetrixC6.Add((UInt32)0x00000004, "管路阻塞");
            m_AlarmMetrixC6.Add((UInt32)0x00000008, "电池耗尽");
            m_AlarmMetrixC6.Add((UInt32)0x00000010, "注射器活塞脱离");
            m_AlarmMetrixC6.Add((UInt32)0x00000020, "注射器压块打开");
            m_AlarmMetrixC6.Add((UInt32)0x00000040, "VTBI输液过程结束");
            m_AlarmMetrixC6.Add((UInt32)0x00000080, "注射完毕");
            m_AlarmMetrixC6.Add((UInt32)0x00000100, "电池量低");
            m_AlarmMetrixC6.Add((UInt32)0x00000200, "没有输液");
            m_AlarmMetrixC6.Add((UInt32)0x00000400, "速率设置超出范围");
            m_AlarmMetrixC6.Add((UInt32)0x00000800, "注射器尺寸错误");
            m_AlarmMetrixC6.Add((UInt32)0x00001000, "注射器残留提示");
            m_AlarmMetrixC6.Add((UInt32)0x00002000, "市电故障或电源线脱落提示");

            //初始化F6报警列表
            m_AlarmMetrixF6.Add((UInt32)0x00000001, "系统出错");
            m_AlarmMetrixF6.Add((UInt32)0x00000002, "驱动出错");
            m_AlarmMetrixF6.Add((UInt32)0x00000004, "管路阻塞");
            m_AlarmMetrixF6.Add((UInt32)0x00000008, "电池耗尽");
            m_AlarmMetrixF6.Add((UInt32)0x00000010, "注射器活塞脱离");
            m_AlarmMetrixF6.Add((UInt32)0x00000020, "注射器压块打开");
            m_AlarmMetrixF6.Add((UInt32)0x00000040, "VTBI输液过程结束");
            m_AlarmMetrixF6.Add((UInt32)0x00000080, "注射完毕");
            m_AlarmMetrixF6.Add((UInt32)0x00000100, "电池量低");
            m_AlarmMetrixF6.Add((UInt32)0x00000200, "没有输液");
            m_AlarmMetrixF6.Add((UInt32)0x00000400, "速率设置超出范围");
            m_AlarmMetrixF6.Add((UInt32)0x00000800, "注射器尺寸错误");
            m_AlarmMetrixF6.Add((UInt32)0x00001000, "注射器残留提示");
            m_AlarmMetrixF6.Add((UInt32)0x00002000, "市电故障或电源线脱落提示");

            //初始化C6T报警列表
            m_AlarmMetrixC6T.Add((UInt32)0x00000001, "系统出错");
            m_AlarmMetrixC6T.Add((UInt32)0x00000002, "驱动出错");
            m_AlarmMetrixC6T.Add((UInt32)0x00000004, "管路阻塞");
            m_AlarmMetrixC6T.Add((UInt32)0x00000008, "电池耗尽");
            m_AlarmMetrixC6T.Add((UInt32)0x00000010, "注射器活塞脱离");
            m_AlarmMetrixC6T.Add((UInt32)0x00000020, "注射器压块打开");
            m_AlarmMetrixC6T.Add((UInt32)0x00000040, "VTBI输液过程结束");
            m_AlarmMetrixC6T.Add((UInt32)0x00000080, "注射完毕");
            m_AlarmMetrixC6T.Add((UInt32)0x00000100, "电池量低");
            m_AlarmMetrixC6T.Add((UInt32)0x00000200, "没有输液");
            m_AlarmMetrixC6T.Add((UInt32)0x00000400, "速率设置超出范围");
            m_AlarmMetrixC6T.Add((UInt32)0x00000800, "注射器尺寸错误");
            m_AlarmMetrixC6T.Add((UInt32)0x00001000, "注射器残留提示");
            m_AlarmMetrixC6T.Add((UInt32)0x00002000, "市电故障或电源线脱落提示");

            //初始化2000报警列表
            m_AlarmMetrix2000.Add((UInt32)0x00000001, "系统出错");
            m_AlarmMetrix2000.Add((UInt32)0x00000002, "驱动出错");
            m_AlarmMetrix2000.Add((UInt32)0x00000004, "管路阻塞");
            m_AlarmMetrix2000.Add((UInt32)0x00000008, "电池耗尽");
            m_AlarmMetrix2000.Add((UInt32)0x00000010, "注射器活塞脱离");
            m_AlarmMetrix2000.Add((UInt32)0x00000020, "注射器压块打开");
            m_AlarmMetrix2000.Add((UInt32)0x00000040, "VTBI输液过程结束");
            m_AlarmMetrix2000.Add((UInt32)0x00000080, "注射完毕");
            m_AlarmMetrix2000.Add((UInt32)0x00000100, "电池量低");
            m_AlarmMetrix2000.Add((UInt32)0x00000200, "没有输液");
            m_AlarmMetrix2000.Add((UInt32)0x00000400, "速率设置超出范围");
            m_AlarmMetrix2000.Add((UInt32)0x00000800, "注射器尺寸错误");
            m_AlarmMetrix2000.Add((UInt32)0x00001000, "注射器残留提示");
            m_AlarmMetrix2000.Add((UInt32)0x00002000, "市电故障或电源线脱落提示");

            //初始化2100报警列表
            m_AlarmMetrix2100.Add((UInt32)0x00000001, "系统出错");
            m_AlarmMetrix2100.Add((UInt32)0x00000002, "驱动出错");
            m_AlarmMetrix2100.Add((UInt32)0x00000004, "管路阻塞");
            m_AlarmMetrix2100.Add((UInt32)0x00000008, "电池耗尽");
            m_AlarmMetrix2100.Add((UInt32)0x00000010, "注射器活塞脱离");
            m_AlarmMetrix2100.Add((UInt32)0x00000020, "注射器压块打开");
            m_AlarmMetrix2100.Add((UInt32)0x00000040, "VTBI输液过程结束");
            m_AlarmMetrix2100.Add((UInt32)0x00000080, "注射完毕");
            m_AlarmMetrix2100.Add((UInt32)0x00000100, "电池量低");
            m_AlarmMetrix2100.Add((UInt32)0x00000200, "没有输液");
            m_AlarmMetrix2100.Add((UInt32)0x00000400, "速率设置超出范围");
            m_AlarmMetrix2100.Add((UInt32)0x00000800, "注射器尺寸错误");
            m_AlarmMetrix2100.Add((UInt32)0x00001000, "注射器残留提示");
            m_AlarmMetrix2100.Add((UInt32)0x00002000, "市电故障或电源线脱落提示");

            //初始化1200报警列表中文泵
            m_AlarmMetrix1200.Add((UInt32)0x00000001, "系统出错1");
            m_AlarmMetrix1200.Add((UInt32)0x00000002, "系统出错2");
            m_AlarmMetrix1200.Add((UInt32)0x00000004, "系统出错3");
            m_AlarmMetrix1200.Add((UInt32)0x00000008, "键出错");
            m_AlarmMetrix1200.Add((UInt32)0x00000010, "电池电量耗尽");
            m_AlarmMetrix1200.Add((UInt32)0x00000020, "驱动出错请重新启动");
            m_AlarmMetrix1200.Add((UInt32)0x00000040, "请排除管路气泡");
            m_AlarmMetrix1200.Add((UInt32)0x00000080, "管路堵塞");
            m_AlarmMetrix1200.Add((UInt32)0x00000100, "输液完毕");
            m_AlarmMetrix1200.Add((UInt32)0x00000200, "门未关");
            m_AlarmMetrix1200.Add((UInt32)0x00000400, "KVO完毕");
            m_AlarmMetrix1200.Add((UInt32)0x00000800, "检查面贴快进处是否下凹");
            m_AlarmMetrix1200.Add((UInt32)0x00001000, "即将输液完毕");
            m_AlarmMetrix1200.Add((UInt32)0x00002000, "您是否忘了启动");
            m_AlarmMetrix1200.Add((UInt32)0x00004000, "电池即将用完");

            //初始化1200报警列表英文泵
            m_AlarmMetrix1200En.Add((UInt32)0x00000001, "系统出错1");
            m_AlarmMetrix1200En.Add((UInt32)0x00000002, "系统出错2");
            m_AlarmMetrix1200En.Add((UInt32)0x00000004, "系统出错3");
            m_AlarmMetrix1200En.Add((UInt32)0x00000008, "系统出错4");
            m_AlarmMetrix1200En.Add((UInt32)0x00000010, "键出错");
            m_AlarmMetrix1200En.Add((UInt32)0x00000020, "电池电量耗尽");
            m_AlarmMetrix1200En.Add((UInt32)0x00000040, "驱动出错请重新启动");
            m_AlarmMetrix1200En.Add((UInt32)0x00000080, "请排除管路气泡");
            m_AlarmMetrix1200En.Add((UInt32)0x00000100, "管路堵塞");
            m_AlarmMetrix1200En.Add((UInt32)0x00000200, "输液完毕");
            m_AlarmMetrix1200En.Add((UInt32)0x00000400, "门未关");
            m_AlarmMetrix1200En.Add((UInt32)0x00000800, "KVO完毕");
            m_AlarmMetrix1200En.Add((UInt32)0x00001000, "检查面贴快进处是否下凹");
            m_AlarmMetrix1200En.Add((UInt32)0x00002000, "即将输液完毕");
            m_AlarmMetrix1200En.Add((UInt32)0x00004000, "您是否忘了启动");
            m_AlarmMetrix1200En.Add((UInt32)0x00008000, "电池即将用完");


            //初始化C8报警列表
            m_AlarmMetrixC8.Add((UInt32)0x00000001, "电池电量低");
            m_AlarmMetrixC8.Add((UInt32)0x00000002, "未启动");
            m_AlarmMetrixC8.Add((UInt32)0x00000004, "即将注射完成");
            m_AlarmMetrixC8.Add((UInt32)0x00000008, "速率设置无效");
            m_AlarmMetrixC8.Add((UInt32)0x00000010, "注射器不匹配");

            m_AlarmMetrixC8.Add((UInt32)0x00010000, "电池耗尽");
            m_AlarmMetrixC8.Add((UInt32)0x00020000, "预设注射完成");
            m_AlarmMetrixC8.Add((UInt32)0x00040000, "注射完成");
            m_AlarmMetrixC8.Add((UInt32)0x00080000, "管路阻塞");
            m_AlarmMetrixC8.Add((UInt32)0x00100000, "装夹错误");

            m_AlarmMetrixC8.Add((UInt32)0x01000000, "系统出错1");
            m_AlarmMetrixC8.Add((UInt32)0x02000000, "系统出错2");
            m_AlarmMetrixC8.Add((UInt32)0x04000000, "系统出错3");
            m_AlarmMetrixC8.Add((UInt32)0x08000000, "系统出错4");
            m_AlarmMetrixC8.Add((UInt32)0x10000000, "系统出错5");
            m_AlarmMetrixC8.Add((UInt32)0x20000000, "系统出错6");
            m_AlarmMetrixC8.Add((UInt32)0x40000000, "系统出错7");

            //初始化C9报警列表
            m_AlarmMetrixC9.Add((UInt32)1, "速率超范围");
            m_AlarmMetrixC9.Add((UInt32)2, "参数未设置");
            m_AlarmMetrixC9.Add((UInt32)3, "速率超范围");
            m_AlarmMetrixC9.Add((UInt32)4, "速率未设置");
            m_AlarmMetrixC9.Add((UInt32)5, "限制量未设置");
            m_AlarmMetrixC9.Add((UInt32)6, "排气量达到最大值");
            m_AlarmMetrixC9.Add((UInt32)7, "电池温度异常");
            m_AlarmMetrixC9.Add((UInt32)8, "该注射器未校准");
            m_AlarmMetrixC9.Add((UInt32)9, "该注射器型号不支持");
            m_AlarmMetrixC9.Add((UInt32)10, "剂量超范围");
            m_AlarmMetrixC9.Add((UInt32)11, "浓度超范围");
            m_AlarmMetrixC9.Add((UInt32)12, "电池电量低");
            m_AlarmMetrixC9.Add((UInt32)13, "遗忘操作");
            m_AlarmMetrixC9.Add((UInt32)14, "即将输注完成");
            m_AlarmMetrixC9.Add((UInt32)15, "电池耗尽");
            m_AlarmMetrixC9.Add((UInt32)16, "管路阻塞");
            m_AlarmMetrixC9.Add((UInt32)17, "输注完成");
            m_AlarmMetrixC9.Add((UInt32)18, "注射器未正确安装");
            m_AlarmMetrixC9.Add((UInt32)19, "注射器未正确安装");
            m_AlarmMetrixC9.Add((UInt32)20, "传动错误");
            m_AlarmMetrixC9.Add((UInt32)21, "系统错误01");
            m_AlarmMetrixC9.Add((UInt32)22, "系统错误02");
            m_AlarmMetrixC9.Add((UInt32)23, "系统错误03");
            m_AlarmMetrixC9.Add((UInt32)24, "系统错误04");
            m_AlarmMetrixC9.Add((UInt32)25, "系统错误05");
            m_AlarmMetrixC9.Add((UInt32)26, "系统错误06");
            m_AlarmMetrixC9.Add((UInt32)27, "系统错误07");
            m_AlarmMetrixC9.Add((UInt32)28, "系统错误08");
            m_AlarmMetrixC9.Add((UInt32)29, "系统错误09");
            m_AlarmMetrixC9.Add((UInt32)30, "系统错误10");
            m_AlarmMetrixC9.Add((UInt32)31, "系统错误11");
            m_AlarmMetrixC9.Add((UInt32)32, "系统错误12");
            m_AlarmMetrixC9.Add((UInt32)33, "系统错误13");
            m_AlarmMetrixC9.Add((UInt32)34, "系统错误14");
            m_AlarmMetrixC9.Add((UInt32)35, "MainFCT未测试");
            m_AlarmMetrixC9.Add((UInt32)36, "UIFCT未测试");
        }

    }
}
