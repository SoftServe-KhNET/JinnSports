using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JinnSports.WEB.Areas.Mvc.Models
{
    public class PageInfo
    {
        private const int PREVIOUSPAGES = 4;

        private const int NEXTPAGES = 4;

        public PageInfo(int totalItems, int currentPage, int pageSize)
        {   
            this.CurrentPage = currentPage;
            this.PageSize = pageSize; 

            this.TotalPages = (int)Math.Ceiling(totalItems / (double)this.PageSize);

            if (this.TotalPages < PREVIOUSPAGES + NEXTPAGES + 1)
            {
                this.StartPage = 1;
                this.EndPage = this.TotalPages;
                return;
            }

            this.StartPage = this.CurrentPage - PREVIOUSPAGES;
            this.EndPage = this.CurrentPage + NEXTPAGES;

            if (this.StartPage < 1)
            {
                this.StartPage = 1;
                this.EndPage = PREVIOUSPAGES + NEXTPAGES + 1;
            }

            
            if (this.EndPage > this.TotalPages)
            {
                this.EndPage = this.TotalPages;
                if (this.EndPage > PREVIOUSPAGES + NEXTPAGES + 1)
                {
                    this.StartPage = this.EndPage - PREVIOUSPAGES - NEXTPAGES;
                }
            }
        }

        public int TotalPages { get; private set; }

        public int CurrentPage { get; private set; }

        public int StartPage { get; private set; }

        public int EndPage { get; private set; }

        public int PageSize { get; private set; }
    }
}