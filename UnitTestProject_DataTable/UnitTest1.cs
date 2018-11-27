using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace UnitTestProject_DataTable
{
    [TestClass]
    public class UnitTest1
    {

        /// <summary>
        /// 初始化datatable数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { new DataColumn("FORAname", typeof(string)),
                                         new DataColumn("FCostName", typeof(string)),
                                         new DataColumn("Score", typeof(int)) });
            dt.Rows.Add("001", "02", 11);
            dt.Rows.Add("001", "03", 11);
            dt.Rows.Add("002", "03", 11);
            dt.Rows.Add("002", "01", 11);
            dt.Rows.Add("003", "02", 11);
            dt.Rows.Add("003", "01", 11);
            return dt;
        }

        /// <summary>
        /// 复制空行结构，填充值新增
        /// </summary>
        [TestMethod]
        public void CloneCopyRowTest()
        {
            DataTable dt = GetDataTable();
            DataRow row = dt.NewRow();
            row["FORAname"] = "04";
            row["FCostName"] = "04";
            row["Score"] = 22;
            dt.Rows.Add(row);
        }

        /// <summary>
        /// datatable 克隆和复制
        /// clone 复制空表结构
        /// copy 结构和值复制
        /// </summary>
        [TestMethod]
        public void CloneCopyTest()
        {
            DataTable dt = GetDataTable();
            DataTable dtClone = dt.Clone();
            DataTable dtCopy = dt.Copy();
            int obj = Convert.ToInt32(dt.Compute("sum(Score)", "true"));
            DataTable dt2 = dt.Clone();
            dt2.Rows.Add(dt.Rows);
        }

        /// <summary>
        /// 汇总
        /// </summary>
        [TestMethod]
        public void SumTest()
        {
            DataTable dt = GetDataTable();
            string score = "Score";
            int obj = Convert.ToInt32(dt.Compute("sum(" + score + ")", "true"));
        }

        /// <summary>
        /// 查询
        /// </summary>
        [TestMethod]
        public void SelectTest()
        {
            DataTable dt = GetDataTable();
            //DataRow[] rows = dt.Select(" FORAname = '001' ");
            DataRow[] rows = dt.Select(" FORAname like '%01%' ");
            //DataRow[] rows = dt.AsEnumerable().Select(x => x.Field<string>("FORAname") == "001").ToList();
        }

        /// <summary>
        /// 排序
        /// AsEnumerable 需要添加System.Data.DataSetExtensions引用，不要using
        /// </summary>
        [TestMethod]
        public void OrderTest()
        {
            DataTable dt = GetDataTable();
            var rows = dt.AsEnumerable()
            .OrderBy(p => p.Field<string>("FCostName"))
            .Select(p => p);
            foreach (DataRow row in rows)
            {
                string str = "group FCostName key " + row["FCostName"];
            }
        }

        /// <summary>
        /// 分组
        /// </summary>
        [TestMethod]
        public void GroupTest()
        {
            DataTable dt = GetDataTable();
            var query = from t in dt.AsEnumerable()
                                group t by new { t1 = t.Field<string>("FCostName") } into m
                                select new
                                {
                                    FCostName = m.Key.t1,
                                    Score = m.Sum(n => n.Field<int>("Score"))
                                };
            foreach (var item in query)
            {
                string key = "group FCostName key " + item.FCostName;
                string score = "group FCostName score " + item.Score;
            }
        }

        /// <summary>
        /// 遍历
        /// </summary>
        [TestMethod]
        public void ForEachTest()
        {
            DataTable dt = GetDataTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string value = dt.Rows[i]["FORAname"].ToString();
            }
            foreach (DataRow row in dt.Rows)
            {
                string value = row[0].ToString();
            }
            foreach (DataRow dr in dt.Rows)
            {
                string value = dr["FORAname"].ToString();
            }
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string value = dr[i].ToString();
                }
            }
        }
    }
}
