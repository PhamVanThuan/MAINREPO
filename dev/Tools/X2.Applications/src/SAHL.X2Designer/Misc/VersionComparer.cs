using System;
using System.Collections;

// ED using NUnit.Core;
// ED using NUnit.Framework;

namespace SAHL.X2Designer.Misc
{
    public class VersionComparer : IComparer
    {
        /// <summary>
        /// compares two version no's a.b.c if new is newer returns 1 else 0 for same or -1
        /// </summary>
        /// <param name="Old"></param>
        /// <param name="New"></param>
        /// <returns></returns>
        public int Compare(object Old, object New)
        {
            string[] ssOld = Old.ToString().Split('.');
            string[] ssNew = New.ToString().Split('.');
            if (Convert.ToInt32(ssOld[0]) == Convert.ToInt32(ssNew[0]))
            {
                if (Convert.ToInt32(ssOld[1]) == Convert.ToInt32(ssNew[1]))
                {
                    if (Convert.ToInt32(ssOld[2]) == Convert.ToInt32(ssNew[2]))
                    {
                        return 0;
                    }
                    else if (Convert.ToInt32(ssOld[2]) < Convert.ToInt32(ssNew[2]))
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (Convert.ToInt32(ssOld[1]) < Convert.ToInt32(ssNew[1]))
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            else if (Convert.ToInt32(ssOld[0]) < Convert.ToInt32(ssNew[0]))
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }

    // ED
    //[TestFixture()]
    //public class VersionCompareTest
    //{
    //  IComparer Compare = new VersionComparer();
    //  [Test()]
    //  public void MajorGreater()
    //  {
    //    string Old = "1.1.987";
    //    string New = "2.0.23";
    //    int Res = Compare.Compare(Old, New);
    //    Assert.AreEqual(1, Res);
    //  }
    //  [Test()]
    //  public void MajorEqual()
    //  {
    //    string Old = "1.1.0";
    //    string New = "1.1.0";
    //    int Res = Compare.Compare(Old, New);
    //    Assert.AreEqual(0, Res);
    //  }
    //  [Test()]
    //  public void MajorLess()
    //  {
    //    string Old = "2.1.34";
    //    string New = "1.1.34";
    //    int Res = Compare.Compare(Old, New);
    //    Assert.AreEqual(-1, Res);
    //  }
    //  [Test()]
    //  public void MinorGreater()
    //  {
    //    string Old = "1.1.987";
    //    string New = "1.2.23";
    //    int Res = Compare.Compare(Old, New);
    //    Assert.AreEqual(1, Res);
    //  }
    //  [Test()]
    //  public void MinorLess()
    //  {
    //    string Old = "1.2.34";
    //    string New = "1.1.34";
    //    int Res = Compare.Compare(Old, New);
    //    Assert.AreEqual(-1, Res);
    //  }
    //  [Test()]
    //  public void BuildGreater()
    //  {
    //    string Old = "1.1.987";
    //    string New = "1.1.988";
    //    int Res = Compare.Compare(Old, New);
    //    Assert.AreEqual(1, Res);
    //  }
    //  [Test()]
    //  public void BuildLess()
    //  {
    //    string Old = "1.1.34";
    //    string New = "1.1.33";
    //    int Res = Compare.Compare(Old, New);
    //    Assert.AreEqual(-1, Res);
    //  }
    //}
}