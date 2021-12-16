using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TEST01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {


                Assembly asm = Assembly.GetExecutingAssembly();
                string path = System.IO.Path.GetDirectoryName(asm.Location);
                Glabel.Text += path;
                Clabel.Text += "C:\\RAPOR.xlsx";
                 DataTable datatableFromData = ConvertData.GetDataTableFromExcel(path + "\\EKSTRE-GIRDI.xlsx");
                List<EKSTRE_GIRDI> listEkstreGirdi = datatableFromData.AsEnumerable().Select(m => new EKSTRE_GIRDI()
                {
                    SIRA_NO = m.Field<string>("SIRA_NO"),
                    ADET = Convert.ToInt32(m.Field<string>("ADET")),
                    KG_DESI = Convert.ToInt32(m.Field<string>("KG_DESI")),
                    MESAFE = m.Field<string>("MESAFE")
                }).ToList();

                datatableFromData = ConvertData.GetDataTableFromExcel(path + "\\FORMUL-GIRDI.xlsx");
                List<FORMUL_GIRDI> listFormulGirdi = datatableFromData.AsEnumerable().Select(m => new FORMUL_GIRDI()
                {
                    DESI_MIN = m.Field<string>("DESİ").Contains("-") ? Convert.ToInt32(m.Field<string>("DESİ").Split('-')[0]) : -1,
                    DESI_MAX = m.Field<string>("DESİ").Contains("-") ? Convert.ToInt32(m.Field<string>("DESİ").Split('-')[1]) : -1,
                    KISA_SEHIRICI_YAKIN = (m.Field<string>("KISA-ŞEHİRİÇİ-YAKIN")),
                    UZAK_ORTA = m.Field<string>("UZAK-ORTA"),
                }).ToList();
                int maxDesi = listFormulGirdi[listFormulGirdi.Count - 2].DESI_MAX;
                double maxKisa = Convert.ToDouble(listFormulGirdi[listFormulGirdi.Count - 2].KISA_SEHIRICI_YAKIN);
                double maxUzak = Convert.ToDouble(listFormulGirdi[listFormulGirdi.Count - 2].UZAK_ORTA);
                listEkstreGirdi = artanHesapla(listEkstreGirdi, maxDesi);
                foreach (EKSTRE_GIRDI item in listEkstreGirdi)
                {
                    switch (item.MESAFE)
                    {
                        case "UZAK":
                            item.MESAFE_KOD = 2;
                            break;
                        case "ORTA":
                            item.MESAFE_KOD = 2;
                            break;
                        default:
                            item.MESAFE_KOD = 1;
                            break;
                    }

                    if (item.ARTAN == 0)
                    {
                        int ucret = 0;
                        switch (item.MESAFE_KOD)
                        {
                            case 1:
                                item.UCRET = Convert.ToDouble(listFormulGirdi.Where(x => x.DESI_MIN <= item.KG_DESI && x.DESI_MAX >= item.KG_DESI).FirstOrDefault().KISA_SEHIRICI_YAKIN);
                                break;
                            case 2:
                                item.UCRET = Convert.ToDouble(listFormulGirdi.Where(x => x.DESI_MIN <= item.KG_DESI && x.DESI_MAX >= item.KG_DESI).FirstOrDefault().UZAK_ORTA);
                                break;
                            default:
                                break;
                        }

                    }
                    else
                    {
                        switch (item.MESAFE_KOD)
                        {
                            case 1:
                                item.UCRET = maxKisa + (Convert.ToDouble(listFormulGirdi.Where(x => x.DESI_MIN == -1 && x.DESI_MAX == -1).FirstOrDefault().KISA_SEHIRICI_YAKIN) * item.ARTAN);
                                break;
                            case 2:
                                item.UCRET = maxUzak + (Convert.ToDouble(listFormulGirdi.Where(x => x.DESI_MIN == -1 && x.DESI_MAX == -1).FirstOrDefault().UZAK_ORTA) * item.ARTAN);
                                break;
                            default:
                                break;
                        }
                    }
                    item.UCRET = item.UCRET * item.ADET;
                    item.UCRET = Math.Round(item.UCRET, 2);
                }
                List<RaporClass> raporExcelList = new List<RaporClass>();

                foreach (var item in listEkstreGirdi)
                {
                    if (raporExcelList.Where(x => x.MESAFE == item.MESAFE).FirstOrDefault() != null)
                    {
                        raporExcelList.Where(x => x.MESAFE == item.MESAFE).FirstOrDefault().KADET += item.ADET;
                        raporExcelList.Where(x => x.MESAFE == item.MESAFE).FirstOrDefault().TOPADET += item.ADET;
                        raporExcelList.Where(x => x.MESAFE == item.MESAFE).FirstOrDefault().TOPKG_DESI += item.KG_DESI;
                        raporExcelList.Where(x => x.MESAFE == item.MESAFE).FirstOrDefault().TOP_UCRET += item.UCRET;
                    }
                    else
                    {
                        raporExcelList.Add(new RaporClass
                        {
                            KADET = item.ADET,
                            TOPADET = item.ADET,
                            MESAFE = item.MESAFE,
                            TOP_UCRET = item.UCRET,
                            TOPKG_DESI = item.KG_DESI
                        });
                    }
                }
                string ouputPath = "C:\\RAPOR.xlsx";
                System.IO.FileInfo f = new System.IO.FileInfo(ouputPath);
                if (f.Exists) f.Delete();

                using (var pck = new ExcelPackage(f))
                {

                    var mi = typeof(EKSTRE_GIRDI)
                        .GetProperties()
                        .Where(pi => pi.Name != "ARTAN" && pi.Name != "MESAFE_KOD")
                        .Select(pi => (MemberInfo)pi)
                        .ToArray();

                    var worksheet = pck.Workbook.Worksheets.Add("İşlem Sonucu");
                    worksheet.Cells.LoadFromCollection(
                        listEkstreGirdi
                        , true
                        , TableStyles.Custom
                        , BindingFlags.Public | BindingFlags.Instance
                        , mi);

                    var worksheet2 = pck.Workbook.Worksheets.Add("Pivot Rapor");
                    var mi2 = typeof(RaporClass)
                        .GetProperties()
                        .Where(pi => pi.Name != "ARTAN")
                        .Select(pi => (MemberInfo)pi)
                        .ToArray();
                    worksheet2.Cells.LoadFromCollection(
                        raporExcelList
                        , true
                        , TableStyles.Custom
                        , BindingFlags.Public | BindingFlags.Instance
                        , mi2);

                    pck.Save();
                }
                System.Diagnostics.Process.Start(ouputPath);
                MessageBox.Show("İşlem Başarılı");
            }
            catch (Exception ex)
            {
                msglbl.Text += " Bir hata oluştu:" + ex.Message;
                MessageBox.Show("Bir hata oluştu:"+ ex.Message);
            }
            //FileInfo fi = new FileInfo(path);
            //var sonuc = GetStudentsFromExcel(fi);
            //using (ExcelPackage excelPackage = new ExcelPackage(fi))
            //{

            //    ExcelWorksheet myWorksheet =
            //        excelPackage.Workbook.Worksheets.FirstOrDefault(x => x.Name == "Sayfa1");
            //    var aa3 =myWorksheet.Tables.First().ConvertTableToObjects<EKSTRE_GIRDI>();


            //    var dt = WorksheetToDataTable(myWorksheet);
            //    //Get the content from cells A1 and B1 as string, in two different notations
            //    string valA1 = myWorksheet.Cells["A1"].Value.ToString();
            //    string valB1 = myWorksheet.Cells[1, 2].Value.ToString();

            //    //Save your file
            //    excelPackage.Save();
            //}
        }

        private List<EKSTRE_GIRDI> artanHesapla(List<EKSTRE_GIRDI> listEkstreGirdi, int maxDesi)
        {
            foreach (EKSTRE_GIRDI item in listEkstreGirdi)
            {
                if (Convert.ToInt32(item.KG_DESI) > maxDesi)
                {
                    item.ARTAN = Convert.ToInt32(item.KG_DESI) - maxDesi;
                }
            };
            return listEkstreGirdi;
        }


    }
}
