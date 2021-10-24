using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Util;
using System.Collections.Concurrent;
using HomeVideo.DTO.BasicDTO;

namespace HomeVideo.BLL.Menu
{
    internal class MenuStringHelper
    {
        internal static IEnumerable<MenuShowInfo> GenerateMenuSting(IEnumerable<MenuInfo> menuInfos)
        {
            if (menuInfos == null || menuInfos.Count() == 0)
            {
                return null;
            }

            var topLevel = menuInfos.Min(x => x.Level);

            var topLevelMenuInfos = menuInfos.Where(x => x.Level == topLevel).OrderBy(x => x.OrderIndex);

            List<MenuShowInfo> result = new List<MenuShowInfo>();

            foreach (var item in topLevelMenuInfos)
            {
                var tmp = new MenuShowInfo(item.Id, item.MenuName, item.PageUrl)
                {
                    SubMenuInfo = OrderMenu(item, item.Level, menuInfos)
                };
                result.Add(tmp);
            }

            return result;
        }

        static IList<MenuShowInfo> OrderMenu(MenuInfo menuInfo, int level, IEnumerable<MenuInfo> menuInfos)
        {
            List<MenuShowInfo> result = new List<MenuShowInfo>();

            var menus = menuInfos.Where(x => x.ParentId == menuInfo.Id && x.Level != level).OrderBy(x => x.OrderIndex);
            foreach (var menu in menus)
            {
                MenuShowInfo tmp = new MenuShowInfo(menu.Id, menu.MenuName, menu.PageUrl)
                {
                    SubMenuInfo = OrderMenu(menu, menu.Level, menuInfos)
                };
                result.Add(tmp);
            }

            return result;
        }

        private static readonly ConcurrentDictionary<string, IEnumerable<MenuShowInfo>> cache = new ConcurrentDictionary<string, IEnumerable<MenuShowInfo>>();

        internal static ConcurrentDictionary<string, IEnumerable<MenuShowInfo>> Cache => cache;
    }

    public struct MenuShowInfo
    {
        public MenuShowInfo(int menuId, string menuName, string pageUrl)
        {
            MenuId = menuId;
            MenuName = menuName;
            PageUrl = pageUrl;
            SubMenuInfo = null;
        }

        public int MenuId { get; set; }

        public string MenuName { get; set; }

        public string PageUrl { get; set; }

        public IList<MenuShowInfo> SubMenuInfo { get; set; }
    }
}
