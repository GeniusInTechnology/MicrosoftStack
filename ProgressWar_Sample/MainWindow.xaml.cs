using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ProgressWar_Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.btn.Click += btn_Click;
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            //THreadsinProgress();
            InitialIzeData();
        //    List<Site> site = FindHiearchy(4);
            AddNewNodes(4, RightTreeData);
            RemoveNewNodes(4, LeftTreeData);
        }

        private List<Site> RightTreeData = new List<Site>();

        private List<Site> LeftTreeData = new List<Site>();

        private List<Site> OrginalData = new List<Site>();

        private void InitialIzeData()
        {
            OrginalData = GetData();
            foreach (var item in OrginalData)
            {
                LeftTreeData.Add(item);
            }
            // LeftTreeData = OrginalData;

        }
        private List<Site> GetData()
        {
            List<Site> lstSite = new List<Site>();
            lstSite.Add(new Site() { Id = 1, Name = "Nutrition", ParentId = 0 });
            lstSite.Add(new Site() { Id = 2, Name = "IceCreaM", ParentId = 0 });
            lstSite.Add(new Site() { Id = 3, Name = "North America", ParentId = 1 });
            lstSite.Add(new Site() { Id = 4, Name = "Pacific", ParentId = 1 });
            lstSite.Add(new Site() { Id = 9, Name = "NOrth", ParentId = 2 });

            lstSite.Add(new Site() { Id = 5, Name = "Site 2", ParentId = 3 });
            lstSite.Add(new Site() { Id = 6, Name = "Site 1", ParentId = 3 });
            lstSite.Add(new Site() { Id = 7, Name = "Site 3", ParentId = 4 });
            lstSite.Add(new Site() { Id = 8, Name = "Site 4", ParentId = 4 });
            lstSite.Add(new Site() { Id = 10, Name = "Site 4", ParentId = 9 });

          
            return lstSite;
        }

        private List<Site> FindHiearchy(int id)
        {
            List<Site> site = new List<Site>();
            var item = OrginalData.SingleOrDefault(x=>x.Id ==id);
            site.Add(item);
            if(item.ParentId !=0)
            {
                site.AddRange(FindHiearchy(item.ParentId));
            }
            return site;
           // if(currentList.Count(x=>x.ParentI)
        }

        private  bool IsSite(int id)
        {
            bool flag = false;

            List<Site> sites = FindHiearchy(id);
            if(sites.Count ==3)
            {
                flag = true;
            }
            return flag;
            
        }

        private bool IsZone(int id)
        {
            bool flag = false;

            List<Site> sites = FindHiearchy(id);
            if (sites.Count == 1)
            {
                flag = true;
            }
            return flag;

        }

        private bool IsRegion(int id)
        {
            bool flag = false;

            List<Site> sites = FindHiearchy(id);
            if (sites.Count == 2)
            {
                flag = true;
            }
            return flag;

        }

        private void AddNewNodes(int id, List<Site> current)
        {
            var item = OrginalData.SingleOrDefault(x => x.Id == id);
            List<Site> chainSites = FindHiearchy(id);
            if(current.Count ==0)
            {
                if(IsSite(id))
                {
                    foreach (var site in chainSites)
                    {
                        if (current.SingleOrDefault(x => x.Id == site.Id) != null)
                        {
                            current.Add(site);
                        }
                    }
                    
                }

                else if(IsRegion(id))
                {
                    foreach (var site in chainSites)
                    {
                        if (current.SingleOrDefault(x => x.Id == site.Id) == null)
                        {
                            current.Add(site);
                        }
                    }

                    foreach (var site in OrginalData.Where(x=>x.ParentId ==id))
                    {
                        if (current.SingleOrDefault(x => x.Id == site.Id) == null)
                        {
                            current.Add(site);
                        }
                    }
                }
                else
                {
                    // its zone
                    foreach (var obj in chainSites)
                    {
                        foreach (var site in OrginalData.Where(x => x.ParentId == obj.Id))
                        {
                            if (current.SingleOrDefault(x => x.Id == site.Id) == null)
                            {
                                current.Add(site);
                            }
                        }
                    }

                }
            }
        }

        private void RemoveNewNodes(int id, List<Site> current)
        {
            var item = OrginalData.SingleOrDefault(x => x.Id == id);
            List<Site> chainSites = FindHiearchy(id);
            if (current.Count != 0)
            {
                if (IsSite(id))
                {
                    bool zoneIsTo = false,regionIsTo=false;
                    int zoneId=0, regionId=0;
                    foreach (var site in chainSites)
                    {
                        if(site.Id != id)
                        {
                            var parent = current.SingleOrDefault(x => x.ParentId == 0);

                            if (parent != null && current.Count(x => x.ParentId == parent.Id) == 1)
                            {
                                zoneIsTo = true;
                                zoneId = parent.Id;
                            }
                            else
                            {
                                regionId = parent.Id;
                                regionIsTo = true;
                            }                            
                        }                        
                    }

                    if(zoneIsTo)
                    {
                        current.Remove(OrginalData.SingleOrDefault(x => x.Id == zoneId));
                    }

                    if(regionIsTo)
                    {
                        current.Remove(OrginalData.SingleOrDefault(x => x.Id == regionId));
                    }

                    current.Remove(OrginalData.SingleOrDefault(x => x.Id == id));

                }

                else if (IsRegion(id))
                {
                    bool zoneIsTo = false;
                    int zoneId = 0;
                    foreach (var site in chainSites)
                    {
                        if (site.Id != id)
                        {
                          
                            if (current.Count(x => x.ParentId == item.ParentId) == 1)
                            {
                                zoneIsTo = true;
                                zoneId = item.ParentId;
                            }                           
                        }                        
                    }

                    if (zoneIsTo)
                    {
                        current.Remove(OrginalData.SingleOrDefault(x => x.Id == zoneId));
                    }

                    
                    foreach (var site in OrginalData.Where(x=>x.ParentId ==id))
	                {
		                current.Remove(OrginalData.SingleOrDefault(x => x.Id == site.Id));
	                }

                    current.Remove(OrginalData.SingleOrDefault(x => x.Id == id));


                }
                else
                {
                    // its zone

                    var zone = OrginalData.SingleOrDefault(x => x.Id == id);


                    foreach (var reg in OrginalData.Where(x=>x.ParentId ==id))
                    {
                        foreach (var site in OrginalData.Where(x=>x.ParentId ==reg.Id))
                        {
                            current.Remove(OrginalData.SingleOrDefault(x => x.Id == site.Id));
                        }
                        current.Remove(OrginalData.SingleOrDefault(x => x.Id == reg.Id));
                        
                    }
                    current.Remove(OrginalData.SingleOrDefault(x => x.Id == id));

                }
            }
        }


        private void THreadsinProgress()
        {
            List<int> list = new List<int> { 1, 2, 3, 4, 5 };
            int count = list.Count();
            int j = 0;

            var t1 = new Task(() =>
            {
                for (int i = 0; i < 50; i++)
                {

                    Thread.Sleep(1000);
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                    {
                        pgr.Value += 100 / 50;
                        pgrText.Text = pgr.Value.ToString();
                    }));
                }


            });
            t1.Start();

            var t2 = new Task(() =>
            {
                Parallel.ForEach(list, l =>
                {
                    Thread.Sleep(list[j++] * 1000);
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                    {
                        pgr1.Value += 100 / count;
                        pgr1Text.Text = pgr1.Value.ToString();
                    }));
                });
            });
            t2.Start();
        }


    }

    public class Site
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ParentId { get; set; }

    }

}
