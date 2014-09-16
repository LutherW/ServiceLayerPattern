using ServiceLayerPattern.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceLayerPattern.WebUI.WebForm
{
    public class Basket
    {
        public Guid Id { get; set; }
        public TicketReservationPresentation Reservation { get; set; }

        public static Basket GetBasket()
        {
            if (HttpContext.Current.Session["BASKET"] == null)
            {
                HttpContext.Current.Session.Add("BASKET", new Basket { Id = Guid.NewGuid() });
            }

            return (Basket)HttpContext.Current.Session["BASKET"];
        }

        public static void Clear() 
        {
            HttpContext.Current.Session["BASKET"] = null;
        }
    }
}